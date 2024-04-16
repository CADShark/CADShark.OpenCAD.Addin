using CADShark.Common.Logging;
using CADShark.Common.MultiConverter;
using CADShark.Common.SolidWorks;
using CADShark.Common.SolidworksPDM;
using CADShark.OpenCAD.Addin.Config;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;

namespace CADShark.OpenCAD.Addin
{
    public class Core
    {
        private static SldWorks _swApp;
        private static ModelDoc2 _swModel;
        private static readonly CadLogger Logger = CadLogger.GetLogger(className: nameof(Core));
        private static HandlingProgressBar _progressBar;
        private static AssemblyDocument _assembly;
        private static ConvertBuilder _convert;
        private static PdmInstanceManager _pdmInst;

        internal Core()
        {
        }

        public Core(SldWorks swApp)
        {
            Logger.Info("public Core(SldWorks swApp)");
            _swApp = swApp;
            _assembly = new AssemblyDocument(swApp);
            _progressBar = new HandlingProgressBar(swApp);
            _convert = new ConvertBuilder(swApp);

        }

        internal static void ConvertPdf()
        {
            Logger.Info("internal static void ConvertPdf()");
            _swModel = (ModelDoc2)_swApp.ActiveDoc;
            LoadSettings(out var savePathPdf, out _, out _);
            switch (_swModel.GetType())
            {
                case (int)swDocumentTypes_e.swDocASSEMBLY:
                    var assemblyFiles = _assembly.GetDistinctComponents();
                    foreach (var file in assemblyFiles)
                    {
                        _assembly.OpenFile(file);
                        _convert.PathBuilder(file, "PDF", null, savePathPdf);
                        _convert.ConvertToPdf();
                        _swApp.CloseDoc(file);
                    }

                    _swModel = null;

                    _swApp.SendMsgToUser2("Виконано!", (int)swMessageBoxIcon_e.swMbInformation,
                        (int)swMessageBoxBtn_e.swMbOk);
                    break;
                case (int)swDocumentTypes_e.swDocPART:

                    string filePath = null;
                    var status = _assembly.CheckExistDrawingFile(_swModel.GetPathName(), ref filePath);
                    if (status)
                    {
                        _assembly.OpenFile(filePath);
                        _convert.PathBuilder(filePath, "PDF", null, savePathPdf);
                        _convert.ConvertToPdf();
                        _swApp.CloseDoc(filePath);
                    }
                    _swModel = null;
                    _swApp.SendMsgToUser2("Виконано!", (int)swMessageBoxIcon_e.swMbInformation,
                        (int)swMessageBoxBtn_e.swMbOk);
                    break;
                case (int)swDocumentTypes_e.swDocDRAWING:

                    filePath = _swModel.GetPathName();
                    _convert.PathBuilder(filePath, "PDF", null, savePathPdf);
                    _convert.ConvertToPdf();
                    _swModel = null;

                    _swApp.SendMsgToUser2("Виконано!", (int)swMessageBoxIcon_e.swMbInformation,
                        (int)swMessageBoxBtn_e.swMbOk);
                    break;
            }
        }

        internal static void ConvertDxf()
        {
            _swModel = (ModelDoc2)_swApp.ActiveDoc;

            LoadSettings(out _, out var savePathDxf, out _);

            string[] vConfig;
            var pos = 0;

            switch (_swModel.GetType())
            {
                case (int)swDocumentTypes_e.swDocASSEMBLY:
                    object[] components = null;

                    var groupedComponents = _assembly.GetDistinctPartComponents(ref components);

                    foreach (var comp in groupedComponents)
                    {
                        if (!_convert.IsSheetMetalComponent(comp.Value)) continue;
                        _swModel = _assembly.ActivateDoc(comp.Value.GetPathName());
                        vConfig = _assembly.GetDerivedConfig();
                        _assembly.SuppressUpdates(false);
                        for (var i = 0; i < vConfig.Length; i++)
                        {
                            _swModel.ShowConfiguration2(vConfig[i]);
                            _convert.PathBuilder(comp.Value.GetPathName(), "DXF", vConfig[i], savePathDxf);
                            _convert.ConvertToDxf(true);
                        }
                        _swApp.CloseDoc(_swModel.GetTitle());
                    }

                    _swModel = null;
                    _swApp.SendMsgToUser2("Виконано!", (int)swMessageBoxIcon_e.swMbInformation,
                        (int)swMessageBoxBtn_e.swMbOk);
                    break;
                case (int)swDocumentTypes_e.swDocPART:

                    if (!_convert.IsSheetMetalComponent()) return;

                    vConfig = _assembly.GetDerivedConfig();

                    var configCount = vConfig.Length;

                    Logger.Debug($"BarUpperBound = {configCount}");
                    _progressBar.Title = @"Початок операції перетворення деталі в DXF";
                    _progressBar.GetProgressBarUpperBound = configCount;
                    _progressBar.StartProgressBar();
                    _assembly.SuppressUpdates(false);
                    for (var index = 0; index < configCount; index++)
                    {
                        pos += 1;

                        var config = vConfig[index];
                        Logger.Debug($"pos = {pos}");
                        _progressBar.Pos = pos;
                        _progressBar.UpdateProgress();
                        _progressBar.Title = $"Перетворення конфігурації: {config}. Виконано: {index}/{configCount}";
                        _progressBar.UpdateTitle();

                        _convert.PathBuilder(_swModel.GetPathName(), "DXF", vConfig[index], savePathDxf);
                        _convert.ConvertToDxf(true);
                    }

                    _progressBar.EndProgress();

                    _assembly.SuppressUpdates(true);
                    _swModel = null;
                    _swApp.SendMsgToUser2("Виконано!", (int)swMessageBoxIcon_e.swMbInformation,
                        (int)swMessageBoxBtn_e.swMbOk);
                    break;
            }

            _swApp = null;
        }

        internal static void ConvertStep()
        {
            _swModel = (ModelDoc2)_swApp.ActiveDoc;

            var filePath = _swModel.GetPathName();

            LoadSettings(out _, out _, out var savePathStep);

            if (IntegStatus())
            {
                _pdmInst.GetFileFromPath(filePath);
            }

            if (_swModel.GetType() == (int)swDocumentTypes_e.swDocDRAWING) return;

            var vConfig = _assembly.GetDerivedConfig();

            for (var i = 0; i < vConfig.Length; i++)
            {
                _convert.PathBuilder(filePath, "STEP", vConfig[i], savePathStep);
                _convert.ConvertToStep();
            }

            _swApp.SendMsgToUser2("Виконано!", (int)swMessageBoxIcon_e.swMbInformation,
                (int)swMessageBoxBtn_e.swMbOk);

            _swModel = null;
        }

        private static bool IntegStatus()
        {
            var data = ConfigurationFile.LoadConfiguration();
            return data.IntegrationStatus;
        }

        private static void LoadSettings(out string savePathPdf, out string savePathDxf, out string savePathStep)
        {
            var data = ConfigurationFile.LoadConfiguration();
            savePathPdf = data.SavePathPdf;
            savePathDxf = data.SavePathPdf;
            savePathStep = data.SavePathPdf;
        }
    }
}
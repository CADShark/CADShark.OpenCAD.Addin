using CADShark.Common.Logging;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swpublished;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;


namespace CADShark.OpenCAD.Addin
{
    [ComVisible(true)]
    [Guid("31B803E0-7A01-4841-A0DE-895B726625C9")]
    [DisplayName("OpenCAD")]
    [Description("Automate your work in SOLIDWORKS!")]
    public partial class OpenCadPlugin : ISwAddin
    {
        #region Public Properties

        public SldWorks SwApp { get; private set; }
        

        #endregion

        private static readonly CadLogger Logger = CadLogger.GetLogger(className: nameof(OpenCadPlugin));
        private int _addinId;


        #region Registration

        private const string AddinKeyTemplate = @"SOFTWARE\SolidWorks\Addins\{{{0}}}";
        private const string AddinStartupKeyTemplate = @"Software\SolidWorks\AddInsStartup\{{{0}}}";
        private const string AddInTitleRegKeyName = "Title";
        private const string AddInDescriptionRegKeyName = "Description";

        [ComRegisterFunction]
        public static void RegisterFunction(Type t)
        {
            try
            {
                string addInTitle;
                string addInDesc;

                var dispNameAtt = t.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault();

                if (dispNameAtt != null)
                {
                    addInTitle = dispNameAtt.DisplayName;
                }
                else
                {
                    addInTitle = t.ToString();
                }

                var descAtt = t.GetCustomAttributes(false).OfType<DescriptionAttribute>().FirstOrDefault();

                if (descAtt != null)
                {
                    addInDesc = descAtt.Description;
                }
                else
                {
                    addInDesc = t.ToString();
                }

                var addInkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(
                    string.Format(AddinKeyTemplate, t.GUID));

                if (addInkey != null)
                {
                    addInkey.SetValue(null, 0);

                    addInkey.SetValue(AddInTitleRegKeyName, addInTitle);
                    addInkey.SetValue(AddInDescriptionRegKeyName, addInDesc);
                }

                var addInStartupkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    string.Format(AddinStartupKeyTemplate, t.GUID));

                if (addInStartupkey != null)
                    addInStartupkey.SetValue(null, Convert.ToInt32(true),
                        Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while registering the addin: " + ex.Message);
            }
        }

        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t)
        {
            try
            {
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(
                    string.Format(AddinKeyTemplate, t.GUID));

                Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(
                    string.Format(AddinStartupKeyTemplate, t.GUID));
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while unregistering the addin: " + e.Message);
            }
        }

        #endregion

        public bool ConnectToSW(object thisSw, int cookie)
        {
            SwApp = thisSw as SldWorks;

            _addinId = cookie;

            if (SwApp == null) return false;

            SwApp.SetAddinCallbackInfo2(0, this, _addinId);

            #region Set up the CommandManager

            CmdMgr = SwApp.GetCommandManager(_addinId);

            CreateCommandMgr();

            #endregion

            return true;
        }

        public bool DisconnectFromSW()
        {
            Marshal.ReleaseComObject(SwApp);
            SwApp = null;
            Marshal.ReleaseComObject(CmdMgr);
            CmdMgr = null;
            // The add-in must call GC.Collect() here in order to retrieve all managed code pointers 
            GC.Collect();
            GC.WaitForPendingFinalizers();

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return true;
        }
    }
}
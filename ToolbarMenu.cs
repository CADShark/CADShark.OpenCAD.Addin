using CADShark.Common.Analytics;
using CADShark.OpenCAD.Addin.Windows;
using CADShark.OpenCAD.Addin.Windows.SettingsWindow;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using SolidWorksTools.File;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CADShark.OpenCAD.Addin
{
    public partial class OpenCadPlugin
    {
        private ICommandGroup _cmdGroup;
        private BitmapHandler _iBmp;
        
        public const int MainCmdGroupId = 5000;

        public ICommandManager CmdMgr { get; private set; }

        public void CreateCommandMgr()
        {
            try
            {
                AddCommandMgr();
            }
            catch (Exception ex)
            {
                Logger.Error("Could not create toolbar MultiConverter", ex);
                //int num = (int) MessageBox.Show(Settings.Default.ProductName + " could not create the toolbar.", SolidWorksMessageBoxIcon.Warning);
            }
        }


        public void AddCommandMgr()
        {
            if (_iBmp == null)
                _iBmp = new BitmapHandler();
            Assembly.GetAssembly(GetType());

            const string title = "OpenCAD";
            const string toolTip = "OpenCAD modules";

            var docTypes = new[]
            {
                (int)swDocumentTypes_e.swDocASSEMBLY,
                (int)swDocumentTypes_e.swDocDRAWING,
                (int)swDocumentTypes_e.swDocPART
            };

            var cmdGroupErr = 0;
            var ignorePrevious = false;

            const int mainItemId1 = 5001;
            const int mainItemId2 = 5002;
            const int mainItemId3 = 5003;
            const int mainItemId100 = 5100;
            const int mainItemId101 = 5101;
            const int mainItemId102 = 5102;


            // Get the ID information stored in the registry
            var getDataResult = CmdMgr.GetGroupDataFromRegistry(MainCmdGroupId, out var registryIDs);


            var knownIDs = new[]
                { mainItemId1, mainItemId2, mainItemId3 };
            //{ mainItemId1, mainItemId2, mainItemId3, mainItemId100, mainItemId101, mainItemId102 };

            if (getDataResult)
            {
                if (!CompareIDs((int[])registryIDs, knownIDs)) // If the IDs don't match, reset the CommandGroup
                {
                    ignorePrevious = true;
                }
            }

            _cmdGroup = CmdMgr.CreateCommandGroup2(MainCmdGroupId, title, toolTip, "Hit CreateCommandGroup2", -1,
                ignorePrevious,
                ref cmdGroupErr);

            var icons = new string[6];

            icons[0] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons20.png";
            icons[1] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            icons[2] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            icons[3] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            icons[4] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            icons[5] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";

            var mainIcons = new string[6];
            mainIcons[0] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons20.png";
            mainIcons[1] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            mainIcons[2] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            mainIcons[3] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            mainIcons[4] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";
            mainIcons[5] = @"D:\Projects\CADShark.OpenCAD.Addin\PDFicons32.png";

            _cmdGroup.IconList = icons;
            _cmdGroup.MainIconList = mainIcons;

            const int menuToolbarOption = (int)swCommandItemType_e.swToolbarItem;
            var cmdIndex0 = _cmdGroup.AddCommandItem2("Export to PDF", 1, "Export to PDF", "Export to PDF", 0,
                "ExportPdfCallback", "", mainItemId1, menuToolbarOption);
            var cmdIndex1 = _cmdGroup.AddCommandItem2("Export to DXF", 2, "Export to DXF", "Export to DXF", 1,
                "ExportDxfCallback", "", mainItemId2, menuToolbarOption);
            var cmdIndex2 = _cmdGroup.AddCommandItem2("Export to STEP", 3, "Export to STEP", "Export to STEP", 2,
                "ExportStepCallback", "", mainItemId3, menuToolbarOption);
            var cmdIndex100 = _cmdGroup.AddCommandItem2("Help", 4, "Показати сторінку онлайн-довідки", "Довідка", 3,
                "OpenKnowledgeBase", "", mainItemId100, menuToolbarOption);
            var cmdIndex101 = _cmdGroup.AddCommandItem2("Feedback", 5,
                "Допоможіть нам покращити OpenCAD, поділившись ідеєю чи помилкою.", "Поділіться своїм відгуком", 4,
                "ShowFeedbackWindow", "", mainItemId101, menuToolbarOption);
            var cmdIndex102 = _cmdGroup.AddCommandItem2("Settings", 6, "Відкрити вікно налаштувань", "Налаштування", 5,
                "ShowSettingsWindow", "", mainItemId102, menuToolbarOption);

            _cmdGroup.HasToolbar = true;
            _cmdGroup.HasMenu = true;
            _cmdGroup.Activate();

            foreach (var type in docTypes)
            {
                var cmdTab = CmdMgr.GetCommandTab(type, title);

                if (cmdTab != null & !getDataResult |
                    ignorePrevious) // If tab exists, but we have ignored the registry info (or changed CommandGroup ID), re-create the tab; otherwise the ids won't match and the tab will be blank
                {
                    CmdMgr.RemoveCommandTab(cmdTab);
                    cmdTab = null;
                }

                // If cmdTab is null, must be first load (possibly after reset), add the commands to the tabs
                if (cmdTab != null) continue;
                cmdTab = CmdMgr.AddCommandTab(type, title);

                var cmdBox = cmdTab.AddCommandTabBox();

                var cmdIDs = new int[6];
                var textType = new int[6];

                cmdIDs[0] = _cmdGroup.CommandID[cmdIndex0];
                textType[0] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                cmdIDs[1] = _cmdGroup.CommandID[cmdIndex1];
                textType[1] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                cmdIDs[2] = _cmdGroup.CommandID[cmdIndex2];
                textType[2] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                cmdIDs[3] = _cmdGroup.CommandID[cmdIndex100];
                textType[3] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                cmdIDs[4] = _cmdGroup.CommandID[cmdIndex101];
                textType[4] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                cmdIDs[5] = _cmdGroup.CommandID[cmdIndex102];
                textType[5] = (int)swCommandTabButtonTextDisplay_e.swCommandTabButton_TextBelow;

                cmdBox.AddCommands(cmdIDs, textType);
                cmdTab.AddSeparator(cmdBox, cmdIDs[3]);
            }
        }

        public bool CompareIDs(int[] storedIDs, int[] addinIDs)
        {
            var storedList = new List<int>(storedIDs);
            var addinList = new List<int>(addinIDs);

            addinList.Sort();
            storedList.Sort();

            if (addinList.Count != storedList.Count)
            {
                return false;
            }

            for (var i = 0; i < addinList.Count; i++)
            {
                if (addinList[i] != storedList[i])
                {
                    return false;
                }
            }

            return true;
        }

        public void RemoveCommandMgr()
        {
            _iBmp.Dispose();
            CmdMgr.RemoveCommandGroup(MainCmdGroupId);
        }

        public void ExportPdfCallback()
        {
            Logger.Info("Toolbar clicked: Export document to PDF", nameof(ExportPdfCallback));
            Telemetry.LogButtonClick("Export document to PDF");
            _ = new Core(SwApp);
            Core.ConvertPdf();
        }

        public void ExportDxfCallback()
        {
            Logger.Info("Toolbar clicked: Export document to DXF", nameof(ExportStepCallback));
            Telemetry.LogButtonClick("Export document to DXF");
            _ = new Core(SwApp);
            Core.ConvertDxf();
        }

        public void ExportStepCallback()
        {
            Logger.Info("Toolbar clicked: Export document to STEP", nameof(ExportStepCallback));
            Telemetry.LogButtonClick("Export document to STEP");
            _ = new Core(SwApp);
            Core.ConvertStep();
        }

        public void OpenKnowledgeBase()
        {
            Logger.Info("Toolbar clicked: show Knowledge Base", nameof(OpenKnowledgeBase));
        }

        public void ShowFeedbackWindow()
        {
            Logger.Info("Toolbar clicked: show feedback window", nameof(ShowFeedbackWindow));
            Telemetry.LogButtonClick("Open Feedback window");
            new FeedbackWindow().ShowDialog();
        }

        public void ShowSettingsWindow()
        {
            Logger.Info("Toolbar clicked: show settings window", nameof(ShowSettingsWindow));
            Telemetry.LogButtonClick("Open Settings window");
            new Windows.SettingsWindow.SettingsWindow().ShowDialog();
        }
    }
}
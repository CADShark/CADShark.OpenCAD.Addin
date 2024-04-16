using System;
using System.Windows.Forms;
using CADShark.Common.Logging;
using CADShark.OpenCAD.Addin.SettingsWindow;

namespace CADShark.OpenCAD.Addin.Windows.SettingsWindow
{
    public partial class SettingsWindow : Form
    {
        private static readonly CadLogger Logger = CadLogger.GetLogger(className: nameof(SettingsWindow));
        private UserControl _userControl;
        public SettingsWindow()
        {
            InitializeComponent();
            List();
        }

        private void List()
        {
            SettingType[] row = { SettingType.SOLIDWORKS_PDM, SettingType.PDF, SettingType.DXF, SettingType.STEP};

            foreach (var item in row)
            {
                listBox1.Items.Add(EnumExtensions.GetDescription(item));
            }
            listBox1.SelectedIndex = 0;
            
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            var item = listBox1.SelectedItem.ToString();

            Enum.TryParse(item, true, out SettingType selectedItem);

            if (_userControl != null)
            {
                tableLayoutPanel1.Controls.Remove(_userControl);
            }

            Logger.Debug($"selectedItem = {selectedItem}");
            FillListBox(selectedItem);
        }
        private void FillListBox(SettingType selectedSettingType)
        {
            _userControl = null;

            switch (selectedSettingType)
            {
                case SettingType.SOLIDWORKS_PDM:
                    _userControl = new PdmUserControl();
                    break;
                case SettingType.PDF:
                    _userControl = new PdfUserControl();
                    break;
                case SettingType.DXF:
                    _userControl = new DxfUserControl();
                    break;
                case SettingType.STEP:
                    _userControl = new StepUserControl();
                    break;
            }

            if (_userControl == null) return;

            _userControl.Dock = DockStyle.Fill;
            tableLayoutPanel1.Controls.Add(_userControl, 1, 0);
        }
    }
}

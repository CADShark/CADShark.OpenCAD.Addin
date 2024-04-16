using System.Windows.Forms;
using CADShark.OpenCAD.Addin.Config;

namespace CADShark.OpenCAD.Addin.SettingsWindow
{
    public partial class PdfUserControl : UserControl
    {
        private string _savePath;
        public PdfUserControl()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void ButtonBrowserDialogClick(object sender, System.EventArgs e)
        {
            var bd = new FolderBrowserDialog();

            if (bd.ShowDialog() == DialogResult.OK)
            {
                _savePath = bd.SelectedPath;
            }
            textBoxPath.Text = _savePath;
        }

        private void LoadSettings()
        {
            var data = ConfigurationFile.LoadConfiguration();
            textBoxPath.Text = data.SavePathPdf;
        }

        private void ButtonSaveClick(object sender, System.EventArgs e)
        {
            SaveSettings();
        }
        private void SaveSettings()
        {
            var data = new ConfigurationData
            {
                SavePathPdf = _savePath
            };
            ConfigurationFile.SaveConfiguration(data);
        }
    }
}

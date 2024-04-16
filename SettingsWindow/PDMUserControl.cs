using System;
using System.Linq;
using System.Windows.Forms;
using CADShark.Common.Logging;
using CADShark.Common.SolidworksPDM;
using CADShark.OpenCAD.Addin.Config;

namespace CADShark.OpenCAD.Addin.SettingsWindow
{
    public partial class PdmUserControl : UserControl
    {
        private static readonly CadLogger Logger = CadLogger.GetLogger(className: nameof(PdmUserControl));
        private string _vaultName;
        public PdmUserControl()
        {
            InitializeComponent();
            LoadSettings();
            //IntegrationStatus();
        }

        private void IntegrationStatus()
        {
            comboBoxVaultViews.Enabled = checkBoxInterg.Checked;
            FillVaultViews();
        }

        private void checkBoxInterg_CheckedChanged(object sender, System.EventArgs e)
        {
            IntegrationStatus();
        }

        private void FillVaultViews()
        {
            var vault = new PdmInstanceManager();
            object[] list = vault.GetVaultViews();

            comboBoxVaultViews.Items.AddRange(list);

            var status = comboBoxVaultViews.Items.Contains(_vaultName);

            if (status)
            {
                comboBoxVaultViews.SelectedItem = _vaultName;
            }
            vault.LoginAuto(_vaultName);
            
        }

        private void LoadSettings()
        {
            var data = ConfigurationFile.LoadConfiguration();
            checkBoxInterg.Checked = data.IntegrationStatus;
            _vaultName = data.VaultName;

            if (!checkBoxInterg.Checked)
            {
                comboBoxVaultViews.Enabled = false;
            }
            else
            {
                comboBoxVaultViews.Enabled = true;
                FillVaultViews();
            }
        }

        private void SaveSettings()
        {
            var data = new ConfigurationData
            {
                IntegrationStatus = checkBoxInterg.Checked,
                VaultName = comboBoxVaultViews.Text,
            };
            ConfigurationFile.SaveConfiguration(data);
        }

        private void ButtonSaveClick(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
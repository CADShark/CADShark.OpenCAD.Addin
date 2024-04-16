namespace CADShark.OpenCAD.Addin.SettingsWindow
{
    partial class PdmUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxVaultViews = new System.Windows.Forms.ComboBox();
            this.checkBoxInterg = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonSave);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxVaultViews);
            this.groupBox1.Controls.Add(this.checkBoxInterg);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(620, 435);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Інтеграція з SOLIDWORKS PDM";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(6, 399);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(93, 30);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "Зберегти";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSaveClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Оберіть сховище:";
            // 
            // comboBoxVaultViews
            // 
            this.comboBoxVaultViews.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxVaultViews.FormattingEnabled = true;
            this.comboBoxVaultViews.Location = new System.Drawing.Point(9, 86);
            this.comboBoxVaultViews.Name = "comboBoxVaultViews";
            this.comboBoxVaultViews.Size = new System.Drawing.Size(170, 25);
            this.comboBoxVaultViews.TabIndex = 1;
            // 
            // checkBoxInterg
            // 
            this.checkBoxInterg.AutoSize = true;
            this.checkBoxInterg.Location = new System.Drawing.Point(6, 33);
            this.checkBoxInterg.Name = "checkBoxInterg";
            this.checkBoxInterg.Size = new System.Drawing.Size(277, 21);
            this.checkBoxInterg.TabIndex = 0;
            this.checkBoxInterg.Text = "Включити інтеграцію з SOLIDWORKS PDM";
            this.checkBoxInterg.UseVisualStyleBackColor = true;
            this.checkBoxInterg.CheckedChanged += new System.EventHandler(this.checkBoxInterg_CheckedChanged);
            // 
            // PDMUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "PdmUserControl";
            this.Size = new System.Drawing.Size(620, 435);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxInterg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxVaultViews;
        private System.Windows.Forms.Button buttonSave;
    }
}

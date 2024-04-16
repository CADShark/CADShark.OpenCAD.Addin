using System;
using System.Windows.Forms;

namespace CADShark.OpenCAD.Addin.Windows
{
    public  partial class FeedbackWindow : Form
    {
        public  FeedbackWindow()
        {
            InitializeComponent();
        }

        private void button_Close_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

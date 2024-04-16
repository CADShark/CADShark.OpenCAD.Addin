using System;

namespace CADShark.OpenCAD.Addin
{
    public partial class OpenCadPlugin
    {
        public static void CreateMenu()
        {
            try
            {
                AddMenu();
            }
            catch (Exception ex)
            {
                Addin.OpenCadPlugin.Logger.Error("Could not create Tools menu", ex);
                //int num = (int) MessageBox.Show(Settings.Default.ProductName + " could not create buttons in Tools menu.", SolidWorksMessageBoxIcon.Warning);
            }
        }

        public static void AddMenu()
        {
        }
    }
}
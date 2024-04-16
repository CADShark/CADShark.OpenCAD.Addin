using System.ComponentModel;

namespace CADShark.OpenCAD.Addin.Windows
{
    internal enum  SettingType
    {
        [Description("SOLIDWORKS PDM")]
        SOLIDWORKS_PDM,
        [Description("PDF")]
        PDF,
        [Description("DXF")]
        DXF,
        [Description("STEP")]
        STEP
    }
}

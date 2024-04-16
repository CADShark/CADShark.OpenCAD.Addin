using System;
using System.IO;
using System.Xml.Serialization;
using CADShark.Common.Logging;

namespace CADShark.OpenCAD.Addin.Config
{
    public class ConfigurationFile
    {
        private static readonly CadLogger Logger = CadLogger.GetLogger(className: nameof(ConfigurationData));
        private const string ConfigFile = @"D:\config.xml";

        public static void SaveConfiguration(ConfigurationData data)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(ConfigurationData));
                using (TextWriter writer = new StreamWriter(ConfigFile))
                {
                    serializer.Serialize(writer, data);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to save configuration: " + ex.Message);
            }
        }

        public static ConfigurationData LoadConfiguration()
        {
            try
            {
                if (File.Exists(ConfigFile))
                {
                    var serializer = new XmlSerializer(typeof(ConfigurationData));
                    using (TextReader reader = new StreamReader(ConfigFile))
                    {
                        return (ConfigurationData)serializer.Deserialize(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Failed to load configuration: " + ex.Message);
            }
            return new ConfigurationData();
        }
        
    }
}
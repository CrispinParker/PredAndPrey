namespace PredAndPrey.Core
{
    using System.IO;
    using System.Xml.Serialization;

    public class SettingsHelper
    {
        private static readonly XmlSerializer Serialiser = new XmlSerializer(typeof(SettingsHelper));

        private static SettingsHelper instance;

        private SettingsHelper()
        {
        }

        public static SettingsHelper Instance
        {
            get
            {
                return instance = instance ?? GetSettings();
            }
        }

        public int MaxAnimals { get; set; }

        public int MaxPlants { get; set; }

        public double ChanceOfMutation { get; set; }

        public double MutationSeverity { get; set; }

        public double HerbivoreInitialSight { get; set; }

        public double HerbivoreInitialSpeed { get; set; }

        public double CarnivoreInitialSight { get; set; }

        public double CarnivoreInitialSpeed { get; set; }

        public double ScreenWidth { get; set; }

        public double ScreenHeight { get; set; }

        public int RunSpeed { get; set; }

        public bool ShowStatistics { get; set; }

        private static SettingsHelper Default
        {
            get
            {
                return new SettingsHelper
                    {
                        MaxAnimals = 350,
                        CarnivoreInitialSight = 50,
                        CarnivoreInitialSpeed = 5,
                        ChanceOfMutation = 0.1,
                        HerbivoreInitialSight = 45,
                        HerbivoreInitialSpeed = 4.5,
                        MaxPlants = 100,
                        MutationSeverity = 0.07,
                        RunSpeed = 1,
                        ScreenHeight = 600,
                        ScreenWidth = 800,
                        ShowStatistics = false
                    };
            }
        }

        public void Save()
        {
            using (var sw = new StreamWriter(SettingFilePath()))
            {
                Serialiser.Serialize(sw, this);
            }
        }

        private static SettingsHelper GetSettings()
        {
            var path = SettingFilePath();
            SettingsHelper output;
            if (File.Exists(path))
            {
                using (var sr = new StreamReader(path))
                {
                    output = (SettingsHelper)Serialiser.Deserialize(sr);
                }
            }
            else
            {
                output = Default;

                var directoryName = Path.GetDirectoryName(path);
                if (directoryName != null && !Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }

                using (var fs = new FileStream(path, FileMode.Create))
                using (var sw = new StreamWriter(fs))
                {
                    Serialiser.Serialize(sw, output);
                }
            }

            return output;
        }

        private static string SettingFilePath()
        {
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), @"PredAndPrey\Setting.Config");
        }
    }
}

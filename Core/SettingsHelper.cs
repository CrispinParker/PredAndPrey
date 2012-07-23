namespace PredAndPrey.Core
{
    using System.IO;
    using System.Xml.Serialization;

    public enum EnvironmentSizeOption
    {
        Small = 0,
        Medium = 1,
        Large = 2,
        XtraLarge = 3,
        Maximum = 4
    }

    public class SettingsHelper
    {
        private static readonly XmlSerializer Serialiser = new XmlSerializer(typeof(SettingsHelper));

        private static SettingsHelper instance;

        private EnvironmentSizeOption environmentSize;

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

        public double HealthCost { get; set; }

        public int RunSpeed { get; set; }

        public bool ShowStatistics { get; set; }

        public EnvironmentSizeOption EnvironmentSize
        {
            get
            {
                return this.environmentSize;
            }

            set
            {
                this.environmentSize = value;
                this.ApplyEnvironmentSize();
            }
        }

        private static SettingsHelper Default
        {
            get
            {
                return new SettingsHelper
                    {
                        MaxAnimals = 350,
                        ChanceOfMutation = 0.1,
                        MutationSeverity = 0.07,
                        RunSpeed = 1,
                        ShowStatistics = false,
                        EnvironmentSize = EnvironmentSizeOption.Medium
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

        private void ApplyEnvironmentSize()
        {
            switch (this.EnvironmentSize)
            {
                case EnvironmentSizeOption.Small:
                    this.ScreenWidth = 640;
                    this.ScreenHeight = 480;
                    this.CarnivoreInitialSight = 45;
                    this.HerbivoreInitialSight = 43;
                    this.CarnivoreInitialSpeed = 2.6;
                    this.HerbivoreInitialSpeed = 2.5;
                    this.MaxPlants = 15;
                    this.HealthCost = 0.19;
                    break;
                case EnvironmentSizeOption.Medium:
                    this.ScreenWidth = 800;
                    this.ScreenHeight = 600;
                    this.CarnivoreInitialSight = 50;
                    this.HerbivoreInitialSight = 47;
                    this.CarnivoreInitialSpeed = 4.3;
                    this.HerbivoreInitialSpeed = 4;
                    this.MaxPlants = 25;
                    this.HealthCost = 0.14;
                    break;
                case EnvironmentSizeOption.Large:
                    this.ScreenWidth = 1200;
                    this.ScreenHeight = 900;
                    this.CarnivoreInitialSight = 60;
                    this.HerbivoreInitialSight = 55;
                    this.CarnivoreInitialSpeed = 5.5;
                    this.HerbivoreInitialSpeed = 5;
                    this.MaxPlants = 50;
                    this.HealthCost = 0.11;
                    break;
                case EnvironmentSizeOption.XtraLarge:
                    this.ScreenWidth = 1800;
                    this.ScreenHeight = 1350;
                    this.CarnivoreInitialSight = 75;
                    this.HerbivoreInitialSight = 70;
                    this.CarnivoreInitialSpeed = 6.5;
                    this.HerbivoreInitialSpeed = 6;
                    this.MaxPlants = 75;
                    this.HealthCost = 0.09;
                    break;
                case EnvironmentSizeOption.Maximum:
                    this.ScreenWidth = 2700;
                    this.ScreenHeight = 2025;
                    this.CarnivoreInitialSight = 85;
                    this.HerbivoreInitialSight = 65;
                    this.CarnivoreInitialSpeed = 7.5;
                    this.HerbivoreInitialSpeed = 6.5;
                    this.MaxPlants = 100;
                    this.HealthCost = 0.07;
                    break;
            }
        }
    }
}

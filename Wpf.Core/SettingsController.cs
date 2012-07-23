namespace PredAndPrey.Wpf.Core
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using PredAndPrey.Core;

    public class SettingsController : INotifyPropertyChanged
    {
        public SettingsController()
        {
            this.CreateCommands();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand SaveAndCloseCommand { get; set; }

        public int MaxAnimals
        {
            get
            {
                return SettingsHelper.Instance.MaxAnimals;
            }

            set
            {
                SettingsHelper.Instance.MaxAnimals = value;

                this.OnPropertyChanged("MaxAnimals");
            }
        }

        public int RunSpeed
        {
            get
            {
                return SettingsHelper.Instance.RunSpeed;
            }

            set
            {
                SettingsHelper.Instance.RunSpeed = value;

                this.OnPropertyChanged("RunSpeed");
            }
        }

        public double ChanceOfMutation
        {
            get
            {
                return SettingsHelper.Instance.ChanceOfMutation;
            }

            set
            {
                SettingsHelper.Instance.ChanceOfMutation = value;

                this.OnPropertyChanged("ChanceOfMutation");
            }
        }

        public double MutationSeverity
        {
            get
            {
                return SettingsHelper.Instance.MutationSeverity;
            }

            set
            {
                SettingsHelper.Instance.MutationSeverity = value;

                this.OnPropertyChanged("MutationSeverity");
            }
        }

        public bool ShowStatistics
        {
            get
            {
                return SettingsHelper.Instance.ShowStatistics;
            }

            set
            {
                SettingsHelper.Instance.ShowStatistics = value;

                this.OnPropertyChanged("ShowStatistics");
            }
        }

        public int EnvironmentSize
        {
            get
            {
                return (int)SettingsHelper.Instance.EnvironmentSize;
            }

            set
            {
                SettingsHelper.Instance.EnvironmentSize = (EnvironmentSizeOption)value;

                this.OnPropertyChanged("ScreenSizeDescription");
            }
        }

        public string ScreenSizeDescription
        {
            get
            {
                return GetScreenSizeDescription();
            }
        }

        private static void SaveAndClose(Window window)
        {
            SettingsHelper.Instance.Save();
            window.Close();
        }

        private static string GetScreenSizeDescription()
        {
            switch (SettingsHelper.Instance.EnvironmentSize)
            {
                case EnvironmentSizeOption.Small:
                    return "Small";
                case EnvironmentSizeOption.Medium:
                    return "Medium";
                case EnvironmentSizeOption.Large:
                    return "Large";
                case EnvironmentSizeOption.XtraLarge:
                    return "Extra Large";
                case EnvironmentSizeOption.Maximum:
                    return "Maximum";
                default:
                    return string.Empty;
            }
        }

        private void CreateCommands()
        {
            this.SaveAndCloseCommand = new UICommand(w => SaveAndClose(w as Window));
        }

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}

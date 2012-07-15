namespace PredAndPrey.Wpf.Core
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Input;

    using PredAndPrey.Core;

    public class SettingsController : INotifyPropertyChanged
    {
        private int screenSize;

        public SettingsController()
        {
            this.CreateCommands();
            this.DetectScreenSize();
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

        public int ScreenSize
        {
            get
            {
                return this.screenSize;
            }

            set
            {
                this.screenSize = value;

                this.ApplyScreenSize();

                this.OnPropertyChanged("ScreenSizeDescription");
            }
        }

        public string ScreenSizeDescription { get; set; }

        private void ApplyScreenSize()
        {
            switch (this.ScreenSize)
            {
                default:
                    this.ScreenSizeDescription = "Small";
                    SettingsHelper.Instance.ScreenWidth = 640;
                    SettingsHelper.Instance.ScreenHeight = 480;
                    SettingsHelper.Instance.CarnivoreInitialSight = 45;
                    SettingsHelper.Instance.HerbivoreInitialSight = 40;
                    SettingsHelper.Instance.CarnivoreInitialSpeed = 3;
                    SettingsHelper.Instance.HerbivoreInitialSpeed = 2.5;
                    break;
                case 2:
                    this.ScreenSizeDescription = "Medium";
                    SettingsHelper.Instance.ScreenWidth = 800;
                    SettingsHelper.Instance.ScreenHeight = 600;
                    SettingsHelper.Instance.CarnivoreInitialSight = 50;
                    SettingsHelper.Instance.HerbivoreInitialSight = 45;
                    SettingsHelper.Instance.CarnivoreInitialSpeed = 4.5;
                    SettingsHelper.Instance.HerbivoreInitialSpeed = 4;
                    break;
                case 3:
                    this.ScreenSizeDescription = "Large";
                    SettingsHelper.Instance.ScreenWidth = 1280;
                    SettingsHelper.Instance.ScreenHeight = 1024;
                    SettingsHelper.Instance.CarnivoreInitialSight = 60;
                    SettingsHelper.Instance.HerbivoreInitialSight = 55;
                    SettingsHelper.Instance.CarnivoreInitialSpeed = 5.5;
                    SettingsHelper.Instance.HerbivoreInitialSpeed = 5;
                    break;
                case 4:
                    this.ScreenSizeDescription = "Xtra Large";
                    SettingsHelper.Instance.ScreenWidth = 1600;
                    SettingsHelper.Instance.ScreenHeight = 1200;
                    SettingsHelper.Instance.CarnivoreInitialSight = 75;
                    SettingsHelper.Instance.HerbivoreInitialSight = 70;
                    SettingsHelper.Instance.CarnivoreInitialSpeed = 6.5;
                    SettingsHelper.Instance.HerbivoreInitialSpeed = 6;
                    break;
                case 5:
                    this.ScreenSizeDescription = "Maximum";
                    SettingsHelper.Instance.ScreenWidth = 2560;
                    SettingsHelper.Instance.ScreenHeight = 1600;
                    SettingsHelper.Instance.CarnivoreInitialSight = 80;
                    SettingsHelper.Instance.HerbivoreInitialSight = 75;
                    SettingsHelper.Instance.CarnivoreInitialSpeed = 7.5;
                    SettingsHelper.Instance.HerbivoreInitialSpeed = 7;
                    break;
            }
        }

        private void CreateCommands()
        {
            this.SaveAndCloseCommand = new UICommand(w => this.SaveAndClose(w as Window));
        }

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void SaveAndClose(Window window)
        {
            SettingsHelper.Instance.Save();
            SettingsHelper.Instance.Save();
            window.Close();
        }

        private void DetectScreenSize()
        {
            var screenWidth = (int)SettingsHelper.Instance.ScreenWidth;

            if (screenWidth == 800)
            {
                this.screenSize = 2;
            }
            else if (screenWidth == 1280)
            {
                this.screenSize  = 3;
            }
            else if (screenWidth == 1600)
            {
                this.screenSize = 4;
            }
            else if (screenWidth == 2560)
            {
                this.screenSize = 5;
            }
            else
            {
                this.screenSize = 1;
            }

            this.ApplyScreenSize();
        }
    }
}

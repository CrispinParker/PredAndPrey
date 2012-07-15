namespace PredAndPrey.Wpf.Core
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
            : this(new SettingsController())
        {
        }

        public SettingsWindow(SettingsController controller)
        {
            this.DataContext = controller;
            this.InitializeComponent();
        }
    }
}

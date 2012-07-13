namespace PredAndPrey.Wpf.Core
{
    using System.Windows;

    using PredAndPrey.Wpf.Core.Properties;

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            this.InitializeComponent();
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Settings.Default.Save();
            PredAndPrey.Core.Properties.Settings.Default.Save();
            this.Close();
        }
    }
}

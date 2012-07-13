namespace PredAndPrey.ScreenSaver
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    using PredAndPrey.Wpf.Core;

    public partial class MainWindow
    {
        private bool isChangingState;

        public MainWindow()
            : this(new MainController())
        {
        }

        public MainWindow(MainController controller)
        {
            this.DataContext = controller;

            this.InitializeComponent();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Maximized && !this.isChangingState)
            {
                this.isChangingState = true;
                WindowState = WindowState.Normal;
                WindowStyle = WindowStyle.None;
                WindowState = WindowState.Maximized;
                ResizeMode = ResizeMode.NoResize;
                IsTabStop = true;
                this.isChangingState = false;
            }

            base.OnStateChanged(e);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
    }
}

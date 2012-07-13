namespace PredAndPrey.ScreenSaver
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using System.Windows.Media;

    using PredAndPrey.Wpf.Core;

    public partial class App
    {
        private HwndSource winWpfContent; 
        private MainWindow winSaver;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var controller = new MainController();

            // Preview mode--display in little window in Screen Saver dialog
            // (Not invoked with Preview button, which runs Screen Saver in
            // normal /s mode).
            var argSwitch = e.Args[0].ToLower();
            if (argSwitch.StartsWith("/p"))
            {
                this.winSaver = new MainWindow(controller);

                var previewHandle = Convert.ToInt32(e.Args[1]);

                var previewHnd = new IntPtr(previewHandle);

                var rect = new RECT();
                Win32API.GetClientRect(previewHnd, ref rect);

                var sourceParams = new HwndSourceParameters("sourceParams")
                    {
                        PositionX = 0,
                        PositionY = 0,
                        Height = rect.Bottom - rect.Top,
                        Width = rect.Right - rect.Left,
                        ParentWindow = previewHnd,
                        WindowStyle =
                            (int)(WindowStyles.WS_VISIBLE | WindowStyles.WS_CHILD | WindowStyles.WS_CLIPCHILDREN)
                    };

                this.winWpfContent = new HwndSource(sourceParams);
                this.winWpfContent.Disposed += this.winWPFContent_Disposed;
                this.winWpfContent.RootVisual = this.winSaver.Content as Visual;
            }
            else if (argSwitch.StartsWith("/s"))
            {
                // Normal screensaver mode.  Either screen saver kicked in normally,
                // or was launched from Preview button
                var win = new MainWindow(controller) { WindowState = WindowState.Maximized };
                win.Show();
            }
            else if (argSwitch.StartsWith("/c"))
            {
                controller.ShowSettings.Execute(null);
            }
            else
            {
                // If not running in one of the sanctioned modes, shut down the app
                // immediately (because we don't have a GUI).
                Current.Shutdown();
            }
        }

        private void winWPFContent_Disposed(object sender, EventArgs e)
        {
            this.winSaver.Close();
        }
    }
}

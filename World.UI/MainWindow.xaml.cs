namespace PredAndPrey.World.UI
{
    using PredAndPrey.Wpf.Core;

    public partial class MainWindow
    {
        public MainWindow()
            : this(new MainController())
        {
        }

        public MainWindow(MainController controller)
        {
            this.DataContext = controller;
            this.InitializeComponent();
        }
    }
}

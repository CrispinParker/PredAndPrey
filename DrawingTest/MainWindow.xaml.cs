namespace DrawingTest
{
    using PredAndPrey.Wpf.Core;

    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var controller = new MainController();
            this.PART_ViewBox.Child = controller.PlantVisualHost;
        }
    }
}

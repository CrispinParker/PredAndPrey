namespace PredAndPrey.Wpf.Core
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;

    public class BoundCanvas : Canvas
    {
        public static readonly DependencyProperty BoundChildrenProperty = DependencyProperty.Register(
            "BoundChildren", 
            typeof(ObservableCollection<FrameworkElement>), 
            typeof(BoundCanvas), 
            new PropertyMetadata(default(ObservableCollection<FrameworkElement>), OnBoundChildrenContainerChanged));

        public ObservableCollection<FrameworkElement> BoundChildren
        {
            get
            {
                return (ObservableCollection<FrameworkElement>)this.GetValue(BoundChildrenProperty);
            }

            set
            {
                this.SetValue(BoundChildrenProperty, value);
            }
        }

        private static void OnBoundChildrenContainerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var boundCanvas = d as BoundCanvas;

            if (boundCanvas == null)
            {
                return;
            }

            var oldCollection = e.OldValue as ObservableCollection<FrameworkElement>;
            if (oldCollection != null)
            {
                oldCollection.CollectionChanged -= boundCanvas.OnBoundChildrenChanged;
            }

            var newCollection = e.NewValue as ObservableCollection<FrameworkElement>;
            if (newCollection != null)
            {
                newCollection.CollectionChanged += boundCanvas.OnBoundChildrenChanged;
            }

        }

        private void OnBoundChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems.OfType<UIElement>())
                    {
                        this.Children.Add(item);
                    }

                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems.OfType<UIElement>())
                    {
                        this.Children.Remove(item);
                    }

                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.Children.Clear();
                    break;
                default:
                    return;
            }
        }
    }
}

namespace PredAndPrey.Wpf.Core
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Media;

    using PredAndPrey.Wpf.Core.Visuals;

    public class VisualHost : FrameworkElement
    {
        private readonly VisualCollection children;

        private readonly DrawingVisual visual;

        public VisualHost()
        {
            this.children = new VisualCollection(this);

            this.visual = new DrawingVisual();

            this.children.Add(visual);
        }

        protected override int VisualChildrenCount
        {
            get
            {
                return this.children.Count;
            }
        }

        public void RenderVisuals(IEnumerable<OrganismVisual> organismVisuals)
        {
            using (var dc = this.visual.RenderOpen())
            {
                foreach (var organismVisual in organismVisuals)
                {
                    dc.DrawGeometry(organismVisual.Fill, organismVisual.Pen, organismVisual.Geometry);
                }
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index < 0 || index >= this.children.Count)
            {
                throw new ArgumentOutOfRangeException();
            }

            return this.children[index];
        }
    }
}
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Martius.App
{
    // source: https://www.wpf-tutorial.com/listview-control/listview-how-to-column-sorting/
    public class SortAdorner : Adorner
    {
        private static readonly Geometry upGeometry = Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");
        private static readonly Geometry downGeometry = Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection SortDirection { get; private set; }

        public SortAdorner(UIElement adornedElement, ListSortDirection sortDirection) : base(adornedElement)
        {
            SortDirection = sortDirection;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (AdornedElement.RenderSize.Width >= 20)
            {
                var transform = new TranslateTransform
                (
                    AdornedElement.RenderSize.Width - 15,
                    (AdornedElement.RenderSize.Height - 5) / 2
                );
                drawingContext.PushTransform(transform);

                var geometry = SortDirection == ListSortDirection.Ascending ? upGeometry : downGeometry;
                drawingContext.DrawGeometry(Brushes.Black, null, geometry);
                drawingContext.Pop();
            }
        }
    }
}
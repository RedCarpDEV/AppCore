using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RCd.AppCore.Ui.Desktop.Controls
{

    public class PathButton : Button
    {

        #region Constructors

        static PathButton()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PathButton), new FrameworkPropertyMetadata(typeof(PathButton)));
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(nameof(Source), typeof(Geometry), typeof(PathButton), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty StretchProperty =
            DependencyProperty.Register(nameof(Stretch), typeof(Stretch), typeof(PathButton), new FrameworkPropertyMetadata(Stretch.Fill));

        public static readonly DependencyProperty ImageBorderProperty =
            DependencyProperty.Register(nameof(ImageBorder), typeof(Brush), typeof(PathButton), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ImageBorderThicknessProperty =
            DependencyProperty.Register(nameof(ImageBorderThickness), typeof(Thickness), typeof(PathButton), new FrameworkPropertyMetadata());

        public static readonly DependencyProperty ImageHeightProperty =
            DependencyProperty.Register(nameof(ImageHeight), typeof(double), typeof(PathButton), new FrameworkPropertyMetadata(double.NaN));

        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register(nameof(ImageWidth), typeof(double), typeof(PathButton), new FrameworkPropertyMetadata(double.NaN));

        #endregion

        #region Properties

        public Geometry Source
        {
            get => (Geometry)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        public Stretch Stretch
        {
            get => (Stretch)GetValue(StretchProperty);
            set => SetValue(StretchProperty, value);
        }

        public Brush ImageBorder
        {
            get => (Brush)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public Thickness ImageBorderThickness
        {
            get => (Thickness)GetValue(ImageBorderThicknessProperty);
            set => SetValue(ImageBorderThicknessProperty, value);
        }

        public double ImageHeight
        {
            get => (double)GetValue(ImageHeightProperty);
            set => SetValue(ImageHeightProperty, value);
        }

        public double ImageWidth
        {
            get => (double)GetValue(ImageWidthProperty);
            set => SetValue(ImageWidthProperty, value);
        }

        #endregion

    }

}
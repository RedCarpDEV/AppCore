using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace RCd.AppCore.Ui.Desktop.Controls
{

    public class AppWindow : Window
    {

        #region Constructors

        static AppWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AppWindow), new FrameworkPropertyMetadata(typeof(AppWindow)));
        }

        public AppWindow()
        {
            CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, OnShowSystemMenuCommandExecute));
            CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, OnCloseWindowCommandExecute));
            CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, OnMinimizeWindowCommandExecute));
            CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, OnMaximizeWindowCommandExecute));
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            SetValue(ToolsSourceProperty, Tools);
        }

        #endregion

        #region Dependency Properties

        public static readonly DependencyProperty MainMenuContentProperty =
            DependencyProperty.Register(nameof(MainMenuContent), typeof(Geometry), typeof(AppWindow));

        public static readonly DependencyProperty TitleStyleProperty =
            DependencyProperty.Register(nameof(TitleStyle), typeof(Style), typeof(AppWindow));

        public static readonly DependencyProperty ToolsTemplateSelectorProperty =
            DependencyProperty.Register(nameof(ToolsTemplateSelector), typeof(DataTemplateSelector), typeof(AppWindow));

        public static readonly DependencyProperty ToolsTemplateProperty =
            DependencyProperty.Register(nameof(ToolsTemplate), typeof(DataTemplate), typeof(AppWindow));

        public static readonly DependencyProperty ToolsPanelProperty =
            DependencyProperty.Register(nameof(ToolsPanel), typeof(ItemsPanelTemplate), typeof(AppWindow));

        public static readonly DependencyProperty ToolsSourceProperty =
            DependencyProperty.Register(nameof(ToolsSource), typeof(IEnumerable), typeof(AppWindow));

        public static readonly DependencyProperty ToolsProperty =
           DependencyProperty.Register(nameof(Tools), typeof(IList), typeof(AppWindow), new FrameworkPropertyMetadata(new List<object>()));



        public static readonly DependencyProperty WindowBarDecoratorProperty =
            DependencyProperty.Register(nameof(WindowBarDecorator), typeof(object), typeof(AppWindow));

        #endregion

        #region Properties

        public Geometry MainMenuContent
        {
            get => (Geometry)GetValue(MainMenuContentProperty);
            set => SetValue(MainMenuContentProperty, value);
        }

        public Style TitleStyle
        {
            get => (Style)GetValue(TitleStyleProperty);
            set => SetValue(TitleStyleProperty, value);
        }

        public DataTemplateSelector ToolsTemplateSelector
        {
            get => (DataTemplateSelector)GetValue(ToolsTemplateSelectorProperty);
            set => SetValue(ToolsTemplateSelectorProperty, value);
        }

        public DataTemplate ToolsTemplate
        {
            get => (DataTemplate)GetValue(ToolsTemplateProperty);
            set => SetValue(ToolsTemplateProperty, value);
        }

        public ItemsPanelTemplate ToolsPanel
        {
            get => (ItemsPanelTemplate)GetValue(ToolsPanelProperty);
            set => SetValue(ToolsPanelProperty, value);
        }

        public IEnumerable ToolsSource
        {
            get => (IEnumerable)GetValue(ToolsSourceProperty);
            set => SetValue(ToolsSourceProperty, value);
        }

        public IList Tools
        {
            get => (IList)GetValue(ToolsProperty);
            set => SetValue(ToolsProperty, value);
        }

        public object WindowBarDecorator
        {
            get => GetValue(WindowBarDecoratorProperty);
            set => SetValue(WindowBarDecoratorProperty, value);
        }

        #endregion

        #region Command Handling

        protected virtual void OnShowSystemMenuCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var window = (Window)e.Parameter;

            var point = new Point(window.Left + 25, window.Top + 45);

            SystemCommands.ShowSystemMenu(window, point);
        }

        private void OnCloseWindowCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var window = (Window)e.Parameter;
            SystemCommands.CloseWindow(window);
        }

        private void OnMinimizeWindowCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var window = (Window)e.Parameter;
            SystemCommands.MinimizeWindow(window);
        }

        private void OnMaximizeWindowCommandExecute(object sender, ExecutedRoutedEventArgs e)
        {
            var window = (Window)e.Parameter;
            if (window.WindowState == WindowState.Maximized)
            {
                SystemCommands.RestoreWindow(window);
            }
            else if (window.WindowState == WindowState.Normal)
            {
                SystemCommands.MaximizeWindow(window);
            }
        }

        #endregion

    }

}
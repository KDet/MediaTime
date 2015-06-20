using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

namespace MediaTime.Win81.Common
{
    public static class ItemClickCommandBehavior
    {
        #region Command Attached Property
        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }
        public static void SetCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static object GetCommandParameter(DependencyObject obj)
        {
            return obj.GetValue(CommandParameterProperty);
        }
        public static void SetCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(CommandParameterProperty, value);
        }

        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof(ICommand), typeof(ItemClickCommandBehavior), 
            new PropertyMetadata(null, OnCommandChanged));
        
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.RegisterAttached("CommandParameter", typeof(object), typeof(ItemClickCommandBehavior), null);
        #endregion

        #region Behavior implementation
        private static void OnCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listViewBase = d as ListViewBase;
            if (listViewBase != null)
                listViewBase.ItemClick += OnClick;

            var flipView = d as FlipView;
            if (flipView != null)
                flipView.PointerReleased += OnPointerReleased;
        }
        private static void OnPointerReleased(object sender, PointerRoutedEventArgs e)
        {
            var flipView = sender as Selector;
            if (flipView == null || flipView.SelectedItem == null) return;

            var command = flipView.GetValue(CommandProperty) as ICommand;
            var parameter = flipView.GetValue(CommandParameterProperty) ?? flipView.SelectedItem;
            
            if (command != null && command.CanExecute(parameter))
                command.Execute(parameter);
        }
        private static void OnClick(object sender, ItemClickEventArgs e)
        {
            var listViewBase = sender as ListViewBase;
            if (listViewBase == null) return;

            var command = listViewBase.GetValue(CommandProperty) as ICommand;
            var parameter = listViewBase.GetValue(CommandParameterProperty) ?? e.ClickedItem;
           
            if (command != null && command.CanExecute(parameter))
                command.Execute(parameter);
        }
        #endregion
    }
}

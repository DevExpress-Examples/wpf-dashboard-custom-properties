using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWpf;
using DevExpress.Mvvm.UI.Interactivity;

namespace Wpf_Dashboard_Custom_Properties {
    public class ItemDescriptionModule : Behavior<DashboardControl> {
        public const string ItemDescriptionPropertyName = "Description";

        protected override void OnAttached() {
            base.OnAttached();
            ResourceDictionary resources = (ResourceDictionary)Application.LoadComponent(new Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/ItemDescriptionModule/ItemResources.xaml", UriKind.Relative));
            AssociatedObject.DashboardItemStyle = resources["itemStyle"] as Style;
        }
        protected override void OnDetaching() {
            AssociatedObject.DashboardItemStyle = null;
            base.OnDetaching();
        }
    }
    public class ShowItemDescriptionCommandConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return new RelayCommand(ShowDescriptionCommand);
        }
        void ShowDescriptionCommand(object value) {
            DashboardItem dashboardItem = (DashboardItem)value;
            MessageBox.Show(dashboardItem.CustomProperties[ItemDescriptionModule.ItemDescriptionPropertyName], "Dashboard Item Description");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
    public class ShowItemDescriptionCommandParameterConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string itemName = (string)values[0];
            IDashboardControlProvider provider = (IDashboardControlProvider)values[1];
            return provider.Dashboard.Items[itemName];
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
    public class ButtonVisibleConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string itemName = (string)values[0];
            IDashboardControlProvider provider = (IDashboardControlProvider)values[1];
            DashboardItem dashboardItem = provider.Dashboard.Items[itemName];
            return !string.IsNullOrEmpty(dashboardItem.CustomProperties[ItemDescriptionModule.ItemDescriptionPropertyName]);
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
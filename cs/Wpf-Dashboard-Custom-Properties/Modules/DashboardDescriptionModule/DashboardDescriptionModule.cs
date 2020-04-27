using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWpf;
using DevExpress.Mvvm.UI.Interactivity;

namespace Wpf_Dashboard_Custom_Properties {
    public class DashboardDescriptionModule : Behavior<DashboardControl> {
        public const string DashboardDescriptionPropertyName = "DashboardDescription";

        protected override void OnAttached() {
            base.OnAttached();
            ResourceDictionary resources = (ResourceDictionary)Application.LoadComponent(new Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/DashboardDescriptionModule/TitleResources.xaml", UriKind.Relative));
            AssociatedObject.TitleCustomizationsTemplate = resources["titleCustomizationsTemplate"] as DataTemplate;
        }
        protected override void OnDetaching() {
            AssociatedObject.TitleCustomizationsTemplate = null;
            base.OnDetaching();
        }
    }

    public class ShowDashboardDescriptionCommandConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return new RelayCommand(ShowDescriptionCommand);
        }
        void ShowDescriptionCommand(object value) {
            Dashboard dashboard = (Dashboard)value;
            MessageBox.Show(dashboard.CustomProperties[DashboardDescriptionModule.DashboardDescriptionPropertyName], "Dashboard Description");
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public class DashboardDescriptionButtonVisibleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            Dashboard dashboard = (Dashboard)value;
            return !string.IsNullOrEmpty(dashboard.CustomProperties[DashboardDescriptionModule.DashboardDescriptionPropertyName]);
        }
        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Windows;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWpf;
using DevExpress.Mvvm.UI.Interactivity;

namespace Wpf_Dashboard_Custom_Properties {
    public class ChartItemModule : Behavior<DashboardControl> {
        public const string ConstantLinePropertyName = "ConstantLine";
        public const string ScaleBreakPropertyName = "ScaleBreak";
        public const string DashStylePropertyName = "DashStyle";

        protected override void OnAttached() {
            base.OnAttached();
            ResourceDictionary resources = (ResourceDictionary)Application.LoadComponent(new Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/ChartItemModule/ChartResources.xaml", UriKind.Relative));
            AssociatedObject.ChartItemStyle = resources["chartStyle"] as Style;
            AssociatedObject.CustomExport += DashboardControl_CustomExport;
        }
        protected override void OnDetaching() {
            AssociatedObject.CustomExport -= DashboardControl_CustomExport;
            AssociatedObject.ChartItemStyle = null;
            base.OnDetaching();
        }
        void DashboardControl_CustomExport(object sender, CustomExportEventArgs e) {
            ConstantLineExtensions.CustomExport(AssociatedObject, e);
            ScaleBreakExtensions.CustomExport(AssociatedObject, e);
            LineStyleExtensions.CustomExport(AssociatedObject, e);
        }
    }
}
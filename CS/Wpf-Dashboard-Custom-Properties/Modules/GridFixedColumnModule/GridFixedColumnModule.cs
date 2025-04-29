using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWpf;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Grid;

namespace Wpf_Dashboard_Custom_Properties {
    public class GridFixedColumnModule : Behavior<DashboardControl> {
        public const string FixedColumnsPropertyName = "FixedColumns";

        protected override void OnAttached() {
            base.OnAttached();
            ResourceDictionary resources = (ResourceDictionary)Application.LoadComponent(new Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/GridFixedColumnModule/GridResources.xaml", UriKind.Relative));
            AssociatedObject.GridItemStyle = resources["gridStyle"] as Style;
        }
        protected override void OnDetaching() {
            AssociatedObject.GridItemStyle = null;
            base.OnDetaching();
        }
    }

    public class GridFixedColumnsConverter : MarkupExtension, IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string fieldName = values[0].ToString();
            DashboardViewModelBase viewModel = values[1] as DashboardViewModelBase;
            if(viewModel != null) {
                Dashboard dashboard = viewModel.DashboardControlProvider.Dashboard;
                GridDashboardItem grid = dashboard.Items[viewModel.DashboardItemName] as GridDashboardItem;
                DevExpress.DashboardCommon.GridColumnBase column = FindColumn(fieldName, grid.Columns);
                if(column != null) {
                    string customProperty = column.CustomProperties.GetValue(GridFixedColumnModule.FixedColumnsPropertyName);
                    if(!string.IsNullOrEmpty(customProperty)) {
                        return System.Convert.ToBoolean(column.CustomProperties.GetValue(GridFixedColumnModule.FixedColumnsPropertyName)) ? FixedStyle.Left : FixedStyle.None;
                    }
                }
            }
            return FixedStyle.None;
        }
        DevExpress.DashboardCommon.GridColumnBase FindColumn(string fieldName, DevExpress.DashboardCommon.GridColumnCollection columns) {
            GridDimensionColumn dimensionColumn = columns.OfType<GridDimensionColumn>().FirstOrDefault(column => column.Dimension.UniqueId == fieldName);
            if(dimensionColumn != null)
                return dimensionColumn;
            GridMeasureColumn measureColumn = columns.OfType<GridMeasureColumn>().FirstOrDefault(column => column.Measure.UniqueId == fieldName);
            if(measureColumn != null)
                return measureColumn;
            GridHyperlinkColumn hyperlinkColumn = columns.OfType<GridHyperlinkColumn>().FirstOrDefault(column => column.DisplayValue.UniqueId == fieldName);
            if(hyperlinkColumn != null)
                return hyperlinkColumn;
            GridDeltaColumn deltaColumn = columns.OfType<GridDeltaColumn>().FirstOrDefault(column => (column.ActualValue ?? column.TargetValue).UniqueId == fieldName);
            if(deltaColumn != null)
                return deltaColumn;
            GridSparklineColumn sparklineColumn = columns.OfType<GridSparklineColumn>().FirstOrDefault(column => column.Measure.UniqueId == fieldName);
            if(sparklineColumn != null)
                return sparklineColumn;
            return null;
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        }
    }
}
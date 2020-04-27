using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWpf;
using DevExpress.Xpf.Charts;
using DevExpress.XtraReports.UI;

namespace Wpf_Dashboard_Custom_Properties {
    public class LineStyleConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string dashboardItemName = (string)values[0];
            IDashboardControlProvider dashboardControlProvider = (IDashboardControlProvider)values[1];
            string valueDataMember = (string)values[2];
            DashStyle dashStyle = DashStyles.Solid;
            ChartDashboardItem chart = dashboardControlProvider.Dashboard.Items[dashboardItemName] as ChartDashboardItem;
            SimpleSeries series = chart.Panes.SelectMany(p => p.Series).OfType<SimpleSeries>().FirstOrDefault(s => s.Value.UniqueId == valueDataMember);
            if(series != null) {
                string propertyName = series.CustomProperties[ChartItemModule.DashStylePropertyName];
                if(!String.IsNullOrEmpty(propertyName)) {
                    switch(propertyName.ToLower()) {
                        case "dash":
                            dashStyle = DashStyles.Dash;
                            break;
                        case "dot":
                            dashStyle = DashStyles.Dot;
                            break;
                        case "dashdot":
                            dashStyle = DashStyles.DashDot;
                            break;
                        case "dashdotdot":
                            dashStyle = DashStyles.DashDotDot;
                            break;
                    }
                }
            }
            return new LineStyle(2) { DashStyle = dashStyle };
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public static class LineStyleExtensions {
        public static void CustomExport(DashboardControl dashboardControl, CustomExportEventArgs e) {
            foreach(KeyValuePair<string, XRControl> keyValue in e.GetPrintableControls()) {
                if(keyValue.Value is XRChart) {
                    ChartContext chartContext = e.GetChartContext(keyValue.Key);
                    ChartDashboardItem chartItem = dashboardControl.Dashboard.Items[keyValue.Key] as ChartDashboardItem;
                    UpdateChart(chartItem, chartContext);
                }
            }
        }
        static void UpdateChart(ChartDashboardItem chartItem, ChartContext context) {
            List<SimpleSeries> lineSeries = chartItem.Panes.SelectMany(p => p.Series).OfType<SimpleSeries>().Where(s => s.SeriesType == SimpleSeriesType.Line).ToList();
            foreach(SimpleSeries series in lineSeries) {
                string customPropertyValue = series.CustomProperties[ChartItemModule.DashStylePropertyName];
                DevExpress.XtraCharts.DashStyle dashStyle;
                if(!string.IsNullOrEmpty(customPropertyValue) && Enum.TryParse(customPropertyValue, true, out dashStyle)) {
                    foreach(var chartSeries in context.GetControlSeries(series)) {
                        DevExpress.XtraCharts.LineSeriesView view = chartSeries.View as DevExpress.XtraCharts.LineSeriesView;
                        if(view != null)
                            view.LineStyle.DashStyle = dashStyle;
                    }
                }
            }
        }
    }
}

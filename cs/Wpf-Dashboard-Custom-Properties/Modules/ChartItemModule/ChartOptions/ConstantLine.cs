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
    public class ConstantLineValueModuleData {
        public static ConstantLineValueModuleData GetDataFromString(string customPropertyValue) {
            if(!string.IsNullOrEmpty(customPropertyValue)) {
                var array = customPropertyValue.Split('_');
                return new ConstantLineValueModuleData() {
                    PaneName = array[0],
                    IsSecondaryAxis = bool.Parse(array[1]),
                    Value = Convert.ToDouble(array[2])
                };
            }
            return new ConstantLineValueModuleData();
        }

        public string PaneName { get; set; }
        public bool IsSecondaryAxis { get; set; }
        public double Value { get; set; }

        public string GetStringFromData() {
            return PaneName + "_" + IsSecondaryAxis.ToString() + "_" + Value;
        }
    }

    public class ConstantLinesConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string itemName = (string)values[0];
            IDashboardControlProvider provider = (IDashboardControlProvider)values[1];
            ChartDashboardItem chartItem = provider.Dashboard.Items[itemName] as ChartDashboardItem;
            string propertyValue = chartItem.CustomProperties[ChartItemModule.ConstantLinePropertyName];
            var moduleData = ConstantLineValueModuleData.GetDataFromString(propertyValue);
            var pane = chartItem.Panes.FirstOrDefault(x => x.Name == moduleData.PaneName);
            ConstantLine line = new ConstantLine();
            if(pane != null) {
                ChartSeries dashboardSeries = pane.Series.FirstOrDefault(s => s.PlotOnSecondaryAxis == moduleData.IsSecondaryAxis);
                if(dashboardSeries != null) {
                    line.Value = moduleData.Value;
                    Color constantLineColor = Color.FromArgb(255, 0, 128, 0);
                    line.Brush = new SolidColorBrush(constantLineColor);
                    line.LineStyle = new LineStyle();
                    line.LineStyle.Thickness = 2;
                    line.LineStyle.DashStyle = DashStyles.Dash;
                    line.Title = new ConstantLineTitle();
                    line.Title.Content = "Value: " + moduleData.Value;
                    line.Title.Foreground = new SolidColorBrush(constantLineColor);
                }
            }
            return new ConstantLineCollection() { line };
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public static class ConstantLineExtensions {
        public static void CustomExport(DashboardControl dashboardControl, CustomExportEventArgs e) {
            Dictionary<string, XRControl> controls = e.GetPrintableControls();
            foreach(var control in controls) {
                XRChart xrChart = control.Value as XRChart;
                if(xrChart != null) {
                    string itemComponentName = control.Key;
                    ChartDashboardItem chart = dashboardControl.Dashboard.Items[itemComponentName] as ChartDashboardItem;
                    ChartContext chartContext = e.GetChartContext(itemComponentName);
                    UpdateChart(chart, chartContext);
                }
            }
        }
        static void UpdateChart(ChartDashboardItem chartDashboardItem, ChartContext chartContext) {
            string propertyValue = chartDashboardItem.CustomProperties[ChartItemModule.ConstantLinePropertyName];
            var moduleData = ConstantLineValueModuleData.GetDataFromString(propertyValue);
            var pane = chartDashboardItem.Panes.FirstOrDefault(x => x.Name == moduleData.PaneName);
            if(pane != null) {
                ChartSeries dashboardSeries = pane.Series.FirstOrDefault(s => s.PlotOnSecondaryAxis == moduleData.IsSecondaryAxis);
                if(dashboardSeries != null) {
                    DevExpress.XtraCharts.Series chartSeries = chartContext.GetControlSeries(dashboardSeries).FirstOrDefault();
                    var chartAxis = (chartSeries.View as DevExpress.XtraCharts.XYDiagramSeriesViewBase)?.AxisY;
                    if(chartAxis != null) {
                        DevExpress.XtraCharts.ConstantLine line = new DevExpress.XtraCharts.ConstantLine() { AxisValue = moduleData.Value };
                        chartAxis.ConstantLines.Clear();
                        chartAxis.ConstantLines.Add(line);
                        line.ShowInLegend = false;
                        line.Color = System.Drawing.Color.Green;
                        line.LineStyle.Thickness = 2;
                        line.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dash;
                        line.Title.Text = "Value: " + moduleData.Value;
                        line.Title.TextColor = line.Color;
                    }
                }
            }
        }
    }
}

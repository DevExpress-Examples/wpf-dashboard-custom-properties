using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWpf;
using DevExpress.Xpf.Charts;
using DevExpress.XtraReports.UI;

namespace Wpf_Dashboard_Custom_Properties {
    public class ScaleBreaksConverter : IMultiValueConverter {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture) {
            string itemName = (string)values[0];
            IDashboardControlProvider provider = (IDashboardControlProvider)values[1];
            ChartDashboardItem chartItem = provider.Dashboard.Items[itemName] as ChartDashboardItem;
            bool scaleBreakEnabled = System.Convert.ToBoolean(chartItem.CustomProperties[ChartItemModule.ScaleBreakPropertyName]);
            return new AutoScaleBreaks() { Enabled = scaleBreakEnabled };
        }
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }

    public static class ScaleBreakExtensions {
        public static void CustomExport(DashboardControl dashboardControl, CustomExportEventArgs e) {
            Dictionary<string, XRControl> controls = e.GetPrintableControls();
            foreach(var control in controls) {
                XRChart xrChart = control.Value as XRChart;
                if(xrChart != null && xrChart.Diagram != null) {
                    DashboardItem chartItem = dashboardControl.Dashboard.Items[control.Key];
                    bool scaleBreakEnabled = Convert.ToBoolean(chartItem.CustomProperties[ChartItemModule.ScaleBreakPropertyName]);
                    ((DevExpress.XtraCharts.XYDiagram)xrChart.Diagram).SecondaryAxesY[0].AutoScaleBreaks.Enabled = scaleBreakEnabled;
                }
            }
        }
    }
}

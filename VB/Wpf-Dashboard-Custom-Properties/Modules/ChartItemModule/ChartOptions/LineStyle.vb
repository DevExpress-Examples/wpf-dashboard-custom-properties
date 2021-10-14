Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Linq
Imports System.Windows.Data
Imports System.Windows.Media
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWpf
Imports DevExpress.Xpf.Charts
Imports DevExpress.XtraReports.UI

Namespace Wpf_Dashboard_Custom_Properties

    Public Class LineStyleConverter
        Implements System.Windows.Data.IMultiValueConverter

        Public Function Convert(ByVal values As Object(), ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements Global.System.Windows.Data.IMultiValueConverter.Convert
            Dim dashboardItemName As String = CStr(values(0))
            Dim dashboardControlProvider As IDashboardControlProvider = CType(values(1), IDashboardControlProvider)
            Dim valueDataMember As String = CStr(values(2))
            Dim dashStyle As System.Windows.Media.DashStyle = System.Windows.Media.DashStyles.Solid
            Dim chart As ChartDashboardItem = TryCast(dashboardControlProvider.Dashboard.Items(dashboardItemName), ChartDashboardItem)
            Dim series As SimpleSeries = chart.Panes.SelectMany(Function(p) p.Series).OfType(Of SimpleSeries)().FirstOrDefault(Function(s) s.Value.UniqueId Is valueDataMember)
            If series IsNot Nothing Then
                Dim propertyName As String = series.CustomProperties(Wpf_Dashboard_Custom_Properties.ChartItemModule.DashStylePropertyName)
                If Not System.[String].IsNullOrEmpty(propertyName) Then
                    Select Case propertyName.ToLower()
                        Case "dash"
                            dashStyle = System.Windows.Media.DashStyles.Dash
                        Case "dot"
                            dashStyle = System.Windows.Media.DashStyles.Dot
                        Case "dashdot"
                            dashStyle = System.Windows.Media.DashStyles.DashDot
                        Case "dashdotdot"
                            dashStyle = System.Windows.Media.DashStyles.DashDotDot
                    End Select
                End If
            End If

            Return New LineStyle(2) With {.DashStyle = dashStyle}
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes As System.Type(), ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements Global.System.Windows.Data.IMultiValueConverter.ConvertBack
            Throw New System.NotImplementedException()
        End Function
    End Class

    Public Module LineStyleExtensions

        Public Sub CustomExport(ByVal dashboardControl As DashboardControl, ByVal e As CustomExportEventArgs)
            For Each keyValue As System.Collections.Generic.KeyValuePair(Of String, XRControl) In e.GetPrintableControls()
                If TypeOf keyValue.Value Is XRChart Then
                    Dim chartContext As ChartContext = e.GetChartContext(keyValue.Key)
                    Dim chartItem As ChartDashboardItem = TryCast(dashboardControl.Dashboard.Items(keyValue.Key), ChartDashboardItem)
                    Call Wpf_Dashboard_Custom_Properties.LineStyleExtensions.UpdateChart(chartItem, chartContext)
                End If
            Next
        End Sub

        Private Sub UpdateChart(ByVal chartItem As ChartDashboardItem, ByVal context As ChartContext)
            Dim lineSeries As System.Collections.Generic.List(Of SimpleSeries) = chartItem.Panes.SelectMany(Function(p) p.Series).OfType(Of SimpleSeries)().Where(Function(s) CBool((s.SeriesType Is SimpleSeriesType.Line))).ToList()
            For Each series As SimpleSeries In lineSeries
                Dim customPropertyValue As String = series.CustomProperties(Wpf_Dashboard_Custom_Properties.ChartItemModule.DashStylePropertyName)
                Dim dashStyle As DevExpress.XtraCharts.DashStyle
                If Not String.IsNullOrEmpty(customPropertyValue) AndAlso System.[Enum].TryParse(customPropertyValue, True, dashStyle) Then
                    For Each chartSeries In context.GetControlSeries(series)
                        Dim view As DevExpress.XtraCharts.LineSeriesView = TryCast(chartSeries.View, DevExpress.XtraCharts.LineSeriesView)
                        If view IsNot Nothing Then view.LineStyle.DashStyle = dashStyle
                    Next
                End If
            Next
        End Sub
    End Module
End Namespace

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Globalization
Imports System.Windows.Data
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWpf
Imports DevExpress.Xpf.Charts
Imports DevExpress.XtraReports.UI

Namespace Wpf_Dashboard_Custom_Properties
	Public Class ScaleBreaksConverter
		Implements IMultiValueConverter
		Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
			Dim itemName As String = CStr(values(0))
			Dim provider As IDashboardControlProvider = CType(values(1), IDashboardControlProvider)
			Dim chartItem As ChartDashboardItem = TryCast(provider.Dashboard.Items(itemName), ChartDashboardItem)
			Dim scaleBreakEnabled As Boolean = System.Convert.ToBoolean(chartItem.CustomProperties(ChartItemModule.ScaleBreakPropertyName))
			Return New AutoScaleBreaks() With {.Enabled = scaleBreakEnabled}
		End Function
		Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function
	End Class

	Public NotInheritable Class ScaleBreakExtensions
		Private Sub New()
		End Sub
		Public Shared Sub CustomExport(ByVal dashboardControl As DashboardControl, ByVal e As CustomExportEventArgs)
			Dim controls As Dictionary(Of String, XRControl) = e.GetPrintableControls()
			For Each control In controls
				Dim xrChart As XRChart = TryCast(control.Value, XRChart)
				If xrChart IsNot Nothing AndAlso xrChart.Diagram IsNot Nothing Then
					Dim chartItem As DashboardItem = dashboardControl.Dashboard.Items(control.Key)
					Dim scaleBreakEnabled As Boolean = Convert.ToBoolean(chartItem.CustomProperties(ChartItemModule.ScaleBreakPropertyName))
					CType(xrChart.Diagram, DevExpress.XtraCharts.XYDiagram).SecondaryAxesY(0).AutoScaleBreaks.Enabled = scaleBreakEnabled
				End If
			Next control
		End Sub
	End Class
End Namespace

Imports Microsoft.VisualBasic
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
	Public Class ConstantLineValueModuleData
		Public Shared Function GetDataFromString(ByVal customPropertyValue As String) As ConstantLineValueModuleData
			If (Not String.IsNullOrEmpty(customPropertyValue)) Then
				Dim array = customPropertyValue.Split("_"c)
				Return New ConstantLineValueModuleData() With {.PaneName = array(0), .IsSecondaryAxis = Boolean.Parse(array(1)), .Value = Convert.ToDouble(array(2))}
			End If
			Return New ConstantLineValueModuleData()
		End Function

		Private privatePaneName As String
		Public Property PaneName() As String
			Get
				Return privatePaneName
			End Get
			Set(ByVal value As String)
				privatePaneName = value
			End Set
		End Property
		Private privateIsSecondaryAxis As Boolean
		Public Property IsSecondaryAxis() As Boolean
			Get
				Return privateIsSecondaryAxis
			End Get
			Set(ByVal value As Boolean)
				privateIsSecondaryAxis = value
			End Set
		End Property
		Private privateValue As Double
		Public Property Value() As Double
			Get
				Return privateValue
			End Get
			Set(ByVal value As Double)
				privateValue = value
			End Set
		End Property

		Public Function GetStringFromData() As String
			Return PaneName & "_" & IsSecondaryAxis.ToString() & "_" & Value
		End Function
	End Class

	Public Class ConstantLinesConverter
		Implements IMultiValueConverter
		Public Function Convert(ByVal values() As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
			Dim itemName As String = CStr(values(0))
			Dim provider As IDashboardControlProvider = CType(values(1), IDashboardControlProvider)
			Dim chartItem As ChartDashboardItem = TryCast(provider.Dashboard.Items(itemName), ChartDashboardItem)
			Dim propertyValue As String = chartItem.CustomProperties(ChartItemModule.ConstantLinePropertyName)
			Dim moduleData = ConstantLineValueModuleData.GetDataFromString(propertyValue)
			Dim pane = chartItem.Panes.FirstOrDefault(Function(x) x.Name = moduleData.PaneName)
			Dim constantLines As New ConstantLineCollection()
			If pane IsNot Nothing Then
				Dim dashboardSeries As ChartSeries = pane.Series.FirstOrDefault(Function(s) s.PlotOnSecondaryAxis = moduleData.IsSecondaryAxis)
				If dashboardSeries IsNot Nothing Then
					Dim line As New ConstantLine()
					line.Value = moduleData.Value
					Dim constantLineColor As Color = Color.FromArgb(255, 0, 128, 0)
					line.Brush = New SolidColorBrush(constantLineColor)
					line.LineStyle = New LineStyle()
					line.LineStyle.Thickness = 2
					line.LineStyle.DashStyle = DashStyles.Dash
					line.Title = New ConstantLineTitle()
					line.Title.Content = "Value: " & moduleData.Value
					line.Title.Foreground = New SolidColorBrush(constantLineColor)
					constantLines.Add(line)
				End If
			End If
			Return constantLines
		End Function
		Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
			Throw New NotImplementedException()
		End Function
	End Class

	Public NotInheritable Class ConstantLineExtensions
		Private Sub New()
		End Sub
		Public Shared Sub CustomExport(ByVal dashboardControl As DashboardControl, ByVal e As CustomExportEventArgs)
			Dim controls As Dictionary(Of String, XRControl) = e.GetPrintableControls()
			For Each control In controls
				Dim xrChart As XRChart = TryCast(control.Value, XRChart)
				If xrChart IsNot Nothing Then
					Dim itemComponentName As String = control.Key
					Dim chart As ChartDashboardItem = TryCast(dashboardControl.Dashboard.Items(itemComponentName), ChartDashboardItem)
					Dim chartContext As ChartContext = e.GetChartContext(itemComponentName)
					UpdateChart(chart, chartContext)
				End If
			Next control
		End Sub
		Private Shared Sub UpdateChart(ByVal chartDashboardItem As ChartDashboardItem, ByVal chartContext As ChartContext)
			Dim propertyValue As String = chartDashboardItem.CustomProperties(ChartItemModule.ConstantLinePropertyName)
			Dim moduleData = ConstantLineValueModuleData.GetDataFromString(propertyValue)
			Dim pane = chartDashboardItem.Panes.FirstOrDefault(Function(x) x.Name = moduleData.PaneName)
			If pane IsNot Nothing Then
				Dim dashboardSeries As ChartSeries = pane.Series.FirstOrDefault(Function(s) s.PlotOnSecondaryAxis = moduleData.IsSecondaryAxis)
				If dashboardSeries IsNot Nothing Then
					Dim chartSeries As DevExpress.XtraCharts.Series = chartContext.GetControlSeries(dashboardSeries).FirstOrDefault()
					Dim chartAxis = (TryCast(chartSeries.View, DevExpress.XtraCharts.XYDiagramSeriesViewBase))?.AxisY
					If chartAxis IsNot Nothing Then
						Dim line As New DevExpress.XtraCharts.ConstantLine() With {.AxisValue = moduleData.Value}
						chartAxis.ConstantLines.Clear()
						chartAxis.ConstantLines.Add(line)
						line.ShowInLegend = False
						line.Color = System.Drawing.Color.Green
						line.LineStyle.Thickness = 2
						line.LineStyle.DashStyle = DevExpress.XtraCharts.DashStyle.Dash
						line.Title.Text = "Value: " & moduleData.Value
						line.Title.TextColor = line.Color
					End If
				End If
			End If
		End Sub
	End Class
End Namespace

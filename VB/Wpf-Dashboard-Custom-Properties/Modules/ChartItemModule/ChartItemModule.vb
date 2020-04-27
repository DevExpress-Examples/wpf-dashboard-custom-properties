Imports System
Imports System.Windows
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWpf
Imports DevExpress.Mvvm.UI.Interactivity

Namespace Wpf_Dashboard_Custom_Properties
	Public Class ChartItemModule
		Inherits Behavior(Of DashboardControl)

		Public Const ConstantLinePropertyName As String = "ConstantLine"
		Public Const ScaleBreakPropertyName As String = "ScaleBreak"
		Public Const DashStylePropertyName As String = "DashStyle"

		Protected Overrides Sub OnAttached()
			MyBase.OnAttached()
			Dim resources As ResourceDictionary = DirectCast(Application.LoadComponent(New Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/ChartItemModule/ChartResources.xaml", UriKind.Relative)), ResourceDictionary)
			AssociatedObject.ChartItemStyle = TryCast(resources("chartStyle"), Style)
			AddHandler AssociatedObject.CustomExport, AddressOf DashboardControl_CustomExport
		End Sub
		Protected Overrides Sub OnDetaching()
			RemoveHandler AssociatedObject.CustomExport, AddressOf DashboardControl_CustomExport
			AssociatedObject.ChartItemStyle = Nothing
			MyBase.OnDetaching()
		End Sub
		Private Sub DashboardControl_CustomExport(ByVal sender As Object, ByVal e As CustomExportEventArgs)
			ConstantLineExtensions.CustomExport(AssociatedObject, e)
			ScaleBreakExtensions.CustomExport(AssociatedObject, e)
			LineStyleExtensions.CustomExport(AssociatedObject, e)
		End Sub
	End Class
End Namespace
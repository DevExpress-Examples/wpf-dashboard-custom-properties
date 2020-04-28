Imports Microsoft.VisualBasic
Imports System.Windows
Imports DevExpress.Xpf.Bars

Namespace Wpf_Dashboard_Custom_Properties
	''' <summary>
	''' Interaction logic for MainWindow.xaml
	''' </summary>
	Partial Public Class MainWindow
		Inherits Window
		Public Sub New()
			InitializeComponent()
			dashboardControl.LoadDashboard("../../Dashboard/Dashboard.xml")
		End Sub
	End Class
End Namespace

Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Input

Namespace Wpf_Dashboard_Custom_Properties
	Public Class RelayCommand
		Implements ICommand
		Private ReadOnly execute_Renamed As Action(Of Object)
		Private ReadOnly canExecute_Renamed As Func(Of Object, Boolean)

		Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
			AddHandler(ByVal value As EventHandler)
				AddHandler CommandManager.RequerySuggested, value
			End AddHandler
			RemoveHandler(ByVal value As EventHandler)
				RemoveHandler CommandManager.RequerySuggested, value
			End RemoveHandler
			RaiseEvent(ByVal sender As System.Object, ByVal e As System.EventArgs)
			End RaiseEvent
		End Event

		Public Sub New(ByVal execute As Action(Of Object), Optional ByVal canExecute As Func(Of Object, Boolean) = Nothing)
			Me.execute_Renamed = execute
			Me.canExecute_Renamed = canExecute
		End Sub
		Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
			Return canExecute_Renamed Is Nothing OrElse canExecute_Renamed(parameter)
		End Function
		Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
			execute_Renamed(parameter)
		End Sub
	End Class
End Namespace

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Threading.Tasks
Imports System.Windows.Input

Namespace Wpf_Dashboard_Custom_Properties
	Public Class RelayCommand
		Implements ICommand

'INSTANT VB NOTE: The field execute was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private ReadOnly execute_Conflict As Action(Of Object)
'INSTANT VB NOTE: The field canExecute was renamed since Visual Basic does not allow fields to have the same name as other class members:
		Private ReadOnly canExecute_Conflict As Func(Of Object, Boolean)

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
			Me.execute_Conflict = execute
			Me.canExecute_Conflict = canExecute
		End Sub
		Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
			Return canExecute_Conflict Is Nothing OrElse canExecute(parameter)
		End Function
		Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
			execute(parameter)
		End Sub
	End Class
End Namespace

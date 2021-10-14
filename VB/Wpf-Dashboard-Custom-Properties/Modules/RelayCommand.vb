Imports System
Imports System.Windows.Input

Namespace Wpf_Dashboard_Custom_Properties

    Public Class RelayCommand
        Implements ICommand

        Private ReadOnly executeField As Action(Of Object)

        Private ReadOnly canExecuteField As Func(Of Object, Boolean)

        Public Custom Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged
            AddHandler(ByVal value As EventHandler)
                AddHandler CommandManager.RequerySuggested, value
            End AddHandler

            RemoveHandler(ByVal value As EventHandler)
                RemoveHandler CommandManager.RequerySuggested, value
            End RemoveHandler

            RaiseEvent(ByVal sender As Object, ByVal e As EventArgs)
            End RaiseEvent
        End Event

        Public Sub New(ByVal execute As Action(Of Object), ByVal Optional canExecute As Func(Of Object, Boolean) = Nothing)
            executeField = execute
            canExecuteField = canExecute
        End Sub

        Public Function CanExecute(ByVal parameter As Object) As Boolean Implements ICommand.CanExecute
            Return canExecuteField Is Nothing OrElse canExecuteField(parameter)
        End Function

        Public Sub Execute(ByVal parameter As Object) Implements ICommand.Execute
            executeField(parameter)
        End Sub
    End Class
End Namespace

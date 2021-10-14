Imports System
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWpf
Imports DevExpress.Mvvm.UI.Interactivity

Namespace Wpf_Dashboard_Custom_Properties

    Public Class DashboardDescriptionModule
        Inherits Behavior(Of DashboardControl)

        Public Const DashboardDescriptionPropertyName As String = "DashboardDescription"

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            Dim resources As ResourceDictionary = CType(Application.LoadComponent(New Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/DashboardDescriptionModule/TitleResources.xaml", UriKind.Relative)), ResourceDictionary)
            AssociatedObject.TitleCustomizationsTemplate = TryCast(resources("titleCustomizationsTemplate"), DataTemplate)
        End Sub

        Protected Overrides Sub OnDetaching()
            AssociatedObject.TitleCustomizationsTemplate = Nothing
            MyBase.OnDetaching()
        End Sub
    End Class

    Public Class ShowDashboardDescriptionCommandConverter
        Implements IValueConverter

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return New RelayCommand(AddressOf ShowDescriptionCommand)
        End Function

        Private Sub ShowDescriptionCommand(ByVal value As Object)
            Dim dashboard As Dashboard = CType(value, Dashboard)
            MessageBox.Show(dashboard.CustomProperties(DashboardDescriptionModule.DashboardDescriptionPropertyName), "Dashboard Description")
        End Sub

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

    Public Class DashboardDescriptionButtonVisibleConverter
        Implements IValueConverter

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim dashboard As Dashboard = CType(value, Dashboard)
            Return Not String.IsNullOrEmpty(dashboard.CustomProperties(DashboardDescriptionModule.DashboardDescriptionPropertyName))
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

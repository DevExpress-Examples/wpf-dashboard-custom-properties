Imports System
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWpf
Imports DevExpress.Mvvm.UI.Interactivity

Namespace Wpf_Dashboard_Custom_Properties

    Public Class ItemDescriptionModule
        Inherits Behavior(Of DashboardControl)

        Public Const ItemDescriptionPropertyName As String = "Description"

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            Dim resources As ResourceDictionary = CType(Application.LoadComponent(New Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/ItemDescriptionModule/ItemResources.xaml", UriKind.Relative)), ResourceDictionary)
            AssociatedObject.DashboardItemStyle = TryCast(resources("itemStyle"), Style)
        End Sub

        Protected Overrides Sub OnDetaching()
            AssociatedObject.DashboardItemStyle = Nothing
            MyBase.OnDetaching()
        End Sub
    End Class

    Public Class ShowItemDescriptionCommandConverter
        Implements IValueConverter

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.Convert
            Return New RelayCommand(AddressOf ShowDescriptionCommand)
        End Function

        Private Sub ShowDescriptionCommand(ByVal value As Object)
            Dim dashboardItem As DashboardItem = CType(value, DashboardItem)
            MessageBox.Show(dashboardItem.CustomProperties(ItemDescriptionModule.ItemDescriptionPropertyName), "Dashboard Item Description")
        End Sub

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

    Public Class ShowItemDescriptionCommandParameterConverter
        Implements IMultiValueConverter

        Public Function Convert(ByVal values As Object(), ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim itemName As String = CStr(values(0))
            Dim provider As IDashboardControlProvider = CType(values(1), IDashboardControlProvider)
            Return provider.Dashboard.Items(itemName)
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes As Type(), ByVal parameter As Object, ByVal culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class

    Public Class ButtonVisibleConverter
        Implements IMultiValueConverter

        Public Function Convert(ByVal values As Object(), ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim itemName As String = CStr(values(0))
            Dim provider As IDashboardControlProvider = CType(values(1), IDashboardControlProvider)
            Dim dashboardItem As DashboardItem = provider.Dashboard.Items(itemName)
            Return Not String.IsNullOrEmpty(dashboardItem.CustomProperties(ItemDescriptionModule.ItemDescriptionPropertyName))
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes As Type(), ByVal parameter As Object, ByVal culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace

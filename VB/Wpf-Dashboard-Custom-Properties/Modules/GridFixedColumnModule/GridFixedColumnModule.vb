Imports System
Imports System.Globalization
Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Markup
Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardWpf
Imports DevExpress.Mvvm.UI.Interactivity
Imports DevExpress.Xpf.Grid

Namespace Wpf_Dashboard_Custom_Properties

    Public Class GridFixedColumnModule
        Inherits Behavior(Of DashboardControl)

        Public Const FixedColumnsPropertyName As String = "FixedColumns"

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            Dim resources As ResourceDictionary = CType(Application.LoadComponent(New Uri("/Wpf-Dashboard-Custom-Properties;component/Modules/GridFixedColumnModule/GridResources.xaml", UriKind.Relative)), ResourceDictionary)
            AssociatedObject.GridItemStyle = TryCast(resources("gridStyle"), Style)
        End Sub

        Protected Overrides Sub OnDetaching()
            AssociatedObject.GridItemStyle = Nothing
            MyBase.OnDetaching()
        End Sub
    End Class

    Public Class GridFixedColumnsConverter
        Inherits MarkupExtension
        Implements IMultiValueConverter

        Public Function Convert(ByVal values As Object(), ByVal targetType As Type, ByVal parameter As Object, ByVal culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim fieldName As String = values(0).ToString()
            Dim viewModel As DashboardViewModelBase = TryCast(values(1), DashboardViewModelBase)
            If viewModel IsNot Nothing Then
                Dim dashboard As Dashboard = viewModel.DashboardControlProvider.Dashboard
                Dim grid As GridDashboardItem = TryCast(dashboard.Items(viewModel.DashboardItemName), GridDashboardItem)
                Dim column As DevExpress.DashboardCommon.GridColumnBase = FindColumn(fieldName, grid.Columns)
                If column IsNot Nothing Then
                    Dim customProperty As String = column.CustomProperties.GetValue(GridFixedColumnModule.FixedColumnsPropertyName)
                    If Not String.IsNullOrEmpty(customProperty) Then
                        Return If(System.Convert.ToBoolean(column.CustomProperties.GetValue(GridFixedColumnModule.FixedColumnsPropertyName)), FixedStyle.Left, FixedStyle.None)
                    End If
                End If
            End If

            Return FixedStyle.None
        End Function

        Private Function FindColumn(ByVal fieldName As String, ByVal columns As DevExpress.DashboardCommon.GridColumnCollection) As DevExpress.DashboardCommon.GridColumnBase
            Dim dimensionColumn As GridDimensionColumn = columns.OfType(Of GridDimensionColumn)().FirstOrDefault(Function(column) column.Dimension.UniqueId Is fieldName)
            If dimensionColumn IsNot Nothing Then Return dimensionColumn
            Dim measureColumn As GridMeasureColumn = columns.OfType(Of GridMeasureColumn)().FirstOrDefault(Function(column) column.Measure.UniqueId Is fieldName)
            If measureColumn IsNot Nothing Then Return measureColumn
            Dim hyperlinkColumn As GridHyperlinkColumn = columns.OfType(Of GridHyperlinkColumn)().FirstOrDefault(Function(column) column.DisplayValue.UniqueId Is fieldName)
            If hyperlinkColumn IsNot Nothing Then Return hyperlinkColumn
            Dim deltaColumn As GridDeltaColumn = columns.OfType(Of GridDeltaColumn)().FirstOrDefault(Function(column) If(column.ActualValue, column.TargetValue).UniqueId Is fieldName)
            If deltaColumn IsNot Nothing Then Return deltaColumn
            Dim sparklineColumn As GridSparklineColumn = columns.OfType(Of GridSparklineColumn)().FirstOrDefault(Function(column) column.Measure.UniqueId Is fieldName)
            If sparklineColumn IsNot Nothing Then Return sparklineColumn
            Return Nothing
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes As Type(), ByVal parameter As Object, ByVal culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotImplementedException()
        End Function

        Public Overrides Function ProvideValue(ByVal serviceProvider As IServiceProvider) As Object
            Return Me
        End Function
    End Class
End Namespace

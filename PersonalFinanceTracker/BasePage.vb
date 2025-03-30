Imports Microsoft.Extensions.DependencyInjection
Imports System
Imports System.Web.UI

Namespace PersonalFinanceTracker
    Public MustInherit Class BasePage
        Inherits Page

        Protected ReadOnly Property PageServiceProvider As IServiceProvider
            Get
                Return Global_asax.ServiceProvider
            End Get
        End Property

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            ' Optional: Inject services common to many pages here if needed
        End Sub

        ' Helper method to resolve services
        Protected Function GetService(Of TService As Class)() As TService
            Return PageServiceProvider.GetService(Of TService)()
        End Function

    End Class
End Namespace

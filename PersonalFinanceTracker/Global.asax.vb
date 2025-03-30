Imports System.Web.Optimization
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Http
Imports System.Web
Imports System.Configuration
Imports System.Net.Http
Imports PersonalFinanceTracker.Services

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Public Shared Property ServiceProvider As IServiceProvider

    Sub Application_Start(sender As Object, e As EventArgs)
        ' Fires when the application is started
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)

        Dim services As New ServiceCollection()
        ConfigureServices(services)
        ServiceProvider = services.BuildServiceProvider()
    End Sub

    Sub ConfigureServices(services As IServiceCollection)
        ' Read Base URL from Web.config
        Dim apiBaseUrl = ConfigurationManager.AppSettings("ApiBaseUrl")
        If String.IsNullOrEmpty(apiBaseUrl) Then
            Throw New ConfigurationErrorsException("ApiBaseUrl is not configured in Web.config appSettings.")
        End If

        ' Register IUserService with a Typed HttpClient
        services.AddHttpClient(Of IUserService, UserService)(Sub(client)
                                                                 client.BaseAddress = New Uri(apiBaseUrl)
                                                                 ' You can add default headers here if needed, e.g.:
                                                                 ' client.DefaultRequestHeaders.Add("Accept", "application/json")
                                                             End Sub)

        ' TODO: Register other services here
    End Sub

End Class
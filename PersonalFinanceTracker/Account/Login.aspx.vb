Imports Microsoft.Extensions.DependencyInjection
Imports PersonalFinanceTracker.Services
Imports PersonalFinanceTracker.Models
Imports System
Imports System.Threading.Tasks
Imports System.Web.UI
Imports System.Web.Security
Imports PersonalFinanceTracker.PersonalFinanceTracker ' Added for FormsAuthentication

Namespace Account
    Public Class Login
        Inherits BasePage ' Inherit from BasePage

        Private _userService As IUserService

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            ' Resolve the IUserService using the helper method from BasePage
            _userService = GetService(Of IUserService)()
            If _userService Is Nothing Then
                Throw New InvalidOperationException("Unable to resolve IUserService.")
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            RegisterHyperLink.NavigateUrl = "Register"
            Dim returnUrl As String = HttpContext.Current.Request.QueryString("ReturnUrl")
            If Not String.IsNullOrEmpty(returnUrl) Then
                RegisterHyperLink.NavigateUrl += "?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl)
            End If
        End Sub


        Protected Sub LogIn_Click(sender As Object, e As EventArgs)
            RegisterAsyncTask(New PageAsyncTask(AddressOf AuthenticateUserAsync))
        End Sub

        Private Async Function AuthenticateUserAsync() As Task
            ErrorMessage.Visible = False ' Hide previous errors
            If Not Page.IsValid Then
                Return
            End If

            Dim usernameValue = Username.Text
            Dim passwordValue = Password.Text

            Try
                Dim result As AuthenticationResult = Await _userService.AuthenticateUserAsync(usernameValue, passwordValue)

                If result.IsSuccess Then
                    ' Authentication successful
                    ' WARNING: Storing user data directly in session is simple but has limitations.
                    ' Consider using ASP.NET Identity or a more robust session management strategy for production.
                    Session("UserLoggedIn") = True
                    Session("Username") = usernameValue
                    ' Session("UserData") = result.UserData ' Optionally store more user data

                    ' Use FormsAuthentication to set the auth cookie
                    FormsAuthentication.SetAuthCookie(usernameValue, False) ' Set False for session cookie

                    ' Redirect to return URL or default page
                    Dim returnUrl = Request.QueryString("ReturnUrl")
                    If Not String.IsNullOrEmpty(returnUrl) AndAlso IsLocalUrl(returnUrl) Then
                        Response.Redirect(returnUrl)
                    Else
                        Response.Redirect("~/Default.aspx")
                    End If
                Else
                    ' Authentication failed
                    ErrorMessage.Text = result.ErrorMessage
                    ErrorMessage.Visible = True
                End If
            Catch ex As Exception
                ' Handle unexpected exceptions
                ErrorMessage.Text = $"An unexpected error occurred: {ex.Message}"
                ErrorMessage.Visible = True
                ' Log the full exception details
            End Try
        End Function

        ' Helper to prevent open redirect attacks
        Private Function IsLocalUrl(url As String) As Boolean
            Return Not String.IsNullOrEmpty(url) AndAlso
                   (url.StartsWith("/") AndAlso Not url.StartsWith("//") AndAlso Not url.StartsWith("/\\")) OrElse
                   (url.StartsWith("~") AndAlso url.Length > 1 AndAlso url(1) = "/")
        End Function

    End Class
End Namespace
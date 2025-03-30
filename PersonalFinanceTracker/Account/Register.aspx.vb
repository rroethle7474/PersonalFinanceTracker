Imports Microsoft.Extensions.DependencyInjection
Imports PersonalFinanceTracker.PersonalFinanceTracker
Imports PersonalFinanceTracker.Services
Imports System
Imports System.Threading.Tasks
Imports System.Web.UI

Namespace Account
    Public Class Register
        Inherits BasePage ' Inherit from BasePage

        Private _userService As IUserService

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            ' Resolve the IUserService using the helper method from BasePage
            _userService = GetService(Of IUserService)()
            If _userService Is Nothing Then
                ' Handle error: Service not resolved
                Throw New InvalidOperationException("Unable to resolve IUserService.")
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            RegisterAsyncTask(New PageAsyncTask(AddressOf LoadDataAsync))
        End Sub

        Private Async Function LoadDataAsync() As Task
            ' If there's any async loading needed on page load, do it here
            Await Task.CompletedTask ' Placeholder
        End Function


        Protected Sub Register_Click(sender As Object, e As EventArgs)
            RegisterAsyncTask(New PageAsyncTask(AddressOf RegisterUserAsync))
        End Sub

        Private Async Function RegisterUserAsync() As Task
            ErrorMessage.Visible = False
            If Not Page.IsValid Then
                Return
            End If

            Dim usernameValue = Username.Text
            Dim emailValue = Email.Text
            Dim passwordValue = Password.Text

            Try
                Dim success As Boolean = Await _userService.RegisterUserAsync(usernameValue, passwordValue, emailValue)

                If success Then
                    ' Registration successful
                    ' TODO: Redirect to a confirmation page or login page
                    ' For now, display a success message
                    ErrorMessage.CssClass = "alert alert-success"
                    ErrorMessage.Text = "Registration successful! You can now log in."
                    ErrorMessage.Visible = True

                    ' Optionally clear the form
                    Username.Text = String.Empty
                    Email.Text = String.Empty
                    ' Password fields are usually not cleared automatically

                Else
                    ' Registration failed (e.g., username/email conflict, API error)
                    ErrorMessage.CssClass = "alert alert-danger"
                    ErrorMessage.Text = "Registration failed. The username or email might already be taken, or an error occurred."
                    ErrorMessage.Visible = True
                End If
            Catch ex As Exception
                ' Handle unexpected exceptions during the registration process
                ErrorMessage.CssClass = "alert alert-danger"
                ErrorMessage.Text = $"An unexpected error occurred: {ex.Message}"
                ErrorMessage.Visible = True
                ' Log the full exception details (ex.ToString())
            End Try

        End Function

    End Class
End Namespace

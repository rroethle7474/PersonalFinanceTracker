Imports System.Threading.Tasks
Imports PersonalFinanceTracker.Models 

Namespace Services
    Public Interface IUserService
        Function RegisterUserAsync(username As String, password As String, email As String, firstName As String, lastName As String) As Task(Of Boolean)

        ' Add method for authentication
        Function AuthenticateUserAsync(username As String, password As String) As Task(Of AuthenticationResult)
    End Interface
End Namespace

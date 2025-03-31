Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Text
Imports PersonalFinanceTracker.Models

Namespace Services
    Public Class UserService
        Implements IUserService

        Private ReadOnly _httpClient As HttpClient

        Public Sub New(httpClient As HttpClient)
            _httpClient = httpClient
        End Sub

        Public Async Function RegisterUserAsync(username As String, password As String, email As String, firstName As String, lastName As String) As Task(Of Boolean) Implements IUserService.RegisterUserAsync
            Const registrationRoute As String = "users/register"

            Dim userData = New With {
                .Username = username,
                .Email = email,
                .PasswordHash = password,
                .FirstName = firstName,
                .LastName = lastName
            }

            Dim jsonContent = New StringContent(JsonConvert.SerializeObject(userData), Encoding.UTF8, "application/json")

            Try
                Dim response = Await _httpClient.PostAsync(registrationRoute, jsonContent)

                If response.StatusCode = System.Net.HttpStatusCode.Conflict Then
                    Dim errorContent = Await response.Content.ReadAsStringAsync()
                    Console.WriteLine($"API Registration Conflict: {errorContent}")
                    Return False
                End If

                response.EnsureSuccessStatusCode()

                Return True
            Catch ex As HttpRequestException
                Console.WriteLine($"API Registration HTTP Error: {ex.Message}")
                Return False
            Catch jsonEx As JsonException
                Console.WriteLine($"API Registration JSON Error: {jsonEx.Message}")
                Return False
            Catch ex As Exception
                Console.WriteLine($"API Registration Unexpected Error: {ex.Message}")
                Return False
            End Try
        End Function

        Public Async Function AuthenticateUserAsync(username As String, password As String) As Task(Of AuthenticationResult) Implements IUserService.AuthenticateUserAsync
            Const authenticateRoute As String = "users/authenticate"

            Dim loginRequest = New With {
                .Username = username,
                .Password = password
            }

            Dim jsonContent = New StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json")

            Try
                Dim response = Await _httpClient.PostAsync(authenticateRoute, jsonContent)

                Dim responseContent = Await response.Content.ReadAsStringAsync()

                If response.IsSuccessStatusCode Then
                    Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Object))(responseContent)

                    If apiResponse IsNot Nothing AndAlso apiResponse.Success Then
                        Return AuthenticationResult.Success(apiResponse.Data)
                    Else
                        Return AuthenticationResult.Failure(If(apiResponse IsNot Nothing AndAlso Not String.IsNullOrEmpty(apiResponse.Message), apiResponse.Message, "Authentication failed."))
                    End If
                ElseIf response.StatusCode = System.Net.HttpStatusCode.Unauthorized OrElse response.StatusCode = System.Net.HttpStatusCode.BadRequest Then
                    Dim errorResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Object))(responseContent)
                    Return AuthenticationResult.Failure(If(errorResponse IsNot Nothing AndAlso Not String.IsNullOrEmpty(errorResponse.Message), errorResponse.Message, "Invalid username or password."))
                Else
                    response.EnsureSuccessStatusCode()
                    Return AuthenticationResult.Failure("An unexpected error occurred during authentication.")
                End If

            Catch ex As HttpRequestException
                Console.WriteLine($"API Authentication HTTP Error: {ex.Message}")
                Return AuthenticationResult.Failure($"API communication error: {ex.Message}")
            Catch jsonEx As JsonException
                Console.WriteLine($"API Authentication JSON Error: {jsonEx.Message}")
                Return AuthenticationResult.Failure("Error processing API response.")
            Catch ex As Exception
                Console.WriteLine($"API Authentication Unexpected Error: {ex.Message}")
                Return AuthenticationResult.Failure($"An unexpected error occurred: {ex.Message}")
            End Try
        End Function

        Private Class ApiResponse(Of T)
            Public Property Success As Boolean
            Public Property Message As String
            Public Property Data As T
        End Class

    End Class
End Namespace

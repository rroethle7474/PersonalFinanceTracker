Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Text
Imports PersonalFinanceTracker.Models

Namespace Services
    Public Class AccountService
        Implements IAccountService

        Private ReadOnly _httpClient As HttpClient
        Private Const AccountsRoute As String = "accounts"

        Public Sub New(httpClient As HttpClient)
            _httpClient = httpClient
        End Sub

        Public Async Function GetAccountByIdAsync(accountId As Integer) As Task(Of Account) Implements IAccountService.GetAccountByIdAsync
            Try
                Dim response = Await _httpClient.GetAsync($"{AccountsRoute}/{accountId}")
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Account))(content)

                If apiResponse.Success Then
                    Return apiResponse.Data
                Else
                    Throw New Exception(apiResponse.Message)
                End If
            Catch ex As HttpRequestException
                Console.WriteLine($"API HTTP Error: {ex.Message}")
                Throw
            Catch ex As Exception
                Console.WriteLine($"API Error: {ex.Message}")
                Throw
            End Try
        End Function

        Public Async Function GetUserAccountsAsync(userId As Integer) As Task(Of IEnumerable(Of Account)) Implements IAccountService.GetUserAccountsAsync
            Try
                Dim response = Await _httpClient.GetAsync($"{AccountsRoute}/user/{userId}")
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Account()))(content)

                If apiResponse.Success Then
                    Return apiResponse.Data
                Else
                    Throw New Exception(apiResponse.Message)
                End If
            Catch ex As HttpRequestException
                Console.WriteLine($"API HTTP Error: {ex.Message}")
                Throw
            Catch ex As Exception
                Console.WriteLine($"API Error: {ex.Message}")
                Throw
            End Try
        End Function

        Public Async Function CreateAccountAsync(account As Account) As Task(Of Account) Implements IAccountService.CreateAccountAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PostAsync(AccountsRoute, jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Account))(content)

                If apiResponse.Success Then
                    Return apiResponse.Data
                Else
                    Throw New Exception(apiResponse.Message)
                End If
            Catch ex As HttpRequestException
                Console.WriteLine($"API HTTP Error: {ex.Message}")
                Throw
            Catch ex As Exception
                Console.WriteLine($"API Error: {ex.Message}")
                Throw
            End Try
        End Function

        Public Async Function UpdateAccountAsync(accountId As Integer, account As Account) As Task(Of Account) Implements IAccountService.UpdateAccountAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(account), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PutAsync($"{AccountsRoute}/{accountId}", jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Account))(content)

                If apiResponse.Success Then
                    Return apiResponse.Data
                Else
                    Throw New Exception(apiResponse.Message)
                End If
            Catch ex As HttpRequestException
                Console.WriteLine($"API HTTP Error: {ex.Message}")
                Throw
            Catch ex As Exception
                Console.WriteLine($"API Error: {ex.Message}")
                Throw
            End Try
        End Function

        Public Async Function UpdateAccountBalanceAsync(accountId As Integer, newBalance As Decimal) As Task(Of Account) Implements IAccountService.UpdateAccountBalanceAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(newBalance), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PutAsync($"{AccountsRoute}/{accountId}/balance", jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Account))(content)

                If apiResponse.Success Then
                    Return apiResponse.Data
                Else
                    Throw New Exception(apiResponse.Message)
                End If
            Catch ex As HttpRequestException
                Console.WriteLine($"API HTTP Error: {ex.Message}")
                Throw
            Catch ex As Exception
                Console.WriteLine($"API Error: {ex.Message}")
                Throw
            End Try
        End Function

        Public Async Function DeleteAccountAsync(accountId As Integer) As Task(Of Boolean) Implements IAccountService.DeleteAccountAsync
            Try
                Dim response = Await _httpClient.DeleteAsync($"{AccountsRoute}/{accountId}")
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Boolean))(content)

                If apiResponse.Success Then
                    Return apiResponse.Data
                Else
                    Throw New Exception(apiResponse.Message)
                End If
            Catch ex As HttpRequestException
                Console.WriteLine($"API HTTP Error: {ex.Message}")
                Throw
            Catch ex As Exception
                Console.WriteLine($"API Error: {ex.Message}")
                Throw
            End Try
        End Function

        ' Helper class to deserialize API responses
        Private Class ApiResponse(Of T)
            Public Property Success As Boolean
            Public Property Message As String
            Public Property Data As T
        End Class
    End Class
End Namespace

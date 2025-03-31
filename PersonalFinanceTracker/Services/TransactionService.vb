Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Text
Imports PersonalFinanceTracker.Models
Imports System.Collections.Generic

Namespace Services
    Public Class TransactionService
        Implements ITransactionService

        Private ReadOnly _httpClient As HttpClient
        Private Const TransactionsRoute As String = "transactions"

        Public Sub New(httpClient As HttpClient)
            _httpClient = httpClient
        End Sub

        Public Async Function GetTransactionByIdAsync(transactionId As Integer) As Task(Of Transaction) Implements ITransactionService.GetTransactionByIdAsync
            Try
                Dim response = Await _httpClient.GetAsync($"{TransactionsRoute}/{transactionId}")
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Transaction))(content)

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

        Public Async Function GetUserTransactionsAsync(userId As Integer, Optional startDate As DateTime? = Nothing, 
                                                     Optional endDate As DateTime? = Nothing, Optional categoryId As Integer? = Nothing, 
                                                     Optional accountId As Integer? = Nothing, Optional isIncome As Boolean? = Nothing) As Task(Of IEnumerable(Of Transaction)) Implements ITransactionService.GetUserTransactionsAsync
            Try
                Dim requestUrl = $"{TransactionsRoute}/user/{userId}"
                Dim queryParams = New List(Of String)()
                
                ' Add optional query parameters if provided
                If startDate.HasValue Then
                    queryParams.Add($"startDate={Uri.EscapeDataString(startDate.Value.ToString("o"))}")
                End If
                
                If endDate.HasValue Then
                    queryParams.Add($"endDate={Uri.EscapeDataString(endDate.Value.ToString("o"))}")
                End If
                
                If categoryId.HasValue Then
                    queryParams.Add($"categoryId={categoryId.Value}")
                End If
                
                If accountId.HasValue Then
                    queryParams.Add($"accountId={accountId.Value}")
                End If
                
                If isIncome.HasValue Then
                    queryParams.Add($"isIncome={isIncome.Value.ToString().ToLower()}")
                End If
                
                ' Append query parameters to the URL if any exist
                If queryParams.Count > 0 Then
                    requestUrl += $"?{String.Join("&", queryParams)}"
                End If
                
                Dim response = Await _httpClient.GetAsync(requestUrl)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Transaction()))(content)

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

        Public Async Function CreateTransactionAsync(transaction As Transaction) As Task(Of Transaction) Implements ITransactionService.CreateTransactionAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PostAsync(TransactionsRoute, jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Transaction))(content)

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

        Public Async Function UpdateTransactionAsync(transactionId As Integer, transaction As Transaction) As Task(Of Transaction) Implements ITransactionService.UpdateTransactionAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(transaction), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PutAsync($"{TransactionsRoute}/{transactionId}", jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of Transaction))(content)

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

        Public Async Function DeleteTransactionAsync(transactionId As Integer) As Task(Of Boolean) Implements ITransactionService.DeleteTransactionAsync
            Try
                Dim response = Await _httpClient.DeleteAsync($"{TransactionsRoute}/{transactionId}")
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

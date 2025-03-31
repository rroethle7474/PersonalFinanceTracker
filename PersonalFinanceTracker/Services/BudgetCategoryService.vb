Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Text
Imports PersonalFinanceTracker.Models

Namespace Services
    Public Class BudgetCategoryService
        Implements IBudgetCategoryService

        Private ReadOnly _httpClient As HttpClient
        Private Const CategoriesRoute As String = "categories"

        Public Sub New(httpClient As HttpClient)
            _httpClient = httpClient
        End Sub

        Public Async Function GetCategoryByIdAsync(categoryId As Integer) As Task(Of BudgetCategory) Implements IBudgetCategoryService.GetCategoryByIdAsync
            Try
                Dim response = Await _httpClient.GetAsync($"{CategoriesRoute}/{categoryId}")
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of BudgetCategory))(content)

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

        Public Async Function GetUserCategoriesAsync(userId As Integer, Optional categoryType As String = Nothing) As Task(Of IEnumerable(Of BudgetCategory)) Implements IBudgetCategoryService.GetUserCategoriesAsync
            Try
                Dim requestUrl = $"{CategoriesRoute}/user/{userId}"
                
                ' Add category type as a query parameter if provided
                If Not String.IsNullOrEmpty(categoryType) Then
                    requestUrl += $"?categoryType={Uri.EscapeDataString(categoryType)}"
                End If
                
                Dim response = Await _httpClient.GetAsync(requestUrl)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of BudgetCategory()))(content)

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

        Public Async Function CreateCategoryAsync(category As BudgetCategory) As Task(Of BudgetCategory) Implements IBudgetCategoryService.CreateCategoryAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PostAsync(CategoriesRoute, jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of BudgetCategory))(content)

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

        Public Async Function UpdateCategoryAsync(categoryId As Integer, category As BudgetCategory) As Task(Of BudgetCategory) Implements IBudgetCategoryService.UpdateCategoryAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PutAsync($"{CategoriesRoute}/{categoryId}", jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of BudgetCategory))(content)

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

        Public Async Function DeleteCategoryAsync(categoryId As Integer) As Task(Of Boolean) Implements IBudgetCategoryService.DeleteCategoryAsync
            Try
                Dim response = Await _httpClient.DeleteAsync($"{CategoriesRoute}/{categoryId}")
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

        Public Async Function SyncCategoryFromSalesforceAsync(category As BudgetCategory) As Task(Of Boolean) Implements IBudgetCategoryService.SyncCategoryFromSalesforceAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PostAsync($"{CategoriesRoute}/sync", jsonContent)
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

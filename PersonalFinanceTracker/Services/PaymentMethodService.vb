Imports System.Net.Http
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports System.Text
Imports PersonalFinanceTracker.Models

Namespace Services
    Public Class PaymentMethodService
        Implements IPaymentMethodService

        Private ReadOnly _httpClient As HttpClient
        Private Const PaymentMethodsRoute As String = "payment-methods"

        Public Sub New(httpClient As HttpClient)
            _httpClient = httpClient
        End Sub

        Public Async Function GetUserPaymentMethodsAsync(userId As Integer) As Task(Of IEnumerable(Of PaymentMethod)) Implements IPaymentMethodService.GetUserPaymentMethodsAsync
            Try
                Dim response = Await _httpClient.GetAsync($"{PaymentMethodsRoute}/user/{userId}")
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of PaymentMethod()))(content)

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

        Public Async Function GetPaymentMethodByIdAsync(paymentMethodId As Integer) As Task(Of PaymentMethod) Implements IPaymentMethodService.GetPaymentMethodByIdAsync
            Try
                Dim response = Await _httpClient.GetAsync($"{PaymentMethodsRoute}/{paymentMethodId}")
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of PaymentMethod))(content)

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

        Public Async Function CreatePaymentMethodAsync(paymentMethod As PaymentMethod) As Task(Of PaymentMethod) Implements IPaymentMethodService.CreatePaymentMethodAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(paymentMethod), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PostAsync(PaymentMethodsRoute, jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of PaymentMethod))(content)

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

        Public Async Function UpdatePaymentMethodAsync(paymentMethodId As Integer, paymentMethod As PaymentMethod) As Task(Of PaymentMethod) Implements IPaymentMethodService.UpdatePaymentMethodAsync
            Try
                Dim jsonContent = New StringContent(JsonConvert.SerializeObject(paymentMethod), Encoding.UTF8, "application/json")
                Dim response = Await _httpClient.PutAsync($"{PaymentMethodsRoute}/{paymentMethodId}", jsonContent)
                response.EnsureSuccessStatusCode()

                Dim content = Await response.Content.ReadAsStringAsync()
                Dim apiResponse = JsonConvert.DeserializeObject(Of ApiResponse(Of PaymentMethod))(content)

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

        Public Async Function DeletePaymentMethodAsync(paymentMethodId As Integer) As Task(Of Boolean) Implements IPaymentMethodService.DeletePaymentMethodAsync
            Try
                Dim response = Await _httpClient.DeleteAsync($"{PaymentMethodsRoute}/{paymentMethodId}")
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

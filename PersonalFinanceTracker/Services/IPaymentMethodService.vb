Imports System.Threading.Tasks
Imports PersonalFinanceTracker.Models 

Namespace Services
    Public Interface IPaymentMethodService
        Function GetUserPaymentMethodsAsync(userId As Integer) As Task(Of IEnumerable(Of PaymentMethod))
        Function GetPaymentMethodByIdAsync(paymentMethodId As Integer) As Task(Of PaymentMethod)
        Function CreatePaymentMethodAsync(paymentMethod As PaymentMethod) As Task(Of PaymentMethod)
        Function UpdatePaymentMethodAsync(paymentMethodId As Integer, paymentMethod As PaymentMethod) As Task(Of PaymentMethod)
        Function DeletePaymentMethodAsync(paymentMethodId As Integer) As Task(Of Boolean)
    End Interface
End Namespace

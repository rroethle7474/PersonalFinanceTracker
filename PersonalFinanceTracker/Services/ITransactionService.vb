Imports System.Threading.Tasks
Imports PersonalFinanceTracker.Models 

Namespace Services
    Public Interface ITransactionService
        Function GetTransactionByIdAsync(transactionId As Integer) As Task(Of Transaction)
        Function GetUserTransactionsAsync(userId As Integer, Optional startDate As DateTime? = Nothing, 
                                         Optional endDate As DateTime? = Nothing, Optional categoryId As Integer? = Nothing, 
                                         Optional accountId As Integer? = Nothing, Optional isIncome As Boolean? = Nothing) As Task(Of IEnumerable(Of Transaction))
        Function CreateTransactionAsync(transaction As Transaction) As Task(Of Transaction)
        Function UpdateTransactionAsync(transactionId As Integer, transaction As Transaction) As Task(Of Transaction)
        Function DeleteTransactionAsync(transactionId As Integer) As Task(Of Boolean)
    End Interface
End Namespace

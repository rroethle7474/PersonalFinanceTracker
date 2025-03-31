Imports System.Threading.Tasks
Imports PersonalFinanceTracker.Models

Namespace Services
    Public Interface IAccountService
        Function GetAccountByIdAsync(accountId As Integer) As Task(Of Account)
        Function GetUserAccountsAsync(userId As Integer) As Task(Of IEnumerable(Of Account))
        Function CreateAccountAsync(account As Account) As Task(Of Account)
        Function UpdateAccountAsync(accountId As Integer, account As Account) As Task(Of Account)
        Function UpdateAccountBalanceAsync(accountId As Integer, newBalance As Decimal) As Task(Of Account)
        Function DeleteAccountAsync(accountId As Integer) As Task(Of Boolean)
    End Interface
End Namespace

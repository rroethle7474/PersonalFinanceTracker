Imports System.Threading.Tasks
Imports PersonalFinanceTracker.Models 

Namespace Services
    Public Interface IBudgetCategoryService
        Function GetCategoryByIdAsync(categoryId As Integer) As Task(Of BudgetCategory)
        Function GetUserCategoriesAsync(userId As Integer, Optional categoryType As String = Nothing) As Task(Of IEnumerable(Of BudgetCategory))
        Function CreateCategoryAsync(category As BudgetCategory) As Task(Of BudgetCategory)
        Function UpdateCategoryAsync(categoryId As Integer, category As BudgetCategory) As Task(Of BudgetCategory)
        Function DeleteCategoryAsync(categoryId As Integer) As Task(Of Boolean)
        Function SyncCategoryFromSalesforceAsync(category As BudgetCategory) As Task(Of Boolean)
    End Interface
End Namespace

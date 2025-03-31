Imports Microsoft.Extensions.DependencyInjection
Imports Newtonsoft.Json
Imports PersonalFinanceTracker.Models
Imports PersonalFinanceTracker.PersonalFinanceTracker
Imports PersonalFinanceTracker.Services
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Threading.Tasks
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace CustomPages
    Public Class BudgetCategories
        Inherits BasePage

        Private _budgetCategoryService As IBudgetCategoryService

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            ' Resolve the IBudgetCategoryService using the helper method from BasePage
            _budgetCategoryService = GetService(Of IBudgetCategoryService)()
            If _budgetCategoryService Is Nothing Then
                ' Handle error: Service not resolved
                Throw New InvalidOperationException("Unable to resolve IBudgetCategoryService.")
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' Check if user is logged in
            If Session("UserLoggedIn") IsNot Nothing AndAlso DirectCast(Session("UserLoggedIn"), Boolean) = True Then
                ' User is logged in, show authenticated content
                AuthenticatedContent.Visible = True
                UnauthenticatedContent.Visible = False

                If Not IsPostBack Then
                    RegisterAsyncTask(New PageAsyncTask(AddressOf LoadBudgetCategoriesAsync))
                End If
            Else
                ' User is not logged in, show unauthenticated content
                AuthenticatedContent.Visible = False
                UnauthenticatedContent.Visible = True
            End If
        End Sub

        Private Async Function LoadBudgetCategoriesAsync() As Task
            Try
                ' Get the user ID from session
                If Session("UserId") Is Nothing Then
                    ShowError("User ID not found in session.")
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(Session("UserId"))
                Dim budgetCategories = Await _budgetCategoryService.GetUserCategoriesAsync(userId)
                
                gvBudgetCategories.DataSource = budgetCategories
                gvBudgetCategories.DataBind()
            Catch ex As Exception
                ShowError($"An error occurred: {ex.Message}")
            End Try
        End Function

        Protected Sub btnAddNew_Click(sender As Object, e As EventArgs)
            ' Clear form fields
            hdnCategoryID.Value = String.Empty
            txtCategoryName.Text = String.Empty
            ddlCategoryType.SelectedIndex = 0
            txtDescription.Text = String.Empty
            txtMonthlyAllocation.Text = String.Empty
            chkIsActive.Checked = True

            ' Set edit mode
            litEditMode.Text = "Add New Budget Category"
            pnlList.Visible = False
            pnlEdit.Visible = True
        End Sub

        Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
            pnlEdit.Visible = False
            pnlList.Visible = True
        End Sub

        Protected Async Sub btnSave_Click(sender As Object, e As EventArgs)
            If Not Page.IsValid Then
                Return
            End If

            Try
                ' Get the user ID from session
                If Session("UserId") Is Nothing Then
                    ShowError("User ID not found in session.")
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(Session("UserId"))
                Dim budgetCategory As New BudgetCategory() With {
                    .UserID = userId,
                    .CategoryName = txtCategoryName.Text.Trim(),
                    .CategoryType = ddlCategoryType.SelectedValue,
                    .Description = txtDescription.Text.Trim(),
                    .IsActive = chkIsActive.Checked
                }

                ' Parse monthly allocation if provided
                If Not String.IsNullOrWhiteSpace(txtMonthlyAllocation.Text) Then
                    budgetCategory.MonthlyAllocation = Decimal.Parse(txtMonthlyAllocation.Text)
                End If

                Dim isNewRecord As Boolean = String.IsNullOrEmpty(hdnCategoryID.Value)

                If isNewRecord Then
                    ' Create new budget category
                    Await _budgetCategoryService.CreateCategoryAsync(budgetCategory)
                    ShowMessage("Budget category created successfully.")
                Else
                    ' Update existing budget category
                    budgetCategory.CategoryID = Integer.Parse(hdnCategoryID.Value)
                    Await _budgetCategoryService.UpdateCategoryAsync(budgetCategory.CategoryID, budgetCategory)
                    ShowMessage("Budget category updated successfully.")
                End If

                ' Refresh the list and return to list view
                Await LoadBudgetCategoriesAsync()
                pnlEdit.Visible = False
                pnlList.Visible = True
            Catch ex As Exception
                ShowError($"An error occurred: {ex.Message}")
            End Try
        End Sub

        Protected Async Sub gvBudgetCategories_RowCommand(sender As Object, e As GridViewCommandEventArgs)
            Try
                Dim categoryId As Integer = Convert.ToInt32(e.CommandArgument)

                If e.CommandName = "EditItem" Then
                    ' Load budget category details for editing
                    Dim budgetCategory = Await _budgetCategoryService.GetCategoryByIdAsync(categoryId)

                    ' Populate form fields
                    hdnCategoryID.Value = budgetCategory.CategoryID.ToString()
                    txtCategoryName.Text = budgetCategory.CategoryName
                    ddlCategoryType.SelectedValue = budgetCategory.CategoryType
                    txtDescription.Text = budgetCategory.Description
                    txtMonthlyAllocation.Text = If(budgetCategory.MonthlyAllocation.HasValue, budgetCategory.MonthlyAllocation.Value.ToString(), String.Empty)
                    chkIsActive.Checked = budgetCategory.IsActive

                    ' Set edit mode
                    litEditMode.Text = "Edit Budget Category"
                    pnlList.Visible = False
                    pnlEdit.Visible = True
                ElseIf e.CommandName = "DeleteItem" Then
                    ' Delete budget category
                    Dim success = Await _budgetCategoryService.DeleteCategoryAsync(categoryId)
                    If success Then
                        ShowMessage("Budget category deleted successfully.")
                        Await LoadBudgetCategoriesAsync()
                    Else
                        ShowError("Failed to delete budget category.")
                    End If
                End If
            Catch ex As Exception
                ShowError($"An error occurred: {ex.Message}")
            End Try
        End Sub

        Private Sub ShowMessage(message As String)
            lblMessage.Text = message
            lblMessage.Visible = True
            lblError.Visible = False
        End Sub

        Private Sub ShowError(errorMessage As String)
            lblError.Text = errorMessage
            lblError.Visible = True
            lblMessage.Visible = False
        End Sub
    End Class
End Namespace
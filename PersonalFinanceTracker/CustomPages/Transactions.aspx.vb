Imports Microsoft.Extensions.DependencyInjection
Imports System.Web.UI.WebControls
Imports PersonalFinanceTracker.Models
Imports PersonalFinanceTracker.Services
Imports System.Threading.Tasks
Imports PersonalFinanceTracker.PersonalFinanceTracker

Namespace CustomPages
    Public Class Transactions
        Inherits BasePage

        Private _transactionService As ITransactionService
        Private _budgetCategoryService As IBudgetCategoryService
        Private _accountService As IAccountService

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            ' Resolve services using the helper method from BasePage
            _transactionService = GetService(Of ITransactionService)()
            _budgetCategoryService = GetService(Of IBudgetCategoryService)()
            _accountService = GetService(Of IAccountService)()

            If _transactionService Is Nothing Then
                Throw New InvalidOperationException("Unable to resolve ITransactionService.")
            End If

            If _budgetCategoryService Is Nothing Then
                Throw New InvalidOperationException("Unable to resolve IBudgetCategoryService.")
            End If

            If _accountService Is Nothing Then
                Throw New InvalidOperationException("Unable to resolve IAccountService.")
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' Check if user is logged in
            If Session("UserLoggedIn") IsNot Nothing AndAlso DirectCast(Session("UserLoggedIn"), Boolean) = True Then
                ' User is logged in, show authenticated content
                AuthenticatedContent.Visible = True
                UnauthenticatedContent.Visible = False

                If Not IsPostBack Then
                    RegisterAsyncTask(New PageAsyncTask(AddressOf LoadInitialDataAsync))
                End If
            Else
                ' User is not logged in, show unauthenticated content
                AuthenticatedContent.Visible = False
                UnauthenticatedContent.Visible = True
            End If
        End Sub

        Private Async Function LoadInitialDataAsync() As Task
            Try
                Await LoadCategoriesAsync()
                Await LoadTransactionsAsync()
            Catch ex As Exception
                ShowError("Error loading data: " & ex.Message)
            End Try
        End Function

        Private Async Function LoadCategoriesAsync() As Task
            Try
                ' Get the user ID from session
                If Session("UserId") Is Nothing Then
                    ShowError("User ID not found in session.")
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(Session("UserId"))

                ' Load categories for dropdown lists
                Dim categories = Await _budgetCategoryService.GetUserCategoriesAsync(userId)

                ' Populate filter dropdown
                ddlCategory.Items.Clear()
                ddlCategory.Items.Add(New ListItem("All Categories", ""))
                For Each category In categories
                    ddlCategory.Items.Add(New ListItem(category.CategoryName, category.CategoryID.ToString()))
                Next

                ' Populate edit form dropdown
                ddlEditCategory.Items.Clear()
                ddlEditCategory.Items.Add(New ListItem("-- Select Category --", ""))
                For Each category In categories
                    ddlEditCategory.Items.Add(New ListItem(category.CategoryName, category.CategoryID.ToString()))
                Next
            Catch ex As Exception
                ShowError("Error loading categories: " & ex.Message)
            End Try
        End Function

        Private Async Function LoadTransactionsAsync(Optional startDate As DateTime? = Nothing, Optional endDate As DateTime? = Nothing,
                                         Optional categoryId As Integer? = Nothing, Optional isIncome As Boolean? = Nothing) As Task
            Try
                ' Get the user ID from session
                If Session("UserId") Is Nothing Then
                    ShowError("User ID not found in session.")
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(Session("UserId"))

                Dim transactions = Await _transactionService.GetUserTransactionsAsync(userId, startDate, endDate, categoryId, Nothing, isIncome)

                ' Bind transactions to grid
                gvTransactions.DataSource = transactions
                gvTransactions.DataBind()
            Catch ex As Exception
                ShowError("Error loading transactions: " & ex.Message)
            End Try
        End Function

        Protected Sub btnAddNew_Click(sender As Object, e As EventArgs)
            ' Show edit panel in Add mode
            pnlList.Visible = False
            pnlEdit.Visible = True
            litEditMode.Text = "Add New Transaction"

            ' Clear form fields
            hdnTransactionID.Value = "0"
            txtTransactionDate.Text = DateTime.Now.ToString("yyyy-MM-dd")
            txtMerchantName.Text = String.Empty
            txtDescription.Text = String.Empty
            ddlEditCategory.SelectedIndex = 0
            txtAmount.Text = String.Empty
            ddlEditTransactionType.SelectedIndex = 0
            chkIsIncome.Checked = False
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

                Dim transactionId As Integer = 0
                Integer.TryParse(hdnTransactionID.Value, transactionId)

                Dim amount As Decimal = 0
                Decimal.TryParse(txtAmount.Text, amount)

                Dim categoryId As Integer = 0
                Integer.TryParse(ddlEditCategory.SelectedValue, categoryId)

                Dim transaction As New Transaction With {
                    .TransactionID = transactionId,
                    .UserID = userId,
                    .TransactionDate = DateTime.Parse(txtTransactionDate.Text),
                    .MerchantName = txtMerchantName.Text,
                    .Description = txtDescription.Text,
                    .CategoryID = categoryId,
                    .Amount = amount,
                    .IsIncome = chkIsIncome.Checked,
                    .TransactionType = ddlEditTransactionType.SelectedValue
                }

                If transactionId > 0 Then
                    Await UpdateTransactionAsync(transactionId, transaction)
                Else
                    Await CreateTransactionAsync(transaction)
                End If
            Catch ex As Exception
                ShowError("Error saving transaction: " & ex.Message)
            End Try
        End Sub

        Private Async Function CreateTransactionAsync(transaction As Transaction) As Task
            Try
                Await _transactionService.CreateTransactionAsync(transaction)

                ' Show success message and return to list
                ShowSuccess("Transaction created successfully.")

                pnlEdit.Visible = False
                pnlList.Visible = True

                ' Reload transactions
                Await LoadTransactionsAsync()
            Catch ex As Exception
                ShowError("Error creating transaction: " & ex.Message)
            End Try
        End Function

        Private Async Function UpdateTransactionAsync(transactionId As Integer, transaction As Transaction) As Task
            Try
                Await _transactionService.UpdateTransactionAsync(transactionId, transaction)

                ' Show success message and return to list
                ShowSuccess("Transaction updated successfully.")

                pnlEdit.Visible = False
                pnlList.Visible = True

                ' Reload transactions
                Await LoadTransactionsAsync()
            Catch ex As Exception
                ShowError("Error updating transaction: " & ex.Message)
            End Try
        End Function

        Protected Sub btnCancel_Click(sender As Object, e As EventArgs)
            ' Return to list view
            pnlEdit.Visible = False
            pnlList.Visible = True
            ClearMessages()
        End Sub

        Protected Async Sub gvTransactions_RowCommand(sender As Object, e As GridViewCommandEventArgs)
            Dim transactionId As Integer = 0

            If Integer.TryParse(e.CommandArgument.ToString(), transactionId) Then
                If e.CommandName = "EditItem" Then
                    Await EditTransactionAsync(transactionId)
                ElseIf e.CommandName = "DeleteItem" Then
                    Await DeleteTransactionAsync(transactionId)
                End If
            End If
        End Sub

        Private Async Function EditTransactionAsync(transactionId As Integer) As Task
            Try
                Dim transaction = Await _transactionService.GetTransactionByIdAsync(transactionId)

                ' Populate form with transaction details
                hdnTransactionID.Value = transaction.TransactionID.ToString()
                txtTransactionDate.Text = transaction.TransactionDate.ToString("yyyy-MM-dd")
                txtMerchantName.Text = transaction.MerchantName
                txtDescription.Text = transaction.Description

                ' Set category dropdown
                If transaction.CategoryID > 0 Then
                    Dim categoryItem = ddlEditCategory.Items.FindByValue(transaction.CategoryID.ToString())
                    If categoryItem IsNot Nothing Then
                        categoryItem.Selected = True
                    End If
                End If

                txtAmount.Text = Math.Abs(transaction.Amount).ToString("F2")
                chkIsIncome.Checked = transaction.IsIncome

                ' Set transaction type dropdown
                Dim typeItem = ddlEditTransactionType.Items.FindByValue(transaction.TransactionType)
                If typeItem IsNot Nothing Then
                    typeItem.Selected = True
                End If

                ' Show edit panel
                litEditMode.Text = "Edit Transaction"
                pnlList.Visible = False
                pnlEdit.Visible = True
                ClearMessages()
            Catch ex As Exception
                ShowError("Error loading transaction details: " & ex.Message)
            End Try
        End Function

        Private Async Function DeleteTransactionAsync(transactionId As Integer) As Task
            Try
                Await _transactionService.DeleteTransactionAsync(transactionId)

                ' Show success message
                ShowSuccess("Transaction deleted successfully.")

                ' Reload transactions
                Await LoadTransactionsAsync()
            Catch ex As Exception
                ShowError("Error deleting transaction: " & ex.Message)
            End Try
        End Function

        Protected Async Sub btnApplyFilter_Click(sender As Object, e As EventArgs)
            ' Apply filters to transaction list
            Dim startDate As DateTime? = Nothing
            Dim endDate As DateTime? = Nothing
            Dim categoryId As Integer? = Nothing
            Dim isIncome As Boolean? = Nothing

            ' Parse start date
            If Not String.IsNullOrEmpty(txtStartDate.Text) Then
                startDate = DateTime.Parse(txtStartDate.Text)
            End If

            ' Parse end date
            If Not String.IsNullOrEmpty(txtEndDate.Text) Then
                endDate = DateTime.Parse(txtEndDate.Text)
            End If

            ' Parse category ID
            If Not String.IsNullOrEmpty(ddlCategory.SelectedValue) Then
                categoryId = Integer.Parse(ddlCategory.SelectedValue)
            End If

            ' Parse transaction type (income/expense)
            If ddlTransactionType.SelectedValue = "Income" Then
                isIncome = True
            ElseIf ddlTransactionType.SelectedValue = "Expense" Then
                isIncome = False
            End If

            ' Load filtered transactions
            Await LoadTransactionsAsync(startDate, endDate, categoryId, isIncome)
        End Sub

        Protected Async Sub btnClearFilter_Click(sender As Object, e As EventArgs)
            ' Clear filter controls
            txtStartDate.Text = String.Empty
            txtEndDate.Text = String.Empty
            ddlCategory.SelectedIndex = 0
            ddlTransactionType.SelectedIndex = 0

            ' Reload all transactions
            Await LoadTransactionsAsync()
        End Sub

        ' Helper methods for displaying messages
        Private Sub ShowError(message As String)
            lblError.Text = message
            lblError.Visible = True
            lblMessage.Visible = False
        End Sub

        Private Sub ShowSuccess(message As String)
            lblMessage.Text = message
            lblMessage.Visible = True
            lblError.Visible = False
        End Sub

        Private Sub ClearMessages()
            lblError.Visible = False
            lblMessage.Visible = False
        End Sub
    End Class
End Namespace
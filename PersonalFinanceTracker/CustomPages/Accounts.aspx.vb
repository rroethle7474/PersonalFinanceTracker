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
    Public Class Accounts
        Inherits BasePage

        Private _accountService As IAccountService

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            ' Resolve the IAccountService using the helper method from BasePage
            _accountService = GetService(Of IAccountService)()
            If _accountService Is Nothing Then
                ' Handle error: Service not resolved
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
                    RegisterAsyncTask(New PageAsyncTask(AddressOf LoadAccountsAsync))
                End If
            Else
                ' User is not logged in, show unauthenticated content
                AuthenticatedContent.Visible = False
                UnauthenticatedContent.Visible = True
            End If
        End Sub

        Private Async Function LoadAccountsAsync() As Task
            Try
                ' Get the user ID from session
                If Session("UserId") Is Nothing Then
                    ShowError("User ID not found in session.")
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(Session("UserId"))
                Dim accounts = Await _accountService.GetUserAccountsAsync(userId)
                
                gvAccounts.DataSource = accounts
                gvAccounts.DataBind()
            Catch ex As Exception
                ShowError($"An error occurred: {ex.Message}")
            End Try
        End Function

        Protected Sub btnAddNew_Click(sender As Object, e As EventArgs)
            ' Clear form fields
            hdnAccountID.Value = String.Empty
            txtAccountName.Text = String.Empty
            ddlAccountType.SelectedIndex = 0
            txtCurrentBalance.Text = "0.00"
            txtCurrencyCode.Text = "USD"
            ddlCategory.SelectedIndex = 0
            chkIsActive.Checked = True
            chkIsFinancialInstitution.Checked = False
            chkIsMerchant.Checked = False

            ' Set edit mode
            litEditMode.Text = "Add New Account"
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
                Dim account As New Account() With {
                    .UserID = userId,
                    .AccountName = txtAccountName.Text.Trim(),
                    .AccountType = ddlAccountType.SelectedValue,
                    .CurrentBalance = Decimal.Parse(txtCurrentBalance.Text),
                    .CurrencyCode = txtCurrencyCode.Text.Trim(),
                    .Category = ddlCategory.SelectedValue,
                    .IsActive = chkIsActive.Checked,
                    .IsFinancialInstitution = chkIsFinancialInstitution.Checked,
                    .IsMerchant = chkIsMerchant.Checked,
                    .LastUpdated = DateTime.UtcNow
                }

                Dim isNewRecord As Boolean = String.IsNullOrEmpty(hdnAccountID.Value)

                If isNewRecord Then
                    ' Create new account
                    Await _accountService.CreateAccountAsync(account)
                    ShowMessage("Account created successfully.")
                Else
                    ' Update existing account
                    account.AccountID = Integer.Parse(hdnAccountID.Value)
                    Await _accountService.UpdateAccountAsync(account.AccountID, account)
                    ShowMessage("Account updated successfully.")
                End If

                ' Refresh the list and return to list view
                Await LoadAccountsAsync()
                pnlEdit.Visible = False
                pnlList.Visible = True
            Catch ex As Exception
                ShowError($"An error occurred: {ex.Message}")
            End Try
        End Sub

        Protected Async Sub gvAccounts_RowCommand(sender As Object, e As GridViewCommandEventArgs)
            Try
                Dim accountId As Integer = Convert.ToInt32(e.CommandArgument)

                If e.CommandName = "EditItem" Then
                    ' Load account details for editing
                    Dim account = Await _accountService.GetAccountByIdAsync(accountId)

                    ' Populate form fields
                    hdnAccountID.Value = account.AccountID.ToString()
                    txtAccountName.Text = account.AccountName
                    ddlAccountType.SelectedValue = account.AccountType
                    txtCurrentBalance.Text = account.CurrentBalance.ToString()
                    txtCurrencyCode.Text = account.CurrencyCode
                    
                    ' Set category if it exists
                    If Not String.IsNullOrEmpty(account.Category) Then
                        Try
                            ddlCategory.SelectedValue = account.Category
                        Catch
                            ' If category not in dropdown, default to first item
                            ddlCategory.SelectedIndex = 0
                        End Try
                    Else
                        ddlCategory.SelectedIndex = 0
                    End If
                    
                    chkIsActive.Checked = account.IsActive
                    chkIsFinancialInstitution.Checked = account.IsFinancialInstitution
                    chkIsMerchant.Checked = account.IsMerchant

                    ' Set edit mode
                    litEditMode.Text = "Edit Account"
                    pnlList.Visible = False
                    pnlEdit.Visible = True
                ElseIf e.CommandName = "DeleteItem" Then
                    ' Delete account
                    Dim success = Await _accountService.DeleteAccountAsync(accountId)
                    If success Then
                        ShowMessage("Account deleted successfully.")
                        Await LoadAccountsAsync()
                    Else
                        ShowError("Failed to delete account.")
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
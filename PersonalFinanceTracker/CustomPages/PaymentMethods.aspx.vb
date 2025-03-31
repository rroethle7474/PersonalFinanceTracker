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
    Public Class PaymentMethods
        Inherits BasePage

        Private _paymentMethodService As IPaymentMethodService

        Protected Overrides Sub OnInit(e As EventArgs)
            MyBase.OnInit(e)
            ' Resolve the IPaymentMethodService using the helper method from BasePage
            _paymentMethodService = GetService(Of IPaymentMethodService)()
            If _paymentMethodService Is Nothing Then
                ' Handle error: Service not resolved
                Throw New InvalidOperationException("Unable to resolve IPaymentMethodService.")
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' Check if user is logged in
            If Session("UserLoggedIn") IsNot Nothing AndAlso DirectCast(Session("UserLoggedIn"), Boolean) = True Then
                ' User is logged in, show authenticated content
                AuthenticatedContent.Visible = True
                UnauthenticatedContent.Visible = False

                If Not IsPostBack Then
                    RegisterAsyncTask(New PageAsyncTask(AddressOf LoadPaymentMethodsAsync))
                End If
            Else
                ' User is not logged in, show unauthenticated content
                AuthenticatedContent.Visible = False
                UnauthenticatedContent.Visible = True
            End If
        End Sub

        Private Async Function LoadPaymentMethodsAsync() As Task
            Try
                ' Get the user ID from session
                If Session("UserId") Is Nothing Then
                    ShowError("User ID not found in session.")
                    Return
                End If

                Dim userId As Integer = Convert.ToInt32(Session("UserId"))
                Dim paymentMethods = Await _paymentMethodService.GetUserPaymentMethodsAsync(userId)
                
                gvPaymentMethods.DataSource = paymentMethods
                gvPaymentMethods.DataBind()
            Catch ex As Exception
                ShowError($"An error occurred: {ex.Message}")
            End Try
        End Function

        Protected Sub btnAddNew_Click(sender As Object, e As EventArgs)
            ' Clear form fields
            hdnPaymentMethodID.Value = String.Empty
            txtMethodName.Text = String.Empty
            ddlMethodType.SelectedIndex = 0
            txtCurrentBalance.Text = String.Empty
            chkIsActive.Checked = True

            ' Set edit mode
            litEditMode.Text = "Add New Payment Method"
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
                Dim paymentMethod As New PaymentMethod() With {
                    .UserID = userId,
                    .MethodName = txtMethodName.Text.Trim(),
                    .MethodType = ddlMethodType.SelectedValue,
                    .IsActive = chkIsActive.Checked
                }

                ' Parse current balance if provided
                If Not String.IsNullOrWhiteSpace(txtCurrentBalance.Text) Then
                    paymentMethod.CurrentBalance = Decimal.Parse(txtCurrentBalance.Text)
                End If

                Dim isNewRecord As Boolean = String.IsNullOrEmpty(hdnPaymentMethodID.Value)

                If isNewRecord Then
                    ' Create new payment method
                    Await _paymentMethodService.CreatePaymentMethodAsync(paymentMethod)
                    ShowMessage("Payment method created successfully.")
                Else
                    ' Update existing payment method
                    paymentMethod.PaymentMethodID = Integer.Parse(hdnPaymentMethodID.Value)
                    Await _paymentMethodService.UpdatePaymentMethodAsync(paymentMethod.PaymentMethodID, paymentMethod)
                    ShowMessage("Payment method updated successfully.")
                End If

                ' Refresh the list and return to list view
                Await LoadPaymentMethodsAsync()
                pnlEdit.Visible = False
                pnlList.Visible = True
            Catch ex As Exception
                ShowError($"An error occurred: {ex.Message}")
            End Try
        End Sub

        Protected Async Sub gvPaymentMethods_RowCommand(sender As Object, e As GridViewCommandEventArgs)
            Try
                Dim paymentMethodId As Integer = Convert.ToInt32(e.CommandArgument)

                If e.CommandName = "EditItem" Then
                    ' Load payment method details for editing
                    Dim paymentMethod = Await _paymentMethodService.GetPaymentMethodByIdAsync(paymentMethodId)

                    ' Populate form fields
                    hdnPaymentMethodID.Value = paymentMethod.PaymentMethodID.ToString()
                    txtMethodName.Text = paymentMethod.MethodName
                    ddlMethodType.SelectedValue = paymentMethod.MethodType
                    txtCurrentBalance.Text = If(paymentMethod.CurrentBalance.HasValue, paymentMethod.CurrentBalance.Value.ToString(), String.Empty)
                    chkIsActive.Checked = paymentMethod.IsActive

                    ' Set edit mode
                    litEditMode.Text = "Edit Payment Method"
                    pnlList.Visible = False
                    pnlEdit.Visible = True
                ElseIf e.CommandName = "DeleteItem" Then
                    ' Delete payment method
                    Dim success = Await _paymentMethodService.DeletePaymentMethodAsync(paymentMethodId)
                    If success Then
                        ShowMessage("Payment method deleted successfully.")
                        Await LoadPaymentMethodsAsync()
                    Else
                        ShowError("Failed to delete payment method.")
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

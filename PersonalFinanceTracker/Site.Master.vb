Imports System.Web.Security

Public Class SiteMaster
    Inherits MasterPage
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ' Check if user is logged in
        If Session("UserLoggedIn") IsNot Nothing AndAlso DirectCast(Session("UserLoggedIn"), Boolean) = True Then
            ' User is logged in, show authenticated UI
            AuthenticatedUI.Visible = True
            AnonymousUI.Visible = False
            AuthenticatedNav.Visible = True
            
            ' Display user's first name if available
            If Session("FirstName") IsNot Nothing Then
                UserFirstName.Text = Session("FirstName").ToString()
            Else
                UserFirstName.Text = Session("Username").ToString()
            End If
        Else
            ' User is not logged in, show anonymous UI
            AuthenticatedUI.Visible = False
            AnonymousUI.Visible = True
            AuthenticatedNav.Visible = False
        End If
    End Sub
    
    Protected Sub LogoutButton_Click(sender As Object, e As EventArgs)
        ' Clear authentication cookie
        FormsAuthentication.SignOut()
        
        ' Clear session variables
        Session.Clear()
        Session.Abandon()
        
        ' Redirect to home page
        Response.Redirect("~/")
    End Sub
End Class
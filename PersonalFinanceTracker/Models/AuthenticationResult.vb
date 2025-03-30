Namespace Models
    ' Represents the outcome of an authentication attempt
    Public Class AuthenticationResult
        Public Property IsSuccess As Boolean
        Public Property ErrorMessage As String
        Public Property UserData As Object ' Using Object for now, could be a specific UserViewModel

        Public Shared Function Success(userData As Object) As AuthenticationResult
            Return New AuthenticationResult With {
                .IsSuccess = True,
                .UserData = userData
            }
        End Function

        Public Shared Function Failure(message As String) As AuthenticationResult
            Return New AuthenticationResult With {
                .IsSuccess = False,
                .ErrorMessage = message
            }
        End Function
    End Class
End Namespace
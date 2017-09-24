
Namespace CommonLib

    ''' <summary>
    ''' Return +1 if x is more than or equal 0
    ''' return -1 otherwise
    ''' </summary>
    Public Class SignFunction
        Implements IActivationFunction

        Public Function [Function](x As Single) As Single Implements IActivationFunction.Function
            If x >= 0 Then
                Return 1
            Else
                Return -1
            End If
        End Function

        Public Function Derivative(x As Single) As Single Implements IActivationFunction.Derivative
            Return 0
        End Function
    End Class
End Namespace


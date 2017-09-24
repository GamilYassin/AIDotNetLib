
Namespace CommonLib

    ''' <summary>
    ''' Return 1 if x is more than or equal 0
    ''' return 0 otherwise
    ''' </summary>
    Public Class StepFunction
        Implements IActivationFunction

        Public Function [Function](x As Single) As Single Implements IActivationFunction.Function
            If x >= 0 Then
                Return 1
            Else
                Return 0
            End If
        End Function

        Public Function [Function](x As Single, Theta As Single) As Single
            If x >= Theta Then
                Return 1
            Else
                Return 0
            End If
        End Function

        Public Function Derivative(x As Single) As Single Implements IActivationFunction.Derivative
            Return [Function](x)
        End Function
    End Class
End Namespace


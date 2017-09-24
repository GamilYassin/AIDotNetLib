
Namespace CommonLib

    ''' <summary>
    ''' Implements f(x) = Max(0,x)
    ''' Returns x if x > 0 or return 0
    ''' </summary>
    Public Class ReluFunction
        Implements IActivationFunction

        Public Function [Function](x As Single) As Single Implements IActivationFunction.Function
            Return Math.Max(x, 0)
        End Function

        Public Function Derivative(x As Single) As Single Implements IActivationFunction.Derivative
            If x >= 0 Then Return 1
            Return 0
        End Function
    End Class
End Namespace


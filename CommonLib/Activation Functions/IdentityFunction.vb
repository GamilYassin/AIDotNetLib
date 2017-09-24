
Namespace CommonLib

    ''' <summary>
    ''' Always returns the same value that was used as its argument
    ''' f(x) = x
    ''' </summary>
    Public Class IdentityFunction
        Implements IActivationFunction

        Public Function [Function](x As Single) As Single Implements IActivationFunction.Function
            Return x
        End Function

        Public Function Derivative(x As Single) As Single Implements IActivationFunction.Derivative
            Return 1
        End Function
    End Class
End Namespace

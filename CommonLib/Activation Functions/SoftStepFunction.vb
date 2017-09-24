

Namespace CommonLib

    ''' <summary>
    ''' Implements logistic function = 1/(1 + exp(-x)) 
    ''' </summary>
    Public Class SoftStepFunction
        Implements IActivationFunction

        Public Function [Function](x As Single) As Single Implements IActivationFunction.Function
            Dim Y As Single

            Y = Math.Exp(-x)
            Y = Y + 1
            Y = 1 / Y
            Return Y
        End Function

        ''' <summary>
        ''' Implements f’(x)=f(x)(1-f(x))
        ''' </summary>
        ''' <param name="x"></param>
        ''' <returns></returns>
        Public Function Derivative(x As Single) As Single Implements IActivationFunction.Derivative
            Dim Y As Single

            Y = [Function](x)
            Return Y * (1 - Y)
        End Function
    End Class
End Namespace


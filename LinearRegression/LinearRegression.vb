Imports AIDotNetLib.CommonLib


Namespace LinearRegression
    ''' <summary>
    ''' Linear Regression Pseudo Algorithm to resolve h(x) = a + b * x
    '''       1- Start with random values for a and b
    '''       2- Iterate through given training set, for each:
    '''            - Calculate h(x)
    '''            - Calculate error = h(x) - Y where y is the correct answer or label
    '''       3- Sum all errors
    '''       4- Calculate MSE (Mean Squared Error) = 1/2*training set size * sum of all errors
    '''       5- Get slope of line touching curve at the current point (for each value of a and b)
    '''            - If +ve slope then Decrease a or b values
    '''            - If -ve slope then increase a or b values
    '''       6- Repeat above steps from 2 to 5 till direction or slope of claculated line changes
    '''       7- Last values for a and b are the optimal values as per MSE minimization
    ''' </summary>
    Public Class LinearRegression
        Private _Inputs As Matrix1D
        Private _Lables As Matrix1D

        Private a As Single
        Private b As Single
        Private min_MSE As Single

        Private r As Single

        Private _Gen As RandomFactory

        Public Sub New()
            _Gen = New RandomFactory
            Randomize()
            r = 0.0001
        End Sub

        Public Sub Randomize()
            a = _Gen.GetRandomSngl(0, 1)
            b = _Gen.GetRandomSngl(0, 1)
        End Sub

        Public Function Hypothesis(X As Single) As Single
            Return a + b * X
        End Function

        Public Sub Train(_Inputs As Matrix1D, _Labels As Matrix1D)
            Dim m As Integer = _Inputs.Size   ' Training set size
            Dim Err As Matrix1D     ' represents sum of all erros in single iteration
            Dim Counter As Integer = 0
            Dim Best_a, Best_b As Single

            If _Inputs.Size <> _Labels.Size Then
                Throw New Exception("Both Inputs and Labels Matrices sizes shall match.")
            End If
            ' Randomize a & b
            Randomize()
            ' Iterate and update a & b values till direction changes
            Do While Counter < 100
                Dim h_Matrix As New Matrix1D(m)
                ' Calculate Error matrix
                Err = New Matrix1D(m)
                For I As Integer = 0 To m - 1
                    h_Matrix.SetValue(I, Hypothesis(_Inputs.GetValue(I)))
                Next
                Err = h_Matrix.Sub(_Labels)
                If CalcCostFunction(Err) < min_MSE OrElse Counter = 0 Then
                    min_MSE = CalcCostFunction(Err)
                    Best_a = a
                    Best_b = b
                End If
                a = a - r * (1 / m) * Err.Sum
                Err = Err.Product(_Inputs)
                b = b - r * (1 / m) * Err.Sum
                Counter += 1
            Loop
            a = Best_a
            b = Best_b
        End Sub

        Private Function CalcCostFunction(_Err As Matrix1D) As Single
            Dim Result As Single
            Dim New_err As New Matrix1D(_Err.Size)

            New_err = _Err.Product(_Err)
            Result = 1 / (2 * _Err.Size) * New_err.Sum

            Return Result
        End Function

        Private Function GetDirection(Value As Single) As Integer
            If Value >= 0 Then Return +1
            Return -1
        End Function

        Public ReadOnly Property Get_A() As Single
            Get
                Return a
            End Get
        End Property
        Public ReadOnly Property Get_B() As Single
            Get
                Return b
            End Get
        End Property

        Public ReadOnly Property GetMinMSE() As Single
            Get
                Return min_MSE
            End Get
        End Property

        Public Property LearningRate() As Single
            Get
                Return r
            End Get
            Set(value As Single)
                r = Math.Max(1, value)
            End Set
        End Property

    End Class
End Namespace


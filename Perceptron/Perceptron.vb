
Imports AIDotNetLib.CommonLib


Namespace Perceptron
    ''' <summary>
    ''' Perceptron is the smallest processing element which mimics single neuron
    ''' </summary>
    Public Class Perceptron
        ''' <summary>
        ''' Size is the number of inputs to Perceptron including X0=1
        ''' </summary>
        Private _Size As Integer
        ''' <summary>
        ''' Weights is 1D Matrix that holds the weights of Perceptron
        ''' Weights size is same as Perceptron size
        ''' </summary>
        Private _Weights As Matrix1D
        ''' <summary>
        ''' Learning Rate between 0 to 1
        ''' </summary>
        Private _LearnRate As Single

        ''' <summary>
        ''' Default learning rate of 0.1
        ''' </summary>
        Private Const DefaultLearnRate As Single = 0.01

        ''' <summary>
        ''' Constructor with 2 parameters Learning rate and size
        ''' Weights are randomly initialized
        ''' </summary>
        ''' <param name="PerceptronSize">Number of inputs including X0</param>
        ''' <param name="LRate"></param>
        Public Sub New(PerceptronSize As Integer, LRate As Single)
            Me._Size = PerceptronSize
            Me._Weights = New Matrix1D(Me.Size)
            Me._Weights.RandomizeValues(-100, 100)
            Me._LearnRate = LRate
        End Sub

        ''' <summary>
        ''' Constructor with 1 parameter Size and default learning rate
        ''' </summary>
        ''' <param name="PerceptronSize"></param>
        Public Sub New(PerceptronSize As Integer)
            Me.New(PerceptronSize, DefaultLearnRate)
        End Sub

        ''' <summary>
        '''  Size is the number of inputs to Perceptron including X0=1
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Size As Integer
            Get
                Return _Size
            End Get
        End Property

        ''' <summary>
        ''' Weights is 1D Matrix that holds the weights of Perceptron
        ''' Weights size is same as Perceptron size
        ''' </summary>
        ''' <returns></returns>
        Public Property Weights As Matrix1D
            Get
                Return _Weights
            End Get
            Set(value As Matrix1D)
                _Weights = value
            End Set
        End Property

        ''' <summary>
        ''' Learning Rate between 0 to 1
        ''' </summary>
        ''' <returns></returns>
        Public Property LearnRate As Single
            Get
                Return _LearnRate
            End Get
            Set(value As Single)
                _LearnRate = value
            End Set
        End Property

        ''' <summary>
        ''' Get single Weight at index location
        ''' </summary>
        ''' <param name="Index"></param>
        ''' <returns></returns>
        Public Function GetWeight(Index As Integer) As Single
            If Index > Me._Weights.Values.Count - 1 Then Return Nothing

            Return Me._Weights.Values(Index)
        End Function

        ''' <summary>
        ''' Sets single Weight with Value at index location
        ''' </summary>
        ''' <param name="Index"></param>
        ''' <param name="Value"></param>
        Public Sub SetWeight(Index As Integer, Value As Single)
            If Index > Me._Weights.Values.Count - 1 Then Exit Sub

            Me._Weights.Values(Index) = Value
        End Sub

        ''' <summary>
        ''' Calculate Hypothesis Function of Perceptron h(x) = Sum (X * W)
        ''' </summary>
        ''' <param name="Input"></param>
        ''' <returns></returns>
        Public Function HypothesisFunction(Input As Matrix1D) As Matrix1D
            If Input.Size <> Me.Size Then Throw New Exception("Input Matrix size shall match " & Me.Size.ToString)
            Dim HypothesisFun As New Matrix1D(Me.Size)

            HypothesisFun = Input.Product(Weights)
            Return HypothesisFun
        End Function

        ''' <summary>
        ''' Calculate Final Perceptron Output = ActivationFunction(h(x))
        ''' </summary>
        ''' <param name="Input"></param>
        ''' <param name="ActivationFunction"></param>
        ''' <returns></returns>
        Public Function CalcOutput(Input As Matrix1D, ActivationFunction As IActivationFunction) As Single
            Dim Hypothesis_x As Single = Me.HypothesisFunction(Input).Sum

            Return ActivationFunction.Function(Hypothesis_x)
        End Function

        ''' <summary>
        ''' Train Perceptron Algorithm
        ''' Given:
        '''     X is input set
        '''     Y is label or desired output
        '''     h(x) = X1*W1+X2*W2+.....Xm*Wm is hypothesis or h function
        ''' Initialize Weights randomly
        ''' For each training set 
        '''     Calculate Ouput h(x) 
        '''     Calculate Error = h(x) - Y
        '''     Update Weights accordingly W = W - r * (h(x) - Y) * X
        ''' Repeat till termination:
        '''     Number of Iterations > threshold value or
        '''     Iteration error less than a threshold
        ''' </summary>
        ''' <param name="Input">Input Matrix</param>
        ''' <param name="Label">Desired Output</param>
        ''' <param name="ActivationFunction"></param>
        Public Sub TrainPerceptron(Input() As Matrix1D, Label() As Single, ActivationFunction As IActivationFunction)
            Dim m As Integer = Input.Count  ' training set size
            Dim Counter As Integer = 0      ' number of iterations
            Dim MSE As Single = 0           ' To track error MSE
            Dim IterateError As Single = 0  ' To Track error in each iteration

            Do
                Counter += 1
                MSE = 0  ' Reset error

                For I As Integer = 0 To m - 1 ' iterate through training set
                    Dim Out As Single = Me.CalcOutput(Input(I), ActivationFunction)
                    IterateError = Out - Label(I)
                    For Index As Integer = 0 To Me.Size - 1
                        Me._Weights.Values(Index) = Me._Weights.Values(Index) - Me.LearnRate * IterateError * Input(I).GetValue(Index)
                    Next
                    MSE += IterateError
                    IterateError = 0
                Next
                ' Calculate MSE  
                MSE = 1 / (2 * m) * MSE * MSE
                ' Check termination condition
            Loop Until MSE < 0.001 OrElse Counter > 10000
        End Sub
    End Class
End Namespace


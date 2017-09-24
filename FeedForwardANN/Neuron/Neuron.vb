Imports AIDotNetLib.CommonLib

Namespace FeedForwardANN
    Public Class Neuron
        Implements INeuron

        Private _InputsCount As Integer
        Private _Weights As Matrix1D

        Private RndGen As New RandomFactory

        Public Sub New(InsCount As Integer)
            Me._InputsCount = InsCount
            Me.Weights = New Matrix1D(InsCount)
            Randomize()
        End Sub

        Public Property Weights As Matrix1D Implements INeuron.Weights
            Get
                Return Me._Weights
            End Get
            Set(value As Matrix1D)
                Me._Weights = value
            End Set
        End Property

        Public ReadOnly Property InputsCount As Integer
            Get
                Return _InputsCount
            End Get
        End Property

        Public Sub SetWeight(Index As Integer, Value As Single) Implements INeuron.SetWeight
            Me._Weights.SetValue(Index, Value)
        End Sub

        Public Sub ForceWeights(Value As Single) Implements INeuron.ForceWeights
            _Weights.ForceValues(Value)
        End Sub

        Public Function GetWeight(Index As Integer) As Single Implements INeuron.GetWeight
            Return Me._Weights.GetValue(Index)
        End Function

        Public Function OutputCalc(Inputs As Matrix1D) As Single Implements INeuron.OutputCalc
            Return MathFunctions.Hypothesis(Inputs, Me._Weights)
        End Function

        Public Function OutputCalc(Inputs As Matrix1D, ActFun As IActivationFunction) As Single Implements INeuron.OutputCalc
            Return ActFun.Function(OutputCalc(Inputs))
        End Function

        Private Sub Randomize()
            For I As Integer = 0 To Me._Weights.Size - 1
                Me._Weights.SetValue(I, RndGen.GetRandomSngl(-10, 10))
            Next
        End Sub
    End Class
End Namespace


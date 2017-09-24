Imports AIDotNetLib.CommonLib

Namespace FeedForwardANN
    Public Class Layer
        Implements ILayer

        Private _NeuronsCount As Integer
        Private _Neurons() As Neuron
        Private _ActFun As IActivationFunction
        Private _Output As Matrix1D

        Public Sub New(Size As Integer, InputsCount As Integer)
            Me._NeuronsCount = Size
            ReDim Me._Neurons(Size - 1)
            Me._Output = New Matrix1D(Size)
            For i As Integer = 0 To Size - 1
                Me._Neurons(i) = New Neuron(InputsCount)
            Next
        End Sub

        Public ReadOnly Property GetNeuronsCount As Integer Implements ILayer.GetNeuronsCount
            Get
                Return _NeuronsCount
            End Get
        End Property

        Public Property ActivationFunction As IActivationFunction Implements ILayer.ActivationFunction
            Get
                Return Me._ActFun
            End Get
            Set(value As IActivationFunction)
                Me._ActFun = value
            End Set
        End Property

        Public Property Output As Matrix1D Implements ILayer.Output
            Get
                Return Me._Output
            End Get
            Set(value As Matrix1D)
                Me._Output = value
            End Set
        End Property

        Public Sub SetWeights(Value As Single) Implements ILayer.SetWeights
            For i As Integer = 0 To Me._NeuronsCount - 1
                Me._Neurons(i).ForceWeights(Value)
            Next
        End Sub

        Public Function OutputCalc(Inputs As Matrix1D) As Matrix1D Implements ILayer.OutputCalc
            Return OutputCalc(Inputs, Me._ActFun)
        End Function

        Public Function OutputCalc(Inputs As Matrix1D, ActFun As IActivationFunction) As Matrix1D Implements ILayer.OutputCalc
            For I As Integer = 0 To Me._Output.Size - 1
                Me._Output.SetValue(I, Me._Neurons(I).OutputCalc(Inputs, ActFun))
            Next

            Return Me._Output
        End Function

    End Class
End Namespace
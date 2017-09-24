Imports AIDotNetLib.CommonLib


Namespace FeedForwardANN
    Public Interface INeuron
        Property Weights() As Matrix1D
        Sub SetWeight(Index As Integer, Value As Single)
        Sub ForceWeights(Value As Single)
        Function GetWeight(Index As Integer) As Single
        Function OutputCalc(Inputs As Matrix1D) As Single

        Function OutputCalc(Inputs As Matrix1D, ActFun As IActivationFunction) As Single
    End Interface
End Namespace


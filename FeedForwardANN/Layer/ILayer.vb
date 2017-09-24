Imports AIDotNetLib.CommonLib

Namespace FeedForwardANN
    Public Interface ILayer
        ReadOnly Property GetNeuronsCount() As Integer
        Property ActivationFunction() As IActivationFunction
        Property Output() As Matrix1D
        Sub SetWeights(Value As Single)

        Function OutputCalc(Inputs As Matrix1D) As Matrix1D
        Function OutputCalc(Inputs As Matrix1D, ActFun As IActivationFunction) As Matrix1D
    End Interface
End Namespace


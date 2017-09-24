Imports AIDotNetLib.CommonLib

Namespace FeedForwardANN
    Public Interface INetwork
        ReadOnly Property Layers() As ILayer()
        Property HiddenLayerFun As IActivationFunction
        Property OutLayerFun As IActivationFunction

        Function OutputCalc(Inputs As Matrix1D) As Matrix1D
    End Interface
End Namespace

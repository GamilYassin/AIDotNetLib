Imports AIDotNetLib.CommonLib

Namespace FeedForwardANN
    Public Class Network
        Implements INetwork

        Private _Layers() As Layer
        Private _InputsCount As Integer
        Private _HiddenLayerFun As IActivationFunction
        Private _OutLayerFun As IActivationFunction

        Public Sub New(LayersInfo() As Integer)
            ReDim Me._Layers(LayersInfo.Count - 1)
            Me._Layers(0) = New Layer(LayersInfo(0), LayersInfo(0))
            Me._Layers(0).SetWeights(1)
            Me._Layers(0).ActivationFunction = New IdentityFunction
            For I As Integer = 1 To Me._Layers.Count - 1
                Me._Layers(I) = New Layer(LayersInfo(I), LayersInfo(I - 1))
            Next
        End Sub

        Public ReadOnly Property Layers As ILayer() Implements INetwork.Layers
            Get
                Return Me._Layers
            End Get
        End Property

        Public Property HiddenLayerFun As IActivationFunction Implements INetwork.HiddenLayerFun
            Get
                Return Me._HiddenLayerFun
            End Get
            Set(value As IActivationFunction)
                _HiddenLayerFun = value
                For I As Integer = 1 To Me._Layers.Count - 2
                    Me._Layers(I).ActivationFunction = value
                Next
            End Set
        End Property

        Public Property OutLayerFun As IActivationFunction Implements INetwork.OutLayerFun
            Get
                Return _OutLayerFun
            End Get
            Set(value As IActivationFunction)
                _OutLayerFun = value
                Me._Layers.Last().ActivationFunction = value
            End Set
        End Property

        Public Function OutputCalc(Inputs As Matrix1D) As Matrix1D Implements INetwork.OutputCalc
            Me._Layers(0).Output = Inputs
            For I As Integer = 1 To Me._Layers.Count - 1
                Me._Layers(I).OutputCalc(Me._Layers(I - 1).Output)
            Next
            Return Me._Layers.Last().Output
        End Function
    End Class
End Namespace
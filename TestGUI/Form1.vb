Imports AIDotNetLib.CommonLib
Imports AIDotNetLib.FeedForwardANN

Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim _Net As New Network({5, 4, 4, 2})

        _Net.HiddenLayerFun = New SoftStepFunction
        _Net.OutLayerFun = New ReluFunction

        Dim Ins As New Matrix1D({10, 11, 17, 10, -5})
        Dim Out As Matrix1D = _Net.OutputCalc(Ins)
        TextBox1.Text = Out.ToString
    End Sub
End Class

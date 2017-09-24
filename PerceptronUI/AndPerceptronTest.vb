Imports AIDotNetLib.CommonLib
Imports AIDotNetLib.Perceptron




Public Class AndPerceptronTest
    Private MyPerceptron As Perceptron.Perceptron
    Private ActivFun As IActivationFunction
    Private Const InputsCount As Integer = 3

    Private Sub AndPerceptronTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MyPerceptron = New Perceptron.Perceptron(InputsCount, 0.1)
        ActivFun = New StepFunction
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim Inputs(3) As Matrix1D
        Dim Labels(3) As Single

        MyPerceptron = New Perceptron.Perceptron(InputsCount, 0.5)
        Inputs(0) = New Matrix1D({1, 1, 1})
        Labels(0) = 1
        Inputs(1) = New Matrix1D({1, 1, 0})
        Labels(1) = 0
        Inputs(2) = New Matrix1D({1, 0, 1})
        Labels(2) = 0
        Inputs(3) = New Matrix1D({1, 0, 0})
        Labels(3) = 0

        'For I As Integer = 0 To 3
        MyPerceptron.TrainPerceptron(Inputs, Labels, ActivFun)
        'Next
        ListBox1.Items.Clear()
        For I As Integer = 0 To MyPerceptron.Weights.Size - 1
            ListBox1.Items.Add(MyPerceptron.GetWeight(I))
        Next
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If Not IsNumeric(TextBox1.Text) Then Exit Sub
        If Not IsNumeric(TextBox2.Text) Then Exit Sub

        Dim Input As New Matrix1D(InputsCount) With {
            .Values = {1, CStr(TextBox1.Text), CStr(TextBox2.Text)}
        }
        TextBox3.Text = Math.Floor(MyPerceptron.CalcOutput(Input, ActivFun)).ToString
    End Sub
End Class

Imports AIDotNetLib.CommonLib
Imports AIDotNetLib.LinearRegression


Public Class TestForm
    Private MyLinearReg As LinearRegression.LinearRegression
    Private _Inputs As Matrix1D
    Private _Labels As Matrix1D

    Private myCanv As Canvas

    Private Sub TestForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim In_Array() As Single = {50, 53, 54, 55, 56, 59, 62, 65, 67, 71, 72, 74, 75, 76, 79, 90, 82, 85, 87, 90, 93, 94, 95, 97, 100}
        Dim Label_Arr() As Single = {122, 118, 128, 121, 125, 136, 144, 142, 149, 161, 167, 168, 162, 171, 175, 182, 180, 183, 188, 200, 194, 206, 207, 210, 219}

        MyLinearReg = New LinearRegression.LinearRegression()
        _Inputs = New Matrix1D(In_Array)
        _Labels = New Matrix1D(Label_Arr)
        myCanv = New Canvas(PictureBox1.Width, PictureBox1.Height)
        TrackBar1.Enabled = False
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        myCanv.Clear()
        MyLinearReg.Train(_Inputs, _Labels)
        TextBox1.Text = MyLinearReg.Get_A.ToString
        TextBox2.Text = MyLinearReg.Get_B.ToString
        TextBox3.Text = MyLinearReg.GetMinMSE.ToString
        DrawTrainingSet()
        FillListBox(_Inputs, ListBox1)
        FillListBox(_Labels, ListBox2)
        With TrackBar1
            .Minimum = _Inputs.GetMin
            .Maximum = _Inputs.GetMax
            .Value = .Minimum
            .Enabled = True
        End With
    End Sub

    Private Sub DrawTrainingSet()
        For i As Integer = 0 To _Inputs.Size - 1
            Dim X As Single
            Dim Y As Single

            X = MathFunctions.Map(_Inputs.GetValue(i), _Inputs.GetMin, _Inputs.GetMax, 10, myCanv.Width - 50)
            Y = MathFunctions.Map(_Labels.GetValue(i), _Labels.GetMin, _Labels.GetMax, myCanv.Height - 50, 10)
            myCanv.DrawCircle(4, X, Y, Color.Red)
        Next
        DrawLine()
        PictureBox1.Image = myCanv.Image
    End Sub

    Private Sub DrawLine()
        Dim _Start As New Vector()
        Dim _Stop As New Vector()

        _Start.x = _Inputs.GetValue(0)
        _Start.y = MyLinearReg.Get_A + MyLinearReg.Get_B * _Start.x

        _Stop.x = _Inputs.GetValue(_Inputs.Size - 1)
        _Stop.y = MyLinearReg.Get_A + MyLinearReg.Get_B * _Stop.x

        _Start.x = MathFunctions.Map(_Start.x, _Inputs.GetMin, _Inputs.GetMax, 10, myCanv.Width - 50)
        _Start.y = MathFunctions.Map(_Start.y, _Labels.GetMin, _Labels.GetMax, myCanv.Height - 50, 10)

        _Stop.x = MathFunctions.Map(_Stop.x, _Inputs.GetMin, _Inputs.GetMax, 10, myCanv.Width - 50)
        _Stop.y = MathFunctions.Map(_Stop.y, _Labels.GetMin, _Labels.GetMax, myCanv.Height - 50, 10)

        myCanv.DrawLine(_Start, _Stop, Color.Blue)
    End Sub

    Private Sub FillListBox(_Values As Matrix1D, ListBox As ListBox)
        ListBox.Items.Clear()

        For I As Integer = 0 To _Values.Size - 1
            ListBox.Items.Add(_Values.GetValue(I))
        Next

    End Sub

    Private Sub TrackBar1_Scroll(sender As Object, e As EventArgs) Handles TrackBar1.Scroll
        Label8.Text = TrackBar1.Value
        TextBox4.Text = MyLinearReg.Hypothesis(TrackBar1.Value).ToString
    End Sub
End Class

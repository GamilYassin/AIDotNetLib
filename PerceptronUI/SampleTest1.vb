Imports AIDotNetLib.CommonLib


Public Class SampleTest1
    Private MyCanvas As Canvas
    Private RndTraininSet As TrainingSet
    Private MyPerceptron As Perceptron.Perceptron
    Private ActivFun As IActivationFunction

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        MyCanvas.Clear()
        RndTraininSet.Randomize()
        RndTraininSet.Draw(MyCanvas)
        PictureBox1.Image = MyCanvas.Image
    End Sub

    Private Sub SampleTest1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Initialize random training set with size 100
        RndTraininSet = New TrainingSet(100, PictureBox1.Width, PictureBox1.Height)
        MyCanvas = New Canvas(PictureBox1.Width, PictureBox1.Height)
        MyPerceptron = New Perceptron.Perceptron(3, 0.1)
        ActivFun = New SignFunction
    End Sub

    Private Sub btnTrain_Click(sender As Object, e As EventArgs) Handles btnTrain.Click
        MyPerceptron.TrainPerceptron(RndTraininSet.Points, RndTraininSet.Labels, ActivFun)
    End Sub

    Private Sub btnDraw_Click(sender As Object, e As EventArgs) Handles btnDraw.Click
        Dim Point_1 As New Point
        Dim point_2 As New Point

        With Point_1
            .X = 0
            .Y = -1 * MyPerceptron.GetWeight(0) / MyPerceptron.GetWeight(2)
        End With
        With point_2
            .X = 600
            .Y = Point_1.Y - MyPerceptron.GetWeight(1) / MyPerceptron.GetWeight(2) * .X
        End With
        MyCanvas.DrawLine(Point_1, point_2, Color.Red)
        PictureBox1.Image = MyCanvas.Image
    End Sub
End Class


''' <summary>
''' Class to build random training set
''' Creates training set within specified width and height limits (for graphical visualization)
''' Positive points are ones above line 300 - 2 / 3 * X (random straight line)
''' Points below this line are negative
''' </summary>
Public Class TrainingSet
    Private _PointsNum As Integer
    Private _Width As Integer
    Private _Height As Integer

    ''' <summary>
    ''' Holds training set inputs matrix
    ''' </summary>
    Private _Points() As Matrix1D
    ''' <summary>
    ''' Holds labels (correct answers) array
    ''' </summary>
    Private _Labels() As Single

    Private _Gen As RandomFactory

    Public Sub New(PointsNum As Integer, Width As Integer, Height As Integer)
        _PointsNum = PointsNum
        _Width = Width
        _Height = Height
        _Gen = New RandomFactory
        ReDim _Points(PointsNum - 1)
        ReDim _Labels(PointsNum - 1)
        Randomize()
    End Sub

    ''' <summary>
    ''' Create random points
    ''' </summary>
    Public Sub Randomize()
        For I As Integer = 0 To _PointsNum - 1
            Points(I) = New Matrix1D(3)
            Points(I).SetValue(0, 1)
            Points(I).SetValue(1, _Gen.GetRandomInt(0, _Width))
            Points(I).SetValue(2, _Gen.GetRandomInt(0, _Height))
            Labels(I) = Classify(Points(I).GetValue(1), Points(I).GetValue(2))
        Next
    End Sub

    Public ReadOnly Property Points As Matrix1D()
        Get
            Return _Points
        End Get
    End Property

    Public ReadOnly Property Labels As Single()
        Get
            Return _Labels
        End Get
    End Property

    ''' <summary>
    ''' Creates labels array by checking points against straight line 300 - 2 / 3 * X
    ''' </summary>
    ''' <param name="X">Point X coordinate</param>
    ''' <param name="Y">Point Y coordinate</param>
    ''' <returns></returns>
    Private Function Classify(X As Single, Y As Single) As Single
        Dim d As Single = 300 - 2 / 3 * X
        If Y >= d Then Return +1
        Return -1
    End Function

    ''' <summary>
    ''' Draws points within passed canvas object
    ''' </summary>
    ''' <param name="MyCanv"></param>
    Public Sub Draw(MyCanv As Canvas)
        For I As Integer = 0 To _PointsNum - 1
            If _Labels(I) = 1 Then
                MyCanv.FillBox(5, Points(I).GetValue(1), Points(I).GetValue(2), Color.Blue)
            Else
                MyCanv.FillCircle(5, Points(I).GetValue(1), Points(I).GetValue(2), Color.Green)
            End If
        Next
    End Sub
End Class

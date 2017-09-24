Imports System.Drawing
Imports System.Drawing.Drawing2D

Namespace CommonLib

    ''' <summary>
    ''' Canvas class wrap functionality of GDI+ and encaspulates most common methods
    ''' </summary>
    Public Class Canvas
        Implements IDisposable

        ''' <summary>
        ''' Main graphics object
        ''' </summary>
        Private g As Graphics
        ''' <summary>
        ''' Drawing area - Width
        ''' This is a mandetory parameter for constructor 
        ''' </summary>
        Private _Width As Integer
        ''' <summary>
        ''' Drawing area - Height
        ''' This is a mandetory parameter for constructor 
        ''' </summary>
        Private _Height As Integer
        ''' <summary>
        ''' Image object for Graphics
        ''' </summary>
        Private _bmp As Bitmap

        ''' <summary>
        ''' Return White color - default color
        ''' </summary>
        Public DEFAULT_COLOR As Color = Color.White

        ''' <summary>
        ''' Canvas constructor function
        ''' </summary>
        ''' <param name="_width">Drawing area - Width</param>
        ''' <param name="_height">Drawing area - Height</param>
        Public Sub New(_width As Integer, _height As Integer)
            Me._Width = _width
            Me._Height = _height
            Me._bmp = New Bitmap(_width, _height)
            g = Graphics.FromImage(_bmp)
            g.SmoothingMode = SmoothingMode.HighQuality
        End Sub

        ''' <summary>
        ''' Drawing area - Width
        ''' This is a mandetory parameter for constructor 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Width As Integer
            Get
                Return _Width
            End Get
        End Property

        ''' <summary>
        ''' Drawing area - Height
        ''' This is a mandetory parameter for constructor 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Height As Integer
            Get
                Return _Height
            End Get
        End Property

        ''' <summary>
        ''' Image object for Graphics
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Image As Bitmap
            Get
                Return _bmp
            End Get
        End Property

        ''' <summary>
        ''' This function draws a line given start and end coordinates as integers
        ''' </summary>
        ''' <param name="X1">Start X coordinates</param>
        ''' <param name="Y1">Start Y coordinates</param>
        ''' <param name="X2">End X coordinates</param>
        ''' <param name="Y2">End Y coordinates</param>
        ''' <param name="LineColor">Color of line</param>
        Public Sub DrawLine(X1 As Integer, Y1 As Integer, X2 As Integer, Y2 As Integer, LineColor As Color)
            Dim pen As New Pen(LineColor)

            g.DrawLine(pen, X1, Y1, X2, Y2)
        End Sub


        ''' <summary>
        ''' This function draws a line given start and end coordinates as vector
        ''' </summary>
        ''' <param name="Start">Start vector</param>
        ''' <param name="[End]">End Vector</param>
        ''' <param name="LineColor">Line Color</param>
        Public Sub DrawLine(Start As Vector, [End] As Vector, LineColor As Color)
            Me.DrawLine(Start.x, Start.y, [End].x, [End].y, LineColor)
        End Sub

        ''' <summary>
        ''' This function draws a line given start and end coordinates as point
        ''' </summary>
        ''' <param name="Point1">Start Point</param>
        ''' <param name="Point2">End Point</param>
        ''' <param name="LineColor">Line Color</param>
        Public Sub DrawLine(Point1 As Point, Point2 As Point, LineColor As Color)
            Me.DrawLine(Point1.X, Point1.Y, Point2.X, Point2.Y, LineColor)
        End Sub

        ''' <summary>
        ''' Draw an empty circle
        ''' </summary>
        ''' <param name="R">circle radius</param>
        ''' <param name="CenterX">X coordinate of center</param>
        ''' <param name="CenterY">Y coordinate of center</param>
        ''' <param name="CircleColor">Color of circle perimeter</param>
        Public Sub DrawCircle(R As Single, CenterX As Integer, CenterY As Integer, CircleColor As Color)
            Dim pen As New Pen(CircleColor)

            g.DrawEllipse(pen, CenterX - R, CenterY - R, 2 * R, 2 * R)
        End Sub

        ''' <summary>
        ''' Draw an empty circle
        ''' </summary>
        ''' <param name="R">circle radius</param>
        ''' <param name="CenterVect">Center Vector</param>
        ''' <param name="CircleColor">Color of circle perimeter</param>
        Public Sub DrawCircle(R As Single, CenterVect As Vector, CircleColor As Color)
            Me.DrawCircle(R, CenterVect.x, CenterVect.y, CircleColor)
        End Sub

        ''' <summary>
        ''' Draw an empty circle
        ''' </summary>
        ''' <param name="R">circle radius</param>
        ''' <param name="CenterPoint">Center Point</param>
        ''' <param name="CircleColor">Color of circle perimeter</param>
        Public Sub DrawCircle(R As Single, CenterPoint As Point, CircleColor As Color)
            Me.DrawCircle(R, CenterPoint.X, CenterPoint.Y, CircleColor)
        End Sub

        ''' <summary>
        ''' Draw and fills a circle of radius R
        ''' </summary>
        ''' <param name="R">circle radius</param>
        ''' <param name="CenterX">X coordinate of center</param>
        ''' <param name="CenterY">Y coordinate of center</param>
        ''' <param name="CircleColor">Color of circle</param>
        Public Sub FillCircle(R As Single, CenterX As Integer, CenterY As Integer, CircleColor As Color)
            Dim Brush As New SolidBrush(CircleColor)

            g.FillEllipse(Brush, CenterX - R, CenterY - R, 2 * R, 2 * R)
        End Sub


        ''' <summary>
        ''' Draw and fills a circle of radius R
        ''' </summary>
        ''' <param name="R">circle radius</param>
        ''' <param name="CenterVect">Center Vector</param>
        ''' <param name="CircleColor">Color of circle</param>
        Public Sub FillCircle(R As Single, CenterVect As Vector, CircleColor As Color)
            Me.FillCircle(R, CenterVect.x, CenterVect.y, CircleColor)
        End Sub

        ''' <summary>
        ''' Draw and fills a circle of radius R
        ''' </summary>
        ''' <param name="R">circle radius</param>
        ''' <param name="CenterPoint">Center Point</param>
        ''' <param name="CircleColor">Color of circle</param>
        Public Sub FillCircle(R As Single, CenterPoint As Point, CircleColor As Color)
            Me.FillCircle(R, CenterPoint.X, CenterPoint.Y, CircleColor)
        End Sub


        ''' <summary>
        ''' Draw an empty square (box) of width = height of W
        ''' </summary>
        ''' <param name="W">Box Widht and Height</param>
        ''' <param name="X">X coordinate of top-left corner</param>
        ''' <param name="Y">Y coordinate of top-left corner</param>
        ''' <param name="boxcolor">Color of box perimeter</param>
        Public Sub DrawBox(W As Integer, X As Integer, Y As Integer, boxcolor As Color)
            Dim pen As New Pen(boxcolor)

            g.DrawRectangle(pen, X, Y, W, W)
        End Sub

        ''' <summary>
        ''' Draw an empty square (box) of width = height of W
        ''' </summary>
        ''' <param name="w">Box Widht and Height</param>
        ''' <param name="LeftTopVector">Top left corner vector</param>
        ''' <param name="BoxColor">Color of box perimeter</param>
        Public Sub DrawBox(w As Integer, LeftTopVector As Vector, BoxColor As Color)
            Me.DrawBox(w, LeftTopVector.x, LeftTopVector.y, BoxColor)
        End Sub

        ''' <summary>
        ''' Draw an empty square (box) of width = height of W
        ''' </summary>
        ''' <param name="w">Box Widht and Height</param>
        ''' <param name="LeftTopPoint">Top left corner point</param>
        ''' <param name="BoxColor">Color of box perimeter</param>
        Public Sub DrawBox(w As Integer, LeftTopPoint As Point, BoxColor As Color)
            Me.DrawBox(w, LeftTopPoint.X, LeftTopPoint.Y, BoxColor)
        End Sub


        ''' <summary>
        ''' Draw and fill square (box) of width = height of W
        ''' </summary>
        ''' <param name="W">Box Widht and Height</param>
        ''' <param name="X">X coordinate of top-left corner</param>
        ''' <param name="Y">Y coordinate of top-left corner</param>
        ''' <param name="Boxcolor">Color of box</param>
        Public Sub FillBox(W As Integer, X As Integer, Y As Integer, Boxcolor As Color)
            Dim brush As New SolidBrush(Boxcolor)

            g.FillRectangle(brush, X, Y, W, W)
        End Sub

        ''' <summary>
        ''' Draw and fill square (box) of width = height of W
        ''' </summary>
        ''' <param name="w">Box Widht and Height</param>
        ''' <param name="LeftTopVector">Top left corner vector</param>
        ''' <param name="BoxColor">Color of box</param>
        Public Sub FillboxBox(w As Integer, LeftTopVector As Vector, BoxColor As Color)
            Me.FillBox(w, LeftTopVector.x, LeftTopVector.y, BoxColor)
        End Sub

        ''' <summary>
        ''' Draw and fill square (box) of width = height of W
        ''' </summary>
        ''' <param name="w">Box Widht and Height</param>
        ''' <param name="LeftTopPoint">Top left corner point</param>
        ''' <param name="BoxColor">Color of box</param>
        Public Sub FillboxBox(w As Integer, LeftTopPoint As Point, BoxColor As Color)
            Me.FillBox(w, LeftTopPoint.X, LeftTopPoint.Y, BoxColor)
        End Sub

        ''' <summary>
        ''' Draw (type) text on graphics area
        ''' </summary>
        ''' <param name="Str">Text to be drawn</param>
        ''' <param name="X">X coordinate of start point</param>
        ''' <param name="Y">Y coordinate of start point</param>
        ''' <param name="textcolor">Text Color</param>
        Public Sub DrawText(Str As String, X As Integer, Y As Integer, textcolor As Color)
            Dim brush As New SolidBrush(textcolor)
            Dim font As New Font(FontFamily.GenericSerif, 9)

            g.DrawString(Str, font, brush, X, Y)
        End Sub

        ''' <summary>
        ''' Draw (type) text on graphics area
        ''' </summary>
        ''' <param name="Str">Text to be drawn</param>
        ''' <param name="StartVector">start point vector</param>
        ''' <param name="textcolor">Text Color</param>
        Public Sub DrawText(Str As String, StartVector As Vector, textcolor As Color)
            Me.DrawBox(Str, StartVector.x, StartVector.y, textcolor)
        End Sub

        ''' <summary>
        ''' Draw (type) text on graphics area
        ''' </summary>
        ''' <param name="Str">Text to be drawn</param>
        ''' <param name="StartPoint">start point coordinate point</param>
        ''' <param name="textcolor">Text Color</param>
        Public Sub DrawText(Str As String, StartPoint As Point, textcolor As Color)
            Me.DrawBox(Str, StartPoint.X, StartPoint.Y, textcolor)
        End Sub

        ''' <summary>
        ''' Clears the entire drawing surface and fills it with White color
        ''' </summary>
        Public Sub Clear()
            Me.g.Clear(DEFAULT_COLOR)
        End Sub

        ''' <summary>
        ''' Releases all used resources 
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose
            g.Dispose()
        End Sub

        ''' <summary>
        '''  Forces execution of all pending graphics operations and returns immediately without waiting for the operations to finish
        ''' </summary>
        Public Sub Flush()
            g.Flush()
        End Sub

    End Class
End Namespace
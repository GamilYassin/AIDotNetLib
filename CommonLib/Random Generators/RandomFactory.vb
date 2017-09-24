Imports System.Drawing

Namespace CommonLib

    ''' <summary>
    ''' RandomFactory class generates different random numbers, characters, colors, etc..
    ''' </summary>
    Public Class RandomFactory
        Private _Gen As System.Random

        ''' <summary>
        ''' Initializes Random generator variable
        ''' </summary>
        Public Sub New()
            _Gen = New System.Random()
        End Sub

        ''' <summary>
        ''' Generates random integer
        ''' </summary>
        ''' <returns></returns>
        Public Function GetRandomInt() As Integer
            Randomize()
            Return _Gen.Next()
        End Function

        ''' <summary>
        ''' Generates random integer less than Max
        ''' </summary>
        ''' <param name="max"></param>
        ''' <returns></returns>
        Public Function GetRandomInt(max As Integer) As Integer
            Randomize()
            If max < 1 Then max = 1
            Return _Gen.Next(max * 100) / 100
        End Function

        ''' <summary>
        ''' Generates Integer random between min and max
        ''' </summary>
        ''' <param name="min"></param>
        ''' <param name="max"></param>
        ''' <returns></returns>
        Public Function GetRandomInt(min As Integer, max As Integer) As Integer
            Randomize()
            Return _Gen.Next(min * 100, max * 100) / 100
        End Function

        ''' <summary>
        ''' Generates next double random between 0 and 1
        ''' </summary>
        ''' <returns></returns>
        Public Function GetRandomDbl() As Double
            Randomize()
            Return _Gen.NextDouble
        End Function

        ''' <summary>
        ''' Generates next double random between min and max
        ''' </summary>
        ''' <param name="min"></param>
        ''' <param name="max"></param>
        ''' <returns></returns>
        Public Function GetRandomDbl(min As Double, max As Double) As Double
            Randomize()
            Return CDbl(MathFunctions.Map(_Gen.NextDouble(), 0, 1, min, max))
        End Function

        ''' <summary>
        ''' Generates next single random between min and max
        ''' </summary>
        ''' <param name="min"></param>
        ''' <param name="max"></param>
        ''' <returns></returns>
        Public Function GetRandomSngl(min As Double, max As Double) As Double
            Randomize()

            Return MathFunctions.Map(CSng(_Gen.NextDouble()), 0, 1, min, max)
        End Function

        ''' <summary>
        ''' Generates next random color by generating 4 random integers Alpha, red, green and blue
        ''' </summary>
        ''' <returns></returns>
        Public Function GetRandomColor() As Color
            Dim MyAlpha As Integer
            Dim MyRed As Integer
            Dim MyGreen As Integer
            Dim MyBlue As Integer
            ' Initialize the random-number generator.
            Randomize()
            ' Generate random value between 1 and 6.
            MyAlpha = CInt(Int((254 * Rnd()) + 0))
            ' Initialize the random-number generator.
            Randomize()
            ' Generate random value between 1 and 6.
            MyRed = CInt(Int((254 * Rnd()) + 0))
            ' Initialize the random-number generator.
            Randomize()
            ' Generate random value between 1 and 6.
            MyGreen = CInt(Int((254 * Rnd()) + 0))
            ' Initialize the random-number generator.
            Randomize()
            ' Generate random value between 1 and 6.
            MyBlue = CInt(Int((254 * Rnd()) + 0))

            Return Color.FromArgb(MyAlpha, MyRed, MyGreen, MyBlue)
        End Function

        ''' <summary>
        ''' Generates next random character as per ASCII codes, from 32 to 122
        ''' </summary>
        ''' <returns></returns>
        Public Function GetRandomChar() As Char
            Randomize()
            Return GetRandomChar(32, 122)
        End Function

        ''' <summary>
        ''' Generates next random char as per ASCII between min to max
        ''' </summary>
        ''' <param name="min"></param>
        ''' <param name="max"></param>
        ''' <returns></returns>
        Public Function GetRandomChar(min As Integer, max As Integer) As Char
            'Return ChrW(GetRandomInt(min, max))
            Randomize()

            ' Store the numbers 1 to 6 in a list '
            Dim allNumbers As New List(Of Integer)(Enumerable.Range(min, max - min + 1))
            ' Store the randomly selected numbers in this list: '
            Dim selectedNumbers As New List(Of Integer)
            For i As Integer = 0 To allNumbers.Count - 1
                ' A random index in numbers '
                Dim index As Integer = _Gen.Next(0, allNumbers.Count)
                ' Copy the item at index from allNumbers. '
                Dim selectedNumber As Integer = allNumbers(index)
                ' And store it in our list of picked numbers. '
                selectedNumbers.Add(selectedNumber)
                ' Remove the item from the list so that it cannot be picked again. '
                allNumbers.RemoveAt(index)
            Next
            Return ChrW(selectedNumbers(0))
        End Function

        ''' <summary>
        '''   Equally likely to return true or false
        ''' </summary>
        ''' <returns></returns>
        Public Function NextBoolean() As Boolean
            Return _Gen.Next(2) > 0
        End Function

        ''' <summary>
        '''   Generates normally distributed numbers using Box-Muller transform by generating 2 random doubles
        '''   Gaussian noise is statistical noise having a probability density function (PDF) equal to that of the normal distribution, 
        '''   which is also known as the Gaussian distribution.
        '''   In other words, the values that the noise can take on are Gaussian-distributed.
        ''' </summary>
        ''' <param name = "Mean">Mean of the distribution, default = 0</param>
        ''' <param name = "StdDeviation">Standard deviation, default = 1</param>
        ''' <returns></returns>
        Public Function NextGaussian(Optional ByVal Mean As Double = 0, Optional ByVal StdDeviation As Double = 1) As Double
            Dim X1 = _Gen.NextDouble()
            Dim X2 = _Gen.NextDouble()
            Dim StdDistribution = Math.Sqrt(-2.0 * Math.Log(X1)) * Math.Sin(2.0 * Math.PI * X2)
            Dim GaussianRnd = Mean + StdDeviation * StdDistribution

            Return GaussianRnd
        End Function


        ''' <summary>
        '''   Generates values from a triangular distribution
        '''   Triangular distribution is a continuous probability distribution with:
        '''       lower limit a
        '''       upper limit b 
        '''       mode c
        '''   where a less than b 
        '''   c is higher than or equal a but lessthan or equal b
        ''' </summary>
        ''' <param name = "min">Minimum</param>
        ''' <param name = "max">Maximum</param>
        ''' <param name = "mode">Mode (most frequent value)</param>
        ''' <returns></returns>
        Public Function NextTriangular(ByVal min As Double, ByVal max As Double, ByVal mode As Double) As Double
            Dim u = _Gen.NextDouble()

            If (u < (mode - min) / (max - min)) Then
                Return min + Math.Sqrt(u * (max - min) * (mode - min))
            Else
                Return max - Math.Sqrt((1 - u) * (max - min) * (max - mode))
            End If
        End Function

        ''' <summary>
        '''   Shuffles a list in O(n) time by using the Fisher-Yates/Knuth algorithm
        ''' </summary>
        ''' <param name = "list"></param>
        Public Sub Shuffle(ByVal list As IList)
            For i = 0 To list.Count - 1
                Dim j = _Gen.Next(0, i + 1)

                Dim temp = list(j)
                list(j) = list(i)
                list(i) = temp
            Next i
        End Sub
    End Class
End Namespace


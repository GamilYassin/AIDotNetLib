
Namespace CommonLib
    ''' <summary>
    ''' MathOperations class contains common shared methods of math operations:
    '''      Map functions
    '''      Constraint function
    '''      Linear Interpolation
    '''      Normalize function
    ''' </summary>
    Public Class MathFunctions


        ''' <summary>
        ''' Re-maps a number from one range to another. In the example above,
        ''' </summary>
        ''' <param name="value"> the incoming value to be converted </param>
        ''' <param name="start1"> lower bound of the value's current range </param>
        ''' <param name="stop1"> upper bound of the value's current range </param>
        ''' <param name="start2"> lower bound of the value's target range </param>
        ''' <param name="stop2"> upper bound of the value's target range </param>
        Public Shared Function Map(ByVal value As Single, ByVal start1 As Single, ByVal stop1 As Single, ByVal start2 As Single, ByVal stop2 As Single) As Single
            Dim Output As Single = start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1))
            Dim errMessage As String = Nothing

            If Output <> Output Then
                errMessage = "NaN (not a number)"
                Throw New Exception(errMessage)
            ElseIf Output = Single.NegativeInfinity OrElse Output = Single.PositiveInfinity Then
                errMessage = "infinity"
                Throw New Exception(errMessage)
            End If
            Return Output
        End Function


        ''' <summary>
        ''' Normalizes a number from another range into a value between 0 and 1.
        ''' Identical to map(value, low, high, 0, 1);
        ''' Numbers outside the range are not clamped to 0 and 1, because
        ''' out-of-range values are often intentional and useful.
        ''' </summary>
        ''' <param name="value"> the incoming value to be converted </param>
        ''' <param name="start"> lower bound of the value's current range </param>
        ''' <param name="stop"> upper bound of the value's current range </param>
        Public Shared Function Norm(ByVal value As Single, ByVal start As Single, ByVal [stop] As Single) As Single
            Return (value - start) / ([stop] - start)
        End Function

        ''' <summary>
        ''' Calculates a number between two numbers at a specific increment. 
        ''' Amount parameter is the amount to interpolate between the two values
        ''' </summary>
        ''' <param name="Start"> first value </param>
        ''' <param name="[Stop]"> second value </param>
        ''' <param name="InterpAmount"> float between 0.0 and 1.0 </param>
        Public Shared Function Lerp(ByVal Start As Single, ByVal [Stop] As Single, ByVal InterpAmount As Single) As Single
            Return Start + ([Stop] - Start) * InterpAmount
        End Function

        ''' <summary>
        ''' Constrains value between min and max values
        '''   if less than min, return min
        '''   more than max, return max
        '''   otherwise return same value
        ''' </summary>
        ''' <param name="Value"></param>
        ''' <param name="min"></param>
        ''' <param name="max"></param>
        ''' <returns></returns>
        Public Function Constraint(Value As Single, min As Single, max As Single) As Single
            If Value <= min Then
                Return min
            ElseIf Value >= max Then
                Return max
            End If
            Return Value
        End Function

        ''' <summary>
        ''' Generates 8 bit array of an integer, value from 0 to 255
        ''' </summary>
        ''' <param name="Value"></param>
        ''' <returns></returns>
        Public Function GetBitArray(Value As Integer) As Integer()
            Dim Result(7) As Integer
            Dim sValue As String
            Dim cValue() As Char

            Value = Constraint(Value, 0, 255)
            sValue = Convert.ToString(Value, 2).PadLeft(8, "0"c)
            cValue = sValue.ToArray
            For i As Integer = 0 To cValue.Count - 1
                If cValue(i) = "1"c Then
                    Result(i) = 1
                Else
                    Result(i) = 0
                End If
            Next
            Return Result
        End Function

        ''' <summary>
        ''' Generates 8 bit array of an integer, value from 0 to 255
        ''' </summary>
        ''' <param name="Value"></param>
        ''' <returns></returns>
        Public Function GetBits(Value As Integer) As Integer()
            Dim _Arr As BitArray
            Dim Result() As Integer = {0, 0, 0, 0, 0, 0, 0, 0}

            _Arr = New BitArray(New Integer() {Value})
            _Arr.CopyTo(Result, 0)
            Return Result
        End Function


        Public Function GetBitsString(Value As Integer) As String
            Dim _Array() As Integer = GetBitArray(Value)
            Dim Result As String = ""

            For I As Integer = 0 To _Array.Length - 1
                Result += _Array(I).ToString
            Next
            Return Result
        End Function

        ''' <summary>
        ''' This function implements Hypoethsis function of a Neuron
        ''' h(x) = sum (Input * Weight)
        ''' </summary>
        ''' <param name="Inputs"></param>
        ''' <param name="Weights"></param>
        ''' <returns></returns>
        Public Shared Function Hypothesis(Inputs As Matrix1D, Weights As Matrix1D) As Single
            Dim Result As New Matrix1D(Inputs)

            Result = Inputs.Product(Weights)
            Return Result.Sum
        End Function

        ''' <summary>
        ''' Hypot is a mathematical function defined to calculate the length of the hypotenuse of a right-angle triangle. 
        ''' It was designed to avoid errors arising due to limited-precision calculations performed on computers 
        ''' sqrt(a^2 + b^2) without under/overflow.
        ''' </summary>
        ''' <param name="a"></param>
        ''' <param name="b"></param>
        ''' <returns></returns>
        Public Shared Function Hypot(ByVal a As Double, ByVal b As Double) As Double
            Dim r As Double
            If Math.Abs(a) > Math.Abs(b) Then
                r = b / a
                r = Math.Abs(a) * Math.Sqrt(1 + r * r)
            ElseIf b <> 0 Then
                r = a / b
                r = Math.Abs(b) * Math.Sqrt(1 + r * r)
            Else
                r = 0.0
            End If
            Return r
        End Function
    End Class

End Namespace


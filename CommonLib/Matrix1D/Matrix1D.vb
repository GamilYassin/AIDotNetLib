

Namespace CommonLib

    ''' <summary>
    ''' Wrap methods and properities of 1D matrix object
    ''' Store its vales as single dimensional array
    ''' </summary>
    Public Class Matrix1D
        Implements IMatrix

        ''' <summary>
        ''' Array to hold matrix values - 1D array
        ''' </summary>
        Private _Values() As Single
        ''' <summary>
        ''' Size or capacity of matrix - Simply number of stored elements in Values arrary
        ''' </summary>
        Private _Size As Integer

        ''' <summary>
        ''' Constructor - Redim Array to only 1 element
        ''' </summary>
        Public Sub New()
            ReDim _Values(1)
        End Sub

        ''' <summary>
        ''' Constructor that receives values array
        ''' </summary>
        ''' <param name="_InitialValues">Initial Elements array</param>
        Public Sub New(_InitialValues() As Single)
            ReDim _Values(_InitialValues.Count - 1)
            Me.Values = _InitialValues
            Me._Size = Me._Values.Count
        End Sub

        ''' <summary>
        ''' Create new matrix same size and values of paased matrix m1
        ''' </summary>
        ''' <param name="m1"></param>
        Public Sub New(m1 As Matrix1D)
            Me.New(m1.Values)
        End Sub

        ''' <summary>
        ''' Constructor that receives values integer array
        ''' All elements are converted to Single type
        ''' </summary>
        ''' <param name="_InitialValues">Initial Elements array</param>
        Public Sub New(_InitialValues() As Integer)
            ReDim _Values(_InitialValues.Count - 1)
            For I As Integer = 0 To Me._Values.Count - 1
                Me.Values(I) = CSng(_InitialValues(I))
            Next
            Me._Size = Me._Values.Count
        End Sub

        ''' <summary>
        ''' Constructor, receives size of matrix
        ''' All elements will be randomized for values between 1 and -1
        ''' </summary>
        ''' <param name="_ReqCapacity">Matrix size</param>
        Public Sub New(_ReqCapacity As Integer)
            ReDim _Values(_ReqCapacity - 1)
            Me.RandomizeValues(-1, 1)
            Me._Size = Me._Values.Count
        End Sub

        ''' <summary>
        ''' Size or capacity of matrix - Simply number of stored elements in Values arrary
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Size As Integer Implements IMatrix.Size
            Get
                Return Me._Values.Count
            End Get
        End Property

        ''' <summary>
        ''' Array to hold matrix values - 1D array
        ''' </summary>
        ''' <returns></returns>
        Public Property Values As Single()
            Get
                Return _Values
            End Get
            Set(value As Single())
                ReDim Me._Values(value.Count - 1)
                Me._Values = value
                Me._Size = Me._Values.Count
            End Set
        End Property

        ''' <summary>
        ''' Return min value within matrix elements
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property GetMin() As Single
            Get
                If Me._Values Is Nothing Then Throw New Exception("Matrix Values has not been set")
                Dim min As Single = 0

                For I As Integer = 0 To Me.Size - 1
                    If I = 0 Then min = Me.GetValue(I)
                    If Me.GetValue(I) < min Then min = Me.GetValue(I)
                Next
                Return min
            End Get
        End Property

        ''' <summary>
        ''' Return max value within matrix elements
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property GetMax() As Single
            Get
                If Me._Values Is Nothing Then Throw New Exception("Matrix Values has not been set")
                Dim max As Single = 0

                For I As Integer = 0 To Me.Size - 1
                    If I = 0 Then max = Me.GetValue(I)
                    If Me.GetValue(I) > max Then max = Me.GetValue(I)
                Next
                Return max
            End Get
        End Property

        ''' <summary>
        ''' Implement Matrix Product method between 2 metrices m1 and m2
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function Product(m1 As Matrix1D, m2 As Matrix1D) As Matrix1D Implements IMatrix.Product
            Dim Output As New Matrix1D(Math.Min(m1.Size, m2.Size))

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) * m2.Values(Index)
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix Product method between current object and m2
        ''' </summary>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function Product(m2 As Matrix1D) As Matrix1D Implements IMatrix.Product
            Return Product(Me, m2)
        End Function

        ''' <summary>
        ''' Implement Matrix Product method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar product of matrix elements</returns>
        Public Function Product(m1 As Matrix1D, Scalar As Integer) As Matrix1D Implements IMatrix.Product
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) * Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix Product method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar product of matrix elements</returns>
        Public Function Product(m1 As Matrix1D, Scalar As Single) As Matrix1D Implements IMatrix.Product
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) * Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix Product method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar product of matrix elements</returns>
        Public Function Product(m1 As Matrix1D, Scalar As Double) As Matrix1D Implements IMatrix.Product
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) * Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix Product method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar product of matrix elements</returns>
        Public Function Product(Scalar As Integer) As Matrix1D Implements IMatrix.Product
            Return Product(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix Product method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar product of matrix elements</returns>
        Public Function Product(Scalar As Single) As Matrix1D Implements IMatrix.Product
            Return Product(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix Product method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar product of matrix elements</returns>
        Public Function Product(Scalar As Double) As Matrix1D Implements IMatrix.Product
            Return Product(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between 2 metrices m1 and m2
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function Add(m1 As Matrix1D, m2 As Matrix1D) As Matrix1D Implements IMatrix.Add
            Dim Output As New Matrix1D(Math.Min(m1.Size, m2.Size))

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) + m2.Values(Index)
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between current object and m2
        ''' </summary>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function Add(m2 As Matrix1D) As Matrix1D Implements IMatrix.Add
            Return Add(Me, m2)
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar addition of matrix elements</returns>
        Public Function Add(m1 As Matrix1D, Scalar As Integer) As Matrix1D Implements IMatrix.Add
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) + Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar addition of matrix elements</returns>
        Public Function Add(m1 As Matrix1D, Scalar As Single) As Matrix1D Implements IMatrix.Add
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) + Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar addition of matrix elements</returns>
        Public Function Add(m1 As Matrix1D, Scalar As Double) As Matrix1D Implements IMatrix.Add
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) + Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar addition of matrix elements</returns>
        Public Function Add(Scalar As Integer) As Matrix1D Implements IMatrix.Add
            Return Add(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar addition of matrix elements</returns>
        Public Function Add(Scalar As Single) As Matrix1D Implements IMatrix.Add
            Return Add(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix addition method between matrix m1 and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar (single) value</param>
        ''' <returns>Scalar product of matrix elements</returns>
        Public Function Add(Scalar As Double) As Matrix1D Implements IMatrix.Add
            Return Add(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix subtraction method between 2 metrices m1 and m2
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function [Sub](m1 As Matrix1D, m2 As Matrix1D) As Matrix1D Implements IMatrix.Sub
            Dim Output As New Matrix1D(Math.Min(m1.Size, m2.Size))

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) - m2.Values(Index)
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix subtraction method between object and m2
        ''' </summary>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function [Sub](m2 As Matrix1D) As Matrix1D Implements IMatrix.Sub
            Return [Sub](Me, m2)
        End Function


        ''' <summary>
        ''' Implement Matrix subtraction method between matrix and scalar
        ''' </summary>
        ''' <param name="m1">matrix</param>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>Scalar subtract of matrix elements</returns>
        Public Function [Sub](m1 As Matrix1D, Scalar As Integer) As Matrix1D Implements IMatrix.Sub
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) - Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix subtraction method between matrix and scalar
        ''' </summary>
        ''' <param name="m1">matrix</param>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>Scalar subtract of matrix elements</returns>
        Public Function [Sub](m1 As Matrix1D, Scalar As Single) As Matrix1D Implements IMatrix.Sub
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) - Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix subtraction method between matrix and scalar
        ''' </summary>
        ''' <param name="m1">matrix</param>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>Scalar subtract of matrix elements</returns>
        Public Function [Sub](m1 As Matrix1D, Scalar As Double) As Matrix1D Implements IMatrix.Sub
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) - Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix subtraction method between matrix and scalar
        ''' </summary>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>Scalar subtract of matrix elements</returns>
        Public Function [Sub](Scalar As Integer) As Matrix1D Implements IMatrix.Sub
            Return [Sub](Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix subtraction method between matrix and scalar
        ''' </summary>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>Scalar subtract of matrix elements</returns>
        Public Function [Sub](Scalar As Single) As Matrix1D Implements IMatrix.Sub
            Return [Sub](Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix subtraction method between matrix and scalar
        ''' </summary>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>Scalar subtract of matrix elements</returns>
        Public Function [Sub](Scalar As Double) As Matrix1D Implements IMatrix.Sub
            Return [Sub](Me, Scalar)
        End Function

        ''' <summary>
        ''' Sum of all matrix elements
        ''' </summary>
        ''' <returns>sum of all matrix elements</returns>
        Public Function Sum() As Single Implements IMatrix.Sum
            Dim Result As Single = 0

            For Index As Integer = 0 To Me.Size - 1
                Result += Me.GetValue(Index)
            Next
            Return Result
        End Function

        ''' <summary>
        ''' Squared Sum of all matrix elements
        ''' </summary>
        ''' <returns>sum of all matrix elements</returns>
        Public Function SquaredSum() As Single
            Dim Result As Single = 0

            For Index As Integer = 0 To Me.Size - 1
                Result += Me.GetValue(Index) * Me.GetValue(Index)
            Next
            Return Result
        End Function

        ''' <summary>
        ''' Implement Matrix divide method between 2 metrices m1 and m2
        ''' </summary>
        ''' <param name="m1">1st matrix</param>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function Divide(m1 As Matrix1D, m2 As Matrix1D) As Matrix1D Implements IMatrix.Divide
            Dim Output As New Matrix1D(Math.Min(m1.Size, m2.Size))

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) / m2.Values(Index)
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix divide method between object and m2
        ''' </summary>
        ''' <param name="m2">2nd matrix</param>
        ''' <returns>New matrix of same size of min size between m1 and m2</returns>
        Public Function Divide(m2 As Matrix1D) As Matrix1D Implements IMatrix.Divide
            Return Divide(Me, m2)
        End Function


        ''' <summary>
        ''' Implement Matrix divide method between matrix and scalar value
        ''' </summary>
        ''' <param name="m1">matrix</param>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>new matrix same size as m1</returns>
        Public Function Divide(m1 As Matrix1D, Scalar As Integer) As Matrix1D Implements IMatrix.Divide
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) / Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix divide method between matrix and scalar value
        ''' </summary>
        ''' <param name="m1">matrix</param>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>new matrix same size as m1</returns>
        Public Function Divide(m1 As Matrix1D, Scalar As Single) As Matrix1D Implements IMatrix.Divide
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) / Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix divide method between matrix and scalar value
        ''' </summary>
        ''' <param name="m1">matrix</param>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>new matrix same size as m1</returns>
        Public Function Divide(m1 As Matrix1D, Scalar As Double) As Matrix1D Implements IMatrix.Divide
            Dim Output As New Matrix1D(m1.Size)

            For Index As Integer = 0 To Output.Size - 1
                Output.Values(Index) = m1.Values(Index) / Scalar
            Next
            Return Output
        End Function

        ''' <summary>
        ''' Implement Matrix divide method between matrix and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>new matrix same size as m1</returns>
        Public Function Divide(Scalar As Integer) As Matrix1D Implements IMatrix.Divide
            Return Divide(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix divide method between matrix and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>new matrix same size as m1</returns>
        Public Function Divide(Scalar As Single) As Matrix1D Implements IMatrix.Divide
            Return Divide(Me, Scalar)
        End Function

        ''' <summary>
        ''' Implement Matrix divide method between matrix and scalar value
        ''' </summary>
        ''' <param name="Scalar">scalar value</param>
        ''' <returns>new matrix same size as m1</returns>
        Public Function Divide(Scalar As Double) As Matrix1D Implements IMatrix.Divide
            Return Divide(Me, Scalar)
        End Function

        ''' <summary>
        ''' Overrides ToString function of base object
        ''' </summary>
        ''' <returns>String represents matric contents [v1,v2,v3,....,vn]</returns>
        Public Overrides Function ToString() As String Implements IMatrix.ToString
            Dim Result As String = "Values["

            For I As Integer = 0 To Me.Size - 1
                Result += Me.Values(I).ToString
                If I <> Me.Size - 1 Then
                    Result += ","
                End If
            Next
            Result += "]"
            Return Result
        End Function

        Private _Rnd As New RandomFactory()

        ''' <summary>
        ''' Randomize matrix elements between min and max values
        ''' </summary>
        ''' <param name="min"></param>
        ''' <param name="max"></param>
        Public Sub RandomizeValues(min As Double, max As Double)
            For I As Integer = 0 To Me._Values.Count - 1
                Me._Values(I) = _Rnd.GetRandomSngl(min, max)
            Next
        End Sub

        ''' <summary>
        ''' Copy contents of m1 into object starting from starting index
        ''' </summary>
        ''' <param name="m1">matrix to be copied</param>
        ''' <param name="StartingIndex">starting index of copy</param>
        ''' <returns>new instance of object</returns>
        Public Function Copy(m1 As Matrix1D, StartingIndex As Integer) As Matrix1D Implements IMatrix.Copy
            Dim I As Integer

            For I = StartingIndex To Me._Values.Count - 1
                If I - StartingIndex > m1.Values.Count - 1 Then Exit For
                Me._Values(I) = m1.Values(I - StartingIndex)
            Next
            Return Me
        End Function


        ''' <summary>
        ''' Copy contents of m1 into object starting from starting index = 0
        ''' </summary>
        ''' <param name="m1">matrix to be copied</param>
        ''' <returns>new instance of object</returns>
        Public Function Copy(m1 As Matrix1D) As Matrix1D Implements IMatrix.Copy
            Return Copy(m1, 0)
        End Function

        ''' <summary>
        ''' Copy contents of object into itself starting from starting index = 0
        ''' </summary>
        ''' <returns>new instance of object</returns>
        Public Function Copy(StartingIndex As Integer) As Matrix1D Implements IMatrix.Copy
            Return Copy(Me, StartingIndex)
        End Function

        ''' <summary>
        ''' Forces all elements of matrix to ForcedValue
        ''' </summary>
        ''' <param name="ForcedValue"></param>
        Public Sub ForceValues(ForcedValue As Single) Implements IMatrix.ForceValues
            For I As Integer = 0 To Me._Values.Count - 1
                Me._Values(I) = ForcedValue
            Next
        End Sub

        ''' <summary>
        ''' Return index element of matrix
        ''' Index starts with 0
        ''' </summary>
        ''' <param name="Index">0 indexed element</param>
        ''' <returns>Element value at index of Index</returns>
        Public Function GetValue(Index As Integer) As Single Implements IMatrix.GetValue
            If Index > Me.Values.Count Then Return Nothing
            Return Me._Values(Index)
        End Function

        ''' <summary>
        ''' Set value of matrix element at position Index
        ''' 0 indexed positions
        ''' </summary>
        ''' <param name="Index">Index (Poistion) starting from 0</param>
        ''' <param name="Value">New value</param>
        Public Sub SetValue(Index As Integer, Value As Single) Implements IMatrix.SetValue
            If Index > Me.Values.Count Then Exit Sub
            Me._Values(Index) = Value
        End Sub
    End Class
End Namespace


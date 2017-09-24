Imports System
Imports System.Runtime.Serialization

Namespace CommonLib
    ''' <summary>.NET GeneralMatrix class.
    ''' 
    ''' The .NET GeneralMatrix Class provides the fundamental operations of numerical
    ''' linear algebra.  Various constructors create Matrices from two dimensional
    ''' arrays of double precision floating point numbers.  Various "gets" and
    ''' "sets" provide access to submatrices and matrix elements.  Several methods 
    ''' implement basic matrix arithmetic, including matrix addition and
    ''' multiplication, matrix norms, and element-by-element array operations.
    ''' Methods for reading and printing matrices are also included.  All the
    ''' operations in this version of the GeneralMatrix Class involve real matrices.
    ''' Complex matrices may be handled in a future version.
    ''' 
    ''' Five fundamental matrix decompositions, which consist of pairs or triples
    ''' of matrices, permutation vectors, and the like, produce results in five
    ''' decomposition classes.  These decompositions are accessed by the GeneralMatrix
    ''' class to compute solutions of simultaneous linear equations, determinants,
    ''' inverses and other matrix functions.  The five decompositions are:
    ''' Cholesky Decomposition of symmetric, positive definite matrices.
    ''' LU Decomposition of rectangular matrices.
    ''' QR Decomposition of rectangular matrices.
    ''' Singular Value Decomposition of rectangular matrices.
    ''' Eigenvalue Decomposition of both symmetric and nonsymmetric square matrices.
    ''' Example of use:
    ''' Solve a linear system A x = b and compute the residual norm, ||b - A x||.
    ''' double[][] vals = {{1.,2.,3},{4.,5.,6.},{7.,8.,10.}};
    ''' GeneralMatrix A = new GeneralMatrix(vals);
    ''' GeneralMatrix b = GeneralMatrix.Random(3,1);
    ''' GeneralMatrix x = A.Solve(b);
    ''' GeneralMatrix r = A.Multiply(x).Subtract(b);
    ''' double rnorm = r.NormInf();
    ''' </summary>
    <Serializable>
    Public Class Matrix2D
        Implements System.ICloneable, System.Runtime.Serialization.ISerializable, System.IDisposable

#Region "Class variables"

        ''' <summary>Array for internal storage of elements.
        ''' @serial internal array storage.
        ''' </summary>
        Private _ArrayValues()() As Double

        ''' <summary>Row and column dimensions.
        ''' @serial row dimension.
        ''' @serial column dimension.
        ''' </summary>
        Private _RowCount, _ColumnCount As Integer

#End Region '  Class variables

#Region "Constructors"

        ''' <summary>Construct an m-by-n matrix of zeros. </summary>
        ''' <param name="m">   Number of rows.
        ''' </param>
        ''' <param name="n">   Number of colums.
        ''' </param>
        Public Sub New(ByVal m As Integer, ByVal n As Integer)
            Me._RowCount = m
            Me._ColumnCount = n
            _ArrayValues = New Double(m - 1)() {}
            For i As Integer = 0 To m - 1
                _ArrayValues(i) = New Double(n - 1) {}
            Next i
        End Sub

        ''' <summary>Construct an m-by-n constant matrix.</summary>
        ''' <param name="m">   Number of rows.
        ''' </param>
        ''' <param name="n">   Number of colums.
        ''' </param>
        ''' <param name="s">   Fill the matrix with this scalar value.
        ''' </param>
        Public Sub New(ByVal m As Integer, ByVal n As Integer, ByVal s As Double)
            Me._RowCount = m
            Me._ColumnCount = n
            _ArrayValues = New Double(m - 1)() {}
            For i As Integer = 0 To m - 1
                _ArrayValues(i) = New Double(n - 1) {}
            Next i
            For i As Integer = 0 To m - 1
                For j As Integer = 0 To n - 1
                    _ArrayValues(i)(j) = s
                Next j
            Next i
        End Sub

        ''' <summary>Construct a matrix from a 2-D array.</summary>
        ''' <param name="A">   Two-dimensional array of doubles.
        ''' </param>
        ''' <exception cref="System.ArgumentException">   All rows must have the same length
        ''' </exception>
        ''' <seealso cref="Create">
        ''' </seealso>
        Public Sub New(ByVal A()() As Double)
            _RowCount = A.Length
            _ColumnCount = A(0).Length
            For i As Integer = 0 To _RowCount - 1
                If A(i).Length <> _ColumnCount Then
                    Throw New System.ArgumentException("All rows must have the same length.")
                End If
            Next i
            Me._ArrayValues = A
        End Sub

        ''' <summary>Construct a matrix quickly without checking arguments.</summary>
        ''' <param name="A">   Two-dimensional array of doubles.
        ''' </param>
        ''' <param name="m">   Number of rows.
        ''' </param>
        ''' <param name="n">   Number of colums.
        ''' </param>
        Public Sub New(ByVal A()() As Double, ByVal m As Integer, ByVal n As Integer)
            Me._ArrayValues = A
            Me._RowCount = m
            Me._ColumnCount = n
        End Sub

        ''' <summary>Construct a matrix from a one-dimensional packed array</summary>
        ''' <param name="vals">One-dimensional array of doubles, packed by columns (ala Fortran).
        ''' </param>
        ''' <param name="m">   Number of rows.
        ''' </param>
        ''' <exception cref="System.ArgumentException">   Array length must be a multiple of m.
        ''' </exception>
        Public Sub New(ByVal vals() As Double, ByVal m As Integer)
            Me._RowCount = m
            _ColumnCount = (If(m <> 0, vals.Length \ m, 0))
            If m * _ColumnCount <> vals.Length Then
                Throw New System.ArgumentException("Array length must be a multiple of m.")
            End If
            _ArrayValues = New Double(m - 1)() {}
            For i As Integer = 0 To m - 1
                _ArrayValues(i) = New Double(_ColumnCount - 1) {}
            Next i
            For i As Integer = 0 To m - 1
                For j As Integer = 0 To _ColumnCount - 1
                    _ArrayValues(i)(j) = vals(i + j * m)
                Next j
            Next i
        End Sub
#End Region '  Constructors


#Region "Public Properties"
        ''' <summary>Access the internal two-dimensional array.</summary>
        ''' <returns>     Pointer to the two-dimensional array of matrix elements.
        ''' </returns>
        Public Overridable ReadOnly Property Array() As Double()()
            Get
                Return _ArrayValues
            End Get
        End Property
        ''' <summary>Copy the internal two-dimensional array.</summary>
        ''' <returns>     Two-dimensional array copy of matrix elements.
        ''' </returns>
        Public Overridable ReadOnly Property ArrayCopy() As Double()()
            Get
                Dim C(_RowCount - 1)() As Double
                For i As Integer = 0 To _RowCount - 1
                    C(i) = New Double(_ColumnCount - 1) {}
                Next i
                For i As Integer = 0 To _RowCount - 1
                    For j As Integer = 0 To _ColumnCount - 1
                        C(i)(j) = _ArrayValues(i)(j)
                    Next j
                Next i
                Return C
            End Get

        End Property

        ''' <summary>Make a one-dimensional column packed copy of the internal array.</summary>
        ''' <returns>     Matrix elements packed in a one-dimensional array by columns.
        ''' </returns>
        Public Overridable ReadOnly Property ColumnPackedCopy() As Double()
            Get
                Dim vals((_RowCount * _ColumnCount) - 1) As Double
                For i As Integer = 0 To _RowCount - 1
                    For j As Integer = 0 To _ColumnCount - 1
                        vals(i + j * _RowCount) = _ArrayValues(i)(j)
                    Next j
                Next i
                Return vals
            End Get

        End Property

        ''' <summary>Make a one-dimensional row packed copy of the internal array.</summary>
        ''' <returns>     Matrix elements packed in a one-dimensional array by rows.
        ''' </returns>
        Public Overridable ReadOnly Property RowPackedCopy() As Double()
            Get
                Dim vals((_RowCount * _ColumnCount) - 1) As Double
                For i As Integer = 0 To _RowCount - 1
                    For j As Integer = 0 To _ColumnCount - 1
                        vals(i * _ColumnCount + j) = _ArrayValues(i)(j)
                    Next j
                Next i
                Return vals
            End Get
        End Property

        ''' <summary>Get row dimension.</summary>
        ''' <returns>     m, the number of rows.
        ''' </returns>
        Public Overridable ReadOnly Property RowDimension() As Integer
            Get
                Return _RowCount
            End Get
        End Property

        ''' <summary>Get column dimension.</summary>
        ''' <returns>     n, the number of columns.
        ''' </returns>
        Public Overridable ReadOnly Property ColumnDimension() As Integer
            Get
                Return _ColumnCount
            End Get
        End Property
#End Region ' Public Properties

#Region "    Public Methods"

        ''' <summary>Construct a matrix from a copy of a 2-D array.</summary>
        ''' <param name="A">   Two-dimensional array of doubles.
        ''' </param>
        ''' <exception cref="System.ArgumentException">   All rows must have the same length
        ''' </exception>

        Public Shared Function Create(ByVal A()() As Double) As Matrix2D
            Dim m As Integer = A.Length
            Dim n As Integer = A(0).Length
            Dim X As New Matrix2D(m, n)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To m - 1
                If A(i).Length <> n Then
                    Throw New System.ArgumentException("All rows must have the same length.")
                End If
                For j As Integer = 0 To n - 1
                    C(i)(j) = A(i)(j)
                Next j
            Next i
            'X.Array = C
            Return X
        End Function

        ''' <summary>Make a deep copy of a matrix</summary>

        Public Overridable Function Copy() As Matrix2D
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = _ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>Get a single element.</summary>
        ''' <param name="i">   Row index.
        ''' </param>
        ''' <param name="j">   Column index.
        ''' </param>
        ''' <returns>     A(i,j)
        ''' </returns>
        ''' <exception cref="System.IndexOutOfRangeException">  
        ''' </exception>

        Public Overridable Function GetElement(ByVal i As Integer, ByVal j As Integer) As Double
            Return _ArrayValues(i)(j)
        End Function

        ''' <summary>Get a submatrix.</summary>
        ''' <param name="i0">  Initial row index
        ''' </param>
        ''' <param name="i1">  Final row index
        ''' </param>
        ''' <param name="j0">  Initial column index
        ''' </param>
        ''' <param name="j1">  Final column index
        ''' </param>
        ''' <returns>     A(i0:i1,j0:j1)
        ''' </returns>
        ''' <exception cref="System.IndexOutOfRangeException">   Submatrix indices
        ''' </exception>

        Public Overridable Function GetMatrix(ByVal i0 As Integer, ByVal i1 As Integer, ByVal j0 As Integer, ByVal j1 As Integer) As Matrix2D
            Dim X As New Matrix2D(i1 - i0 + 1, j1 - j0 + 1)
            Dim B()() As Double = X.Array
            Try
                For i As Integer = i0 To i1
                    For j As Integer = j0 To j1
                        B(i - i0)(j - j0) = _ArrayValues(i)(j)
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
            Return X
        End Function

        ''' <summary>Get a submatrix.</summary>
        ''' <param name="r">   Array of row indices.
        ''' </param>
        ''' <param name="c">   Array of column indices.
        ''' </param>
        ''' <returns>     A(r(:),c(:))
        ''' </returns>
        ''' <exception cref="System.IndexOutOfRangeException">   Submatrix indices
        ''' </exception>

        Public Overridable Function GetMatrix(ByVal r() As Integer, ByVal c() As Integer) As Matrix2D
            Dim X As New Matrix2D(r.Length, c.Length)
            Dim B()() As Double = X.Array
            Try
                For i As Integer = 0 To r.Length - 1
                    For j As Integer = 0 To c.Length - 1
                        B(i)(j) = _ArrayValues(r(i))(c(j))
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
            Return X
        End Function

        ''' <summary>Get a submatrix.</summary>
        ''' <param name="i0">  Initial row index
        ''' </param>
        ''' <param name="i1">  Final row index
        ''' </param>
        ''' <param name="c">   Array of column indices.
        ''' </param>
        ''' <returns>     A(i0:i1,c(:))
        ''' </returns>
        ''' <exception cref="System.IndexOutOfRangeException">   Submatrix indices
        ''' </exception>

        Public Overridable Function GetMatrix(ByVal i0 As Integer, ByVal i1 As Integer, ByVal c() As Integer) As Matrix2D
            Dim X As New Matrix2D(i1 - i0 + 1, c.Length)
            Dim B()() As Double = X.Array
            Try
                For i As Integer = i0 To i1
                    For j As Integer = 0 To c.Length - 1
                        B(i - i0)(j) = _ArrayValues(i)(c(j))
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
            Return X
        End Function

        ''' <summary>Get a submatrix.</summary>
        ''' <param name="r">   Array of row indices.
        ''' </param>
        ''' <param name="j0">  Initial column index
        ''' </param>
        ''' <param name="j1">  Final column index
        ''' </param>
        ''' <returns>     A(r(:),j0:j1)
        ''' </returns>
        ''' <exception cref="System.IndexOutOfRangeException">   Submatrix indices
        ''' </exception>

        Public Overridable Function GetMatrix(ByVal r() As Integer, ByVal j0 As Integer, ByVal j1 As Integer) As Matrix2D
            Dim X As New Matrix2D(r.Length, j1 - j0 + 1)
            Dim B()() As Double = X.Array
            Try
                For i As Integer = 0 To r.Length - 1
                    For j As Integer = j0 To j1
                        B(i)(j - j0) = _ArrayValues(r(i))(j)
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
            Return X
        End Function

        ''' <summary>Set a single element.</summary>
        ''' <param name="i">   Row index.
        ''' </param>
        ''' <param name="j">   Column index.
        ''' </param>
        ''' <param name="s">   A(i,j).
        ''' </param>
        ''' <exception cref="System.IndexOutOfRangeException">  
        ''' </exception>

        Public Overridable Sub SetElement(ByVal i As Integer, ByVal j As Integer, ByVal s As Double)
            _ArrayValues(i)(j) = s
        End Sub

        ''' <summary>Set a submatrix.</summary>
        ''' <param name="i0">  Initial row index
        ''' </param>
        ''' <param name="i1">  Final row index
        ''' </param>
        ''' <param name="j0">  Initial column index
        ''' </param>
        ''' <param name="j1">  Final column index
        ''' </param>
        ''' <param name="X">   A(i0:i1,j0:j1)
        ''' </param>
        ''' <exception cref="System.IndexOutOfRangeException">  Submatrix indices
        ''' </exception>

        Public Overridable Sub SetMatrix(ByVal i0 As Integer, ByVal i1 As Integer, ByVal j0 As Integer, ByVal j1 As Integer, ByVal X As Matrix2D)
            Try
                For i As Integer = i0 To i1
                    For j As Integer = j0 To j1
                        _ArrayValues(i)(j) = X.GetElement(i - i0, j - j0)
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
        End Sub

        ''' <summary>Set a submatrix.</summary>
        ''' <param name="r">   Array of row indices.
        ''' </param>
        ''' <param name="c">   Array of column indices.
        ''' </param>
        ''' <param name="X">   A(r(:),c(:))
        ''' </param>
        ''' <exception cref="System.IndexOutOfRangeException">  Submatrix indices
        ''' </exception>

        Public Overridable Sub SetMatrix(ByVal r() As Integer, ByVal c() As Integer, ByVal X As Matrix2D)
            Try
                For i As Integer = 0 To r.Length - 1
                    For j As Integer = 0 To c.Length - 1
                        _ArrayValues(r(i))(c(j)) = X.GetElement(i, j)
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
        End Sub

        ''' <summary>Set a submatrix.</summary>
        ''' <param name="r">   Array of row indices.
        ''' </param>
        ''' <param name="j0">  Initial column index
        ''' </param>
        ''' <param name="j1">  Final column index
        ''' </param>
        ''' <param name="X">   A(r(:),j0:j1)
        ''' </param>
        ''' <exception cref="System.IndexOutOfRangeException"> Submatrix indices
        ''' </exception>

        Public Overridable Sub SetMatrix(ByVal r() As Integer, ByVal j0 As Integer, ByVal j1 As Integer, ByVal X As Matrix2D)
            Try
                For i As Integer = 0 To r.Length - 1
                    For j As Integer = j0 To j1
                        _ArrayValues(r(i))(j) = X.GetElement(i, j - j0)
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
        End Sub

        ''' <summary>Set a submatrix.</summary>
        ''' <param name="i0">  Initial row index
        ''' </param>
        ''' <param name="i1">  Final row index
        ''' </param>
        ''' <param name="c">   Array of column indices.
        ''' </param>
        ''' <param name="X">   A(i0:i1,c(:))
        ''' </param>
        ''' <exception cref="System.IndexOutOfRangeException">  Submatrix indices
        ''' </exception>

        Public Overridable Sub SetMatrix(ByVal i0 As Integer, ByVal i1 As Integer, ByVal c() As Integer, ByVal X As Matrix2D)
            Try
                For i As Integer = i0 To i1
                    For j As Integer = 0 To c.Length - 1
                        _ArrayValues(i)(c(j)) = X.GetElement(i - i0, j)
                    Next j
                Next i
            Catch e As System.IndexOutOfRangeException
                Throw New System.IndexOutOfRangeException("Submatrix indices", e)
            End Try
        End Sub

        ''' <summary>Matrix transpose.</summary>
        ''' <returns>    A'
        ''' </returns>

        Public Overridable Function Transpose() As Matrix2D
            Dim X As New Matrix2D(_ColumnCount, _RowCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(j)(i) = _ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>One norm</summary>
        ''' <returns>    maximum column sum.
        ''' </returns>

        Public Overridable Function Norm1() As Double
            Dim f As Double = 0
            For j As Integer = 0 To _ColumnCount - 1
                Dim s As Double = 0
                For i As Integer = 0 To _RowCount - 1
                    s += System.Math.Abs(_ArrayValues(i)(j))
                Next i
                f = System.Math.Max(f, s)
            Next j
            Return f
        End Function

        ''' <summary>Two norm</summary>
        ''' <returns>    maximum singular value.
        ''' </returns>

        Public Overridable Function Norm2() As Double
            Return ((New SingularValueDecomposition(Me)).Norm2())
        End Function

        ''' <summary>Infinity norm</summary>
        ''' <returns>    maximum row sum.
        ''' </returns>

        Public Overridable Function NormInf() As Double
            Dim f As Double = 0
            For i As Integer = 0 To _RowCount - 1
                Dim s As Double = 0
                For j As Integer = 0 To _ColumnCount - 1
                    s += System.Math.Abs(_ArrayValues(i)(j))
                Next j
                f = System.Math.Max(f, s)
            Next i
            Return f
        End Function

        ''' <summary>Frobenius norm</summary>
        ''' <returns>    sqrt of sum of squares of all elements.
        ''' </returns>

        Public Overridable Function NormF() As Double
            Dim f As Double = 0
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    f = MathFunctions.Hypot(f, _ArrayValues(i)(j))
                Next j
            Next i
            Return f
        End Function

        ''' <summary>Unary minus</summary>
        ''' <returns>    -A
        ''' </returns>

        Public Overridable Function UnaryMinus() As Matrix2D
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = -_ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>C = A + B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A + B
        ''' </returns>

        Public Overridable Function Add(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = _ArrayValues(i)(j) + B._ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>A = A + B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A + B
        ''' </returns>

        Public Overridable Function AddEquals(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    _ArrayValues(i)(j) = _ArrayValues(i)(j) + B._ArrayValues(i)(j)
                Next j
            Next i
            Return Me
        End Function

        ''' <summary>C = A - B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A - B
        ''' </returns>

        Public Overridable Function Subtract(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = _ArrayValues(i)(j) - B._ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>A = A - B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A - B
        ''' </returns>

        Public Overridable Function SubtractEquals(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    _ArrayValues(i)(j) = _ArrayValues(i)(j) - B._ArrayValues(i)(j)
                Next j
            Next i
            Return Me
        End Function

        ''' <summary>Element-by-element multiplication, C = A.*B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A.*B
        ''' </returns>

        Public Overridable Function ArrayMultiply(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = _ArrayValues(i)(j) * B._ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>Element-by-element multiplication in place, A = A.*B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A.*B
        ''' </returns>

        Public Overridable Function ArrayMultiplyEquals(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    _ArrayValues(i)(j) = _ArrayValues(i)(j) * B._ArrayValues(i)(j)
                Next j
            Next i
            Return Me
        End Function

        ''' <summary>Element-by-element right division, C = A./B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A./B
        ''' </returns>

        Public Overridable Function ArrayRightDivide(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = _ArrayValues(i)(j) / B._ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>Element-by-element right division in place, A = A./B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A./B
        ''' </returns>

        Public Overridable Function ArrayRightDivideEquals(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    _ArrayValues(i)(j) = _ArrayValues(i)(j) / B._ArrayValues(i)(j)
                Next j
            Next i
            Return Me
        End Function

        ''' <summary>Element-by-element left division, C = A.\B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A.\B
        ''' </returns>

        Public Overridable Function ArrayLeftDivide(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = B._ArrayValues(i)(j) / _ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>Element-by-element left division in place, A = A.\B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     A.\B
        ''' </returns>

        Public Overridable Function ArrayLeftDivideEquals(ByVal B As Matrix2D) As Matrix2D
            CheckMatrixDimensions(B)
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    _ArrayValues(i)(j) = B._ArrayValues(i)(j) / _ArrayValues(i)(j)
                Next j
            Next i
            Return Me
        End Function

        ''' <summary>Multiply a matrix by a scalar, C = s*A</summary>
        ''' <param name="s">   scalar
        ''' </param>
        ''' <returns>     s*A
        ''' </returns>

        Public Overridable Function Multiply(ByVal s As Double) As Matrix2D
            Dim X As New Matrix2D(_RowCount, _ColumnCount)
            Dim C()() As Double = X.Array
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    C(i)(j) = s * _ArrayValues(i)(j)
                Next j
            Next i
            Return X
        End Function

        ''' <summary>Multiply a matrix by a scalar in place, A = s*A</summary>
        ''' <param name="s">   scalar
        ''' </param>
        ''' <returns>     replace A by s*A
        ''' </returns>

        Public Overridable Function MultiplyEquals(ByVal s As Double) As Matrix2D
            For i As Integer = 0 To _RowCount - 1
                For j As Integer = 0 To _ColumnCount - 1
                    _ArrayValues(i)(j) = s * _ArrayValues(i)(j)
                Next j
            Next i
            Return Me
        End Function

        ''' <summary>Linear algebraic matrix multiplication, A * B</summary>
        ''' <param name="B">   another matrix
        ''' </param>
        ''' <returns>     Matrix product, A * B
        ''' </returns>
        ''' <exception cref="System.ArgumentException">  Matrix inner dimensions must agree.
        ''' </exception>

        Public Overridable Function Multiply(ByVal B As Matrix2D) As Matrix2D
            If B._RowCount <> _ColumnCount Then
                Throw New System.ArgumentException("GeneralMatrix inner dimensions must agree.")
            End If
            Dim X As New Matrix2D(_RowCount, B._ColumnCount)
            Dim C()() As Double = X.Array
            Dim Bcolj(_ColumnCount - 1) As Double
            For j As Integer = 0 To B._ColumnCount - 1
                For k As Integer = 0 To _ColumnCount - 1
                    Bcolj(k) = B._ArrayValues(k)(j)
                Next k
                For i As Integer = 0 To _RowCount - 1
                    Dim Arowi() As Double = _ArrayValues(i)
                    Dim s As Double = 0
                    For k As Integer = 0 To _ColumnCount - 1
                        s += Arowi(k) * Bcolj(k)
                    Next k
                    C(i)(j) = s
                Next i
            Next j
            Return X
        End Function

#Region "Operator Overloading"

        ''' <summary>
        '''  Addition of matrices
        ''' </summary>
        ''' <param name="m1"></param>
        ''' <param name="m2"></param>
        ''' <returns></returns>
        Public Shared Operator +(ByVal m1 As Matrix2D, ByVal m2 As Matrix2D) As Matrix2D
            Return m1.Add(m2)
        End Operator

        ''' <summary>
        ''' Subtraction of matrices
        ''' </summary>
        ''' <param name="m1"></param>
        ''' <param name="m2"></param>
        ''' <returns></returns>
        Public Shared Operator -(ByVal m1 As Matrix2D, ByVal m2 As Matrix2D) As Matrix2D
            Return m1.Subtract(m2)
        End Operator

        ''' <summary>
        ''' Multiplication of matrices
        ''' </summary>
        ''' <param name="m1"></param>
        ''' <param name="m2"></param>
        ''' <returns></returns>
        Public Shared Operator *(ByVal m1 As Matrix2D, ByVal m2 As Matrix2D) As Matrix2D
            Return m1.Multiply(m2)
        End Operator

#End Region 'Operator Overloading

        ''' <summary>LU Decomposition</summary>
        ''' <returns>     LUDecomposition
        ''' </returns>
        ''' <seealso cref="LUDecomposition">
        ''' </seealso>

        Public Overridable Function LUD() As LUDecomposition
            Return New LUDecomposition(Me)
        End Function

        ''' <summary>QR Decomposition</summary>
        ''' <returns>     QRDecomposition
        ''' </returns>
        ''' <seealso cref="QRDecomposition">
        ''' </seealso>

        Public Overridable Function QRD() As QRDecomposition
            Return New QRDecomposition(Me)
        End Function

        ''' <summary>Cholesky Decomposition</summary>
        ''' <returns>     CholeskyDecomposition
        ''' </returns>
        ''' <seealso cref="CholeskyDecomposition">
        ''' </seealso>

        Public Overridable Function Chol() As CholeskyDecomposition
            Return New CholeskyDecomposition(Me)
        End Function

        ''' <summary>Singular Value Decomposition</summary>
        ''' <returns>     SingularValueDecomposition
        ''' </returns>
        ''' <seealso cref="SingularValueDecomposition">
        ''' </seealso>

        Public Overridable Function SVD() As SingularValueDecomposition
            Return New SingularValueDecomposition(Me)
        End Function

        ''' <summary>Eigenvalue Decomposition</summary>
        ''' <returns>     EigenvalueDecomposition
        ''' </returns>
        ''' <seealso cref="EigenvalueDecomposition">
        ''' </seealso>

        Public Overridable Function Eigen() As EigenvalueDecomposition
            Return New EigenvalueDecomposition(Me)
        End Function

        ''' <summary>Solve A*X = B</summary>
        ''' <param name="B">   right hand side
        ''' </param>
        ''' <returns>     solution if A is square, least squares solution otherwise
        ''' </returns>

        Public Overridable Function Solve(ByVal B As Matrix2D) As Matrix2D
            Return (If(_RowCount = _ColumnCount, (New LUDecomposition(Me)).Solve(B), (New QRDecomposition(Me)).Solve(B)))
        End Function

        ''' <summary>Solve X*A = B, which is also A'*X' = B'</summary>
        ''' <param name="B">   right hand side
        ''' </param>
        ''' <returns>     solution if A is square, least squares solution otherwise.
        ''' </returns>

        Public Overridable Function SolveTranspose(ByVal B As Matrix2D) As Matrix2D
            Return Transpose().Solve(B.Transpose())
        End Function

        ''' <summary>Matrix inverse or pseudoinverse</summary>
        ''' <returns>     inverse(A) if A is square, pseudoinverse otherwise.
        ''' </returns>
        Public Overridable Function Inverse() As Matrix2D
            Return Solve(Identity(_RowCount, _RowCount))
        End Function

        ''' <summary>GeneralMatrix determinant</summary>
        ''' <returns>     determinant
        ''' </returns>

        Public Overridable Function Determinant() As Double
            Return (New LUDecomposition(Me)).Determinant()
        End Function

        ''' <summary>GeneralMatrix rank</summary>
        ''' <returns>     effective numerical rank, obtained from SVD.
        ''' </returns>

        Public Overridable Function Rank() As Integer
            Return (New SingularValueDecomposition(Me)).Rank()
        End Function

        ''' <summary>Matrix condition (2 norm)</summary>
        ''' <returns>     ratio of largest to smallest singular value.
        ''' </returns>

        Public Overridable Function Condition() As Double
            Return (New SingularValueDecomposition(Me)).Condition()
        End Function

        ''' <summary>Matrix trace.</summary>
        ''' <returns>     sum of the diagonal elements.
        ''' </returns>

        Public Overridable Function Trace() As Double
            Dim t As Double = 0
            Dim i As Integer = 0
            Do While i < System.Math.Min(_RowCount, _ColumnCount)
                t += _ArrayValues(i)(i)
                i += 1
            Loop
            Return t
        End Function

        ''' <summary>Generate matrix with random elements</summary>
        ''' <param name="m">   Number of rows.
        ''' </param>
        ''' <param name="n">   Number of colums.
        ''' </param>
        ''' <returns>     An m-by-n matrix with uniformly distributed random elements.
        ''' </returns>

        Public Shared Function Random(ByVal m As Integer, ByVal n As Integer) As Matrix2D
            'INSTANT VB NOTE: The local variable random was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
            Dim random_Renamed As New System.Random()

            Dim A As New Matrix2D(m, n)
            Dim X()() As Double = A.Array
            For i As Integer = 0 To m - 1
                For j As Integer = 0 To n - 1
                    X(i)(j) = random_Renamed.NextDouble()
                Next j
            Next i
            Return A
        End Function

        ''' <summary>Generate identity matrix</summary>
        ''' <param name="m">   Number of rows.
        ''' </param>
        ''' <param name="n">   Number of colums.
        ''' </param>
        ''' <returns>     An m-by-n matrix with ones on the diagonal and zeros elsewhere.
        ''' </returns>

        Public Shared Function Identity(ByVal m As Integer, ByVal n As Integer) As Matrix2D
            Dim A As New Matrix2D(m, n)
            Dim X()() As Double = A.Array
            For i As Integer = 0 To m - 1
                For j As Integer = 0 To n - 1
                    X(i)(j) = (If(i = j, 1.0, 0.0))
                Next j
            Next i
            Return A
        End Function

#End Region '  Public Methods

#Region "    Private Methods"

        ''' <summary>Check if size(A) == size(B) *</summary>

        Private Sub CheckMatrixDimensions(ByVal B As Matrix2D)
            If B._RowCount <> _RowCount OrElse B._ColumnCount <> _ColumnCount Then
                Throw New System.ArgumentException("GeneralMatrix dimensions must agree.")
            End If
        End Sub
#End Region '  Private Methods

#Region "Implement IDisposable"
        ''' <summary>
        ''' Do not make this method virtual.
        ''' A derived class should not be able to override this method.
        ''' </summary>
        Public Sub Dispose() Implements System.IDisposable.Dispose
            Dispose(True)
        End Sub

        ''' <summary>
        ''' Dispose(bool disposing) executes in two distinct scenarios.
        ''' If disposing equals true, the method has been called directly
        ''' or indirectly by a user's code. Managed and unmanaged resources
        ''' can be disposed.
        ''' If disposing equals false, the method has been called by the 
        ''' runtime from inside the finalizer and you should not reference 
        ''' other objects. Only unmanaged resources can be disposed.
        ''' </summary>
        ''' <param name="disposing"></param>
        Private Sub Dispose(ByVal disposing As Boolean)
            ' This object will be cleaned up by the Dispose method.
            ' Therefore, you should call GC.SupressFinalize to
            ' take this object off the finalization queue 
            ' and prevent finalization code for this object
            ' from executing a second time.
            If disposing Then
                GC.SuppressFinalize(Me)
            End If
        End Sub

        ''' <summary>
        ''' This destructor will run only if the Dispose method 
        ''' does not get called.
        ''' It gives your base class the opportunity to finalize.
        ''' Do not provide destructors in types derived from this class.
        ''' </summary>
        Protected Overrides Sub Finalize()
            ' Do not re-create Dispose clean-up code here.
            ' Calling Dispose(false) is optimal in terms of
            ' readability and maintainability.
            Dispose(False)
        End Sub
#End Region '  Implement IDisposable

        ''' <summary>Clone the GeneralMatrix object.</summary>
        Public Function Clone() As System.Object Implements System.ICloneable.Clone
            Return Me.Copy()
        End Function

        ''' <summary>
        ''' A method called when serializing this class
        ''' </summary>
        ''' <param name="info"></param>
        ''' <param name="context"></param>
        Private Sub ISerializable_GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
        End Sub
    End Class
End Namespace
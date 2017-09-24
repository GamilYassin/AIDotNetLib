Imports System
Imports System.Runtime.Serialization

Namespace CommonLib

    ''' <summary>LU Decomposition.
    ''' For an m-by-n matrix A with m >= n, the LU decomposition is an m-by-n
    ''' unit lower triangular matrix L, an n-by-n upper triangular matrix U,
    ''' and a permutation vector piv of length m so that A(piv,:) = L*U.
    ''' <code> If m lessthan n, then L is m-by-m and U is m-by-n. </code>
    ''' The LU decompostion with pivoting always exists, even if the matrix is
    ''' singular, so the constructor will never fail.  The primary use of the
    ''' LU decomposition is in the solution of square systems of simultaneous
    ''' linear equations.  This will fail if IsNonSingular() returns false.
    ''' </summary>

    <Serializable>
    Public Class LUDecomposition
        Implements System.Runtime.Serialization.ISerializable

#Region "Class variables"

        ''' <summary>Array for internal storage of decomposition.
        ''' @serial internal array storage.
        ''' </summary>
        Private LU()() As Double

        ''' <summary>Row and column dimensions, and pivot sign.
        ''' @serial column dimension.
        ''' @serial row dimension.
        ''' @serial pivot sign.
        ''' </summary>
        Private m, n, pivsign As Integer

        ''' <summary>Internal storage of pivot vector.
        ''' @serial pivot vector.
        ''' </summary>
        Private piv() As Integer

#End Region '  Class variables

#Region "Constructor"

        ''' <summary>LU Decomposition</summary>
        ''' <param name="A">  Rectangular matrix
        ''' </param>
        Public Sub New(ByVal A As Matrix2D)
            ' Use a "left-looking", dot-product, Crout/Doolittle algorithm.

            LU = A.ArrayCopy
            m = A.RowDimension
            n = A.ColumnDimension
            piv = New Integer(m - 1) {}
            For i As Integer = 0 To m - 1
                piv(i) = i
            Next i
            pivsign = 1
            Dim LUrowi() As Double
            Dim LUcolj(m - 1) As Double

            ' Outer loop.

            For j As Integer = 0 To n - 1

                ' Make a copy of the j-th column to localize references.

                For i As Integer = 0 To m - 1
                    LUcolj(i) = LU(i)(j)
                Next i

                ' Apply previous transformations.

                For i As Integer = 0 To m - 1
                    LUrowi = LU(i)

                    ' Most of the time is spent in the following dot product.

                    Dim kmax As Integer = System.Math.Min(i, j)
                    Dim s As Double = 0.0
                    For k As Integer = 0 To kmax - 1
                        s += LUrowi(k) * LUcolj(k)
                    Next k

                    LUcolj(i) -= s
                    LUrowi(j) = LUcolj(i)
                Next i

                ' Find pivot and exchange if necessary.

                Dim p As Integer = j
                For i As Integer = j + 1 To m - 1
                    If Math.Abs(LUcolj(i)) > Math.Abs(LUcolj(p)) Then
                        p = i
                    End If
                Next i
                If p <> j Then
                    For k As Integer = 0 To n - 1
                        Dim t As Double = LU(p)(k)
                        LU(p)(k) = LU(j)(k)
                        LU(j)(k) = t
                    Next k
                    Dim k2 As Integer = piv(p)
                    piv(p) = piv(j)
                    piv(j) = k2
                    pivsign = -pivsign
                End If

                ' Compute multipliers.

                If j < m And LU(j)(j) <> 0.0 Then
                    For i As Integer = j + 1 To m - 1
                        LU(i)(j) /= LU(j)(j)
                    Next i
                End If
            Next j
        End Sub
#End Region '  Constructor

#Region "Public Properties"
        ''' <summary>Is the matrix nonsingular?</summary>
        ''' <returns>     true if U, and hence A, is nonsingular.
        ''' </returns>
        Public Overridable ReadOnly Property IsNonSingular() As Boolean
            Get
                For j As Integer = 0 To n - 1
                    If LU(j)(j) = 0 Then
                        Return False
                    End If
                Next j
                Return True
            End Get
        End Property

        ''' <summary>Return lower triangular factor</summary>
        ''' <returns>     L
        ''' </returns>
        Public Overridable ReadOnly Property L() As Matrix2D
            Get
                Dim X As New Matrix2D(m, n)
                'INSTANT VB NOTE: The local variable L was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
                Dim L_Renamed()() As Double = X.Array
                For i As Integer = 0 To m - 1
                    For j As Integer = 0 To n - 1
                        If i > j Then
                            L_Renamed(i)(j) = LU(i)(j)
                        ElseIf i = j Then
                            L_Renamed(i)(j) = 1.0
                        Else
                            L_Renamed(i)(j) = 0.0
                        End If
                    Next j
                Next i
                Return X
            End Get
        End Property

        ''' <summary>Return upper triangular factor</summary>
        ''' <returns>     U
        ''' </returns>
        Public Overridable ReadOnly Property U() As Matrix2D
            Get
                Dim X As New Matrix2D(n, n)
                'INSTANT VB NOTE: The local variable U was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
                Dim U_Renamed()() As Double = X.Array
                For i As Integer = 0 To n - 1
                    For j As Integer = 0 To n - 1
                        If i <= j Then
                            U_Renamed(i)(j) = LU(i)(j)
                        Else
                            U_Renamed(i)(j) = 0.0
                        End If
                    Next j
                Next i
                Return X
            End Get
        End Property

        ''' <summary>Return pivot permutation vector</summary>
        ''' <returns>     piv
        ''' </returns>
        Public Overridable ReadOnly Property Pivot() As Integer()
            Get
                Dim p(m - 1) As Integer
                For i As Integer = 0 To m - 1
                    p(i) = piv(i)
                Next i
                Return p
            End Get
        End Property

        ''' <summary>Return pivot permutation vector as a one-dimensional double array</summary>
        ''' <returns>     (double) piv
        ''' </returns>
        Public Overridable ReadOnly Property DoublePivot() As Double()
            Get
                Dim vals(m - 1) As Double
                For i As Integer = 0 To m - 1
                    vals(i) = CDbl(piv(i))
                Next i
                Return vals
            End Get
        End Property

#End Region '  Public Properties

#Region "Public Methods"

        ''' <summary>Determinant</summary>
        ''' <returns>     det(A)
        ''' </returns>
        ''' <exception cref="System.ArgumentException">  Matrix must be square
        ''' </exception>

        Public Overridable Function Determinant() As Double
            If m <> n Then
                Throw New System.ArgumentException("Matrix must be square.")
            End If
            Dim d As Double = CDbl(pivsign)
            For j As Integer = 0 To n - 1
                d *= LU(j)(j)
            Next j
            Return d
        End Function

        ''' <summary>Solve A*X = B</summary>
        ''' <param name="B">  A Matrix with as many rows as A and any number of columns.
        ''' </param>
        ''' <returns>     X so that L*U*X = B(piv,:)
        ''' </returns>
        ''' <exception cref="System.ArgumentException"> Matrix row dimensions must agree.
        ''' </exception>
        ''' <exception cref="System.SystemException"> Matrix is singular.
        ''' </exception>

        Public Overridable Function Solve(ByVal B As Matrix2D) As Matrix2D
            If B.RowDimension <> m Then
                Throw New System.ArgumentException("Matrix row dimensions must agree.")
            End If
            If Not Me.IsNonSingular Then
                Throw New System.SystemException("Matrix isnot singular.")
            End If

            ' Copy right hand side with pivoting
            Dim nx As Integer = B.ColumnDimension
            Dim Xmat As Matrix2D = B.GetMatrix(piv, 0, nx - 1)
            Dim X()() As Double = Xmat.Array

            ' Solve L*Y = B(piv,:)
            For k As Integer = 0 To n - 1
                For i As Integer = k + 1 To n - 1
                    For j As Integer = 0 To nx - 1
                        X(i)(j) -= X(k)(j) * LU(i)(k)
                    Next j
                Next i
            Next k
            ' Solve U*X = Y;
            For k As Integer = n - 1 To 0 Step -1
                For j As Integer = 0 To nx - 1
                    X(k)(j) /= LU(k)(k)
                Next j
                For i As Integer = 0 To k - 1
                    For j As Integer = 0 To nx - 1
                        X(i)(j) -= X(k)(j) * LU(i)(k)
                    Next j
                Next i
            Next k
            Return Xmat
        End Function

#End Region '  Public Methods

        ' A method called when serializing this class.
        Private Sub ISerializable_GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
        End Sub
    End Class
End Namespace
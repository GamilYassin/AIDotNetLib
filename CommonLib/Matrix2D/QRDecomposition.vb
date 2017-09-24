Imports System
Imports System.Runtime.Serialization

Namespace CommonLib

    ''' <summary>QR Decomposition.
    ''' For an m-by-n matrix A with m >= n, the QR decomposition is an m-by-n
    ''' orthogonal matrix Q and an n-by-n upper triangular matrix R so that
    ''' A = Q*R.
    ''' 
    ''' The QR decompostion always exists, even if the matrix does not have
    ''' full rank, so the constructor will never fail.  The primary use of the
    ''' QR decomposition is in the least squares solution of nonsquare systems
    ''' of simultaneous linear equations.  This will fail if IsFullRank()
    ''' returns false.
    ''' </summary>

    <Serializable>
    Public Class QRDecomposition
        Implements System.Runtime.Serialization.ISerializable

#Region "Class variables"

        ''' <summary>Array for internal storage of decomposition.
        ''' @serial internal array storage.
        ''' </summary>
        Private QR()() As Double

        ''' <summary>Row and column dimensions.
        ''' @serial column dimension.
        ''' @serial row dimension.
        ''' </summary>
        Private m, n As Integer

        ''' <summary>Array for internal storage of diagonal of R.
        ''' @serial diagonal of R.
        ''' </summary>
        Private Rdiag() As Double

#End Region '  Class variables

#Region "Constructor"

        ''' <summary>QR Decomposition, computed by Householder reflections.</summary>
        ''' <param name="A">   Rectangular matrix
        ''' </param>
        Public Sub New(ByVal A As Matrix2D)
            ' Initialize.
            QR = A.ArrayCopy
            m = A.RowDimension
            n = A.ColumnDimension
            Rdiag = New Double(n - 1) {}

            ' Main loop.
            For k As Integer = 0 To n - 1
                ' Compute 2-norm of k-th column without under/overflow.
                Dim nrm As Double = 0
                For i As Integer = k To m - 1
                    nrm = MathFunctions.Hypot(nrm, QR(i)(k))
                Next i

                If nrm <> 0.0 Then
                    ' Form k-th Householder vector.
                    If QR(k)(k) < 0 Then
                        nrm = -nrm
                    End If
                    For i As Integer = k To m - 1
                        QR(i)(k) /= nrm
                    Next i
                    QR(k)(k) += 1.0

                    ' Apply transformation to remaining columns.
                    For j As Integer = k + 1 To n - 1
                        Dim s As Double = 0.0
                        For i As Integer = k To m - 1
                            s += QR(i)(k) * QR(i)(j)
                        Next i
                        s = (-s) / QR(k)(k)
                        For i As Integer = k To m - 1
                            QR(i)(j) += s * QR(i)(k)
                        Next i
                    Next j
                End If
                Rdiag(k) = -nrm
            Next k
        End Sub

#End Region '  Constructor

#Region "Public Properties"

        ''' <summary>Is the matrix full rank?</summary>
        ''' <returns>     true if R, and hence A, has full rank.
        ''' </returns>
        Public Overridable ReadOnly Property FullRank() As Boolean
            Get
                For j As Integer = 0 To n - 1
                    If Rdiag(j) = 0 Then
                        Return False
                    End If
                Next j
                Return True
            End Get
        End Property

        ''' <summary>Return the Householder vectors</summary>
        ''' <returns>     Lower trapezoidal matrix whose columns define the reflections
        ''' </returns>
        Public Overridable ReadOnly Property H() As Matrix2D
            Get
                Dim X As New Matrix2D(m, n)
                'INSTANT VB NOTE: The local variable H was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
                Dim H_Renamed()() As Double = X.Array
                For i As Integer = 0 To m - 1
                    For j As Integer = 0 To n - 1
                        If i >= j Then
                            H_Renamed(i)(j) = QR(i)(j)
                        Else
                            H_Renamed(i)(j) = 0.0
                        End If
                    Next j
                Next i
                Return X
            End Get

        End Property

        ''' <summary>Return the upper triangular factor</summary>
        ''' <returns>     R
        ''' </returns>
        Public Overridable ReadOnly Property R() As Matrix2D
            Get
                Dim X As New Matrix2D(n, n)
                'INSTANT VB NOTE: The local variable R was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
                Dim R_Renamed()() As Double = X.Array
                For i As Integer = 0 To n - 1
                    For j As Integer = 0 To n - 1
                        If i < j Then
                            R_Renamed(i)(j) = QR(i)(j)
                        ElseIf i = j Then
                            R_Renamed(i)(j) = Rdiag(i)
                        Else
                            R_Renamed(i)(j) = 0.0
                        End If
                    Next j
                Next i
                Return X
            End Get
        End Property

        ''' <summary>Generate and return the (economy-sized) orthogonal factor</summary>
        ''' <returns>     Q
        ''' </returns>
        Public Overridable ReadOnly Property Q() As Matrix2D
            Get
                Dim X As New Matrix2D(m, n)
                'INSTANT VB NOTE: The local variable Q was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
                Dim Q_Renamed()() As Double = X.Array
                For k As Integer = n - 1 To 0 Step -1
                    For i As Integer = 0 To m - 1
                        Q_Renamed(i)(k) = 0.0
                    Next i
                    Q_Renamed(k)(k) = 1.0
                    For j As Integer = k To n - 1
                        If QR(k)(k) <> 0 Then
                            Dim s As Double = 0.0
                            For i As Integer = k To m - 1
                                s += QR(i)(k) * Q_Renamed(i)(j)
                            Next i
                            s = (-s) / QR(k)(k)
                            For i As Integer = k To m - 1
                                Q_Renamed(i)(j) += s * QR(i)(k)
                            Next i
                        End If
                    Next j
                Next k
                Return X
            End Get
        End Property
#End Region '  Public Properties

#Region "Public Methods"

        ''' <summary>Least squares solution of A*X = B</summary>
        ''' <param name="B">   A Matrix with as many rows as A and any number of columns.
        ''' </param>
        ''' <returns>     X that minimizes the two norm of Q*R*X-B.
        ''' </returns>
        ''' <exception cref="System.ArgumentException"> Matrix row dimensions must agree.
        ''' </exception>
        ''' <exception cref="System.SystemException"> Matrix is rank deficient.
        ''' </exception>

        Public Overridable Function Solve(ByVal B As Matrix2D) As Matrix2D
            If B.RowDimension <> m Then
                Throw New System.ArgumentException("GeneralMatrix row dimensions must agree.")
            End If
            If Not Me.FullRank Then
                Throw New System.SystemException("Matrix is rank deficient.")
            End If

            ' Copy right hand side
            Dim nx As Integer = B.ColumnDimension
            Dim X()() As Double = B.ArrayCopy

            ' Compute Y = transpose(Q)*B
            For k As Integer = 0 To n - 1
                For j As Integer = 0 To nx - 1
                    Dim s As Double = 0.0
                    For i As Integer = k To m - 1
                        s += QR(i)(k) * X(i)(j)
                    Next i
                    s = (-s) / QR(k)(k)
                    For i As Integer = k To m - 1
                        X(i)(j) += s * QR(i)(k)
                    Next i
                Next j
            Next k
            ' Solve R*X = Y;
            For k As Integer = n - 1 To 0 Step -1
                For j As Integer = 0 To nx - 1
                    X(k)(j) /= Rdiag(k)
                Next j
                For i As Integer = 0 To k - 1
                    For j As Integer = 0 To nx - 1
                        X(i)(j) -= X(k)(j) * QR(i)(k)
                    Next j
                Next i
            Next k

            Return ((New Matrix2D(X, n, nx)).GetMatrix(0, n - 1, 0, nx - 1))
        End Function

#End Region '  Public Methods

        ' A method called when serializing this class.
        Private Sub ISerializable_GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
        End Sub
    End Class
End Namespace
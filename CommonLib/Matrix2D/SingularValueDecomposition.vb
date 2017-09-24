Imports System
Imports System.Runtime.Serialization

Namespace CommonLib

    ''' <summary>Singular Value Decomposition.
    ''' For an m-by-n matrix A with m >= n, the singular value decomposition is
    ''' an m-by-n orthogonal matrix U, an n-by-n diagonal matrix S, and
    ''' an n-by-n orthogonal matrix V so that A = U*S*V'.
    ''' The singular values, sigma[k] = S[k][k], are ordered so that
    ''' sigma[0] >= sigma[1] >= ... >= sigma[n-1].
    ''' The singular value decompostion always exists, so the constructor will
    ''' never fail.  The matrix condition number and the effective numerical
    ''' rank can be computed from this decomposition.
    ''' </summary>
    <Serializable>
    Public Class SingularValueDecomposition
        Implements System.Runtime.Serialization.ISerializable

#Region "Class variables"

        ''' <summary>Arrays for internal storage of U and V.
        ''' @serial internal storage of U.
        ''' @serial internal storage of V.
        ''' </summary>
        Private U()(), V()() As Double

        ''' <summary>Array for internal storage of singular values.
        ''' @serial internal storage of singular values.
        ''' </summary>
        Private _Serial() As Double

        ''' <summary>Row and column dimensions.
        ''' @serial row dimension.
        ''' @serial column dimension.
        ''' </summary>
        Private m, n As Integer

#End Region 'Class variables

#Region "Constructor"

        ''' <summary>Construct the singular value decomposition</summary>
        ''' <param name="Arg">   Rectangular matrix
        ''' </param>
        Public Sub New(ByVal Arg As Matrix2D)
            ' Derived from LINPACK code.
            ' Initialize.
            Dim A()() As Double = Arg.ArrayCopy
            m = Arg.RowDimension
            n = Arg.ColumnDimension
            Dim nu As Integer = System.Math.Min(m, n)
            _Serial = New Double(System.Math.Min(m + 1, n) - 1) {}
            U = New Double(m - 1)() {}
            For i As Integer = 0 To m - 1
                U(i) = New Double(nu - 1) {}
            Next i
            V = New Double(n - 1)() {}
            For i2 As Integer = 0 To n - 1
                V(i2) = New Double(n - 1) {}
            Next i2
            Dim e(n - 1) As Double
            Dim work(m - 1) As Double
            Dim wantu As Boolean = True
            Dim wantv As Boolean = True

            ' Reduce A to bidiagonal form, storing the diagonal elements
            ' in s and the super-diagonal elements in e.

            Dim nct As Integer = System.Math.Min(m - 1, n)
            Dim nrt As Integer = System.Math.Max(0, System.Math.Min(n - 2, m))
            Dim k As Integer = 0
            Do While k < System.Math.Max(nct, nrt)
                If k < nct Then

                    ' Compute the transformation for the k-th column and
                    ' place the k-th diagonal in s[k].
                    ' Compute 2-norm of k-th column without under/overflow.
                    _Serial(k) = 0
                    For i As Integer = k To m - 1
                        _Serial(k) = MathFunctions.Hypot(_Serial(k), A(i)(k))
                    Next i
                    If _Serial(k) <> 0.0 Then
                        If A(k)(k) < 0.0 Then
                            _Serial(k) = -_Serial(k)
                        End If
                        For i As Integer = k To m - 1
                            A(i)(k) /= _Serial(k)
                        Next i
                        A(k)(k) += 1.0
                    End If
                    _Serial(k) = -_Serial(k)
                End If
                For j As Integer = k + 1 To n - 1
                    If (k < nct) And (_Serial(k) <> 0.0) Then

                        ' Apply the transformation.

                        Dim t As Double = 0
                        For i As Integer = k To m - 1
                            t += A(i)(k) * A(i)(j)
                        Next i
                        t = (-t) / A(k)(k)
                        For i As Integer = k To m - 1
                            A(i)(j) += t * A(i)(k)
                        Next i
                    End If

                    ' Place the k-th row of A into e for the
                    ' subsequent calculation of the row transformation.

                    e(j) = A(k)(j)
                Next j
                If wantu And (k < nct) Then

                    ' Place the transformation in U for subsequent back
                    ' multiplication.

                    For i As Integer = k To m - 1
                        U(i)(k) = A(i)(k)
                    Next i
                End If
                If k < nrt Then

                    ' Compute the k-th row transformation and place the
                    ' k-th super-diagonal in e[k].
                    ' Compute 2-norm without under/overflow.
                    e(k) = 0
                    For i As Integer = k + 1 To n - 1
                        e(k) = MathFunctions.Hypot(e(k), e(i))
                    Next i
                    If e(k) <> 0.0 Then
                        If e(k + 1) < 0.0 Then
                            e(k) = -e(k)
                        End If
                        For i As Integer = k + 1 To n - 1
                            e(i) /= e(k)
                        Next i
                        e(k + 1) += 1.0
                    End If
                    e(k) = -e(k)
                    If (k + 1 < m) And (e(k) <> 0.0) Then

                        ' Apply the transformation.

                        For i As Integer = k + 1 To m - 1
                            work(i) = 0.0
                        Next i
                        For j As Integer = k + 1 To n - 1
                            For i As Integer = k + 1 To m - 1
                                work(i) += e(j) * A(i)(j)
                            Next i
                        Next j
                        For j As Integer = k + 1 To n - 1
                            Dim t As Double = (-e(j)) / e(k + 1)
                            For i As Integer = k + 1 To m - 1
                                A(i)(j) += t * work(i)
                            Next i
                        Next j
                    End If
                    If wantv Then

                        ' Place the transformation in V for subsequent
                        ' back multiplication.

                        For i As Integer = k + 1 To n - 1
                            V(i)(k) = e(i)
                        Next i
                    End If
                End If
                k += 1
            Loop

            ' Set up the final bidiagonal matrix or order p.

            Dim p As Integer = System.Math.Min(n, m + 1)
            If nct < n Then
                _Serial(nct) = A(nct)(nct)
            End If
            If m < p Then
                _Serial(p - 1) = 0.0
            End If
            If nrt + 1 < p Then
                e(nrt) = A(nrt)(p - 1)
            End If
            e(p - 1) = 0.0

            ' If required, generate U.

            If wantu Then
                For j As Integer = nct To nu - 1
                    For i As Integer = 0 To m - 1
                        U(i)(j) = 0.0
                    Next i
                    U(j)(j) = 1.0
                Next j
                For Counter As Integer = nct - 1 To 0 Step -1
                    If _Serial(Counter) <> 0.0 Then
                        For j As Integer = Counter + 1 To nu - 1
                            Dim t As Double = 0
                            For Index As Integer = Counter To m - 1
                                t += U(Index)(Counter) * U(Index)(j)
                            Next Index
                            t = (-t) / U(Counter)(Counter)
                            For Index As Integer = Counter To m - 1
                                U(Index)(j) += t * U(Index)(Counter)
                            Next Index
                        Next j
                        For Index As Integer = Counter To m - 1
                            U(Index)(Counter) = -U(Index)(Counter)
                        Next Index
                        U(Counter)(Counter) = 1.0 + U(Counter)(Counter)
                        Dim i As Integer = 0
                        Do While i < Counter - 1
                            U(i)(Counter) = 0.0
                            i += 1
                        Loop
                    Else
                        For i As Integer = 0 To m - 1
                            U(i)(Counter) = 0.0
                        Next i
                        U(Counter)(Counter) = 1.0
                    End If
                Next Counter
            End If

            ' If required, generate V.

            If wantv Then
                For Counter As Integer = n - 1 To 0 Step -1
                    If (Counter < nrt) And (e(Counter) <> 0.0) Then
                        For j As Integer = Counter + 1 To nu - 1
                            Dim t As Double = 0
                            For i As Integer = Counter + 1 To n - 1
                                t += V(i)(Counter) * V(i)(j)
                            Next i
                            t = (-t) / V(Counter + 1)(Counter)
                            For i As Integer = Counter + 1 To n - 1
                                V(i)(j) += t * V(i)(Counter)
                            Next i
                        Next j
                    End If
                    For i As Integer = 0 To n - 1
                        V(i)(Counter) = 0.0
                    Next i
                    V(Counter)(Counter) = 1.0
                Next Counter
            End If

            ' Main iteration loop for the singular values.

            Dim pp As Integer = p - 1
            Dim iter As Integer = 0
            Dim eps As Double = System.Math.Pow(2.0, -52.0)
            Do While p > 0
                Dim Counter, kase As Integer

                ' Here is where a test for too many iterations would go.

                ' This section of the program inspects for
                ' negligible elements in the s and e arrays.  On
                ' completion the variables kase and k are set as follows.

                ' kase = 1     if s(p) and e[k-1] are negligible and k<p
                ' kase = 2     if s(k) is negligible and k<p
                ' kase = 3     if e[k-1] is negligible, k<p, and
                '              s(k), ..., s(p) are not negligible (qr step).
                ' kase = 4     if e(p-1) is negligible (convergence).

                For Counter = p - 2 To -1 Step -1
                    If Counter = -1 Then
                        Exit For
                    End If
                    If System.Math.Abs(e(Counter)) <= eps * (System.Math.Abs(_Serial(Counter)) + System.Math.Abs(_Serial(Counter + 1))) Then
                        e(Counter) = 0.0
                        Exit For
                    End If
                Next Counter
                If Counter = p - 2 Then
                    kase = 4
                Else
                    Dim ks As Integer
                    For ks = p - 1 To Counter Step -1
                        If ks = Counter Then
                            Exit For
                        End If
                        Dim t As Double = (If(ks <> p, System.Math.Abs(e(ks)), 0.0)) + (If(ks <> Counter + 1, System.Math.Abs(e(ks - 1)), 0.0))
                        If System.Math.Abs(_Serial(ks)) <= eps * t Then
                            _Serial(ks) = 0.0
                            Exit For
                        End If
                    Next ks
                    If ks = Counter Then
                        kase = 3
                    ElseIf ks = p - 1 Then
                        kase = 1
                    Else
                        kase = 2
                        Counter = ks
                    End If
                End If
                Counter += 1

                ' Perform the task indicated by kase.

                Select Case kase


                    ' Deflate negligible s(p).
                    Case 1
                        Dim f As Double = e(p - 2)
                        e(p - 2) = 0.0
                        For j As Integer = p - 2 To Counter Step -1
                            Dim t As Double = MathFunctions.Hypot(_Serial(j), f)
                            Dim cs As Double = _Serial(j) / t
                            Dim sn As Double = f / t
                            _Serial(j) = t
                            If j <> Counter Then
                                f = (-sn) * e(j - 1)
                                e(j - 1) = cs * e(j - 1)
                            End If
                            If wantv Then
                                For i As Integer = 0 To n - 1
                                    t = cs * V(i)(j) + sn * V(i)(p - 1)
                                    V(i)(p - 1) = (-sn) * V(i)(j) + cs * V(i)(p - 1)
                                    V(i)(j) = t
                                Next i
                            End If
                        Next j

                    ' Split at negligible s(k).


                    Case 2
                        Dim f As Double = e(Counter - 1)
                        e(Counter - 1) = 0.0
                        For j As Integer = Counter To p - 1
                            Dim t As Double = MathFunctions.Hypot(_Serial(j), f)
                            Dim cs As Double = _Serial(j) / t
                            Dim sn As Double = f / t
                            _Serial(j) = t
                            f = (-sn) * e(j)
                            e(j) = cs * e(j)
                            If wantu Then
                                For i As Integer = 0 To m - 1
                                    t = cs * U(i)(j) + sn * U(i)(Counter - 1)
                                    U(i)(Counter - 1) = (-sn) * U(i)(j) + cs * U(i)(Counter - 1)
                                    U(i)(j) = t
                                Next i
                            End If
                        Next j

                    ' Perform one qr step.


                    Case 3
                        ' Calculate the shift.

                        Dim scale As Double = System.Math.Max(System.Math.Max(System.Math.Max(System.Math.Max(System.Math.Abs(_Serial(p - 1)), System.Math.Abs(_Serial(p - 2))), System.Math.Abs(e(p - 2))), System.Math.Abs(_Serial(Counter))), System.Math.Abs(e(Counter)))
                        Dim sp As Double = _Serial(p - 1) / scale
                        Dim spm1 As Double = _Serial(p - 2) / scale
                        Dim epm1 As Double = e(p - 2) / scale
                        Dim sk As Double = _Serial(Counter) / scale
                        Dim ek As Double = e(Counter) / scale
                        Dim b As Double = ((spm1 + sp) * (spm1 - sp) + epm1 * epm1) / 2.0
                        Dim c As Double = (sp * epm1) * (sp * epm1)
                        Dim shift As Double = 0.0
                        If (b <> 0.0) Or (c <> 0.0) Then
                            shift = System.Math.Sqrt(b * b + c)
                            If b < 0.0 Then
                                shift = -shift
                            End If
                            shift = c / (b + shift)
                        End If
                        Dim f As Double = (sk + sp) * (sk - sp) + shift
                        Dim g As Double = sk * ek

                        ' Chase zeros.

                        Dim j As Integer = Counter
                        Do While j < p - 1
                            Dim t As Double = MathFunctions.Hypot(f, g)
                            Dim cs As Double = f / t
                            Dim sn As Double = g / t
                            If j <> Counter Then
                                e(j - 1) = t
                            End If
                            f = cs * _Serial(j) + sn * e(j)
                            e(j) = cs * e(j) - sn * _Serial(j)
                            g = sn * _Serial(j + 1)
                            _Serial(j + 1) = cs * _Serial(j + 1)
                            If wantv Then
                                For i As Integer = 0 To n - 1
                                    t = cs * V(i)(j) + sn * V(i)(j + 1)
                                    V(i)(j + 1) = (-sn) * V(i)(j) + cs * V(i)(j + 1)
                                    V(i)(j) = t
                                Next i
                            End If
                            t = MathFunctions.Hypot(f, g)
                            cs = f / t
                            sn = g / t
                            _Serial(j) = t
                            f = cs * e(j) + sn * _Serial(j + 1)
                            _Serial(j + 1) = (-sn) * e(j) + cs * _Serial(j + 1)
                            g = sn * e(j + 1)
                            e(j + 1) = cs * e(j + 1)
                            If wantu AndAlso (j < m - 1) Then
                                For i As Integer = 0 To m - 1
                                    t = cs * U(i)(j) + sn * U(i)(j + 1)
                                    U(i)(j + 1) = (-sn) * U(i)(j) + cs * U(i)(j + 1)
                                    U(i)(j) = t
                                Next i
                            End If
                            j += 1
                        Loop
                        e(p - 2) = f
                        iter = iter + 1

                    ' Convergence.


                    Case 4
                        ' Make the singular values positive.

                        If _Serial(Counter) <= 0.0 Then
                            _Serial(Counter) = (If(_Serial(Counter) < 0.0, -_Serial(Counter), 0.0))
                            If wantv Then
                                For i As Integer = 0 To pp
                                    V(i)(Counter) = -V(i)(Counter)
                                Next i
                            End If
                        End If

                        ' Order the singular values.

                        Do While Counter < pp
                            If _Serial(Counter) >= _Serial(Counter + 1) Then
                                Exit Do
                            End If
                            Dim t As Double = _Serial(Counter)
                            _Serial(Counter) = _Serial(Counter + 1)
                            _Serial(Counter + 1) = t
                            If wantv AndAlso (Counter < n - 1) Then
                                For i As Integer = 0 To n - 1
                                    t = V(i)(Counter + 1)
                                    V(i)(Counter + 1) = V(i)(Counter)
                                    V(i)(Counter) = t
                                Next i
                            End If
                            If wantu AndAlso (Counter < m - 1) Then
                                For i As Integer = 0 To m - 1
                                    t = U(i)(Counter + 1)
                                    U(i)(Counter + 1) = U(i)(Counter)
                                    U(i)(Counter) = t
                                Next i
                            End If
                            Counter += 1
                        Loop
                        iter = 0
                        p -= 1
                End Select
            Loop
        End Sub
#End Region 'Constructor

#Region "Public Properties"
        ''' <summary>Return the one-dimensional array of singular values</summary>
        ''' <returns>     diagonal of S.
        ''' </returns>
        Public Overridable ReadOnly Property SingularValues() As Double()
            Get
                Return _Serial
            End Get
        End Property

        ''' <summary>Return the diagonal matrix of singular values</summary>
        ''' <returns>     S
        ''' </returns>
        Public Overridable ReadOnly Property S() As Matrix2D
            Get
                Dim X As New Matrix2D(n, n)
                'INSTANT VB NOTE: The local variable S was renamed since Visual Basic will not allow local variables with the same name as their enclosing function or property:
                Dim S_Renamed()() As Double = X.Array
                For i As Integer = 0 To n - 1
                    For j As Integer = 0 To n - 1
                        S_Renamed(i)(j) = 0.0
                    Next j
                    S_Renamed(i)(i) = Me._Serial(i)
                Next i
                Return X
            End Get
        End Property
#End Region '  Public Properties

#Region "    Public Methods"

        ''' <summary>Return the left singular vectors</summary>
        ''' <returns>     U
        ''' </returns>

        Public Overridable Function GetU() As Matrix2D
            Return New Matrix2D(U, m, System.Math.Min(m + 1, n))
        End Function

        ''' <summary>Return the right singular vectors</summary>
        ''' <returns>     V
        ''' </returns>

        Public Overridable Function GetV() As Matrix2D
            Return New Matrix2D(V, n, n)
        End Function

        ''' <summary>Two norm</summary>
        ''' <returns>     max(S)
        ''' </returns>

        Public Overridable Function Norm2() As Double
            Return _Serial(0)
        End Function

        ''' <summary>Two norm condition number</summary>
        ''' <returns>     max(S)/min(S)
        ''' </returns>

        Public Overridable Function Condition() As Double
            Return _Serial(0) / _Serial(System.Math.Min(m, n) - 1)
        End Function

        ''' <summary>Effective numerical matrix rank</summary>
        ''' <returns>     Number of nonnegligible singular values.
        ''' </returns>

        Public Overridable Function Rank() As Integer
            Dim eps As Double = System.Math.Pow(2.0, -52.0)
            Dim tol As Double = System.Math.Max(m, n) * _Serial(0) * eps
            Dim r As Integer = 0
            For i As Integer = 0 To _Serial.Length - 1
                If _Serial(i) > tol Then
                    r += 1
                End If
            Next i
            Return r
        End Function
#End Region 'Public Methods

        ' A method called when serializing this class.
        Private Sub ISerializable_GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
        End Sub
    End Class
End Namespace
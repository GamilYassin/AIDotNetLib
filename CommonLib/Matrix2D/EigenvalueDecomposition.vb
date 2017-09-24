Imports System
Imports System.Runtime.InteropServices
Imports System.Runtime.Serialization

Namespace CommonLib

    ''' <summary>Eigenvalues and eigenvectors of a real matrix. 
    ''' If A is symmetric, then A = V*D*V' where the eigenvalue matrix D is
    ''' diagonal and the eigenvector matrix V is orthogonal.
    ''' I.e. A = V.Multiply(D.Multiply(V.Transpose())) and 
    ''' V.Multiply(V.Transpose()) equals the identity matrix.
    ''' If A is not symmetric, then the eigenvalue matrix D is block diagonal
    ''' with the real eigenvalues in 1-by-1 blocks and any complex eigenvalues,
    ''' lambda + i*mu, in 2-by-2 blocks, [lambda, mu; -mu, lambda].  The
    ''' columns of V represent the eigenvectors in the sense that A*V = V*D,
    ''' i.e. A.Multiply(V) equals V.Multiply(D).  The matrix V may be badly
    ''' conditioned, or even singular, so the validity of the equation
    ''' A = V*D*Inverse(V) depends upon V.cond().
    ''' 
    ''' </summary>

    <Serializable>
    Public Class EigenvalueDecomposition
        Implements System.Runtime.Serialization.ISerializable

#Region "    Class variables"

        ''' <summary>Row and column dimension (square matrix).
        ''' @serial matrix dimension.
        ''' </summary>
        Private n As Integer

        ''' <summary>Symmetry flag.
        ''' @serial internal symmetry flag.
        ''' </summary>
        Private issymmetric As Boolean

        ''' <summary>Arrays for internal storage of eigenvalues.
        ''' @serial internal storage of eigenvalues.
        ''' </summary>
        Private _D(), e() As Double

        ''' <summary>Array for internal storage of eigenvectors.
        ''' @serial internal storage of eigenvectors.
        ''' </summary>
        Private V()() As Double

        ''' <summary>Array for internal storage of nonsymmetric Hessenberg form.
        ''' @serial internal storage of nonsymmetric Hessenberg form.
        ''' </summary>
        Private H()() As Double

        ''' <summary>Working storage for nonsymmetric algorithm.
        ''' @serial working storage for nonsymmetric algorithm.
        ''' </summary>
        Private ort() As Double

#End Region '  Class variables

#Region "Private Methods"

        ' Symmetric Householder reduction to tridiagonal form.

        Private Sub Tred2()
            '  This is derived from the Algol procedures tred2 by
            '  Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            '  Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            '  Fortran subroutine in EISPACK.

            For j As Integer = 0 To n - 1
                _D(j) = V(n - 1)(j)
            Next j

            ' Householder reduction to tridiagonal form.

            For Counter As Integer = n - 1 To 1 Step -1
                ' Scale to avoid under/overflow.

                Dim scale As Double = 0.0
                Dim _H As Double = 0.0
                For k As Integer = 0 To Counter - 1
                    scale = scale + System.Math.Abs(_D(k))
                Next k
                If scale = 0.0 Then
                    e(Counter) = _D(Counter - 1)
                    For j As Integer = 0 To Counter - 1
                        _D(j) = V(Counter - 1)(j)
                        V(Counter)(j) = 0.0
                        V(j)(Counter) = 0.0
                    Next j
                Else
                    ' Generate Householder vector.

                    For k As Integer = 0 To Counter - 1
                        _D(k) /= scale
                        _H += _D(k) * _D(k)
                    Next k
                    Dim f As Double = _D(Counter - 1)
                    Dim g As Double = System.Math.Sqrt(_H)
                    If f > 0 Then
                        g = -g
                    End If
                    e(Counter) = scale * g
                    _H = _H - f * g
                    _D(Counter - 1) = f - g
                    For j As Integer = 0 To Counter - 1
                        e(j) = 0.0
                    Next j

                    ' Apply similarity transformation to remaining columns.

                    For j As Integer = 0 To Counter - 1
                        f = _D(j)
                        V(j)(Counter) = f
                        g = e(j) + V(j)(j) * f
                        Dim k As Integer = j + 1
                        Do While k <= Counter - 1
                            g += V(k)(j) * _D(k)
                            e(k) += V(k)(j) * f
                            k += 1
                        Loop
                        e(j) = g
                    Next j
                    f = 0.0
                    For j As Integer = 0 To Counter - 1
                        e(j) /= _H
                        f += e(j) * _D(j)
                    Next j
                    Dim hh As Double = f / (_H + _H)
                    For j As Integer = 0 To Counter - 1
                        e(j) -= hh * _D(j)
                    Next j
                    For j As Integer = 0 To Counter - 1
                        f = _D(j)
                        g = e(j)
                        Dim k As Integer = j
                        Do While k <= Counter - 1
                            V(k)(j) -= (f * e(k) + g * _D(k))
                            k += 1
                        Loop
                        _D(j) = V(Counter - 1)(j)
                        V(Counter)(j) = 0.0
                    Next j
                End If
                _D(Counter) = _H
            Next Counter

            ' Accumulate transformations.

            Dim i As Integer = 0
            Do While i < n - 1
                V(n - 1)(i) = V(i)(i)
                V(i)(i) = 1.0
                Dim _H As Double = _D(i + 1)
                If _H <> 0.0 Then
                    For k As Integer = 0 To i
                        _D(k) = V(k)(i + 1) / _H
                    Next k
                    For j As Integer = 0 To i
                        Dim g As Double = 0.0
                        For k As Integer = 0 To i
                            g += V(k)(i + 1) * V(k)(j)
                        Next k
                        For k As Integer = 0 To i
                            V(k)(j) -= g * _D(k)
                        Next k
                    Next j
                End If
                For k As Integer = 0 To i
                    V(k)(i + 1) = 0.0
                Next k
                i += 1
            Loop
            For j As Integer = 0 To n - 1
                _D(j) = V(n - 1)(j)
                V(n - 1)(j) = 0.0
            Next j
            V(n - 1)(n - 1) = 1.0
            e(0) = 0.0
        End Sub

        ' Symmetric tridiagonal QL algorithm.

        Private Sub Tql2()
            '  This is derived from the Algol procedures tql2, by
            '  Bowdler, Martin, Reinsch, and Wilkinson, Handbook for
            '  Auto. Comp., Vol.ii-Linear Algebra, and the corresponding
            '  Fortran subroutine in EISPACK.

            For Counter As Integer = 1 To n - 1
                e(Counter - 1) = e(Counter)
            Next Counter
            e(n - 1) = 0.0

            Dim f As Double = 0.0
            Dim tst1 As Double = 0.0
            Dim eps As Double = System.Math.Pow(2.0, -52.0)
            For l As Integer = 0 To n - 1
                ' Find small subdiagonal element

                tst1 = System.Math.Max(tst1, System.Math.Abs(_D(l)) + System.Math.Abs(e(l)))
                Dim m As Integer = l
                Do While m < n
                    If System.Math.Abs(e(m)) <= eps * tst1 Then
                        Exit Do
                    End If
                    m += 1
                Loop

                ' If m == l, d[l] is an eigenvalue,
                ' otherwise, iterate.

                If m > l Then
                    Dim iter As Integer = 0
                    Do
                        iter = iter + 1 ' (Could check iteration count here.)

                        ' Compute implicit shift

                        Dim g As Double = _D(l)
                        Dim p As Double = (_D(l + 1) - g) / (2.0 * e(l))
                        Dim r As Double = MathFunctions.Hypot(p, 1.0)
                        If p < 0 Then
                            r = -r
                        End If
                        _D(l) = e(l) / (p + r)
                        _D(l + 1) = e(l) * (p + r)
                        Dim dl1 As Double = _D(l + 1)
                        Dim _H As Double = g - _D(l)
                        For Counter As Integer = l + 2 To n - 1
                            _D(Counter) -= _H
                        Next Counter
                        f = f + _H

                        ' Implicit QL transformation.

                        p = _D(m)
                        Dim c As Double = 1.0
                        Dim c2 As Double = c
                        Dim c3 As Double = c
                        Dim el1 As Double = e(l + 1)
                        Dim s As Double = 0.0
                        Dim s2 As Double = 0.0
                        For Counter As Integer = m - 1 To l Step -1
                            c3 = c2
                            c2 = c
                            s2 = s
                            g = c * e(Counter)
                            _H = c * p
                            r = MathFunctions.Hypot(p, e(Counter))
                            e(Counter + 1) = s * r
                            s = e(Counter) / r
                            c = p / r
                            p = c * _D(Counter) - s * g
                            _D(Counter + 1) = _H + s * (c * g + s * _D(Counter))

                            ' Accumulate transformation.

                            For k As Integer = 0 To n - 1
                                _H = V(k)(Counter + 1)
                                V(k)(Counter + 1) = s * V(k)(Counter) + c * _H
                                V(k)(Counter) = c * V(k)(Counter) - s * _H
                            Next k
                        Next Counter
                        p = (-s) * s2 * c3 * el1 * e(l) / dl1
                        e(l) = s * p
                        _D(l) = c * p

                        ' Check for convergence.
                    Loop While System.Math.Abs(e(l)) > eps * tst1
                End If
                _D(l) = _D(l) + f
                e(l) = 0.0
            Next l

            ' Sort eigenvalues and corresponding vectors.

            Dim i As Integer = 0
            Do While i < n - 1
                Dim k As Integer = i
                Dim p As Double = _D(i)
                For j As Integer = i + 1 To n - 1
                    If _D(j) < p Then
                        k = j
                        p = _D(j)
                    End If
                Next j
                If k <> i Then
                    _D(k) = _D(i)
                    _D(i) = p
                    For j As Integer = 0 To n - 1
                        p = V(j)(i)
                        V(j)(i) = V(j)(k)
                        V(j)(k) = p
                    Next j
                End If
                i += 1
            Loop
        End Sub

        ' Nonsymmetric reduction to Hessenberg form.

        Private Sub Orthes()
            '  This is derived from the Algol procedures orthes and ortran,
            '  by Martin and Wilkinson, Handbook for Auto. Comp.,
            '  Vol.ii-Linear Algebra, and the corresponding
            '  Fortran subroutines in EISPACK.

            Dim low As Integer = 0
            Dim high As Integer = n - 1

            Dim m As Integer = low + 1
            Do While m <= high - 1

                ' Scale column.

                Dim scale As Double = 0.0
                For i As Integer = m To high
                    scale = scale + System.Math.Abs(H(i)(m - 1))
                Next i
                If scale <> 0.0 Then

                    ' Compute Householder transformation.

                    Dim _Holder As Double = 0.0
                    For i As Integer = high To m Step -1
                        ort(i) = H(i)(m - 1) / scale
                        _Holder += ort(i) * ort(i)
                    Next i
                    Dim g As Double = System.Math.Sqrt(_Holder)
                    If ort(m) > 0 Then
                        g = -g
                    End If
                    _Holder = _Holder - ort(m) * g
                    ort(m) = ort(m) - g

                    ' Apply Householder similarity transformation
                    ' H = (I-u*u'/h)*H*(I-u*u')/h)

                    For j As Integer = m To n - 1
                        Dim f As Double = 0.0
                        For i As Integer = high To m Step -1
                            f += ort(i) * H(i)(j)
                        Next i
                        f = f / _Holder
                        For i As Integer = m To high
                            H(i)(j) -= f * ort(i)
                        Next i
                    Next j

                    For i As Integer = 0 To high
                        Dim f As Double = 0.0
                        For j As Integer = high To m Step -1
                            f += ort(j) * H(i)(j)
                        Next j
                        f = f / _Holder
                        For j As Integer = m To high
                            H(i)(j) -= f * ort(j)
                        Next j
                    Next i
                    ort(m) = scale * ort(m)
                    H(m)(m - 1) = scale * g
                End If
                m += 1
            Loop

            ' Accumulate transformations (Algol's ortran).

            For i As Integer = 0 To n - 1
                For j As Integer = 0 To n - 1
                    V(i)(j) = (If(i = j, 1.0, 0.0))
                Next j
            Next i

            m = high - 1
            Do While m >= low + 1
                If H(m)(m - 1) <> 0.0 Then
                    For i As Integer = m + 1 To high
                        ort(i) = H(i)(m - 1)
                    Next i
                    For j As Integer = m To high
                        Dim g As Double = 0.0
                        For i As Integer = m To high
                            g += ort(i) * V(i)(j)
                        Next i
                        ' Double division avoids possible underflow
                        g = (g / ort(m)) / H(m)(m - 1)
                        For i As Integer = m To high
                            V(i)(j) += g * ort(i)
                        Next i
                    Next j
                End If
                m -= 1
            Loop
        End Sub


        ' Complex scalar division.

        <NonSerialized()>
        Private cdivr, cdivi As Double

        Private Sub Cdiv(ByVal xr As Double, ByVal xi As Double, ByVal yr As Double, ByVal yi As Double)
            Dim r, _d As Double
            If System.Math.Abs(yr) > System.Math.Abs(yi) Then
                r = yi / yr
                _d = yr + r * yi
                cdivr = (xr + r * xi) / _d
                cdivi = (xi - r * xr) / _d
            Else
                r = yr / yi
                _d = yi + r * yr
                cdivr = (r * xr + xi) / _d
                cdivi = (r * xi - xr) / _d
            End If
        End Sub


        ' Nonsymmetric reduction from Hessenberg to real Schur form.

        Private Sub Hqr2()
            '  This is derived from the Algol procedure hqr2,
            '  by Martin and Wilkinson, Handbook for Auto. Comp.,
            '  Vol.ii-Linear Algebra, and the corresponding
            '  Fortran subroutine in EISPACK.

            ' Initialize

            Dim nn As Integer = Me.n
            Dim n As Integer = nn - 1
            Dim low As Integer = 0
            Dim high As Integer = nn - 1
            Dim eps As Double = System.Math.Pow(2.0, -52.0)
            Dim exshift As Double = 0.0
            Dim p As Double = 0, q As Double = 0, r As Double = 0, s As Double = 0, z As Double = 0, t As Double, w As Double, x As Double, y As Double

            ' Store roots isolated by balanc and compute matrix norm

            Dim norm As Double = 0.0
            For i As Integer = 0 To nn - 1
                If i < low Or i > high Then
                    _D(i) = H(i)(i)
                    e(i) = 0.0
                End If
                For j As Integer = System.Math.Max(i - 1, 0) To nn - 1
                    norm = norm + System.Math.Abs(H(i)(j))
                Next j
            Next i

            ' Outer loop over eigenvalue index

            Dim iter As Integer = 0
            Do While n >= low

                ' Look for single small sub-diagonal element

                Dim l As Integer = n
                Do While l > low
                    s = System.Math.Abs(H(l - 1)(l - 1)) + System.Math.Abs(H(l)(l))
                    If s = 0.0 Then
                        s = norm
                    End If
                    If System.Math.Abs(H(l)(l - 1)) < eps * s Then
                        Exit Do
                    End If
                    l -= 1
                Loop

                ' Check for convergence
                ' One root found

                If l = n Then
                    H(n)(n) = H(n)(n) + exshift
                    _D(n) = H(n)(n)
                    e(n) = 0.0
                    n -= 1
                    iter = 0

                    ' Two roots found
                ElseIf l = n - 1 Then
                    w = H(n)(n - 1) * H(n - 1)(n)
                    p = (H(n - 1)(n - 1) - H(n)(n)) / 2.0
                    q = p * p + w
                    z = System.Math.Sqrt(System.Math.Abs(q))
                    H(n)(n) = H(n)(n) + exshift
                    H(n - 1)(n - 1) = H(n - 1)(n - 1) + exshift
                    x = H(n)(n)

                    ' Real pair

                    If q >= 0 Then
                        If p >= 0 Then
                            z = p + z
                        Else
                            z = p - z
                        End If
                        _D(n - 1) = x + z
                        _D(n) = _D(n - 1)
                        If z <> 0.0 Then
                            _D(n) = x - w / z
                        End If
                        e(n - 1) = 0.0
                        e(n) = 0.0
                        x = H(n)(n - 1)
                        s = System.Math.Abs(x) + System.Math.Abs(z)
                        p = x / s
                        q = z / s
                        r = System.Math.Sqrt(p * p + q * q)
                        p = p / r
                        q = q / r

                        ' Row modification

                        For j As Integer = n - 1 To nn - 1
                            z = H(n - 1)(j)
                            H(n - 1)(j) = q * z + p * H(n)(j)
                            H(n)(j) = q * H(n)(j) - p * z
                        Next j

                        ' Column modification

                        For i As Integer = 0 To n
                            z = H(i)(n - 1)
                            H(i)(n - 1) = q * z + p * H(i)(n)
                            H(i)(n) = q * H(i)(n) - p * z
                        Next i

                        ' Accumulate transformations

                        For i As Integer = low To high
                            z = V(i)(n - 1)
                            V(i)(n - 1) = q * z + p * V(i)(n)
                            V(i)(n) = q * V(i)(n) - p * z
                        Next i

                        ' Complex pair
                    Else
                        _D(n - 1) = x + p
                        _D(n) = x + p
                        e(n - 1) = z
                        e(n) = -z
                    End If
                    n = n - 2
                    iter = 0

                    ' No convergence yet
                Else

                    ' Form shift

                    x = H(n)(n)
                    y = 0.0
                    w = 0.0
                    If l < n Then
                        y = H(n - 1)(n - 1)
                        w = H(n)(n - 1) * H(n - 1)(n)
                    End If

                    ' Wilkinson's original ad hoc shift

                    If iter = 10 Then
                        exshift += x
                        For i As Integer = low To n
                            H(i)(i) -= x
                        Next i
                        s = System.Math.Abs(H(n)(n - 1)) + System.Math.Abs(H(n - 1)(n - 2))
                        y = 0.75 * s
                        x = y
                        w = (-0.4375) * s * s
                    End If

                    ' MATLAB's new ad hoc shift

                    If iter = 30 Then
                        s = (y - x) / 2.0
                        s = s * s + w
                        If s > 0 Then
                            s = System.Math.Sqrt(s)
                            If y < x Then
                                s = -s
                            End If
                            s = x - w / ((y - x) / 2.0 + s)
                            For i As Integer = low To n
                                H(i)(i) -= s
                            Next i
                            exshift += s
                            w = 0.964
                            y = w
                            x = y
                        End If
                    End If

                    iter = iter + 1 ' (Could check iteration count here.)

                    ' Look for two consecutive small sub-diagonal elements

                    Dim m As Integer = n - 2
                    Do While m >= l
                        z = H(m)(m)
                        r = x - z
                        s = y - z
                        p = (r * s - w) / H(m + 1)(m) + H(m)(m + 1)
                        q = H(m + 1)(m + 1) - z - r - s
                        r = H(m + 2)(m + 1)
                        s = System.Math.Abs(p) + System.Math.Abs(q) + System.Math.Abs(r)
                        p = p / s
                        q = q / s
                        r = r / s
                        If m = l Then
                            Exit Do
                        End If
                        If System.Math.Abs(H(m)(m - 1)) * (System.Math.Abs(q) + System.Math.Abs(r)) < eps * (System.Math.Abs(p) * (System.Math.Abs(H(m - 1)(m - 1)) + System.Math.Abs(z) + System.Math.Abs(H(m + 1)(m + 1)))) Then
                            Exit Do
                        End If
                        m -= 1
                    Loop

                    For i As Integer = m + 2 To n
                        H(i)(i - 2) = 0.0
                        If i > m + 2 Then
                            H(i)(i - 3) = 0.0
                        End If
                    Next i

                    ' Double QR step involving rows l:n and columns m:n

                    Dim k As Integer = m
                    Do While k <= n - 1
                        Dim notlast As Boolean = (k <> n - 1)
                        If k <> m Then
                            p = H(k)(k - 1)
                            q = H(k + 1)(k - 1)
                            r = (If(notlast, H(k + 2)(k - 1), 0.0))
                            x = System.Math.Abs(p) + System.Math.Abs(q) + System.Math.Abs(r)
                            If x <> 0.0 Then
                                p = p / x
                                q = q / x
                                r = r / x
                            End If
                        End If
                        If x = 0.0 Then
                            Exit Do
                        End If
                        s = System.Math.Sqrt(p * p + q * q + r * r)
                        If p < 0 Then
                            s = -s
                        End If
                        If s <> 0 Then
                            If k <> m Then
                                H(k)(k - 1) = (-s) * x
                            ElseIf l <> m Then
                                H(k)(k - 1) = -H(k)(k - 1)
                            End If
                            p = p + s
                            x = p / s
                            y = q / s
                            z = r / s
                            q = q / p
                            r = r / p

                            ' Row modification

                            For j As Integer = k To nn - 1
                                p = H(k)(j) + q * H(k + 1)(j)
                                If notlast Then
                                    p = p + r * H(k + 2)(j)
                                    H(k + 2)(j) = H(k + 2)(j) - p * z
                                End If
                                H(k)(j) = H(k)(j) - p * x
                                H(k + 1)(j) = H(k + 1)(j) - p * y
                            Next j

                            ' Column modification

                            Dim i As Integer = 0
                            Do While i <= System.Math.Min(n, k + 3)
                                p = x * H(i)(k) + y * H(i)(k + 1)
                                If notlast Then
                                    p = p + z * H(i)(k + 2)
                                    H(i)(k + 2) = H(i)(k + 2) - p * r
                                End If
                                H(i)(k) = H(i)(k) - p
                                H(i)(k + 1) = H(i)(k + 1) - p * q
                                i += 1
                            Loop

                            ' Accumulate transformations

                            For Counter As Integer = low To high
                                p = x * V(Counter)(k) + y * V(Counter)(k + 1)
                                If notlast Then
                                    p = p + z * V(Counter)(k + 2)
                                    V(Counter)(k + 2) = V(Counter)(k + 2) - p * r
                                End If
                                V(Counter)(k) = V(Counter)(k) - p
                                V(Counter)(k + 1) = V(Counter)(k + 1) - p * q
                            Next Counter
                        End If ' (s != 0)
                        k += 1
                    Loop ' k loop
                End If ' check convergence
            Loop ' while (n >= low)

            ' Backsubstitute to find vectors of upper triangular form

            If norm = 0.0 Then
                Return
            End If

            For n = nn - 1 To 0 Step -1
                p = _D(n)
                q = e(n)

                ' Real vector

                If q = 0 Then
                    Dim l As Integer = n
                    H(n)(n) = 1.0
                    For i As Integer = n - 1 To 0 Step -1
                        w = H(i)(i) - p
                        r = 0.0
                        For j As Integer = l To n
                            r = r + H(i)(j) * H(j)(n)
                        Next j
                        If e(i) < 0.0 Then
                            z = w
                            s = r
                        Else
                            l = i
                            If e(i) = 0.0 Then
                                If w <> 0.0 Then
                                    H(i)(n) = (-r) / w
                                Else
                                    H(i)(n) = (-r) / (eps * norm)
                                End If

                                ' Solve real equations
                            Else
                                x = H(i)(i + 1)
                                y = H(i + 1)(i)
                                q = (_D(i) - p) * (_D(i) - p) + e(i) * e(i)
                                t = (x * s - z * r) / q
                                H(i)(n) = t
                                If System.Math.Abs(x) > System.Math.Abs(z) Then
                                    H(i + 1)(n) = (-r - w * t) / x
                                Else
                                    H(i + 1)(n) = (-s - y * t) / z
                                End If
                            End If

                            ' Overflow control

                            t = System.Math.Abs(H(i)(n))
                            If (eps * t) * t > 1 Then
                                For j As Integer = i To n
                                    H(j)(n) = H(j)(n) / t
                                Next j
                            End If
                        End If
                    Next i

                    ' Complex vector
                ElseIf q < 0 Then
                    Dim l As Integer = n - 1

                    ' Last vector component imaginary so matrix is triangular

                    If System.Math.Abs(H(n)(n - 1)) > System.Math.Abs(H(n - 1)(n)) Then
                        H(n - 1)(n - 1) = q / H(n)(n - 1)
                        H(n - 1)(n) = (-(H(n)(n) - p)) / H(n)(n - 1)
                    Else
                        Cdiv(0.0, -H(n - 1)(n), H(n - 1)(n - 1) - p, q)
                        H(n - 1)(n - 1) = cdivr
                        H(n - 1)(n) = cdivi
                    End If
                    H(n)(n - 1) = 0.0
                    H(n)(n) = 1.0
                    For i As Integer = n - 2 To 0 Step -1
                        Dim ra, sa, vr, vi As Double
                        ra = 0.0
                        sa = 0.0
                        For j As Integer = l To n
                            ra = ra + H(i)(j) * H(j)(n - 1)
                            sa = sa + H(i)(j) * H(j)(n)
                        Next j
                        w = H(i)(i) - p

                        If e(i) < 0.0 Then
                            z = w
                            r = ra
                            s = sa
                        Else
                            l = i
                            If e(i) = 0 Then
                                Cdiv(-ra, -sa, w, q)
                                H(i)(n - 1) = cdivr
                                H(i)(n) = cdivi
                            Else

                                ' Solve complex equations

                                x = H(i)(i + 1)
                                y = H(i + 1)(i)
                                vr = (_D(i) - p) * (_D(i) - p) + e(i) * e(i) - q * q
                                vi = (_D(i) - p) * 2.0 * q
                                If vr = 0.0 And vi = 0.0 Then
                                    vr = eps * norm * (System.Math.Abs(w) + System.Math.Abs(q) + System.Math.Abs(x) + System.Math.Abs(y) + System.Math.Abs(z))
                                End If
                                Cdiv(x * r - z * ra + q * sa, x * s - z * sa - q * ra, vr, vi)
                                H(i)(n - 1) = cdivr
                                H(i)(n) = cdivi
                                If System.Math.Abs(x) > (System.Math.Abs(z) + System.Math.Abs(q)) Then
                                    H(i + 1)(n - 1) = (-ra - w * H(i)(n - 1) + q * H(i)(n)) / x
                                    H(i + 1)(n) = (-sa - w * H(i)(n) - q * H(i)(n - 1)) / x
                                Else
                                    Cdiv(-r - y * H(i)(n - 1), -s - y * H(i)(n), z, q)
                                    H(i + 1)(n - 1) = cdivr
                                    H(i + 1)(n) = cdivi
                                End If
                            End If

                            ' Overflow control

                            t = System.Math.Max(System.Math.Abs(H(i)(n - 1)), System.Math.Abs(H(i)(n)))
                            If (eps * t) * t > 1 Then
                                For j As Integer = i To n
                                    H(j)(n - 1) = H(j)(n - 1) / t
                                    H(j)(n) = H(j)(n) / t
                                Next j
                            End If
                        End If
                    Next i
                End If
            Next n

            ' Vectors of isolated roots

            For i As Integer = 0 To nn - 1
                If i < low Or i > high Then
                    For j As Integer = i To nn - 1
                        V(i)(j) = H(i)(j)
                    Next j
                End If
            Next i

            ' Back transformation to get eigenvectors of original matrix

            For j As Integer = nn - 1 To low Step -1
                Dim i As Integer = low
                Do While i <= high
                    z = 0.0
                    Dim k As Integer = low
                    Do While k <= System.Math.Min(j, high)
                        z = z + V(i)(k) * H(k)(j)
                        k += 1
                    Loop
                    V(i)(j) = z
                    i += 1
                Loop
            Next j
        End Sub

#End Region '  Private Methods


#Region "Constructor"

        ''' <summary>Check for symmetry, then construct the eigenvalue decomposition</summary>
        ''' <param name="Arg">   Square matrix
        ''' </param>
        Public Sub New(ByVal Arg As Matrix2D)
            Dim A()() As Double = Arg.Array
            n = Arg.ColumnDimension
            V = New Double(n - 1)() {}
            For i As Integer = 0 To n - 1
                V(i) = New Double(n - 1) {}
            Next i
            _D = New Double(n - 1) {}
            e = New Double(n - 1) {}

            issymmetric = True
            Dim j As Integer = 0
            Do While (j < n) And issymmetric
                Dim i As Integer = 0
                Do While (i < n) And issymmetric
                    issymmetric = (A(i)(j) = A(j)(i))
                    i += 1
                Loop
                j += 1
            Loop

            If issymmetric Then
                For i As Integer = 0 To n - 1
                    For Counter As Integer = 0 To n - 1
                        V(i)(Counter) = A(i)(Counter)
                    Next Counter
                Next i

                ' Tridiagonalize.
                Tred2()

                ' Diagonalize.
                Tql2()
            Else
                H = New Double(n - 1)() {}
                For i2 As Integer = 0 To n - 1
                    H(i2) = New Double(n - 1) {}
                Next i2
                ort = New Double(n - 1) {}

                For Counter As Integer = 0 To n - 1
                    For i As Integer = 0 To n - 1
                        H(i)(Counter) = A(i)(Counter)
                    Next i
                Next Counter

                ' Reduce to Hessenberg form.
                Orthes()

                ' Reduce Hessenberg to real Schur form.
                Hqr2()
            End If
        End Sub

#End Region '  Constructor

#Region "Public Properties"
        ''' <summary>Return the real parts of the eigenvalues</summary>
        ''' <returns>     real(diag(D))
        ''' </returns>
        Public Overridable ReadOnly Property RealEigenvalues() As Double()
            Get
                Return _D
            End Get
        End Property
        ''' <summary>Return the imaginary parts of the eigenvalues</summary>
        ''' <returns>     imag(diag(D))
        ''' </returns>
        Public Overridable ReadOnly Property ImagEigenvalues() As Double()
            Get
                Return e
            End Get
        End Property


#End Region '  Public Properties

#Region "Public Methods"

        ''' <summary>Return the eigenvector matrix</summary>
        ''' <returns>     V
        ''' </returns>

        Public Overridable Function GetV() As Matrix2D
            Return New Matrix2D(V, n, n)
        End Function
#End Region '  Public Methods

        ' A method called when serializing this class.
        Private Sub ISerializable_GetObjectData(ByVal info As SerializationInfo, ByVal context As StreamingContext) Implements ISerializable.GetObjectData
        End Sub
    End Class
End Namespace
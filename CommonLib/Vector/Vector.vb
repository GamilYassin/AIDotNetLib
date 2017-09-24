
Imports System


Namespace CommonLib
    ''' <summary>
    ''' A class to describe a two or three dimensional vector
    ''' Normally vector holds poistion and direction information
    ''' </summary>
    <Serializable>
    Public Class Vector
        ''' <summary>
        ''' The x component of the vector
        ''' </summary>
        Public x As Single

        ''' <summary>
        ''' The y component of the vector
        ''' </summary>
        Public y As Single

        ''' <summary>
        ''' The z component of the vector
        ''' </summary>
        Public z As Single


        <NonSerialized>
        Private _RandGenerator As RandomFactory

        ''' <summary>
        ''' Constructor for an empty vector: x, y, and z are set to 0.
        ''' </summary>
        Public Sub New()
            x = 0
            y = 0
            z = 0
            _RandGenerator = New RandomFactory
        End Sub


        ''' <summary>
        ''' Constructor for a 3D vector.
        ''' </summary>
        ''' <param name="x"> the x coordinate. </param>
        ''' <param name="y"> the y coordinate. </param>
        ''' <param name="z"> the z coordinate. </param>
        Public Sub New(ByVal x As Single, ByVal y As Single, ByVal z As Single)
            Me.New
            Me.x = x
            Me.y = y
            Me.z = z
        End Sub


        ''' <summary>
        ''' Constructor for a 2D vector: z coordinate is set to 0.
        ''' </summary>
        Public Sub New(ByVal x As Single, ByVal y As Single)
            Me.New
            Me.x = x
            Me.y = y
        End Sub


        ''' <summary>
        ''' Sets the x, y, and z component of the vector using two or three separate
        ''' variables, the data from a vector, or the values from a float array.
        '''  </summary>
        ''' <param name="x"> the x component of the vector </param>
        ''' <param name="y"> the y component of the vector </param>
        ''' <param name="z"> the z component of the vector </param>
        Public Overridable Function [Set](ByVal x As Single, ByVal y As Single, ByVal z As Single) As Vector
            Me.x = x
            Me.y = y
            Me.z = z
            Return Me
        End Function


        ''' <summary>
        ''' Sets the x, y, and z component of the vector using two or three separate
        ''' variables, the data from a vector, or the values from a float array.
        '''  </summary>
        ''' <param name="x"> the x component of the vector </param>
        ''' <param name="y"> the y component of the vector </param>
        Public Overridable Function [Set](ByVal x As Single, ByVal y As Single) As Vector
            Me.x = x
            Me.y = y
            Me.z = 0
            Return Me
        End Function


        ''' <summary>
        ''' Sets the x, y, and z component of the vector from another vector
        '''  </summary>
        ''' <param name="v"> vector to copy from </param>
        Public Overridable Function [Set](ByVal v As Vector) As Vector
            x = v.x
            y = v.y
            z = v.z
            Return Me
        End Function


        ''' <summary>
        ''' Set the x, y (and maybe z) coordinates using a float[] array as the source. </summary>
        ''' <param name="source"> array to copy from </param>
        Public Overridable Function [Set](ByVal source() As Single) As Vector
            If source.Length >= 2 Then
                x = source(0)
                y = source(1)
            End If
            If source.Length >= 3 Then
                z = source(2)
            Else
                z = 0
            End If
            Return Me
        End Function

        ''' <summary>
        ''' Randmize X, Y and Z components of vector between 0 and 1
        ''' </summary>
        Public Sub Randomize()
            Me.x = _RandGenerator.GetRandomDbl
            Me.y = _RandGenerator.GetRandomDbl
            Me.z = _RandGenerator.GetRandomDbl
        End Sub

        ''' <summary>
        ''' Make a new 2D unit vector from an angle
        ''' </summary>
        ''' <param name="target"> the target vector (if null, a new vector will be created) </param>
        ''' <returns> the vector </returns>
        Public Function FromAngle(ByVal angle As Single, ByVal target As Vector) As Vector
            Dim Output As New Vector()

            Output.Set(CSng(Math.Cos(angle)), CSng(Math.Sin(angle)), 0)
            Return Output
        End Function

        ''' <summary>
        ''' Make a new 2D unit vector from an angle.
        ''' </summary>
        ''' <param name="angle"> the angle in radians </param>
        ''' <returns> the new unit vector </returns>
        Public Function FromAngle(ByVal angle As Single) As Vector
            Return FromAngle(angle, Me)
        End Function


        ''' <summary>
        ''' Gets a copy of the vector, returns a vector object.
        ''' </summary>
        Public Overridable Function Copy() As Vector
            Return New Vector(x, y, z)
        End Function


        ''' <summary>
        ''' Return vector values as array
        ''' </summary>
        ''' <returns></returns>
        Public Overridable Function GetArray() As Single()
            Dim Output(2) As Single

            Output(0) = x
            Output(1) = y
            Output(2) = z

            Return Output
        End Function


        ''' <summary>
        ''' Calculates the magnitude (length) of the vector and returns the result
        ''' </summary>
        ''' <returns> magnitude (length) of the vector </returns>
        Public Overridable Function Magnitude() As Single
            Return CSng(Math.Sqrt(x * x + y * y + z * z))
        End Function


        ''' <summary>
        ''' Calculates the squared magnitude of the vector and returns the result
        ''' </summary>
        ''' <returns> squared magnitude of the vector </returns>
        Public Overridable Function MagSq() As Single
            Return (x * x + y * y + z * z)
        End Function


        ''' <summary>
        ''' Adds x, y, and z components to a vector, adds one vector to another, or
        ''' adds two independent vectors together
        ''' </summary>
        ''' <param name="v"> the vector to be added </param>
        Public Overridable Function Add(ByVal v As Vector) As Vector
            x += v.x
            y += v.y
            z += v.z
            Return Me
        End Function


        ''' <param name="x"> x component of the vector </param>
        ''' <param name="y"> y component of the vector </param>
        Public Overridable Function Add(ByVal x As Single, ByVal y As Single) As Vector
            Me.x += x
            Me.y += y
            Return Me
        End Function


        ''' <param name="z"> z component of the vector </param>
        Public Overridable Function Add(ByVal x As Single, ByVal y As Single, ByVal z As Single) As Vector
            Me.x += x
            Me.y += y
            Me.z += z
            Return Me
        End Function


        ''' <summary>
        ''' Add two vectors into a target vector </summary>
        Public Shared Function Add(ByVal v1 As Vector, ByVal v2 As Vector) As Vector
            Dim target As New Vector()

            target.Set(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z)
            Return target
        End Function


        ''' <summary>
        ''' Subtracts x, y, and z components from a vector, subtracts one vector
        ''' from another, or subtracts two independent vectors
        ''' </summary>
        ''' <param name="v"> any variable of type vector </param>
        Public Overridable Function [Sub](ByVal v As Vector) As Vector
            x -= v.x
            y -= v.y
            z -= v.z
            Return Me
        End Function


        ''' <param name="x"> the x component of the vector </param>
        ''' <param name="y"> the y component of the vector </param>
        Public Overridable Function [Sub](ByVal x As Single, ByVal y As Single) As Vector
            Me.x -= x
            Me.y -= y
            Return Me
        End Function


        ''' <param name="z"> the z component of the vector </param>
        Public Overridable Function [Sub](ByVal x As Single, ByVal y As Single, ByVal z As Single) As Vector
            Me.x -= x
            Me.y -= y
            Me.z -= z
            Return Me
        End Function


        ''' <summary>
        ''' Subtract one vector from another and store in another vector </summary>
        Public Shared Function [Sub](ByVal v1 As Vector, ByVal v2 As Vector) As Vector
            Dim target As New Vector

            target.Set(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z)
            Return target
        End Function


        ''' <summary>
        ''' Multiplies a vector by a scalar or multiplies one vector by another.
        ''' </summary>
        ''' <param name="n"> the number to multiply with the vector </param>
        Public Overridable Function Mult(ByVal n As Single) As Vector
            x *= n
            y *= n
            z *= n
            Return Me
        End Function


        ''' <summary>
        ''' Multiply a vector by a scalar, and write the result into a target vector. </summary>
        Public Shared Function Mult(ByVal v As Vector, ByVal n As Single) As Vector
            Dim target As New Vector

            target.Set(v.x * n, v.y * n, v.z * n)
            Return target
        End Function


        ''' <summary>
        ''' Divides a vector by a scalar or divides one vector by another.
        ''' </summary>
        ''' <param name="n"> the number by which to divide the vector </param>
        Public Overridable Function Div(ByVal n As Single) As Vector
            x /= n
            y /= n
            z /= n
            Return Me
        End Function

        ''' <summary>
        ''' Divide a vector by a scalar and store the result in another vector. </summary>
        Public Shared Function Div(ByVal v As Vector, ByVal n As Single) As Vector
            Dim target As New Vector

            target.Set(v.x / n, v.y / n, v.z / n)

            Return target
        End Function


        ''' <summary>
        ''' Calculates the Euclidean distance between two vectors
        ''' </summary>
        ''' <param name="v"> the x, y, and z coordinates of a vector</param>
        Public Overridable Function Distance(ByVal v As Vector) As Single
            Dim dx As Single = x - v.x
            Dim dy As Single = y - v.y
            Dim dz As Single = z - v.z
            Return CSng(Math.Sqrt(dx * dx + dy * dy + dz * dz))
        End Function


        ''' <param name="v1"> any variable of type vector </param>
        ''' <param name="v2"> any variable of type vector </param>
        ''' <returns> the Euclidean distance between v1 and v2 </returns>
        Public Shared Function Distance(ByVal v1 As Vector, ByVal v2 As Vector) As Single
            Dim dx As Single = v1.x - v2.x
            Dim dy As Single = v1.y - v2.y
            Dim dz As Single = v1.z - v2.z
            Return CSng(Math.Sqrt(dx * dx + dy * dy + dz * dz))
        End Function


        ''' <summary>
        ''' Calculates the dot product of two vectors.
        ''' </summary>
        ''' <param name="v"> any variable of type vector </param>
        ''' <returns> the dot product </returns>
        Public Overridable Function Dot(ByVal v As Vector) As Single
            Return x * v.x + y * v.y + z * v.z
        End Function


        ''' <param name="x"> x component of the vector </param>
        ''' <param name="y"> y component of the vector </param>
        ''' <param name="z"> z component of the vector </param>
        Public Overridable Function Dot(ByVal x As Single, ByVal y As Single, ByVal z As Single) As Single
            Return Me.x * x + Me.y * y + Me.z * z
        End Function


        ''' <param name="v1"> any variable of type vector </param>
        ''' <param name="v2"> any variable of type vector </param>
        Public Shared Function Dot(ByVal v1 As Vector, ByVal v2 As Vector) As Single
            Return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z
        End Function


        ''' <summary>
        ''' Calculates and returns a vector composed of the cross product between two vectors
        ''' </summary>
        ''' <param name="v"> 2nd vector vector </param>
        Public Overridable Function Cross(ByVal v As Vector) As Vector
            Dim target As New Vector
            Dim crossX As Single = y * v.z - v.y * z
            Dim crossY As Single = z * v.x - v.z * x
            Dim crossZ As Single = x * v.y - v.x * y

            target.Set(crossX, crossY, crossZ)
            Return target
        End Function


        ''' <param name="v1"> any variable of type vector </param>
        ''' <param name="v2"> any variable of type vector </param>
        Public Shared Function Cross(ByVal v1 As Vector, ByVal v2 As Vector) As Vector
            Dim target As New Vector
            Dim crossX As Single = v1.y * v2.z - v2.y * v1.z
            Dim crossY As Single = v1.z * v2.x - v2.z * v1.x
            Dim crossZ As Single = v1.x * v2.y - v2.x * v1.y

            target.Set(crossX, crossY, crossZ)
            Return target
        End Function


        ''' <summary>
        ''' Normalize the vector to length 1 (make it a unit vector).
        ''' </summary>
        Public Overridable Function Normalize() As Vector
            Dim m As Single = Magnitude()

            If m <> 0 AndAlso m <> 1 Then
                Div(m)
            End If
            Return Me
        End Function


        ''' <param name="target"> Set to null to create a new vector </param>
        ''' <returns> a new vector (if target was null), or target </returns>
        Public Overridable Function Normalize(ByVal target As Vector) As Vector
            If target Is Nothing Then
                target = New Vector()
            End If
            Dim m As Single = Magnitude()
            If m > 0 Then
                target.Set(x / m, y / m, z / m)
            Else
                target.Set(x, y, z)
            End If
            Return target
        End Function


        ''' <summary>
        ''' Limit the magnitude of this vector to the value passed as max parameter
        ''' </summary>
        ''' <param name="max"> the maximum magnitude for the vector </param>
        Public Overridable Function Limit(ByVal max As Single) As Vector
            If MagSq() > max * max Then
                Normalize()
                Mult(max)
            End If
            Return Me
        End Function


        ''' <summary>
        ''' Set the magnitude of this vector to the value passed as len parameter
        ''' </summary>
        ''' <param name="len"> the new length for this vector </param>
        Public Overridable Function SetMag(ByVal len As Single) As Vector
            Normalize()
            Mult(len)
            Return Me
        End Function


        ''' <summary>
        ''' Sets the magnitude of this vector, storing the result in another vector. </summary>
        ''' <param name="target"> Set to null to create a new vector </param>
        ''' <param name="len"> the new length for the new vector </param>
        ''' <returns> a new vector (if target was null), or target </returns>
        Public Overridable Function SetMag(ByVal target As Vector, ByVal len As Single) As Vector
            target = Normalize(target)
            target.Mult(len)
            Return target
        End Function


        ''' <summary>
        ''' Calculate the angle of rotation for this vector (only 2D vectors)
        ''' </summary>
        ''' <returns> the angle of rotation </returns>
        Public Overridable Function Heading() As Single
            Dim angle As Single = CSng(Math.Atan2(y, x))

            Return angle
        End Function

        ''' <summary>
        ''' Rotate the vector by an angle (only 2D vectors), magnitude remains the same
        ''' </summary>
        ''' <param name="theta"> the angle of rotation </param>
        Public Overridable Function Rotate(ByVal theta As Single) As Vector
            Dim temp As Single = x

            x = x * Math.Cos(theta) - y * Math.Sin(theta)
            y = temp * Math.Sin(theta) + y * Math.Cos(theta)
            Return Me
        End Function


        ''' <summary>
        ''' Linear interpolate the vector to another vector
        ''' </summary>
        ''' <param name="v"> the vector to lerp to </param>
        ''' <param name="Amount">  The amount of interpolation; some value between 0.0 (old vector) and 1.0 (new vector). 0.1 is very near the old vector; 0.5 is halfway in between. </param>
        Public Overridable Function Lerp(ByVal v As Vector, ByVal Amount As Single) As Vector
            x = MathFunctions.Lerp(x, v.x, Amount)
            y = MathFunctions.Lerp(y, v.y, Amount)
            z = MathFunctions.Lerp(z, v.z, Amount)
            Return Me
        End Function


        ''' <summary>
        ''' Linear interpolate between two vectors (returns a new vector object) </summary>
        ''' <param name="v1"> the vector to start from </param>
        ''' <param name="v2"> the vector to lerp to </param>
        Public Shared Function Lerp(ByVal v1 As Vector, ByVal v2 As Vector, ByVal Amount As Single) As Vector
            Dim v As Vector = v1.Copy()
            v.Lerp(v2, Amount)
            Return v
        End Function


        ''' <summary>
        ''' Linear interpolate the vector to x,y,z values </summary>
        ''' <param name="x"> the x component to lerp to </param>
        ''' <param name="y"> the y component to lerp to </param>
        ''' <param name="z"> the z component to lerp to </param>
        Public Overridable Function Lerp(ByVal x As Single, ByVal y As Single, ByVal z As Single, ByVal Amount As Single) As Vector
            Me.x = MathFunctions.Lerp(Me.x, x, Amount)
            Me.y = MathFunctions.Lerp(Me.y, y, Amount)
            Me.z = MathFunctions.Lerp(Me.z, z, Amount)
            Return Me
        End Function


        ''' <summary>
        ''' Calculates and returns the angle (in radians) between two vectors.
        ''' </summary>
        ''' <param name="v1"> 1st vector </param>
        ''' <param name="v2"> 2nd vector </param>
        Public Shared Function AngleBetween(ByVal v1 As Vector, ByVal v2 As Vector) As Single
            Dim dot As Double = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z
            Dim V1Mag As Double = v1.Magnitude
            Dim V2Mag As Double = v2.Magnitude
            Dim Amount As Double = dot / (V1Mag * V2Mag)

            If v1.x = 0 AndAlso v1.y = 0 AndAlso v1.z = 0 Then
                Return 0.0F
            End If
            If v2.x = 0 AndAlso v2.y = 0 AndAlso v2.z = 0 Then
                Return 0.0F
            End If
            If Amount <= -1 Then
                Return Math.PI
            ElseIf Amount >= 1 Then
                Return 0
            End If
            Return CSng(Math.Acos(Amount))
        End Function

        ''' <summary>
        ''' Returns coordinates of vector [x,y,z]
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function ToString() As String
            Return "[ " & x & ", " & y & ", " & z & " ]"
        End Function


        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If Not (TypeOf obj Is Vector) Then
                Return False
            End If
            Dim p As Vector = DirectCast(obj, Vector)
            Return x = p.x AndAlso y = p.y AndAlso z = p.z
        End Function


        Public Overrides Function GetHashCode() As Integer
            Dim result As Integer = 1
            result = 31 * result
            result = 31 * result
            result = 31 * result
            Return result
        End Function
    End Class
End Namespace
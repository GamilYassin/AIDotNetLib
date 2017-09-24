
Imports AIDotNetLib.CommonLib

Namespace LinearRegression
    Public Class MultipleRegression
        Public Shared Function NormalEquations(ByVal Input_Matrix As Matrix2D, ByVal Labels_Matrix As Matrix2D) As Matrix2D
            If Input_Matrix.RowDimension <> Labels_Matrix.RowDimension Then
                Throw New ArgumentException("Input Matrix Row Count must match Labels Size")
            End If

            If Input_Matrix.ColumnDimension > Labels_Matrix.RowDimension Then
                Throw New ArgumentException("Input Matrix Column Count is bigger than Labels Size")
            End If
            Dim Result As New CholeskyDecomposition(Input_Matrix)

            Return Result.Solve(Labels_Matrix)
        End Function
    End Class
End Namespace

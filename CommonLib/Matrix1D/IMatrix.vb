Imports AIDotNetLib.CommonLib

Namespace CommonLib
    Public Interface IMatrix

        ReadOnly Property Size As Integer

        Function Product(m2 As Matrix1D) As Matrix1D
        Function Product(m1 As Matrix1D, m2 As Matrix1D) As Matrix1D
        Function Product(m1 As Matrix1D, Scalar As Integer) As Matrix1D
        Function Product(m1 As Matrix1D, Scalar As Single) As Matrix1D
        Function Product(m1 As Matrix1D, Scalar As Double) As Matrix1D
        Function Product(Scalar As Integer) As Matrix1D
        Function Product(Scalar As Single) As Matrix1D
        Function Product(Scalar As Double) As Matrix1D

        Function Divide(m2 As Matrix1D) As Matrix1D
        Function Divide(m1 As Matrix1D, m2 As Matrix1D) As Matrix1D
        Function Divide(m1 As Matrix1D, Scalar As Integer) As Matrix1D
        Function Divide(m1 As Matrix1D, Scalar As Single) As Matrix1D
        Function Divide(m1 As Matrix1D, Scalar As Double) As Matrix1D
        Function Divide(Scalar As Integer) As Matrix1D
        Function Divide(Scalar As Single) As Matrix1D
        Function Divide(Scalar As Double) As Matrix1D

        Function Add(m2 As Matrix1D) As Matrix1D
        Function Add(m1 As Matrix1D, m2 As Matrix1D) As Matrix1D
        Function Add(m1 As Matrix1D, Scalar As Integer) As Matrix1D
        Function Add(m1 As Matrix1D, Scalar As Single) As Matrix1D
        Function Add(m1 As Matrix1D, Scalar As Double) As Matrix1D
        Function Add(Scalar As Integer) As Matrix1D
        Function Add(Scalar As Single) As Matrix1D
        Function Add(Scalar As Double) As Matrix1D

        Function [Sub](m2 As Matrix1D) As Matrix1D
        Function [Sub](m1 As Matrix1D, m2 As Matrix1D) As Matrix1D
        Function [Sub](m1 As Matrix1D, Scalar As Integer) As Matrix1D
        Function [Sub](m1 As Matrix1D, Scalar As Single) As Matrix1D
        Function [Sub](m1 As Matrix1D, Scalar As Double) As Matrix1D
        Function [Sub](Scalar As Integer) As Matrix1D
        Function [Sub](Scalar As Single) As Matrix1D
        Function [Sub](Scalar As Double) As Matrix1D

        Function Sum() As Single

        Function Copy(m1 As Matrix1D) As Matrix1D
        Function Copy(m1 As Matrix1D, StartingIndex As Integer) As Matrix1D
        Function Copy(StartingIndex As Integer) As Matrix1D

        Sub ForceValues(ForcedValue As Single)

        Function GetValue(Index As Integer) As Single
        Sub SetValue(Index As Integer, Value As Single)

        Function ToString() As String
    End Interface
End Namespace


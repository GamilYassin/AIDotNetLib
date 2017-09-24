
Imports AIDotNetLib.CommonLib

Namespace LinearRegression
    Public Class SimpleRegression
        ''' <summary>
        ''' 1-	Calculate Average of inputs and Labels (Xavg and Yavg)
        '''     Average = sum/number
        ''' 2-	Calculate sample Covariance
        ''' 3-	Calculate sample Variance
        ''' 4-	Calculate m = Cov/Var
        ''' 5-	Calculate a= Yavg – m * Xavg
        ''' </summary>
        ''' <param name="X_Inputs"></param>
        ''' <param name="Y_Labels"></param>
        ''' <returns>Line with X as intercept and Y as slope</returns>
        Public Shared Function LineBestFit(X_Inputs As Matrix1D, Y_Labels As Matrix1D) As Vector
            Dim Line As New Vector
            Dim X_avg As Double = 0
            Dim Y_Avg As Double = 0
            Dim Cov_XY As Double = 0
            Dim Var_X As Double = 0
            Dim X_Copy As New Matrix1D(X_Inputs)
            Dim Y_Copy As New Matrix1D(Y_Labels)

            ' 1-	Calculate Average of inputs and Labels (Xavg & Yavg)
            X_avg = X_Inputs.Sum / X_Inputs.Size
            Y_Avg = Y_Labels.Sum / Y_Labels.Size
            ' 2-	Calculate sample Covariance
            X_Copy = X_Inputs.Sub(X_avg)
            Y_Copy = Y_Labels.Sub(Y_Avg)
            Cov_XY = X_Copy.Product(X_Copy, Y_Copy).Sum
            ' 3-	Calculate sample Variance
            Var_X = X_Copy.SquaredSum
            ' 4-	Calculate m = Cov/Var
            Line.y = CSng(Cov_XY / Var_X)
            ' 5-	Calculate a= Yavg – m * Xavg
            Line.x = CSng(Y_Avg - Line.y * X_avg)

            Return Line
        End Function

        ''' <summary>
        ''' Return MSE (Mean Square Error) from given Input Data set, Labels and estimated equation
        ''' </summary>
        ''' <param name="X_Inputs"></param>
        ''' <param name="Y_Labels"></param>
        ''' <param name="Line">X is intercept and Y is slope</param>
        ''' <returns></returns>
        Public Shared Function Validate(X_Inputs As Matrix1D, Y_Labels As Matrix1D, Line As Vector) As Single
            Dim Err As New Matrix1D(Y_Labels)
            Dim Y_Estimate As New Matrix1D(Y_Labels)

            Y_Estimate = X_Inputs.Product(Line.y)
            Y_Estimate = Y_Estimate.Add(Line.x)
            Err = Y_Labels.Sub(Y_Estimate)

            Return Err.SquaredSum / (2 * X_Inputs.Size)
        End Function
    End Class
End Namespace

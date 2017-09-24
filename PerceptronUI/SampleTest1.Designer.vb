<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SampleTest1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnCreate = New System.Windows.Forms.Button()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.btnTrain = New System.Windows.Forms.Button()
        Me.btnDraw = New System.Windows.Forms.Button()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCreate
        '
        Me.btnCreate.Location = New System.Drawing.Point(618, 12)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(129, 23)
        Me.btnCreate.TabIndex = 0
        Me.btnCreate.Text = "Create TrainingSet"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'PictureBox1
        '
        Me.PictureBox1.BackColor = System.Drawing.Color.White
        Me.PictureBox1.Location = New System.Drawing.Point(12, 12)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(600, 400)
        Me.PictureBox1.TabIndex = 1
        Me.PictureBox1.TabStop = False
        '
        'btnTrain
        '
        Me.btnTrain.Location = New System.Drawing.Point(618, 41)
        Me.btnTrain.Name = "btnTrain"
        Me.btnTrain.Size = New System.Drawing.Size(129, 23)
        Me.btnTrain.TabIndex = 2
        Me.btnTrain.Text = "Train Perceptron"
        Me.btnTrain.UseVisualStyleBackColor = True
        '
        'btnDraw
        '
        Me.btnDraw.Location = New System.Drawing.Point(618, 70)
        Me.btnDraw.Name = "btnDraw"
        Me.btnDraw.Size = New System.Drawing.Size(129, 23)
        Me.btnDraw.TabIndex = 3
        Me.btnDraw.Text = "Draw h(x)"
        Me.btnDraw.UseVisualStyleBackColor = True
        '
        'SampleTest1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(764, 419)
        Me.Controls.Add(Me.btnDraw)
        Me.Controls.Add(Me.btnTrain)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.btnCreate)
        Me.Name = "SampleTest1"
        Me.Text = "SampleTest1"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents btnCreate As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents btnTrain As Button
    Friend WithEvents btnDraw As Button
End Class

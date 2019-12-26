'
' Created by SharpDevelop.
' User: Alfred
' Date: 17/07/2018
' Time: 18:19
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class Setup
	Inherits System.Windows.Forms.Form
	
	''' <summary>
	''' Designer variable used to keep track of non-visual components.
	''' </summary>
	Private components As System.ComponentModel.IContainer
	
	''' <summary>
	''' Disposes resources used by the form.
	''' </summary>
	''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
	Protected Overrides Sub Dispose(ByVal disposing As Boolean)
		If disposing Then
			If components IsNot Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(disposing)
	End Sub
	
	''' <summary>
	''' This method is required for Windows Forms designer support.
	''' Do not change the method contents inside the source code editor. The Forms designer might
	''' not be able to load this method if it was changed manually.
	''' </summary>
	Private Sub InitializeComponent()
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Setup))
		Me.label1 = New System.Windows.Forms.Label()
		Me.label2 = New System.Windows.Forms.Label()
		Me.butok = New System.Windows.Forms.Button()
		Me.butcancel = New System.Windows.Forms.Button()
		Me.label3 = New System.Windows.Forms.Label()
		Me.label4 = New System.Windows.Forms.Label()
		Me.cbsli = New System.Windows.Forms.ComboBox()
		Me.nudp = New System.Windows.Forms.NumericUpDown()
		Me.chkbxdid = New System.Windows.Forms.CheckBox()
		CType(Me.nudp,System.ComponentModel.ISupportInitialize).BeginInit
		Me.SuspendLayout
		'
		'label1
		'
		Me.label1.Location = New System.Drawing.Point(16, 87)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(137, 19)
		Me.label1.TabIndex = 0
		Me.label1.Text = "Server Listening Interface:"
		Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'label2
		'
		Me.label2.Location = New System.Drawing.Point(16, 132)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(70, 23)
		Me.label2.TabIndex = 1
		Me.label2.Text = "Server Port:"
		Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'butok
		'
		Me.butok.Location = New System.Drawing.Point(16, 226)
		Me.butok.Name = "butok"
		Me.butok.Size = New System.Drawing.Size(75, 23)
		Me.butok.TabIndex = 2
		Me.butok.Text = "Ok"
		Me.butok.UseVisualStyleBackColor = true
		AddHandler Me.butok.Click, AddressOf Me.Butok_Click
		'
		'butcancel
		'
		Me.butcancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
		Me.butcancel.Location = New System.Drawing.Point(297, 226)
		Me.butcancel.Name = "butcancel"
		Me.butcancel.Size = New System.Drawing.Size(75, 23)
		Me.butcancel.TabIndex = 3
		Me.butcancel.Text = "Cancel"
		Me.butcancel.UseVisualStyleBackColor = true
		AddHandler Me.butcancel.Click, AddressOf Me.Butcancel_Click
		'
		'label3
		'
		Me.label3.Font = New System.Drawing.Font("Consolas", 24!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.label3.Location = New System.Drawing.Point(38, 9)
		Me.label3.Name = "label3"
		Me.label3.Size = New System.Drawing.Size(321, 57)
		Me.label3.TabIndex = 4
		Me.label3.Text = "Setup Server"
		Me.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
		'
		'label4
		'
		Me.label4.Location = New System.Drawing.Point(16, 173)
		Me.label4.Name = "label4"
		Me.label4.Size = New System.Drawing.Size(100, 23)
		Me.label4.TabIndex = 5
		Me.label4.Text = "Delay in Data:"
		Me.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'cbsli
		'
		Me.cbsli.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
		Me.cbsli.FormattingEnabled = true
		Me.cbsli.Location = New System.Drawing.Point(182, 87)
		Me.cbsli.Name = "cbsli"
		Me.cbsli.Size = New System.Drawing.Size(177, 21)
		Me.cbsli.TabIndex = 6
		'
		'nudp
		'
		Me.nudp.Location = New System.Drawing.Point(182, 135)
		Me.nudp.Maximum = New Decimal(New Integer() {65535, 0, 0, 0})
		Me.nudp.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
		Me.nudp.Name = "nudp"
		Me.nudp.Size = New System.Drawing.Size(177, 20)
		Me.nudp.TabIndex = 7
		Me.nudp.Value = New Decimal(New Integer() {100, 0, 0, 0})
		'
		'chkbxdid
		'
		Me.chkbxdid.Location = New System.Drawing.Point(182, 173)
		Me.chkbxdid.Name = "chkbxdid"
		Me.chkbxdid.Size = New System.Drawing.Size(16, 24)
		Me.chkbxdid.TabIndex = 8
		Me.chkbxdid.UseVisualStyleBackColor = true
		'
		'Setup
		'
		Me.AcceptButton = Me.butok
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.CancelButton = Me.butcancel
		Me.ClientSize = New System.Drawing.Size(384, 261)
		Me.Controls.Add(Me.chkbxdid)
		Me.Controls.Add(Me.nudp)
		Me.Controls.Add(Me.cbsli)
		Me.Controls.Add(Me.label4)
		Me.Controls.Add(Me.label3)
		Me.Controls.Add(Me.butcancel)
		Me.Controls.Add(Me.butok)
		Me.Controls.Add(Me.label2)
		Me.Controls.Add(Me.label1)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MaximizeBox = false
		Me.MaximumSize = New System.Drawing.Size(400, 300)
		Me.MinimizeBox = false
		Me.MinimumSize = New System.Drawing.Size(400, 300)
		Me.Name = "Setup"
		Me.ShowIcon = false
		Me.ShowInTaskbar = false
		Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
		Me.Text = "Setup Server"
		AddHandler FormClosing, AddressOf Me.Setup_FormClosing
		AddHandler FormClosed, AddressOf Me.Setup_FormClosed
		AddHandler Load, AddressOf Me.Setup_Load
		CType(Me.nudp,System.ComponentModel.ISupportInitialize).EndInit
		Me.ResumeLayout(false)
	End Sub
	Private chkbxdid As System.Windows.Forms.CheckBox
	Private nudp As System.Windows.Forms.NumericUpDown
	Private cbsli As System.Windows.Forms.ComboBox
	Private label4 As System.Windows.Forms.Label
	Private label3 As System.Windows.Forms.Label
	Private butcancel As System.Windows.Forms.Button
	Private butok As System.Windows.Forms.Button
	Private label2 As System.Windows.Forms.Label
	Private label1 As System.Windows.Forms.Label
End Class

'
' Created by SharpDevelop.
' User: Alfred
' Date: 15/07/2018
' Time: 10:01
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Partial Class MainForm
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
		Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
		Me.tableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
		Me.txtbx = New System.Windows.Forms.TextBox()
		Me.tableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
		Me.lblstat = New System.Windows.Forms.Label()
		Me.pbstat = New System.Windows.Forms.ProgressBar()
		Me.butcancel = New System.Windows.Forms.Button()
		Me.groupBox1 = New System.Windows.Forms.GroupBox()
		Me.tableLayoutPanel3 = New System.Windows.Forms.TableLayoutPanel()
		Me.butss = New System.Windows.Forms.Button()
		Me.dudsa = New System.Windows.Forms.DomainUpDown()
		Me.label1 = New System.Windows.Forms.Label()
		Me.label2 = New System.Windows.Forms.Label()
		Me.txtbxp = New System.Windows.Forms.TextBox()
		Me.butrs = New System.Windows.Forms.Button()
		Me.groupBox2 = New System.Windows.Forms.GroupBox()
		Me.tableLayoutPanel4 = New System.Windows.Forms.TableLayoutPanel()
		Me.buta = New System.Windows.Forms.Button()
		Me.butcls = New System.Windows.Forms.Button()
		Me.butsave = New System.Windows.Forms.Button()
		Me.butopen = New System.Windows.Forms.Button()
		Me.txtbxname = New System.Windows.Forms.TextBox()
		Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
		Me.saveFileDialog1 = New System.Windows.Forms.SaveFileDialog()
		Me.tableLayoutPanel1.SuspendLayout
		Me.tableLayoutPanel2.SuspendLayout
		Me.groupBox1.SuspendLayout
		Me.tableLayoutPanel3.SuspendLayout
		Me.groupBox2.SuspendLayout
		Me.tableLayoutPanel4.SuspendLayout
		Me.SuspendLayout
		'
		'tableLayoutPanel1
		'
		Me.tableLayoutPanel1.ColumnCount = 1
		Me.tableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
		Me.tableLayoutPanel1.Controls.Add(Me.txtbx, 0, 2)
		Me.tableLayoutPanel1.Controls.Add(Me.tableLayoutPanel2, 0, 3)
		Me.tableLayoutPanel1.Controls.Add(Me.groupBox1, 0, 0)
		Me.tableLayoutPanel1.Controls.Add(Me.groupBox2, 0, 1)
		Me.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
		Me.tableLayoutPanel1.Name = "tableLayoutPanel1"
		Me.tableLayoutPanel1.RowCount = 4
		Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15!))
		Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15!))
		Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70!))
		Me.tableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30!))
		Me.tableLayoutPanel1.Size = New System.Drawing.Size(584, 361)
		Me.tableLayoutPanel1.TabIndex = 0
		'
		'txtbx
		'
		Me.txtbx.Dock = System.Windows.Forms.DockStyle.Fill
		Me.txtbx.Font = New System.Drawing.Font("Consolas", 12!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.txtbx.Location = New System.Drawing.Point(3, 101)
		Me.txtbx.MaxLength = 65535
		Me.txtbx.Multiline = true
		Me.txtbx.Name = "txtbx"
		Me.txtbx.ScrollBars = System.Windows.Forms.ScrollBars.Both
		Me.txtbx.Size = New System.Drawing.Size(578, 225)
		Me.txtbx.TabIndex = 0
		Me.txtbx.WordWrap = false
		AddHandler Me.txtbx.KeyDown, AddressOf Me.Txtbx_KeyDown
		'
		'tableLayoutPanel2
		'
		Me.tableLayoutPanel2.ColumnCount = 3
		Me.tableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
		Me.tableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
		Me.tableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40!))
		Me.tableLayoutPanel2.Controls.Add(Me.lblstat, 0, 0)
		Me.tableLayoutPanel2.Controls.Add(Me.pbstat, 1, 0)
		Me.tableLayoutPanel2.Controls.Add(Me.butcancel, 2, 0)
		Me.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tableLayoutPanel2.Location = New System.Drawing.Point(3, 332)
		Me.tableLayoutPanel2.Name = "tableLayoutPanel2"
		Me.tableLayoutPanel2.RowCount = 1
		Me.tableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
		Me.tableLayoutPanel2.Size = New System.Drawing.Size(578, 26)
		Me.tableLayoutPanel2.TabIndex = 1
		'
		'lblstat
		'
		Me.lblstat.Dock = System.Windows.Forms.DockStyle.Fill
		Me.lblstat.Location = New System.Drawing.Point(3, 0)
		Me.lblstat.Name = "lblstat"
		Me.lblstat.Size = New System.Drawing.Size(263, 26)
		Me.lblstat.TabIndex = 0
		Me.lblstat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'pbstat
		'
		Me.pbstat.Dock = System.Windows.Forms.DockStyle.Fill
		Me.pbstat.Location = New System.Drawing.Point(272, 3)
		Me.pbstat.Name = "pbstat"
		Me.pbstat.Size = New System.Drawing.Size(263, 20)
		Me.pbstat.Style = System.Windows.Forms.ProgressBarStyle.Continuous
		Me.pbstat.TabIndex = 1
		'
		'butcancel
		'
		Me.butcancel.Dock = System.Windows.Forms.DockStyle.Fill
		Me.butcancel.Enabled = false
		Me.butcancel.Location = New System.Drawing.Point(541, 3)
		Me.butcancel.Name = "butcancel"
		Me.butcancel.Size = New System.Drawing.Size(34, 20)
		Me.butcancel.TabIndex = 2
		Me.butcancel.Text = "X"
		Me.butcancel.UseVisualStyleBackColor = true
		AddHandler Me.butcancel.Click, AddressOf Me.Butcancel_Click
		'
		'groupBox1
		'
		Me.groupBox1.Controls.Add(Me.tableLayoutPanel3)
		Me.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.groupBox1.Location = New System.Drawing.Point(3, 3)
		Me.groupBox1.Name = "groupBox1"
		Me.groupBox1.Size = New System.Drawing.Size(578, 43)
		Me.groupBox1.TabIndex = 2
		Me.groupBox1.TabStop = false
		Me.groupBox1.Text = "Server Stats"
		'
		'tableLayoutPanel3
		'
		Me.tableLayoutPanel3.ColumnCount = 6
		Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66528!))
		Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66695!))
		Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66695!))
		Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66695!))
		Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66695!))
		Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66695!))
		Me.tableLayoutPanel3.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20!))
		Me.tableLayoutPanel3.Controls.Add(Me.butss, 5, 0)
		Me.tableLayoutPanel3.Controls.Add(Me.dudsa, 1, 0)
		Me.tableLayoutPanel3.Controls.Add(Me.label1, 0, 0)
		Me.tableLayoutPanel3.Controls.Add(Me.label2, 2, 0)
		Me.tableLayoutPanel3.Controls.Add(Me.txtbxp, 3, 0)
		Me.tableLayoutPanel3.Controls.Add(Me.butrs, 4, 0)
		Me.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tableLayoutPanel3.Location = New System.Drawing.Point(3, 16)
		Me.tableLayoutPanel3.Name = "tableLayoutPanel3"
		Me.tableLayoutPanel3.RowCount = 1
		Me.tableLayoutPanel3.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
		Me.tableLayoutPanel3.Size = New System.Drawing.Size(572, 24)
		Me.tableLayoutPanel3.TabIndex = 0
		'
		'butss
		'
		Me.butss.Dock = System.Windows.Forms.DockStyle.Fill
		Me.butss.Font = New System.Drawing.Font("Microsoft Sans Serif", 6!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.butss.Location = New System.Drawing.Point(478, 3)
		Me.butss.Name = "butss"
		Me.butss.Size = New System.Drawing.Size(91, 18)
		Me.butss.TabIndex = 6
		Me.butss.Text = "Stop Server"
		Me.butss.UseVisualStyleBackColor = true
		AddHandler Me.butss.Click, AddressOf Me.Butss_Click
		'
		'dudsa
		'
		Me.dudsa.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.dudsa.Location = New System.Drawing.Point(98, 3)
		Me.dudsa.Name = "dudsa"
		Me.dudsa.ReadOnly = true
		Me.dudsa.Size = New System.Drawing.Size(89, 20)
		Me.dudsa.TabIndex = 1
		'
		'label1
		'
		Me.label1.Dock = System.Windows.Forms.DockStyle.Fill
		Me.label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.label1.Location = New System.Drawing.Point(3, 0)
		Me.label1.Name = "label1"
		Me.label1.Size = New System.Drawing.Size(89, 24)
		Me.label1.TabIndex = 2
		Me.label1.Text = "Server Adress(es):"
		Me.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'label2
		'
		Me.label2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.label2.Location = New System.Drawing.Point(193, 0)
		Me.label2.Name = "label2"
		Me.label2.Size = New System.Drawing.Size(89, 24)
		Me.label2.TabIndex = 3
		Me.label2.Text = "Server Port:"
		Me.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
		'
		'txtbxp
		'
		Me.txtbxp.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.txtbxp.Location = New System.Drawing.Point(288, 3)
		Me.txtbxp.Name = "txtbxp"
		Me.txtbxp.ReadOnly = true
		Me.txtbxp.Size = New System.Drawing.Size(89, 20)
		Me.txtbxp.TabIndex = 4
		'
		'butrs
		'
		Me.butrs.Dock = System.Windows.Forms.DockStyle.Fill
		Me.butrs.Font = New System.Drawing.Font("Microsoft Sans Serif", 6!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.butrs.Location = New System.Drawing.Point(383, 3)
		Me.butrs.Name = "butrs"
		Me.butrs.Size = New System.Drawing.Size(89, 18)
		Me.butrs.TabIndex = 5
		Me.butrs.Text = "Reset Server"
		Me.butrs.UseVisualStyleBackColor = true
		AddHandler Me.butrs.Click, AddressOf Me.Butrs_Click
		'
		'groupBox2
		'
		Me.groupBox2.Controls.Add(Me.tableLayoutPanel4)
		Me.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill
		Me.groupBox2.Location = New System.Drawing.Point(3, 52)
		Me.groupBox2.Name = "groupBox2"
		Me.groupBox2.Size = New System.Drawing.Size(578, 43)
		Me.groupBox2.TabIndex = 3
		Me.groupBox2.TabStop = false
		Me.groupBox2.Text = "Program Management"
		'
		'tableLayoutPanel4
		'
		Me.tableLayoutPanel4.ColumnCount = 5
		Me.tableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.09804!))
		Me.tableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.72549!))
		Me.tableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.72549!))
		Me.tableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.72549!))
		Me.tableLayoutPanel4.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.72549!))
		Me.tableLayoutPanel4.Controls.Add(Me.buta, 4, 0)
		Me.tableLayoutPanel4.Controls.Add(Me.butcls, 3, 0)
		Me.tableLayoutPanel4.Controls.Add(Me.butsave, 2, 0)
		Me.tableLayoutPanel4.Controls.Add(Me.butopen, 1, 0)
		Me.tableLayoutPanel4.Controls.Add(Me.txtbxname, 0, 0)
		Me.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill
		Me.tableLayoutPanel4.Location = New System.Drawing.Point(3, 16)
		Me.tableLayoutPanel4.Name = "tableLayoutPanel4"
		Me.tableLayoutPanel4.RowCount = 1
		Me.tableLayoutPanel4.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
		Me.tableLayoutPanel4.Size = New System.Drawing.Size(572, 24)
		Me.tableLayoutPanel4.TabIndex = 0
		'
		'buta
		'
		Me.buta.Dock = System.Windows.Forms.DockStyle.Fill
		Me.buta.Font = New System.Drawing.Font("Microsoft Sans Serif", 6!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.buta.Location = New System.Drawing.Point(494, 3)
		Me.buta.Name = "buta"
		Me.buta.Size = New System.Drawing.Size(75, 18)
		Me.buta.TabIndex = 9
		Me.buta.Text = "About"
		Me.buta.UseVisualStyleBackColor = true
		AddHandler Me.buta.Click, AddressOf Me.Buta_Click
		'
		'butcls
		'
		Me.butcls.Dock = System.Windows.Forms.DockStyle.Fill
		Me.butcls.Font = New System.Drawing.Font("Microsoft Sans Serif", 6!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.butcls.Location = New System.Drawing.Point(416, 3)
		Me.butcls.Name = "butcls"
		Me.butcls.Size = New System.Drawing.Size(72, 18)
		Me.butcls.TabIndex = 8
		Me.butcls.Text = "Clear"
		Me.butcls.UseVisualStyleBackColor = true
		AddHandler Me.butcls.Click, AddressOf Me.Butcls_Click
		'
		'butsave
		'
		Me.butsave.Dock = System.Windows.Forms.DockStyle.Fill
		Me.butsave.Font = New System.Drawing.Font("Microsoft Sans Serif", 6!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.butsave.Location = New System.Drawing.Point(338, 3)
		Me.butsave.Name = "butsave"
		Me.butsave.Size = New System.Drawing.Size(72, 18)
		Me.butsave.TabIndex = 7
		Me.butsave.Text = "Save"
		Me.butsave.UseVisualStyleBackColor = true
		AddHandler Me.butsave.Click, AddressOf Me.Butsave_Click
		'
		'butopen
		'
		Me.butopen.Dock = System.Windows.Forms.DockStyle.Fill
		Me.butopen.Font = New System.Drawing.Font("Microsoft Sans Serif", 6!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.butopen.Location = New System.Drawing.Point(260, 3)
		Me.butopen.Name = "butopen"
		Me.butopen.Size = New System.Drawing.Size(72, 18)
		Me.butopen.TabIndex = 6
		Me.butopen.Text = "Open"
		Me.butopen.UseVisualStyleBackColor = true
		AddHandler Me.butopen.Click, AddressOf Me.Butopen_Click
		'
		'txtbxname
		'
		Me.txtbxname.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
		Me.txtbxname.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0,Byte))
		Me.txtbxname.Location = New System.Drawing.Point(3, 3)
		Me.txtbxname.Name = "txtbxname"
		Me.txtbxname.Size = New System.Drawing.Size(251, 20)
		Me.txtbxname.TabIndex = 0
		Me.txtbxname.WordWrap = false
		'
		'OpenFileDialog1
		'
		Me.OpenFileDialog1.Filter = "All Files (*.*)|*.*"
		'
		'saveFileDialog1
		'
		Me.saveFileDialog1.Filter = "All Files (*.*)|*.*"
		'
		'MainForm
		'
		Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
		Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		Me.ClientSize = New System.Drawing.Size(584, 361)
		Me.Controls.Add(Me.tableLayoutPanel1)
		Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
		Me.MinimumSize = New System.Drawing.Size(600, 400)
		Me.Name = "MainForm"
		Me.Text = "OC Upload Download File Server"
		AddHandler FormClosing, AddressOf Me.MainForm_FormClosing
		AddHandler FormClosed, AddressOf Me.MainForm_FormClosed
		AddHandler Load, AddressOf Me.MainForm_Load
		AddHandler Shown, AddressOf Me.MainForm_Shown
		Me.tableLayoutPanel1.ResumeLayout(false)
		Me.tableLayoutPanel1.PerformLayout
		Me.tableLayoutPanel2.ResumeLayout(false)
		Me.groupBox1.ResumeLayout(false)
		Me.tableLayoutPanel3.ResumeLayout(false)
		Me.tableLayoutPanel3.PerformLayout
		Me.groupBox2.ResumeLayout(false)
		Me.tableLayoutPanel4.ResumeLayout(false)
		Me.tableLayoutPanel4.PerformLayout
		Me.ResumeLayout(false)
	End Sub
	Private buta As System.Windows.Forms.Button
	Private saveFileDialog1 As System.Windows.Forms.SaveFileDialog
	Private OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
	Private butopen As System.Windows.Forms.Button
	Private butsave As System.Windows.Forms.Button
	Private butcls As System.Windows.Forms.Button
	Private butrs As System.Windows.Forms.Button
	Private butss As System.Windows.Forms.Button
	Private txtbxp As System.Windows.Forms.TextBox
	Private label2 As System.Windows.Forms.Label
	Private dudsa As System.Windows.Forms.DomainUpDown
	Private label1 As System.Windows.Forms.Label
	Private txtbxname As System.Windows.Forms.TextBox
	Private tableLayoutPanel4 As System.Windows.Forms.TableLayoutPanel
	Private tableLayoutPanel3 As System.Windows.Forms.TableLayoutPanel
	Private groupBox2 As System.Windows.Forms.GroupBox
	Private groupBox1 As System.Windows.Forms.GroupBox
	Private butcancel As System.Windows.Forms.Button
	Private pbstat As System.Windows.Forms.ProgressBar
	Private lblstat As System.Windows.Forms.Label
	Private tableLayoutPanel2 As System.Windows.Forms.TableLayoutPanel
	Private txtbx As System.Windows.Forms.TextBox
	Private tableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
End Class

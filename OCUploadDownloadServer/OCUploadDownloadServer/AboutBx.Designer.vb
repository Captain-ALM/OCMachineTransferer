﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AboutBx
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

    Friend WithEvents TableLayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LabelProductName As System.Windows.Forms.Label
    Friend WithEvents LabelVersion As System.Windows.Forms.Label
    Friend WithEvents LabelCompanyName As System.Windows.Forms.Label
    Friend WithEvents LabelCopyright As System.Windows.Forms.Label

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
    	Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AboutBx))
    	Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
    	Me.LabelProductName = New System.Windows.Forms.Label()
    	Me.LabelVersion = New System.Windows.Forms.Label()
    	Me.LabelCopyright = New System.Windows.Forms.Label()
    	Me.LabelCompanyName = New System.Windows.Forms.Label()
    	Me.OKButton = New System.Windows.Forms.Button()
    	Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
    	Me.TextBox1 = New System.Windows.Forms.TextBox()
    	Me.TextBoxDescription = New System.Windows.Forms.TextBox()
    	Me.TableLayoutPanel.SuspendLayout
    	Me.TableLayoutPanel1.SuspendLayout
    	Me.SuspendLayout
    	'
    	'TableLayoutPanel
    	'
    	Me.TableLayoutPanel.ColumnCount = 1
    	Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100!))
    	Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20!))
    	Me.TableLayoutPanel.Controls.Add(Me.LabelProductName, 0, 0)
    	Me.TableLayoutPanel.Controls.Add(Me.LabelVersion, 0, 1)
    	Me.TableLayoutPanel.Controls.Add(Me.LabelCopyright, 0, 2)
    	Me.TableLayoutPanel.Controls.Add(Me.LabelCompanyName, 0, 3)
    	Me.TableLayoutPanel.Controls.Add(Me.OKButton, 0, 5)
    	Me.TableLayoutPanel.Controls.Add(Me.TableLayoutPanel1, 0, 4)
    	Me.TableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.TableLayoutPanel.Location = New System.Drawing.Point(9, 9)
    	Me.TableLayoutPanel.Name = "TableLayoutPanel"
    	Me.TableLayoutPanel.RowCount = 6
    	Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10!))
    	Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10!))
    	Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10!))
    	Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10!))
    	Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50!))
    	Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10!))
    	Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20!))
    	Me.TableLayoutPanel.Size = New System.Drawing.Size(396, 258)
    	Me.TableLayoutPanel.TabIndex = 0
    	'
    	'LabelProductName
    	'
    	Me.LabelProductName.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.LabelProductName.Location = New System.Drawing.Point(6, 0)
    	Me.LabelProductName.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    	Me.LabelProductName.MaximumSize = New System.Drawing.Size(0, 17)
    	Me.LabelProductName.Name = "LabelProductName"
    	Me.LabelProductName.Size = New System.Drawing.Size(387, 17)
    	Me.LabelProductName.TabIndex = 0
    	Me.LabelProductName.Text = "Product Name"
    	Me.LabelProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    	'
    	'LabelVersion
    	'
    	Me.LabelVersion.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.LabelVersion.Location = New System.Drawing.Point(6, 25)
    	Me.LabelVersion.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    	Me.LabelVersion.MaximumSize = New System.Drawing.Size(0, 17)
    	Me.LabelVersion.Name = "LabelVersion"
    	Me.LabelVersion.Size = New System.Drawing.Size(387, 17)
    	Me.LabelVersion.TabIndex = 0
    	Me.LabelVersion.Text = "Version"
    	Me.LabelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    	'
    	'LabelCopyright
    	'
    	Me.LabelCopyright.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.LabelCopyright.Location = New System.Drawing.Point(6, 50)
    	Me.LabelCopyright.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    	Me.LabelCopyright.MaximumSize = New System.Drawing.Size(0, 17)
    	Me.LabelCopyright.Name = "LabelCopyright"
    	Me.LabelCopyright.Size = New System.Drawing.Size(387, 17)
    	Me.LabelCopyright.TabIndex = 0
    	Me.LabelCopyright.Text = "Copyright"
    	Me.LabelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    	'
    	'LabelCompanyName
    	'
    	Me.LabelCompanyName.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.LabelCompanyName.Location = New System.Drawing.Point(6, 75)
    	Me.LabelCompanyName.Margin = New System.Windows.Forms.Padding(6, 0, 3, 0)
    	Me.LabelCompanyName.MaximumSize = New System.Drawing.Size(0, 17)
    	Me.LabelCompanyName.Name = "LabelCompanyName"
    	Me.LabelCompanyName.Size = New System.Drawing.Size(387, 17)
    	Me.LabelCompanyName.TabIndex = 0
    	Me.LabelCompanyName.Text = "Company Name"
    	Me.LabelCompanyName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
    	'
    	'OKButton
    	'
    	Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right),System.Windows.Forms.AnchorStyles)
    	Me.OKButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
    	Me.OKButton.Location = New System.Drawing.Point(318, 232)
    	Me.OKButton.Name = "OKButton"
    	Me.OKButton.Size = New System.Drawing.Size(75, 23)
    	Me.OKButton.TabIndex = 0
    	Me.OKButton.Text = "&OK"
    	'
    	'TableLayoutPanel1
    	'
    	Me.TableLayoutPanel1.ColumnCount = 2
    	Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
    	Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50!))
    	Me.TableLayoutPanel1.Controls.Add(Me.TextBox1, 0, 0)
    	Me.TableLayoutPanel1.Controls.Add(Me.TextBoxDescription, 0, 0)
    	Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 103)
    	Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
    	Me.TableLayoutPanel1.RowCount = 1
    	Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100!))
    	Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 123!))
    	Me.TableLayoutPanel1.Size = New System.Drawing.Size(390, 123)
    	Me.TableLayoutPanel1.TabIndex = 1
    	'
    	'TextBox1
    	'
    	Me.TextBox1.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.TextBox1.Location = New System.Drawing.Point(201, 3)
    	Me.TextBox1.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
    	Me.TextBox1.Multiline = true
    	Me.TextBox1.Name = "TextBox1"
    	Me.TextBox1.ReadOnly = true
    	Me.TextBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both
    	Me.TextBox1.Size = New System.Drawing.Size(186, 117)
    	Me.TextBox1.TabIndex = 2
    	Me.TextBox1.TabStop = false
    	Me.TextBox1.Text = "License :"&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"(At runtime, the labels' text will be replaced with the application'"& _ 
    	"s assembly information."&Global.Microsoft.VisualBasic.ChrW(13)&Global.Microsoft.VisualBasic.ChrW(10)&"Customize the application's assembly information in the"& _ 
    	" Application pane of Project Designer.)"
    	'
    	'TextBoxDescription
    	'
    	Me.TextBoxDescription.Dock = System.Windows.Forms.DockStyle.Fill
    	Me.TextBoxDescription.Location = New System.Drawing.Point(6, 3)
    	Me.TextBoxDescription.Margin = New System.Windows.Forms.Padding(6, 3, 3, 3)
    	Me.TextBoxDescription.Multiline = true
    	Me.TextBoxDescription.Name = "TextBoxDescription"
    	Me.TextBoxDescription.ReadOnly = true
    	Me.TextBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both
    	Me.TextBoxDescription.Size = New System.Drawing.Size(186, 117)
    	Me.TextBoxDescription.TabIndex = 1
    	Me.TextBoxDescription.TabStop = false
    	Me.TextBoxDescription.Text = resources.GetString("TextBoxDescription.Text")
    	'
    	'AboutBx
    	'
    	Me.AutoScaleDimensions = New System.Drawing.SizeF(6!, 13!)
    	Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
    	Me.CancelButton = Me.OKButton
    	Me.ClientSize = New System.Drawing.Size(414, 276)
    	Me.Controls.Add(Me.TableLayoutPanel)
    	Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
    	Me.Icon = CType(resources.GetObject("$this.Icon"),System.Drawing.Icon)
    	Me.MaximizeBox = false
    	Me.MinimizeBox = false
    	Me.Name = "AboutBx"
    	Me.Padding = New System.Windows.Forms.Padding(9)
    	Me.ShowInTaskbar = false
    	Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
    	Me.Text = "AboutBx"
    	Me.TableLayoutPanel.ResumeLayout(false)
    	Me.TableLayoutPanel1.ResumeLayout(false)
    	Me.TableLayoutPanel1.PerformLayout
    	Me.ResumeLayout(false)
    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TextBoxDescription As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents OKButton As System.Windows.Forms.Button

End Class

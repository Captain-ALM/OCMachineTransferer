Imports System.Net.Sockets
Imports System.Net
Imports System.Threading
Imports System.IO
Imports OCUploadDownloadServer

'
' Created by SharpDevelop.
' User: Alfred
' Date: 15/07/2018
' Time: 10:01
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Public Partial Class MainForm
	
	Public sock As socket = Nothing
	Public client_sock As Socket = Nothing
	Public l_thread As Thread = Nothing
	Public s_thread As Thread = Nothing
	Public r_thread As Thread = Nothing
	Public c_thread As Thread = Nothing
	Public u_thread As Thread = Nothing
	Public r_q As New Queue(Of Byte())
	Public s_q As New Queue(Of Byte())
	Public u_q As New Queue(Of [Delegate])
	'Protected vupslockobj As New Object()
	Public exec As Boolean = False
	Public m As Mode = Mode.None
	Public stage As Integer = 0
	Public filepath As String = ""
	Public canc As Boolean = False
	Public safe As Boolean = True
	Public runf As Boolean = False
	Public lic As String = ""
	Public dis As String = ""
	Public dir As String = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly.Location)
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub MainForm_Load(sender As Object, e As EventArgs)
		butcancel.Enabled = False
		contrvis(False)
		try
			If File.Exists(dir & "\license.txt") Then
				lic = File.ReadAllText(dir & "\license.txt")
			End If
		Catch ex As IOException
			lic = ""
		End Try
		try
			If File.Exists(dir & "\description.txt") Then
				dis = File.ReadAllText(dir & "\description.txt")
			End If
		Catch ex As IOException
			dis = ""
		End Try
	End Sub
	
	Sub MainForm_Shown(sender As Object, e As EventArgs)
		Dim f As New Setup()
		f.ShowDialog(Me)
		If f.DialogResult = DialogResult.Cancel Then
			End
		Else
			exec = True
		End If
		sock = New Socket(f.selected_interface.AddressFamily,SocketType.Stream, ProtocolType.Tcp)
		sock.NoDelay = Not f.delay
		sock.ReceiveBufferSize = Int16.MaxValue
		sock.SendBufferSize = Int16.MaxValue
		sock.Bind(New IPEndPoint(f.selected_interface, f.port))
		l_thread = New Thread(New ThreadStart(AddressOf Listener))
		c_thread = New Thread(New ThreadStart(AddressOf Runner))
		u_thread = New Thread(New ThreadStart(AddressOf visupdater))
		l_thread.IsBackground = True
		c_thread.IsBackground = True
		u_thread.IsBackground = True
		dudsa.Items.Add(f.selected_interface.ToString)
		If f.selected_interface.ToString = IPAddress.Any.ToString Then
			For Each ip As IPAddress In f.getNetworkAdapterIPsAndNames().Values
				dudsa.Items.Add(ip.ToString)
			Next
		End If
		dudsa.SelectedIndex = 0
		txtbxp.Text = f.port.ToString
		If Not f.IsDisposed And Not f.Disposing Then
			f.Dispose()
			f = Nothing
		End If
		runf = True
		u_thread.Start()
		c_thread.Start()
		l_thread.Start()
		contrvis(True)
	End Sub
	
	Private Sub Listener
		sock.Listen(1)
		While exec
			Try
				client_sock = sock.Accept()
				client_sock.NoDelay = sock.NoDelay
				client_sock.ReceiveBufferSize = sock.ReceiveBufferSize
				client_sock.SendBufferSize = sock.SendBufferSize
				client_sock.ReceiveTimeout = 5000
				client_sock.SendTimeout = 5000
				If SocketConnected(client_sock) Then
					r_thread = New Thread(New ThreadStart(AddressOf Receiver))
					s_thread = New Thread(New Threadstart(AddressOf Sender))
					r_thread.IsBackground = True
					s_thread.IsBackground = True
				End If
				If SocketConnected(client_sock) Then
					r_thread.Start()
					s_thread.Start()
				Else
					r_thread = Nothing
					s_thread = Nothing 
				End If
				While SocketConnected(client_sock)
					Thread.Sleep(100)
				End While
				Try
					client_sock.Shutdown(SocketShutdown.Both)
				Catch ex As SocketException
				End Try
				client_sock.Close()
				client_sock = Nothing
				If Not r_thread Is Nothing Then
					While r_thread.IsAlive 
						Thread.Sleep(100)
					End While
				End If
				If Not s_thread Is Nothing Then
					While  s_thread.IsAlive
						Thread.Sleep(100)
					End While
				End If
			Catch ex As ThreadAbortException
				Try
					If Not client_sock Is Nothing Then
						If SocketConnected(client_sock) Then
							client_sock.Shutdown(SocketShutdown.Both)
						End If
						client_sock.Close()
						client_sock = Nothing
					End If
				Catch ex2 As SocketException
					client_sock = Nothing
				End Try
			Catch ex As SocketException
				client_sock = Nothing
			End Try
		End While
		sock.Shutdown(SocketShutdown.Both)
		sock.Close()
	End Sub
	
	Private Sub Receiver
		Try
			While Not client_sock Is Nothing
				Try
					If client_sock.Available > 0 Then
						Dim bound As Integer = client_sock.Available
						Dim recbyte(bound - 1) As Byte
						client_sock.Receive(recbyte, bound, SocketFlags.None)
						r_q.Enqueue(recbyte)
					End If
					'Dim recbyte(client_sock.ReceiveBufferSize - 1) As Byte
					'client_sock.Receive(recbyte)
					'r_q.Enqueue(removetrailer(recbyte))
				Catch ex As NullReferenceException
				Catch ex As SocketException
				End Try
				Thread.Sleep(100)
			End While
		Catch ex As SocketException
		End Try
	End Sub
	
	Private Function removetrailer(bts As Byte()) As Byte()
		Dim ret As New List(Of Byte)
		For i = 0 To bts.Length - 1 Step 1
			If bts(i) <> 0 Then
				ret.Add(bts(i))
			End If
		Next
		Return ret.ToArray
	End Function
	
	Private Sub Sender
		Try
			While Not client_sock Is Nothing
				Try
					While s_q.Count > 0
						client_sock.Send(s_q.Dequeue())
					End While
				Catch ex As NullReferenceException
				Catch ex As SocketException
				End Try
				Thread.Sleep(100)
			End While
		Catch ex As SocketException
		End Try
	End Sub
	
	Private Sub Runner
		setSafe()
		While exec And runf
			If m = Mode.Open Then
				contrvis(False)
				eq_vd(Sub() lblstat.Text = "Opening...")
				eq_vd(Sub() pbstat.Style = ProgressBarStyle.Marquee)
				If File.Exists(filepath) Then
					Dim filenom As String = Path.GetFileName(filepath)
					eq_vd(Sub() txtbxname.Text = filenom)
					eq_vd(Sub() txtbxname.DeselectAll())
					unsetSafe()
					setCancel()
					Dim data As String = File.ReadAllText(filepath)
					unsetCancel()
					setSafe()
					eq_vd(Sub() txtbx.Text = data)
					eq_vd(Sub() txtbx.DeselectAll())
				End If
				filepath = ""
				eq_vd(Sub() pbstat.Style = ProgressBarStyle.Continuous)
				eq_vd(Sub() lblstat.Text = "")
				contrvis(True)
				m = Mode.None
			ElseIf m = Mode.Save Then
				contrvis(False)
				eq_vd(Sub() lblstat.Text = "Saving...")
				eq_vd(Sub() pbstat.Style = ProgressBarStyle.Marquee)
				unsetSafe()
				setCancel()
				File.WriteAllText(filepath, txtbx.Text)
				unsetCancel()
				setSafe()
				filepath = ""
				eq_vd(Sub() pbstat.Style = ProgressBarStyle.Continuous)
				eq_vd(Sub() lblstat.Text = "")
				contrvis(True)
				m = Mode.None
			ElseIf m = Mode.Clear Then
				contrvis(False)
				eq_vd(Sub() lblstat.Text = "Clearing...")
				eq_vd(Sub() pbstat.Style = ProgressBarStyle.Marquee)
				eq_vd(Sub() txtbxname.Text = "")
				eq_vd(Sub() txtbxname.DeselectAll())
				eq_vd(Sub() txtbx.Text = "")
				eq_vd(Sub() txtbx.DeselectAll())
				eq_vd(Sub() pbstat.Style = ProgressBarStyle.Continuous)
				eq_vd(Sub() lblstat.Text = "")
				contrvis(True)
				m = Mode.None
			ElseIf m = Mode.Upload Then
				contrvis(False)
				eq_vd(Sub() lblstat.Text = "Receiving...")
				eq_vd(Sub() lblstat.Text = "Receiving : Sending Handshake...")
				setCancel()
				stage = 1
				If canc Then
					canc = False
					sendHandshake("0".ToCharArray()(0))
					GoTo fu
				Else 
					sendHandshake("1".ToCharArray()(0))
				End If
				eq_vd(Sub() pbstat.Value = CType(1*(100/3), integer))
				eq_vd(Sub() lblstat.Text = "Receiving : Waiting For Data...")
				stage = 2
				While r_q.Count = 0 And SocketConnected(client_sock) And Not canc
					Thread.Sleep(100)
				End While
				If canc Then
					canc = False
					Try
						client_sock.Shutdown(SocketShutdown.Both)
					Catch ex As SocketException
					End Try
					Try
						client_sock.Close()
					Catch ex As SocketException
					End Try
					client_sock = Nothing
					s_q.Clear()
					r_q.Clear()
					GoTo fu
				End If
				If Not SocketConnected(client_sock) Then
					s_q.Clear()
					r_q.Clear()
					GoTo fu
				End If
				unsetCancel()
				eq_vd(Sub() pbstat.Value = CType(2*(100/3), integer))
				eq_vd(Sub() lblstat.Text = "Receiving : Processing Data...")
				stage = 3
				Dim bts As Byte() = r_q.Dequeue()
				Dim str As String = System.Text.Encoding.ASCII.GetString(bts)
				If str.Contains(ControlChars.Cr) And Not str.Contains(ControlChars.Lf) Then
					str = str.Replace(ControlChars.Cr, ControlChars.CrLf)
				End If
				If str.Contains(ControlChars.Lf) And Not str.Contains(ControlChars.Cr) Then
					str = str.Replace(ControlChars.Lf, ControlChars.CrLf)
				End If
				eq_vd(Sub() txtbxname.Text = "")
				eq_vd(Sub() txtbxname.DeselectAll())
				eq_vd(Sub() txtbx.Text = str)
				eq_vd(Sub() txtbx.DeselectAll())
				eq_vd(Sub() pbstat.Value = CType(3*(100/3), integer))
				eq_vd(Sub() lblstat.Text = "Receiving : Sending Handshake...")
				setCancel()
				stage = 4
				If canc Then
					canc = False
					sendHandshake("0".ToCharArray()(0)) 
				Else 
					sendHandshake("1".ToCharArray()(0))
				End If
				fu:
				unsetCancel()
				stage = 0
				eq_vd(Sub() pbstat.Value = 0)
				eq_vd(Sub() lblstat.Text = "")
				contrvis(True)
				m = Mode.None
			ElseIf m = Mode.Download Then
				contrvis(False)
				eq_vd(Sub() lblstat.Text = "Sending...")
				eq_vd(Sub() lblstat.Text = "Sending : Sending Handshake...")
				setCancel()
				stage = 1
				If canc Then
					canc = False
					sendHandshake("0".ToCharArray()(0))
					GoTo fd
				Else 
					sendHandshake("1".ToCharArray()(0))
				End If
				eq_vd(Sub() pbstat.Value = ctype(1*(100/3), integer))
				eq_vd(Sub() lblstat.Text = "Sending : Waiting For Handshake...")
				stage = 2
				While r_q.Count = 0 And SocketConnected(client_sock) And Not canc
					Thread.Sleep(100)
				End While
				If canc Then
					canc = False
					Try
						client_sock.Shutdown(SocketShutdown.Both)
					Catch ex As SocketException
					End Try
					Try
						client_sock.Close()
					Catch ex As SocketException
					End Try
					client_sock = Nothing
					s_q.Clear()
					r_q.Clear()
					GoTo fd
				End If
				If Not SocketConnected(client_sock) Then
					s_q.Clear()
					r_q.Clear()
					GoTo fd
				End If
				If getHandshake("0".ToCharArray()(0)) Then
					GoTo fd
				End If
				If canc Then
					canc = False
					Try
						client_sock.Shutdown(SocketShutdown.Both)
					Catch ex As SocketException
					End Try
					Try
						client_sock.Close()
					Catch ex As SocketException
					End Try
					client_sock = Nothing
					s_q.Clear()
					r_q.Clear()
					GoTo fd
				End If
				eq_vd(Sub() pbstat.Value = ctype(2*(100/3), integer))
				eq_vd(Sub() lblstat.Text = "Sending : Sending Data...")
				stage = 3
				If Not canc Then
					Dim bts As Byte() = System.Text.Encoding.ASCII.GetBytes(txtbx.Text)
					s_q.Enqueue(bts)
				End If
				If canc Then
					canc = False
					Try
						client_sock.Shutdown(SocketShutdown.Both)
					Catch ex As SocketException
					End Try
					Try
						client_sock.Close()
					Catch ex As SocketException
					End Try
					client_sock = Nothing
					s_q.Clear()
					r_q.Clear()
					GoTo fd
				End If
				eq_vd(Sub() pbstat.Value = ctype(3*(100/3), integer))
				eq_vd(Sub() lblstat.Text = "Sending : Waiting For Handshake...")
				stage = 4
				While r_q.Count = 0 And SocketConnected(client_sock) And canc
					Thread.Sleep(100)
				End While
				If canc Then
					canc = False
					Try
						client_sock.Shutdown(SocketShutdown.Both)
					Catch ex As SocketException
					End Try
					Try
						client_sock.Close()
					Catch ex As SocketException
					End Try
					client_sock = Nothing
					s_q.Clear()
					r_q.Clear()
					GoTo fd
				End If
				If Not SocketConnected(client_sock) Then
					s_q.Clear()
					r_q.Clear()
					GoTo fd
				End If
				If getHandshake("0".ToCharArray()(0)) Then
					GoTo fd
				End If
				fd:
				unsetCancel()
				stage = 0
				eq_vd(Sub() pbstat.Value = 0)
				eq_vd(Sub() lblstat.Text = "")
				contrvis(True)
				m = Mode.None
			Else
				While r_q.Count > 0
					Dim cb As Byte() = r_q.Dequeue()
					If cb.Length > 0 Then
						If cb(0) = 0 Then
							If Not client_sock Is Nothing Then
								If Not SocketConnected(client_sock) Then
									Try
										client_sock.Shutdown(SocketShutdown.Both)
										client_sock.Close()
									Catch ex As SocketException
									End Try
									client_sock = Nothing
								End If
							End If
						ElseIf System.Text.Encoding.ASCII.GetString(cb) = "1" Then
							If m = Mode.None Then m = Mode.Download
						ElseIf System.Text.Encoding.ASCII.GetString(cb) = "2" Then
							If m = Mode.None Then m = Mode.Upload
						End If
					End If
				End While
			End If
			Thread.Sleep(100)
		End While
	End Sub
	
	Sub Butopen_Click(sender As Object, e As EventArgs)
		contrvis(False)
		OpenFileDialog1.FileName = txtbxname.Text
		Dim res As DialogResult = OpenFileDialog1.ShowDialog()
		If res = DialogResult.OK Then
			If m = Mode.None Then
				filepath = OpenFileDialog1.FileName
				m = Mode.Open
			End If
		End If
		contrvis(True)
	End Sub
	
	Sub Butsave_Click(sender As Object, e As EventArgs)
		contrvis(False)
		SaveFileDialog1.FileName = txtbxname.Text
		Dim res As DialogResult = SaveFileDialog1.ShowDialog()
		If res = DialogResult.OK Then
			If m = Mode.None Then
				filepath = SaveFileDialog1.FileName
				txtbxname.Text = Path.GetFileName(SaveFileDialog1.FileName)
				m = Mode.Save
			End If
		End If
		contrvis(True)
	End Sub
	
	Sub Butcls_Click(sender As Object, e As EventArgs)
		contrvis(False)
		If m = Mode.None Then
			m = Mode.Clear
		End If
		contrvis(True)
	End Sub
	
	Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs)
		contrvis(False, Vism.cancel or Vism.program_management or Vism.server_stats or Vism.state or Vism.textbox)
		exec = False
	End Sub
	
	Sub MainForm_FormClosed(sender As Object, e As FormClosedEventArgs)
		threadcls()
	End Sub
	
	Public Function SocketConnected(ByRef s As Socket) As Boolean
		Dim ret As Boolean = True
		Try
			ret = Not (s.Poll(0, SelectMode.SelectRead) And s.Available = 0)
		Catch ex As ObjectDisposedException
			ret = False
		Catch ex As NullReferenceException
			ret = False
		Catch ex As SocketException
			ret = False
		End Try
		Return ret
	End Function
	
	Sub Butss_Click(sender As Object, e As EventArgs)
		contrvis(False, Vism.cancel or Vism.program_management or Vism.server_stats Or Vism.state or Vism.textbox)
		Me.Close()		
	End Sub
	
	Sub Txtbx_KeyDown(sender As Object, e As KeyEventArgs)
		If (e.KeyCode = Keys.A And e.Control) Then
			e.SuppressKeyPress = True
			txtbx.SelectAll()
			e.Handled = True
		End If
	End Sub
	
	Sub Butrs_Click(sender As Object, e As EventArgs)
		contrvis(False, Vism.cancel or Vism.program_management or Vism.server_stats or Vism.state Or Vism.textbox)
		exec = False
		threadcls()
		stage = 0
		pbstat.Value = 0
		pbstat.Style = ProgressBarStyle.Continuous
		lblstat.Text = ""
		m = Mode.None
		sock  = Nothing
		client_sock = Nothing
		l_thread  = Nothing
		s_thread  = Nothing
		r_thread  = Nothing
		c_thread  = Nothing
		u_thread = Nothing
		r_q.Clear()
		s_q.Clear()
		dudsa.SelectedIndex = -1
		dudsa.Items.Clear()
		txtbx.Text =""
		txtbxname.Text = ""
		txtbxp.Text = ""
		filepath = ""
		safe = True
		canc = False
		Dim f As New Setup()
		f.ShowDialog()
		If f.DialogResult = DialogResult.Cancel Then
			End
		Else
			exec = True
		End If
		sock = New Socket(f.selected_interface.AddressFamily,SocketType.Stream, ProtocolType.Tcp)
		sock.NoDelay = Not f.delay
		sock.ReceiveBufferSize = Int16.MaxValue
		sock.SendBufferSize = Int16.MaxValue
		sock.Bind(New IPEndPoint(f.selected_interface, f.port))
		l_thread = New Thread(New ThreadStart(AddressOf Listener))
		c_thread = New Thread(New ThreadStart(AddressOf Runner))
		u_thread = New Thread(New ThreadStart(AddressOf visupdater))
		l_thread.IsBackground = True
		c_thread.IsBackground = True
		u_thread.IsBackground = True
		dudsa.Items.Add(f.selected_interface.ToString)
		If f.selected_interface.ToString = IPAddress.Any.ToString Then
			For Each ip As IPAddress In f.getNetworkAdapterIPsAndNames().Values
				dudsa.Items.Add(ip.ToString)
			Next
		End If
		dudsa.SelectedIndex = 0
		txtbxp.Text = f.port.ToString
		If Not f.IsDisposed And Not f.Disposing Then
			f.Dispose()
			f = Nothing
		End If
		runf = True
		u_thread.Start()
		c_thread.Start()
		l_thread.Start()
		contrvis(True)
	End Sub
	
	Sub threadcls()
		If l_thread.IsAlive Then
			Try
				If Not client_sock Is Nothing Then
					client_sock.Shutdown(SocketShutdown.Both)
					client_sock.Close()
				End If
			Catch ex As NullReferenceException
			Catch ex As SocketException
			End Try
			Try
				sock.Close()
			Catch ex As NullReferenceException
			Catch ex As SocketException
			End Try
			l_thread.Abort()
		End If
		Try
			If c_thread.IsAlive Then
				c_thread.Abort()
			End If
		Catch ex As NullReferenceException
		End Try
		Try
			If r_thread.IsAlive Then
				r_thread.Abort()
			End If
		Catch ex As NullReferenceException
		End Try
		try
			If s_thread.IsAlive Then
				s_thread.Abort()
			End If
		Catch ex As NullReferenceException
		End Try
		While l_thread.IsAlive Or c_thread.IsAlive Or u_thread.IsAlive
			Thread.Sleep(100)
		End While
	End Sub
	
	Sub setCancel()
		If Not butcancel.Enabled Then butcancel.Invoke(Sub() butcancel.Enabled = True)
	End Sub
	
	Sub unsetCancel()
		If butcancel.Enabled Then butcancel.Invoke(Sub() butcancel.Enabled = False)
		canc = False
	End Sub
	
	Sub setSafe()
		safe = True
	End Sub
	
	Sub unsetSafe()
		safe = False
	End Sub
	
	Sub Butcancel_Click(sender As Object, e As EventArgs)
		butcancel.Enabled = False
		If safe Then
			canc = True
		Else
			runf = False
			If c_thread.IsAlive Then
				c_thread.Join(2500)
			End If
			If c_thread.IsAlive Then
				c_thread.Abort()
			End If
			m = Mode.None
			stage = 0
			filepath = ""
			lblstat.Text = ""
			pbstat.Value = 0
			pbstat.Style = ProgressBarStyle.Continuous
			c_thread = New Thread(New ThreadStart(AddressOf Runner))
			c_thread.IsBackground = True
			runf = True
			c_thread.Start()
		End If
	End Sub
	
	Sub sendHandshake(ch As Char)
		Dim bts As Byte() = System.Text.Encoding.ASCII.GetBytes(ch)
		s_q.Enqueue(bts)
	End Sub
	
	Function getHandshake(ch As Char) As Boolean
		If r_q.Count = 0 Then Return False
		Return System.Text.Encoding.ASCII.GetString(r_q.Dequeue()) = ch
	End Function
	
	Private Sub contrvis(ByVal vis As Boolean, Optional ByVal vm As Vism = Vism.server_stats or Vism.program_management or Vism.textbox or Vism.state)
		If Me.InvokeRequired Then
			eq_vd(Sub() contrvis(vis,vm))
		Else
			If vm.HasFlag(Vism.server_stats) Then
				dudsa.Enabled = vis
				txtbxp.Enabled = vis
				butrs.Enabled = vis
				butss.Enabled = vis
			End If
			If vm.HasFlag(Vism.program_management) Then
				txtbxname.Enabled = vis
				butopen.Enabled = vis
				butsave.Enabled = vis
				butcls.Enabled = vis
				buta.Enabled = vis
			End If
			If vm.HasFlag(Vism.textbox) Then
				txtbx.Enabled = vis
			End If
			If vm.HasFlag(Vism.state) Then
				lblstat.Enabled = vis
				pbstat.Enabled = vis
			End If
			If vm.HasFlag(Vism.cancel) Then
				butcancel.Enabled = vis
			End If
		End If
	End Sub
	
	Sub Buta_Click(sender As Object, e As EventArgs)
		contrvis(False)
		Dim f As New AboutBx()
		f.setupdata(dis,lic)
		f.ShowDialog(Me)
		If Not f.IsDisposed And Not f.Disposing Then
			f.Dispose()
			f = Nothing
		End If
		contrvis(True)
	End Sub
	
	Private Sub visupdater()
		While exec
			If c_vd() > 0 Then
				Me.Invoke(dq_vd())
			End If
			Thread.Sleep(100)
		End While
	End Sub
	
	Sub eq_vd(del As [Delegate])
		'SyncLock vupslockobj
			u_q.Enqueue(del)
		'End SyncLock
	End Sub
	
	Function dq_vd() As [Delegate]
		Dim toret As [Delegate] = Nothing
		'SyncLock vupslockobj
			toret = u_q.Dequeue()
		'End SyncLock
		Return toret
	End Function
	
	Function c_vd() As Integer
		Dim toret As Integer = 0
		'SyncLock vupslockobj
			toret = u_q.Count
		'End SyncLock
		Return toret
	End Function
End Class

Public Enum Mode As Integer
	None = 0
	Open = 1
	Save = 2
	Upload = 3
	Download = 4
	Clear = 5
End Enum

<Flags()>
Public Enum Vism As Integer
	none = 0	
	server_stats = 1
	program_management = 2
	textbox = 4
	state = 8
	cancel = 16
end Enum	
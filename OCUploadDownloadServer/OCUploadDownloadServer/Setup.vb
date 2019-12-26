'
' Created by SharpDevelop.
' User: Alfred
' Date: 17/07/2018
' Time: 18:19
' 
' To change this template use Tools | Options | Coding | Edit Standard Headers.
'
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Net.Sockets

Public Partial Class Setup
	Public selected_interface As IPAddress = IPAddress.Any
	Public port As Integer = 100
	Public delay As Boolean = False
	Public interfaces As Dictionary(Of String, IPAddress)
	Public ver As Boolean = False
	
	Public Sub New()
		' The Me.InitializeComponent call is required for Windows Forms designer support.
		Me.InitializeComponent()
		
		'
		' TODO : Add constructor code after InitializeComponents
		'
	End Sub
	
	Sub Setup_Load(sender As Object, e As EventArgs)
		butcancel.Enabled = False
        butok.Enabled = False
        cbsli.Enabled = False
        chkbxdid.Enabled = False
        nudp.Enabled = False
		interfaces = getNetworkAdapterIPsAndNames()
        interfaces.Add("Listen on All Interfaces : 0.0.0.0", IPAddress.Any)
        nudp.Value = 100
        For Each current As String In interfaces.Keys
            cbsli.Items.Add(current)
        Next
        cbsli.SelectedItem = "Listen on All Interfaces : 0.0.0.0"
        butcancel.Enabled = True
        butok.Enabled = True
        cbsli.Enabled = True
        chkbxdid.Enabled = True
        nudp.Enabled = True
	End Sub
	
	Public Function getNetworkAdapterIPsAndNames() As Dictionary(Of String, IPAddress)
        Dim list As New Dictionary(Of String, IPAddress)
        Dim allNetworkInterfaces As NetworkInterface() = NetworkInterface.GetAllNetworkInterfaces()
        For i As Integer = 0 To allNetworkInterfaces.Length - 1
            Dim networkInterface As NetworkInterface = allNetworkInterfaces(i)
            'If networkInterface.OperationalStatus = OperationalStatus.Up AndAlso networkInterface.NetworkInterfaceType <> NetworkInterfaceType.Loopback Then
            'the above is if we want no loopback
            If networkInterface.OperationalStatus = OperationalStatus.Up Then
                For Each current As UnicastIPAddressInformation In networkInterface.GetIPProperties().UnicastAddresses
                    If current.Address.AddressFamily = AddressFamily.InterNetwork Then
                        list.Add(networkInterface.Name & " : " & current.Address.ToString, current.Address)
                    End If
                Next
            End If
        Next
        Return list
    End Function
	
	Sub Butok_Click(sender As Object, e As EventArgs)
		butcancel.Enabled = False
        butok.Enabled = False
        cbsli.Enabled = False
        chkbxdid.Enabled = False
        nudp.Enabled = False
        selected_interface = interfaces(cbsli.SelectedItem.ToString)
        port = CType(nudp.Value, Integer)
        delay = chkbxdid.Checked
		ver = True
		Me.Close()
		Me.DialogResult = DialogResult.OK
	End Sub
	
	Sub Setup_FormClosed(sender As Object, e As FormClosedEventArgs)
		If Not Me.DialogResult = DialogResult.OK Then
			selected_interface = IPAddress.Any
            port = 100
            delay = False
			ver = False
			Me.DialogResult = DialogResult.Cancel
		End If
	End Sub
	
	Sub Butcancel_Click(sender As Object, e As EventArgs)
		butcancel.Enabled = False
        butok.Enabled = False
        cbsli.Enabled = False
        chkbxdid.Enabled = False
        nudp.Enabled = False
		ver = False
		Me.Close()
		Me.DialogResult = DialogResult.Cancel
	End Sub
	
	Sub Setup_FormClosing(sender As Object, e As FormClosingEventArgs)
		butcancel.Enabled = False
        butok.Enabled = False
        cbsli.Enabled = False
        chkbxdid.Enabled = False
        nudp.Enabled = False
	End Sub
End Class

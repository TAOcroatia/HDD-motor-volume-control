Imports System
Imports System.Windows.Forms
Imports System.Runtime.InteropServices

Public Class Form1
    Inherits Form
    Private Const APPCOMMAND_VOLUME_UP As Integer = &HA0000
    Private Const APPCOMMAND_VOLUME_DOWN As Integer = &H90000
    Private Const WM_APPCOMMAND As Integer = &H319
    Dim mehand As Integer

    <DllImport("user32.dll")>
    Public Shared Function SendMessageW(ByVal hWnd As IntPtr,
               ByVal Msg As Integer, ByVal wParam As IntPtr,
               ByVal lParam As IntPtr) As IntPtr
    End Function
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Hides window
        Me.Opacity = 0
        Me.WindowState = FormWindowState.Minimized
        Me.Hide()
        Try
            'Find last COM port and sets it
            SerialPort1.PortName = My.Computer.Ports.SerialPortNames(My.Computer.Ports.SerialPortNames.Count - 1)
            SerialPort1.Open()
        Catch ex As System.UnauthorizedAccessException
            MsgBox("Port is being used")
        End Try
        mehand = Me.Handle
    End Sub

    Private Sub SerialPort1_DataReceived(sender As Object, e As IO.Ports.SerialDataReceivedEventArgs) Handles SerialPort1.DataReceived
        Dim speed As Integer
        Dim tmp As String = SerialPort1.ReadLine
        'Determines how fast volume should change based on the voltage(= speed) it recives.
        speed = (tmp.Substring(1, tmp.Count - 1))
        If speed < 6 Then
            speed = 1
        ElseIf speed < 11 And speed > 5 Then
            speed = 2
        ElseIf speed > 10 And speed < 20 Then
            speed = 3
        ElseIf speed > 20 And speed < 50 Then
            speed = 5
        End If
        'Determines whether disc spinned foward or backwars and changes volume multiple times, which depends on speed.
        If tmp(0) = "1" Then
            For i As Integer = 1 To speed
                SendMessageW(mehand, WM_APPCOMMAND, mehand, New IntPtr(APPCOMMAND_VOLUME_UP))
            Next
        ElseIf tmp(0) = "0" Then
            For i As Integer = 1 To speed
                SendMessageW(mehand, WM_APPCOMMAND, mehand, New IntPtr(APPCOMMAND_VOLUME_DOWN))
            Next
        End If
    End Sub
End Class

using System.Net;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;
using WindowsInput;
using System.Threading;

internal class Program
{
    [DllImport("user32.dll")]
    static extern int SetForegroundWindow(IntPtr point);

    [DllImport("user32.dll")]
    static extern IntPtr SetFocus(HandleRef hWnd);

    [DllImport("user32.dll")]
    internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private static bool isRunning;

    static void Main(string[] args)
    {
        
        if(args.Length < 1)
        {
            throw new ArgumentException($"ERROR: You must provide a server address. Example <monday.eu.moose.gg:28010>");
        }

        Process[] processes = Process.GetProcessesByName("RustClient");


        Process? rustApp = processes.FirstOrDefault();
        

        if (rustApp != null)
        {
            IntPtr hWnd = rustApp.MainWindowHandle;
            SetForegroundWindow(hWnd);
            SetFocus(new HandleRef(null, rustApp.Handle));

            isRunning = true;
            ConnectToServer(args[0], hWnd, rustApp);
        }
    }


    /// <summary>
    /// Okay so in this block we are just ensuring that while we loop rust window is focused 
    /// then we are going to open console and clear any text that may be present
    /// </summary>
    /// <param name="serverAddress"></param>
    /// <param name="hWnd"></param>
    /// <param name="app"></param>
    static void ConnectToServer(string serverAddress, IntPtr hWnd, Process app)
    {
        while(isRunning)
        {

            SetForegroundWindow(hWnd);
            SetFocus(new HandleRef(null, app.Handle));
            ShowWindow(hWnd, 5);


            if (OperatingSystem.IsWindows())
            {
                InputSimulator inputs = new InputSimulator();
                inputs.Keyboard.KeyPress(VirtualKeyCode.F1);
                Thread.Sleep(300);

                inputs.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_A);
                Thread.Sleep(300);
                inputs.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.BACK);
                Thread.Sleep(300);

                inputs.Keyboard.TextEntry("connect " + serverAddress);
                Thread.Sleep(300);

                inputs.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                Thread.Sleep(300);

                inputs.Keyboard.KeyPress(VirtualKeyCode.F1);
                Thread.Sleep(300);
            }

            Thread.Sleep(6000);
        }
        
    }
}
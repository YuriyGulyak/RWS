#if UNITY_EDITOR_WIN

using System;
using UnityEditor;
using System.Runtime.InteropServices;
using UnityEngine;

[InitializeOnLoad]
public class MaximizeWindow : Editor
{
    #region Windows native

    const int MONITOR_DEFAULTTONEAREST = 0x00000002;
    const int GWL_STYLE = -16;
    const int SWP_SHOWWINDOW = 0x40;
    const int defaultStyle = 0x17CF0000;

    [Serializable, StructLayout( LayoutKind.Sequential )]
    struct Rectangle
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    [StructLayout( LayoutKind.Sequential, CharSet = CharSet.Auto )]
    sealed class MonitorInfo
    {
        public int size = Marshal.SizeOf( typeof( MonitorInfo ) );
        public Rectangle monitor;
        public Rectangle work;
        public int flags;
    }

    [DllImport( "user32.dll" )]
    static extern int GetActiveWindow();

    [DllImport( "user32.dll" )]
    static extern int SetWindowLong( int hWnd, int nIndex, int dwNewLong );

    [DllImport( "user32.dll" )]
    static extern bool SetWindowPos( int hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags );

    [DllImport( "user32.dll" )]
    static extern bool ShowWindow( int hWnd, int nCmdShow );

    [DllImport( "user32.dll" )]
    static extern int MonitorFromWindow( int hwnd, int dwFlags );

    [DllImport( "user32.dll" )]
    static extern bool GetMonitorInfo( int hMonitor, MonitorInfo lpmi );

    #endregion


    [MenuItem( "Window/Maximize Window" )]
    static void Maximize()
    {
        var windowHandle = GetActiveWindow();
        var resolution = GetResolution();

        ShowWindow( windowHandle, 1 );
        SetWindowLong( windowHandle, GWL_STYLE, 0 );
        SetWindowPos( windowHandle, 0, 0, 0, resolution.width, resolution.height, SWP_SHOWWINDOW );
    }

    [MenuItem( "Window/Restore Window" )]
    static void Restore()
    {
        var windowHandle = GetActiveWindow();
        var resolution = GetResolution();

        ShowWindow( windowHandle, 3 );
        SetWindowLong( windowHandle, GWL_STYLE, defaultStyle );
        SetWindowPos( windowHandle, 0, 0, 0, resolution.width, resolution.height, SWP_SHOWWINDOW );
    }

    static Resolution GetResolution()
    {
        var handle = MonitorFromWindow( GetActiveWindow(), MONITOR_DEFAULTTONEAREST );
        var info = new MonitorInfo();
        GetMonitorInfo( handle, info );

        return new Resolution
        {
            width = info.monitor.right - info.monitor.left,
            height = info.monitor.bottom - info.monitor.top
        };
    }
}

#endif
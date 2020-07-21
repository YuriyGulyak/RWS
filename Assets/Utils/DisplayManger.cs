#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DisplayManger : MonoBehaviour
{
    #region Windows native

    [StructLayout( LayoutKind.Sequential )]
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

    delegate bool MonitorEnumProc( int hDesktop, int hdc, ref Rectangle pRect, int dwData );

    //----------------------------------------------------------------------------------------------------

    [DllImport( "user32.dll" )]
    static extern int GetForegroundWindow();

    [DllImport( "user32.dll", EntryPoint = "MoveWindow" )]
    static extern int MoveWindow( int hWnd, int x, int y, int nWidth, int nHeight, int bRepaint );

    [DllImport( "user32.dll" )]
    static extern bool ShowWindow( int hWnd, int nCmdShow );

    [DllImport( "user32" )]
    static extern bool EnumDisplayMonitors( int hdc, int lpRect, MonitorEnumProc callback, int dwData );

    [DllImport( "user32.dll" )]
    static extern bool GetMonitorInfo( int hMonitor, MonitorInfo lpmi );

    #endregion


    public struct DisplayInfo
    {
        public Vector2Int position;
        public Vector2Int size;
    }

    public DisplayInfo[] GetDisplays()
    {
        var displays = new List<DisplayInfo>();

        MonitorEnumProc callback = ( int hDesktop, int hdc, ref Rectangle prect, int d ) =>
        {
            var monitorInfo = new MonitorInfo();

            if( GetMonitorInfo( hDesktop, monitorInfo ) )
            {
                var displayInfo = new DisplayInfo
                {
                    position = new Vector2Int
                    {
                        x = prect.left,
                        y = prect.top
                    },
                    size = new Vector2Int
                    {
                        x = monitorInfo.monitor.right - monitorInfo.monitor.left,
                        y = monitorInfo.monitor.bottom - monitorInfo.monitor.top
                    }
                    
                };
                displays.Add( displayInfo );
            }
            return true;
        };
        EnumDisplayMonitors( 0, 0, callback, 0 );

        return displays.ToArray();
    }

    public void SetTargetDisplay( int displayIndex )
    {
        if( setTargetDisplayCoroutine != null )
        {
            StopCoroutine( setTargetDisplayCoroutine );
        }
        setTargetDisplayCoroutine = SetTargetDisplayCoroutine( displayIndex );
        StartCoroutine( setTargetDisplayCoroutine );
    }

    //----------------------------------------------------------------------------------------------------

    IEnumerator setTargetDisplayCoroutine;

    IEnumerator SetTargetDisplayCoroutine( int displayIndex )
    {
        var windowHandle = GetForegroundWindow();
        var displays = GetDisplays();
        var displayInfo = displays[ Mathf.Min( displayIndex, displays.GetUpperBound( 0 ) ) ];

        Screen.fullScreen = false;
        yield return null;

        ShowWindow( windowHandle, 5 );
        MoveWindow( windowHandle, displayInfo.position.x, displayInfo.position.y, displayInfo.size.x, displayInfo.size.y, 1 );
        Screen.SetResolution( displayInfo.size.x, displayInfo.size.y, true );

        setTargetDisplayCoroutine = null;
    }
}

#endif
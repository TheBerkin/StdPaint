using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace StdPaint
{
    /// <summary>
    /// Allows drawing to the console buffer.
    /// </summary>
    public static class Painter
    {
        static Thread drawThread, renderThread;
        static IntPtr conHandle = Native.GetStdHandle(-11);
        static bool enabled;

        static IntPtr _hookID = IntPtr.Zero;
        static IntPtr consoleHandle = Native.GetConsoleWindow();
        static WndProcCallback _proc = HookCallback;
        static Rectangle clientRect;

        static ConsoleBuffer backBuffer, frontBuffer, activeBuffer = null;

        static int refreshInterval = 1;

        static ConsoleEventCallback closeEvent = ConsoleCloseEvent;

        /// <summary>
        /// Raised when the back buffer is about to start redrawing.
        /// </summary>
        public static event EventHandler Paint;

        /// <summary>
        /// Raised after Run() is called and before any drawing begins.
        /// </summary>
        public static event EventHandler Starting;

        /// <summary>
        /// Raised when the mouse is moved within the console.
        /// </summary>
        public static event EventHandler<PainterMouseEventArgs> MouseMove;

        /// <summary>
        /// Raised when the left mouse button is pressed down.
        /// </summary>
        public static event EventHandler<PainterMouseEventArgs> LeftButtonDown;

        /// <summary>
        /// Raised when the left mouse button is released.
        /// </summary>
        public static event EventHandler<PainterMouseEventArgs> LeftButtonUp;

        /// <summary>
        /// Raised when the right mouse button is pressed down.
        /// </summary>
        public static event EventHandler<PainterMouseEventArgs> RightButtonDown;

        /// <summary>
        /// Raised when the right mouse button is released.
        /// </summary>
        public static event EventHandler<PainterMouseEventArgs> RightButtonUp;

        /// <summary>
        /// Raised when the scroll wheel is moved.
        /// </summary>
        public static event EventHandler<PainterMouseEventArgs> Scroll;
        

        /// <summary>
        /// Gets the active buffer.
        /// </summary>
        public static ConsoleBuffer ActiveBuffer
        {
            get { return activeBuffer; }
        }

        /// <summary>
        /// Gets the back buffer. This buffer contains the final output image.
        /// </summary>
        public static ConsoleBuffer BackBuffer
        {
            get { return backBuffer; }
        }

        /// <summary>
        /// Gets the width of the active buffer.
        /// </summary>
        public static int ActiveBufferWidth
        {
            get { return activeBuffer.Width; }
        }

        /// <summary>
        /// Gets the height of the active buffer.
        /// </summary>
        public static int ActiveBufferHeight
        {
            get { return activeBuffer.Height; }
        }

        /// <summary>
        /// Gets the character and attribute data for the active buffer.
        /// </summary>
        public static BufferUnitInfo[,] ActiveBufferData
        {
            get { return activeBuffer.Buffer; }
        }

        /// <summary>
        /// Starts the Painter with the specified size and refresh rate.
        /// </summary>
        /// <param name="width">The width of the console, in units.</param>
        /// <param name="height">The height of the console, in units.</param>
        /// <param name="bufferRefreshRate">The refresh rate for the back buffer.</param>
        public static void Run(int width, int height, int bufferRefreshRate)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            backBuffer = activeBuffer = ConsoleBuffer.CreateScreenBuffer();
            frontBuffer = ConsoleBuffer.CreateScreenBuffer();

            refreshInterval = bufferRefreshRate;

            if (Starting != null)
            {
                Starting(null, null);
            }

            enabled = true;

            drawThread = null;
            drawThread = new Thread(GraphicsDrawThread);
            drawThread.Start();

            renderThread = null;
            renderThread = new Thread(GraphicsRenderThread);
            renderThread.Start();

            AddHook();

            Native.SetConsoleCtrlHandler(closeEvent, true);

            Application.Run();
        }

        static bool ConsoleCloseEvent(uint code)
        {
            Native.UnhookWindowsHookEx(_hookID);
            Stop();
            return false;
        }

        static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Native.GetClientRect(consoleHandle, out clientRect);

            if (nCode >= 0)
            {
                var minfo = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                var p = minfo.Point;
                Native.ScreenToClient(Native.GetConsoleWindow(), ref p);
                p.X = (int)((float)p.X * ((float)Console.BufferWidth / clientRect.Right));
                p.Y = (int)((float)p.Y * ((float)Console.BufferHeight / clientRect.Bottom));
                if (InBounds(p.X, p.Y))
                {
                    switch ((MouseMessages)wParam)
                    {
                        case MouseMessages.WM_MOUSEMOVE:
                            {
                                if (MouseMove != null)
                                {
                                    MouseMove(null, new PainterMouseEventArgs(p));
                                }
                            }
                            break;
                        case MouseMessages.WM_LBUTTONDOWN:
                            {
                                if (LeftButtonDown != null)
                                {
                                    LeftButtonDown(null, new PainterMouseEventArgs(p));
                                }
                            }
                            break;
                        case MouseMessages.WM_LBUTTONUP:
                            {
                                if (LeftButtonUp != null)
                                {
                                    LeftButtonUp(null, new PainterMouseEventArgs(p));
                                }
                            }
                            break;
                        case MouseMessages.WM_RBUTTONDOWN:
                            {
                                if (RightButtonDown != null)
                                {
                                    RightButtonDown(null, new PainterMouseEventArgs(p));
                                }
                            }
                            break;
                        case MouseMessages.WM_RBUTTONUP:
                            {
                                if (RightButtonUp != null)
                                {
                                    RightButtonUp(null, new PainterMouseEventArgs(p));
                                }
                            }
                            break;
                        case MouseMessages.WM_MOUSEWHEEL:
                            {
                                if (Scroll != null)
                                {
                                    Scroll(null, new PainterMouseEventArgs(p));
                                }
                            }
                            break;
                    }
                }
            }
            return Native.CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        const int WH_MOUSE_LL = 14;

        static IntPtr AddHook()
        {
            using (var process = Process.GetCurrentProcess())
            using (var module = process.MainModule)
            {
                return Native.SetWindowsHookEx(WH_MOUSE_LL, _proc, Native.GetModuleHandle(module.ModuleName), 0);
            }
        }

        /// <summary>
        /// Gets a boolean value indicating if the Painter is active.
        /// </summary>
        public static bool Enabled
        {
            get { return enabled; }
        }

        /// <summary>
        /// Clears the active buffer to the specified attributes.
        /// </summary>
        /// <param name="clearColor">The attributes to fill the buffer with.</param>
        public static void Clear(BufferColor clearColor = BufferColor.Black)
        {
            ActiveBuffer.Clear(clearColor);
        }
        
        private static void GraphicsDrawThread()
        {
            var bb = backBuffer.Buffer;
            var fb = frontBuffer.Buffer;
            int length = backBuffer.UnitCount * BufferUnitInfo.SizeBytes;
            unsafe
            {
                fixed (BufferUnitInfo* bbPtr = bb)
                fixed (BufferUnitInfo* fbPtr = fb)
                {
                    while (enabled)
                    {
                        if (Paint != null)
                        {
                            Paint(null, null);
                        }

                        Native.CopyMemory(fbPtr, bbPtr, length);
                        Thread.Sleep(refreshInterval);
                    }
                }
            }
        }

        private static void GraphicsRenderThread()
        {     
            int w = Console.BufferWidth;
            int h = Console.BufferHeight;
            COORD cFrom = new COORD(0, 0);
            COORD cTo = new COORD((short)w, (short)h);
            SMALL_RECT rect = new SMALL_RECT(0, 0, (short)w, (short)h);
            var bb = backBuffer.Buffer;
            var fb = frontBuffer.Buffer;
            unsafe
            {
                fixed (BufferUnitInfo* bbPtr = bb)
                fixed (BufferUnitInfo* fbPtr = fb)
                {
                    while (enabled)
                    {                        
                        Native.WriteConsoleOutput(conHandle, fb, cTo, cFrom, ref rect);
                        Thread.Sleep(refreshInterval);
                    }
                }
            }
        }

        private static bool InBounds(int x, int y)
        {
            return x >= 0 && x < ActiveBuffer.Width && y >= 0 && y < ActiveBuffer.Height;
        }

        /// <summary>
        /// Stops the Painter.
        /// </summary>
        public static void Stop()
        {
            enabled = false;
            Application.Exit();
        }
    }
}

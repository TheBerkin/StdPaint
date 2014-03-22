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
    public static class Painter
    {
        static Thread drawThread, displayThread;
        static IntPtr conHandle = Native.GetStdHandle(-11);
        static bool enabled;

        static IntPtr _hookID = IntPtr.Zero;
        static IntPtr consoleHandle = Native.GetConsoleWindow();
        static WndProcCallback _proc = HookCallback;
        static RECT clientRect;

        static ConsoleBuffer backBuffer, activeBuffer, frontBuffer = null;

        public static event EventHandler Paint;
        public static event EventHandler Starting;
        public static event EventHandler<PainterMouseEventArgs> MouseMove, LeftButtonDown, LeftButtonUp, RightButtonDown, RightButtonUp, Scroll;

        private static ConsoleEventCallback closeEvent = ConsoleCloseEvent;

        public static ConsoleBuffer ActiveBuffer
        {
            get { return activeBuffer; }
        }

        public static ConsoleBuffer BackBuffer
        {
            get { return backBuffer; }
        }

        public static int ActiveBufferWidth
        {
            get { return activeBuffer.Width; }
        }

        public static int ActiveBufferHeight
        {
            get { return activeBuffer.Height; }
        }

        public static BufferUnitInfo[,] ActiveBufferData
        {
            get { return activeBuffer.Buffer; }
        }

        public static void Run(int width, int height)
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(width, height);
            Console.SetBufferSize(width, height);

            backBuffer = activeBuffer = ConsoleBuffer.CreateScreenBuffer();
            frontBuffer = ConsoleBuffer.CreateScreenBuffer();

            if (Starting != null)
            {
                Starting(null, null);
            }

            drawThread = null;
            drawThread = new Thread(GraphicsDrawThread);
            drawThread.Start();

            displayThread = null;
            displayThread = new Thread(GraphicsDisplayThread);
            displayThread.Start();

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

        public static bool Enabled
        {
            get { return enabled; }
        }

        public static void Clear(BufferUnitAttributes clearAttributes = BufferUnitAttributes.None)
        {
            ActiveBuffer.Clear(clearAttributes);
        }

        public static void SetUnitAttributes(int x, int y, BufferUnitAttributes attributes)
        {
            ActiveBuffer.Buffer[y, x].Attributes = attributes;
        }

        public static BufferUnitAttributes GetUnitAttributes(int x, int y)
        {
            return ActiveBuffer.Buffer[y, x].Attributes;
        }
        
        private static void GraphicsDisplayThread()
        {
            int w = Console.BufferWidth;
            int h = Console.BufferHeight;
            COORD cFrom = new COORD(0, 0);
            COORD cTo = new COORD((short)w, (short)h);
            SMALL_RECT rect = new SMALL_RECT(0, 0, (short)w, (short)h);

            while(enabled)
            {
                Native.WriteConsoleOutput(conHandle, frontBuffer.Buffer, cTo, cFrom, ref rect);
                Thread.Sleep(1);
            }
        }

        private static void GraphicsDrawThread()
        {
            enabled = true;

            while(enabled)
            {     
                if (Paint != null)
                {
                    Paint(null, null);
                }

                Array.Copy(backBuffer.Buffer, frontBuffer.Buffer, backBuffer.Buffer.Length);
                Thread.Sleep(50);
            }
        }

        private static bool InBounds(int x, int y)
        {
            return x >= 0 && x < ActiveBuffer.Width && y >= 0 && y < ActiveBuffer.Height;
        }

        public static void Stop()
        {
            enabled = false;
            Application.Exit();
        }
    }
}

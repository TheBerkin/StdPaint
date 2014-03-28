using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using StdPaint.GUI;

namespace StdPaint
{
    /// <summary>
    /// Allows drawing to the console buffer.
    /// </summary>
    public static class Painter
    {
        #region Private fields
        static bool enabled;

        static Thread drawThread, renderThread;
        
        static Rectangle clientRect;
        static Stopwatch fpsCounter = new Stopwatch();

        static ConsoleBuffer backBuffer, frontBuffer, activeBuffer = null;

        static int refreshInterval = 1;
        #endregion

        #region Hook/Interop stuff
        static ConsoleEventCallback closeEvent = ConsoleCloseEvent;

        const int WH_MOUSE_LL = 14;
        const int WH_KEYBOARD_LL = 13;

        static IntPtr mouseHook, keyboardHook;
        static WndProcCallback mouseProc = MouseHookCallback;
        static WndProcCallback keyboardProc = KeyboardHookCallback;
        static bool[] keyboardState = new bool[256];

        static IntPtr stdOutHandle = Native.GetStdHandle(-11);
        static IntPtr consoleHandle = Native.GetConsoleWindow();
        #endregion

        #region Event handlers
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
        /// Raised when a key is pressed.
        /// </summary>
        public static event EventHandler<PainterKeyEventArgs> KeyDown;

        /// <summary>
        /// Raised when a key is released.
        /// </summary>
        public static event EventHandler<PainterKeyEventArgs> KeyUp;

        #endregion

        #region Public fields/properties

        /// <summary>
        /// Contains the GUI elements currently being displayed by the engine.
        /// </summary>
        public static List<Element> Elements = new List<Element>();

        /// <summary>
        /// Gets a boolean value indicating if the Painter is active.
        /// </summary>
        public static bool Enabled
        {
            get { return enabled; }
        }

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
        /// Gets the current frame rate.
        /// </summary>
        public static long CurrentFrameRate
        {
            get
            {
                long t = fpsCounter.ElapsedTicks;
                return t > 0 ? Stopwatch.Frequency / t : Stopwatch.Frequency;
            }
        }

        #endregion

        #region Hook methods

        static bool ConsoleCloseEvent(uint code)
        {
            Native.UnhookWindowsHookEx(mouseHook);
            Native.UnhookWindowsHookEx(keyboardHook);
            Stop();
            return false;
        }

        static IntPtr MouseHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            Native.GetClientRect(consoleHandle, out clientRect);

            if (nCode >= 0 && Native.GetForegroundWindow() == consoleHandle)
            {
                var minfo = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));                
                Native.ScreenToClient(Native.GetConsoleWindow(), ref minfo.Point);
                var p = minfo.Point;
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
                                    MouseMove(null, new PainterMouseEventArgs(p, minfo.Point));
                                }
                            }
                            break;
                        case MouseMessages.WM_LBUTTONDOWN:
                            {
                                if (LeftButtonDown != null)
                                {
                                    LeftButtonDown(null, new PainterMouseEventArgs(p, minfo.Point));
                                }
                            }
                            break;
                        case MouseMessages.WM_LBUTTONUP:
                            {
                                if (LeftButtonUp != null)
                                {
                                    LeftButtonUp(null, new PainterMouseEventArgs(p, minfo.Point));
                                }
                            }
                            break;
                        case MouseMessages.WM_RBUTTONDOWN:
                            {
                                if (RightButtonDown != null)
                                {
                                    RightButtonDown(null, new PainterMouseEventArgs(p, minfo.Point));
                                }
                            }
                            break;
                        case MouseMessages.WM_RBUTTONUP:
                            {
                                if (RightButtonUp != null)
                                {
                                    RightButtonUp(null, new PainterMouseEventArgs(p, minfo.Point));
                                }
                            }
                            break;
                        case MouseMessages.WM_MOUSEWHEEL:
                            {
                                if (Scroll != null)
                                {
                                    Scroll(null, new PainterMouseEventArgs(p, minfo.Point));
                                }
                            }
                            break;
                    }
                }
            }
            return Native.CallNextHookEx(mouseHook, nCode, wParam, lParam);
        }

        static IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                var info = (KBLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBLLHOOKSTRUCT));
                var flags = info.GetFlags();
                if (flags.Released)
                {
                    keyboardState[info.KeyCode] = false;
                    if (KeyUp != null)
                    {                        
                        KeyUp(null, new PainterKeyEventArgs((Keys)info.KeyCode));
                    }
                }
                else if (Native.GetForegroundWindow() == consoleHandle) // Make sure that keystrokes aren't acknowledged when the console is out of focus.
                {
                    if (!keyboardState[info.KeyCode])
                    {
                        keyboardState[info.KeyCode] = true;
                        if (KeyDown != null)
                        {                        
                            KeyDown(null, new PainterKeyEventArgs((Keys)info.KeyCode));
                        }
                    }
                }
            }
            return Native.CallNextHookEx(keyboardHook, nCode, wParam, lParam);
        }

        static void AddHooks()
        {
            using (var process = Process.GetCurrentProcess())
            using (var module = process.MainModule)
            {
                IntPtr mh = Native.GetModuleHandle(module.ModuleName);
                mouseHook = Native.SetWindowsHookEx(WH_MOUSE_LL, mouseProc, mh, 0);
                keyboardHook = Native.SetWindowsHookEx(WH_KEYBOARD_LL, keyboardProc, mh, 0);
            }
        }

        #endregion

        #region Threads

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

                        foreach (var element in Elements)
                        {
                            element.Draw();
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
            while (enabled)
            {
                fpsCounter.Restart();
                Native.WriteConsoleOutput(stdOutHandle, fb, cTo, cFrom, ref rect);
                Thread.Sleep(refreshInterval);
                fpsCounter.Stop();
            }
        }

        #endregion

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

            refreshInterval = bufferRefreshRate;


            // Make the buffers

            backBuffer = activeBuffer = ConsoleBuffer.CreateScreenBuffer();
            frontBuffer = ConsoleBuffer.CreateScreenBuffer();


            // Trigger any Starting events and set the Enabled flag to true.

            if (Starting != null)
            {
                Starting(null, null);
            }

            enabled = true;


            // Center the console window.

            Rectangle windowRect = new Rectangle();
            Native.GetWindowRect(consoleHandle, out windowRect);
            int halfWidth = windowRect.Right / 2;
            int halfHeight = windowRect.Bottom / 2;
            var screenBounds = Screen.PrimaryScreen.Bounds;     
            Native.SetWindowPos(consoleHandle, HWND.Top, screenBounds.Width / 2 - halfWidth, screenBounds.Height / 2 - halfHeight, windowRect.Width, windowRect.Height, SWP.SHOWWINDOW);


            // Start threads, add hooks.

            drawThread = null;
            drawThread = new Thread(GraphicsDrawThread);
            drawThread.Start();

            renderThread = null;
            renderThread = new Thread(GraphicsRenderThread);
            renderThread.Start();

            AddHooks();

            Native.SetConsoleCtrlHandler(closeEvent, true);

            Application.Run();
        }

        /// <summary>
        /// Stops the Painter.
        /// </summary>
        public static void Stop()
        {
            enabled = false;
            Application.Exit();
        }

        /// <summary>
        /// Returns whether a specific key is pressed.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns></returns>
        public static bool IsKeyDown(Keys key)
        {
            return keyboardState[(int)key];
        }

        /// <summary>
        /// Clears the active buffer to the specified attributes.
        /// </summary>
        /// <param name="clearColor">The attributes to fill the buffer with.</param>
        public static void Clear(BufferColor clearColor = BufferColor.Black)
        {
            ActiveBuffer.Clear(clearColor);
        }

        private static bool InBounds(int x, int y)
        {
            return x >= 0 && x < ActiveBuffer.Width && y >= 0 && y < ActiveBuffer.Height;
        }
    }
}

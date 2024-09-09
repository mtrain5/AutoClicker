using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace MouseAutomation
{
    class Program
    {
        // Importing user32.dll functions
        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, IntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        // Constants for mouse events
        const uint LEFTDOWN = 0x02;
        const uint LEFTUP = 0x04;

        // Constants for hotkeys
        const int SOFTAC_HOTKEY = 0x071;
        const int AC_HOTKEY = 0x73;
        const int INSTALUNGE_HOTKEY = 0x72;

        // State flags
        static bool SoftACEnabled = false;
        static bool InstaLungeEnabled = false;
        static bool ACenabled = false;
        static bool MB1PRESS_A = false;
        static bool MB1PRESS_B = false;
        static bool PROGRAM_CLICK = false;

        static void Main(string[] args)
        {
            // Start the hotkey detection thread
            Thread hotkeyCheck = new Thread(new ThreadStart(DetectHotkey));
            hotkeyCheck.Start();

            while (true)
            {
                if (SoftACEnabled)
                {
                    if (MB1PRESS_B)
                    {
                        Thread.Sleep(550);
                        MB1PRESS_B = false;
                    }
                    MouseClick2();
                    Thread.Sleep(190);
                    continue;
                }
                if (ACenabled)
                {
                    if (MB1PRESS_B)
                    {
                        Thread.Sleep(550);
                        MB1PRESS_B = false;
                    }
                    MouseClick();
                    Thread.Sleep(190);
                    continue;
                }
            }
        }

        static void DetectHotkey()
        {
            while (true)
            {
                if (GetAsyncKeyState(SOFTAC_HOTKEY) < 0)
                {
                    SoftACEnabled = !SoftACEnabled;
                    Thread.Sleep(300);
                }
                if (GetAsyncKeyState(AC_HOTKEY) < 0)
                {
                    ACenabled = !ACenabled;
                    Thread.Sleep(300);
                }
                if (GetAsyncKeyState(INSTALUNGE_HOTKEY) < 0)
                {
                    InstaLungeEnabled = !InstaLungeEnabled;
                    Thread.Sleep(300);
                }
                if ((SoftACEnabled && !PROGRAM_CLICK) && GetAsyncKeyState(0x01) < 0)
                {
                    MB1PRESS_B = true;
                }
                if (InstaLungeEnabled && GetAsyncKeyState(0x01) < 0)
                {
                    MB1PRESS_A = true;
                }
                else
                {
                    if (MB1PRESS_A)
                    {
                        DoubleClick();
                        MB1PRESS_A = false;
                    }
                }
            }
        }

        static void BypassClick()
        {
            for (int i = 0; i < 9; i++)
            {
                if (!ACenabled) { continue; }
                Random random = new Random();
                Thread.Sleep(random.Next(30, 100));
                MouseClick();
            }
        }

        static void DoubleClick()
        {
            Thread.Sleep(20);
            MouseClick();
            Thread.Sleep(20);
            MouseClick();
        }

        static void MouseClick()
        {
            PROGRAM_CLICK = true;
            mouse_event(LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(23);
            mouse_event(LEFTUP, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(30);
            mouse_event(LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(26);
            mouse_event(LEFTUP, 0, 0, 0, IntPtr.Zero);
            PROGRAM_CLICK = false;
        }

        static void MouseClick2()
        {
            PROGRAM_CLICK = true;
            mouse_event(LEFTDOWN, 0, 0, 0, IntPtr.Zero);
            Thread.Sleep(20);
            mouse_event(LEFTUP, 0, 0, 0, IntPtr.Zero);
            PROGRAM_CLICK = false;

        }
    }
}

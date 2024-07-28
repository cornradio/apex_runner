using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public static class AltShift
{
    private static bool isDisabled = false;
    private static LowLevelKeyboardProc proc = HookCallback;
    private static IntPtr hookID = IntPtr.Zero;

    public static void Enable()
    {
        if (isDisabled)
        {
            UnhookWindowsHookEx(hookID);
            isDisabled = false;
        }
    }

    public static void Disable()
    {
        if (!isDisabled)
        {
            hookID = SetHook(proc);
            isDisabled = true;
        }
    }

    private static IntPtr SetHook(LowLevelKeyboardProc proc)
    {
        using (var curProcess = Process.GetCurrentProcess())
        using (var curModule = curProcess.MainModule)
        {
            return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }
    }

    private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && (wParam == (IntPtr)WM_KEYDOWN || wParam == (IntPtr)WM_SYSKEYDOWN))
        {
            int vkCode = Marshal.ReadInt32(lParam);

            if ((vkCode == (int)Keys.Menu || vkCode == (int)Keys.LMenu || vkCode == (int)Keys.RMenu) && Control.ModifierKeys.HasFlag(Keys.Shift))
            {
                return (IntPtr)1; // Block the Alt key press
            }
            else if ((vkCode == (int)Keys.ShiftKey || vkCode == (int)Keys.LShiftKey || vkCode == (int)Keys.RShiftKey) && Control.ModifierKeys.HasFlag(Keys.Alt))
            {
                return (IntPtr)1; // Block the Shift key press
            }
        }
        return CallNextHookEx(hookID, nCode, wParam, lParam);
    }

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int WM_SYSKEYDOWN = 0x0104;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);
}


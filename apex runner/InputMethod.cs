using System;
using System.Runtime.InteropServices;

public static class InputMethod
{
    // 常量定义，代表输入法的语言代码
    private const int LANG_CHINESE = 0x0804; // 中文（中国）
    private const int LANG_ENGLISH = 0x0409; // 英文（美国）

    // 定义输入法枚举
    public enum InputMethodType
    {
        Chinese,
        English,
        Unknown
    }

    // Windows API 函数
    [DllImport("user32.dll")]
    private static extern IntPtr GetKeyboardLayout(int idThread);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool ActivateKeyboardLayout(IntPtr hkl, uint Flags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr LoadKeyboardLayout(string pwszKLID, uint Flags);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    public static void ChangeToChinese()
    {
        IntPtr hkl = LoadKeyboardLayout("00000804", 1); // 00000804 表示中文（中国）
        ActivateKeyboardLayout(hkl, 0);
    }

    public static void ChangeToEnglish()
    {
        IntPtr hkl = LoadKeyboardLayout("00000409", 1); // 00000409 表示英文（美国）
        ActivateKeyboardLayout(hkl, 0);
    }

    public static InputMethodType CurrentMethod()
    {
        IntPtr hwnd = GetForegroundWindow();
        int processId;
        int threadId = GetWindowThreadProcessId(hwnd, out processId);
        IntPtr hkl = GetKeyboardLayout(threadId);
        uint localeId = (uint)hkl & 0xFFFF;

        switch (localeId)
        {
            case LANG_CHINESE:
                return InputMethodType.Chinese;
            case LANG_ENGLISH:
                return InputMethodType.English;
            default:
                return InputMethodType.Unknown;
        }
    }
}

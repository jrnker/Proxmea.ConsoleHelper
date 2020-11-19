using System;
using System.Runtime.InteropServices;

namespace Proxmea.ConsoleHelper
{
    public class KernalHelper
    {
        // https://stackoverflow.com/questions/12355378/read-from-location-on-console-c-sharp
        // https://stackoverflow.com/a/12366307/1983076 Simon Mourier
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr GetStdHandle(int num);
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
        [return: MarshalAs(UnmanagedType.Bool)] //   ̲┌──────────────────^
        public static extern bool ReadConsoleOutputCharacterA(
          IntPtr hStdout,   // result of 'GetStdHandle(-11)'
          out byte ch,      // A̲N̲S̲I̲ character result
          uint c_in,        // (set to '1')
          uint coord_XY,    // screen location to read, X:loword, Y:hiword
          out uint c_out);  // (unwanted, discard)

        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)] //   ̲┌───────────────────^
        public static extern bool ReadConsoleOutputCharacterW(
            IntPtr hStdout,   // result of 'GetStdHandle(-11)'
            out Char ch,      // U̲n̲i̲c̲o̲d̲e̲ character result
            uint c_in,        // (set to '1')
            uint coord_XY,    // screen location to read, X:loword, Y:hiword
            out uint c_out);  // (unwanted, discard) 

        // https://stackoverflow.com/questions/54317061/redirect-output-from-running-process-visual-c/54317865
        // https://stackoverflow.com/a/54317865/1983076 Derviş Kayımbaşıoğlu
        [DllImport("kernel32.dll")]
        public static extern bool GetConsoleScreenBufferInfo(
            IntPtr hConsoleOutput,
            out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo
        );
        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct COORD
        {
            public short X;
            public short Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct SMALL_RECT
        {
            public short Left;
            public short Top;
            public short Right;
            public short Bottom;
        }
        public const int STD_OUTPUT_HANDLE = -11;
        public const Int64 INVALID_HANDLE_VALUE = -1;
    }
}

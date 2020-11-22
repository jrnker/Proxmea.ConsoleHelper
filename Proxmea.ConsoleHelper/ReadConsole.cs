using System;
using System.Collections.Generic;
using System.ComponentModel;
using static Proxmea.ConsoleHelper.KernalHelper;

namespace Proxmea.ConsoleHelper
{
    public static class ReadConsole
    {
        /// <summary>
        /// Get current cursor position from console window.
        /// In .Net 5 > use Console.GetCursorPosition
        /// </summary>
        /// <returns>Cursor position</returns>
        public static COORD GetCursorPosition()
        {
            // Get a handle for the console
            var stdout = GetStdHandle(STD_OUTPUT_HANDLE);

            // In .net 5 there's Console.GetCursorPosition for this
            return GetConsoleInfo(stdout).dwCursorPosition;
        }
        /// <summary>
        /// Retrieves information about the current screen buffer window
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static CONSOLE_SCREEN_BUFFER_INFO GetConsoleInfo()
        {
            // Get a handle for the console
            var stdout = GetStdHandle(STD_OUTPUT_HANDLE);
            return GetConsoleInfo(stdout);
        }
        public static CONSOLE_SCREEN_BUFFER_INFO GetConsoleInfo(IntPtr ptr)
        {
            if (!GetConsoleScreenBufferInfo(ptr, out CONSOLE_SCREEN_BUFFER_INFO outInfo))
                throw new Win32Exception();
            return outInfo;
        }
        /// <summary>
        /// Find text in console window
        /// </summary>
        /// <param name="text"></param>
        /// <returns>List of found coordinates</returns>
        public static List<COORD> IndexOfInConsole(string text)
        {
            return IndexOfInConsole(new[] { text });
        }
        /// <summary>
        /// Find texts in console window
        /// </summary>
        /// <param name="text"></param>
        /// <returns>List of found coordinates</returns>
        public static List<COORD> IndexOfInConsole(string[] text)
        {
            var coords = new List<COORD>();

            // Get a handle for the console
            var stdout = GetStdHandle(STD_OUTPUT_HANDLE);

            // Get Console Info
            var consoleInfo = GetConsoleInfo(stdout);

            for (int y = 0; y < consoleInfo.dwCursorPosition.Y; y += 1)
            {
                var line = GetText(0, y, stdout);

                // Search through the line and put the results in coords
                foreach (var t in text)
                {
                    var xPos = 0;
                    while (true)
                    {
                        var pos = line.IndexOf(t, xPos);
                        if (pos == -1)
                            break;
                        coords.Add(new COORD() { X = (short)pos, Y = (short)y });
                        xPos = pos + 1;
                    }
                }
            }
            return coords;
        }
        /// <summary>
        /// Retrieve character from console window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>One character</returns>
        public static char GetChar(int x, int y)
        {
            // Get a handle for the console
            var stdout = GetStdHandle(STD_OUTPUT_HANDLE);

            // Call it with a pointer
            return GetChar(x, y, stdout);
        }
        /// <summary>
        /// Retrieve character from console window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ptr"></param>
        /// <returns>One character</returns>
        public static char GetChar(int x, int y, IntPtr ptr)
        {
            // Convert to coord and call it
            return GetChar(new COORD() { X = (short)x, Y = (short)y }, ptr);
        }
        /// <summary>
        /// Retrieve character from console window
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="ptr"></param>
        /// <returns>One character</returns>
        public static char GetChar(COORD coordinate, IntPtr ptr)
        {
            // Convert the coordinates to a uint containing both
            uint coord = (uint)coordinate.X;
            coord |= (uint)coordinate.Y << 16;

            if (!ReadConsoleOutputCharacterW(
                        ptr,
                        out char chUnicode, // result: single Unicode char
                        1,                  // # of chars to read
                        coord,              // (X,Y) screen location to read (see above)
                        out _))             // result: actual # of chars (unwanted)
                throw new Win32Exception();
            return chUnicode;
        }
        /// <summary>
        /// Retrieve text from console window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>All text on the line until end</returns>
        public static string GetText(int x, int y)
        {
            // Get a handle for the console
            var stdout = GetStdHandle(STD_OUTPUT_HANDLE);

            // Let's call it with a console pointer
            return GetText(x, y, stdout);
        }
        /// <summary>
        /// Retrieve text from console window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <returns>The specified text on the line</returns>
        public static string GetText(int x, int y, int length)
        {
            // Get a handle for the console
            var stdout = GetStdHandle(STD_OUTPUT_HANDLE);

            // Let's call it with a console pointer
            return GetText(x, y, length, stdout);
        }
        /// <summary>
        /// Retrieve text from console window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="ptr"></param>
        /// <returns>All text on the line until end</returns>
        public static string GetText(int x, int y, IntPtr ptr)
        {
            // Get Console Info
            var consoleInfo = GetConsoleInfo(ptr);

            // Let's call it with the remaining bit of the x screen buffer
            return GetText(x, y, consoleInfo.dwSize.X - x, ptr);
        }
        /// <summary>
        /// Retrieve text from console window
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="length"></param>
        /// <param name="ptr"></param>
        /// <returns>The specified text on the line</returns>
        public static string GetText(int x, int y, int length, IntPtr ptr)
        {
            // Convert to coord and call it
            return GetText(new COORD() { X = (short)x, Y = (short)y }, length, ptr);
        }
        /// <summary>
        /// Retrieve text from console window
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="length"></param>
        /// <param name="ptr"></param>
        /// <returns>The specified text on the line</returns>
        public static string GetText(COORD coordinate, int length, IntPtr ptr)
        {
            var text = "";
            for (var x = coordinate.X; x < coordinate.X + length; x += 1)
                text += GetChar(x, coordinate.Y);
            return text;
        }
    }
}

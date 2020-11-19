using Proxmea;
using System;

namespace Proxmea.ConsoleHelper.TestConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            // Let's draw some stuff
            Populate_Console();

            // Find the border characters
            var found = ReadConsole.IndexOfInConsole(new[] { "│", "─", "┌", "┐", "┘", "└" });

            // Let's save the current position
            var curPos = ReadConsole.GetCursorPosition();

            // Go through the results and replace the finds with *
            foreach (var c in found)
            {
                Console.SetCursorPosition(c.X, c.Y);
                Console.Write("*");
            }

            // Your output should now not have a border with lines, but with *'s

            // Move back the cursor
            Console.SetCursorPosition(curPos.X, curPos.Y);
            Console.Write("Done");

            // Just to check, let's fetch a character on a known position and check it
            var character = ReadConsole.GetChar(1, 1);
            if (character != '*')
                throw new Exception("An asterix wasn't found");

            // Just to check some more, let's fetch a text on a known position and length and check it
            var text = ReadConsole.GetText(4, 3, 5);
            if (text != "H I J")
                throw new Exception("An text wasn't found");

            // Just to check some more more, let's fetch a whole line. 
            // This will be the full line, including the trailing empty spaces
            var line = ReadConsole.GetText(0, 4);  
        }

        static void Populate_Console()
        {
            Console.Clear();
            Console.Write(@"
 ┌───────┐
1│C D E F│
2│G H I J│
3│K L M N│
4│O P Q R│
 └───────┘
  2 4 6 8          ");
            Console.Write('\n');
        }
    }
}

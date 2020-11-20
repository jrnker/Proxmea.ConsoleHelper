using System;
using System.Threading;

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
            var found = ReadConsole.IndexOfInConsole(new[] { "┌", "┐", "└", "┘", "│", "─" });

            // Let's save the current position
            var curPos = ReadConsole.GetCursorPosition();

            // Go through the results and replace the finds with *
            foreach (var c in found)
            {
                Console.SetCursorPosition(c.X, c.Y);
                Console.Write("*");
                Thread.Sleep(100);
            }

            // Your output should now not have a border with lines, but with *'s

            // Move back the cursor
            Console.SetCursorPosition(curPos.X, curPos.Y);
            Console.WriteLine("Done");

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

            // Search for a string
            var stringToFind = "H I J";
            found = ReadConsole.IndexOfInConsole(stringToFind);
            Console.WriteLine($"Searching for {stringToFind}");
            foreach (var f in found)
                Console.WriteLine($"  Found at X {f.X}, Y {f.Y}");
        }

        static void Populate_Console()
        {
            Console.Clear();
            Console.Write(@"Y Find and replace border with *, with a delay
↓┌───────┐
2│C D E F│
3│G H I J│
4│K L M N│
5│O P Q R│
 └───────┘
X→2 4 6 8          ");
            Console.Write('\n');
        }
    }
}

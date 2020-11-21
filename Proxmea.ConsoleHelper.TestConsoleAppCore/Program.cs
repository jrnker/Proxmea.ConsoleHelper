using System;
using System.Threading;
using static Proxmea.ConsoleHelper.KernalHelper;

namespace Proxmea.ConsoleHelper.TestConsoleAppCore
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            //Console.ReadKey();

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

            Console.WriteLine("\nDisplaying time and waiting for you to");
            Console.WriteLine("press any key to exit");

            // Draw something in the bottom right corner
            // and make sure it stays there
            COORD previousBottomRight = new COORD();
            COORD previousCoord = new COORD();
            while (!Console.KeyAvailable)
            {
                text = DateTime.Now.ToLongTimeString();

                // Get console window properties
                var consoleInfo = ReadConsole.GetConsoleInfo();

                // Get bottom right coordinates from the console properties
                var bottomRight = new COORD() { X = consoleInfo.srWindow.Right, Y = consoleInfo.srWindow.Bottom };

                // The coordinate we're interested in is X - length of text
                var coord = new COORD() { X = (short)(bottomRight.X - text.Length), Y = bottomRight.Y };

                // If the window has scrolled, then remove the old text
                if (bottomRight.X != previousBottomRight.X || bottomRight.Y != previousBottomRight.Y)
                {
                    // Removing the text is far more complex than this, as there are many aspects to how a windows properties
                    // changes when resizing, but this works an it's essece.
                    curPos = ReadConsole.GetCursorPosition();
                    Console.SetCursorPosition(previousCoord.X, previousCoord.Y);
                    Console.Write("".PadLeft(text.Length, ' '));
                    Console.SetCursorPosition(curPos.X, curPos.Y);
                }

                // Save these new coordinates for the next cycle
                // If the window scrolls, then we need to make sure it still looks good
                previousBottomRight = bottomRight;
                previousCoord = coord;

                // Draw the text
                curPos = ReadConsole.GetCursorPosition();
                Console.SetCursorPosition(coord.X, coord.Y);
                Console.Write(text);
                Console.SetCursorPosition(curPos.X, curPos.Y);

                Thread.Sleep(100);
            }
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

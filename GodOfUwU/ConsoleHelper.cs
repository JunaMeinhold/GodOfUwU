namespace GodOfUwU
{
    using System;

    public static class ConsoleHelper
    {
        public static int ReadInt(string title)
        {
            Console.Clear();
            Console.Write(title);
            int value;
            string? line = Console.ReadLine();
            while (!int.TryParse(line, out value))
            {
                Console.Clear();
                Console.Write(title);
                line = Console.ReadLine();
            }

            return value;
        }

        public static ulong ReadULong(string title)
        {
            Console.Clear();
            Console.Write(title);
            ulong value;
            string? line = Console.ReadLine();
            while (!ulong.TryParse(line, out value))
            {
                Console.Clear();
                Console.Write(title);
                line = Console.ReadLine();
            }

            return value;
        }
    }
}
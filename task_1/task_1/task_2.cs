using System;

namespace task_2
{
    public class Task
    {
        public static void Start()
        {
            Console.WriteLine("Please, write n..");
            int n;
            while (!Int32.TryParse(Console.ReadLine(), out n))
            {
                Console.WriteLine("Please, write correct number");
            }
            if (n <= 0)
            {
                return;
            }
            Console.Write("The Fibonacci Sequence for n " + n.ToString() + " is :");
            Console.ForegroundColor = ConsoleColor.Green;
            int a = 1;
            int b = 1;
            for (int i = 0; i < n; i++)
            {
                if (i >= 2)
                {
                    Console.Write((a + b).ToString() + " ");
                    int c = b;
                    b = a + b;
                    a = c;
                }
                else
                {
                    Console.Write("1 ");
                }
            }
            Console.ResetColor();
        }
    }
}
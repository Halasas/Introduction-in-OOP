using System;
using task_1;
using task_2;
using task_3;

namespace MainProgram
{
    public static class Task1_Class
    {
        static void Main(string[] args)
        {
            while(true)
            { 
                Console.WriteLine("Choose your task: ");
                Console.WriteLine("1. Sum of numbers from file");
                Console.WriteLine("2. The Fibonacci Sequence for n");
                Console.WriteLine("3. Convert Roman Number");
                Console.WriteLine("4. Exit");

                int n = -1;
                Int32.TryParse(Console.ReadLine(), out n);
                while(0 > n || n > 4)
                {
                    Console.WriteLine(n.ToString());
                    Console.WriteLine("Please, write correct number..");
                    Int32.TryParse(Console.ReadLine(), out n);
                }
                Console.WriteLine("-------------------------------");

                switch (n)
                {
                    case 1:
                        task_1.Task.Start();

                        break;
                    case 2:
                        task_2.Task.Start();

                        break;
                    case 3:
                        task_3.Task.Start();

                        break;
                    case 4:
                        return;
                }
                Console.Write('\n');
                Console.WriteLine("Task compilte..");
                Console.Write('\n');
            }

        }
    }

}

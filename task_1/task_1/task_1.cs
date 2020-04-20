using System;
using System.IO;

namespace task_1
{
    public class Task
    {
        private static string path_in = @"D:\Programing\Introduction-in-OOP\task_1\input.txt";
        private static string path_out = @"D:\Programing\Introduction-in-OOP\task_1\output.txt";

        public static void Start()
        {
            string input = TryToReadInput();
            char[] separators = { ' ', '\n' };
            string[] words = input.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            long sum = 0;
            foreach (string s in words)
            {
                long tmp;
                if (Int64.TryParse(s, out tmp))
                    sum += tmp;
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Some numbers are invalid..");
                    Console.ResetColor();
                }
            }
            TryToWriteOutput(sum.ToString());
        }

        private static void ErrorMessage(string str, Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(str);
            Console.WriteLine(e.Message);
            Console.ResetColor();
            Console.WriteLine("Press any button");
            Console.ReadKey();
        }
        private static string TryToReadInput()
        {
            string s;
            Console.WriteLine("Start reading from file...");
            try
            {
                using (StreamReader sr = new StreamReader(path_in))
                {
                    try
                    {
                        s = sr.ReadToEnd();
                    }
                    catch (IOException e)
                    {
                        ErrorMessage("Input file couldnt be readed becouse", e);
                        return "";
                    }
                    catch (OutOfMemoryException e)
                    {
                        ErrorMessage("File too big, sorry :c", e);
                        return "";
                    }
                    catch (Exception e)
                    {
                        ErrorMessage("Something goes wrong..", e);
                        return "";
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                ErrorMessage("File not found...", e);
                return "";
            }
            catch (Exception e)
            {
                ErrorMessage("Something goes wrong..", e);
                return "";
            }
            Console.WriteLine("Reading complite");
            return s;
        }

        private static bool TryToWriteOutput(string s)
        {
            Console.WriteLine("Start writing into file...");
            try
            {
                using (StreamWriter sw = new StreamWriter(path_out, false))
                {
                    try
                    {
                        sw.WriteLine(s);
                    }
                    catch (Exception e)
                    {
                        ErrorMessage("Failed to write..", e);
                        return false;
                    }
                }
            }
            catch (FileNotFoundException e)
            {
                ErrorMessage("File not found...", e);
                return false;
            }
            catch (Exception e)
            {
                ErrorMessage("Something goes wrong..", e);
                return false;
            }
            Console.WriteLine("Writing complite succesful");
            return true;
        }
    }
}
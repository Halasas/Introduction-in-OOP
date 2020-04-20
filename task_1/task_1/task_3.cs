using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace task_3
{
    public class Task
    {
        public static void Start()
        {
            Console.WriteLine("Please, write Roman number..");
            string s = Console.ReadLine();
            int res = RomanConverter.GetInt(s);
            if (res == -1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Couldnt parse your number correctly..");
                Console.ResetColor();
                return;
            }
            Console.Write("Your number in dec = ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(res.ToString());
            Console.ResetColor();
        }

        public static class RomanConverter
        {
            public static int GetInt(string s)
            {
                Dictionary<char, int> romanDigits = GetRomanDigits();
                int result = 0;

                Regex regex = new Regex("^M{0,3}(CM|CD|D?C{0,3})(XC|XL|L?X{0,3})(IX|IV|V?I{0,3})$");

                if (!regex.IsMatch(s))
                {
                    Console.WriteLine("Number is invalid");
                    return -1;
                }

                for (int i = 0; i < s.Length; i++)
                {
                    if (i != s.Length - 1)
                    {
                        if (romanDigits[s[i]] < romanDigits[s[i + 1]])
                            result -= romanDigits[s[i]];
                        else
                            result += romanDigits[s[i]];
                    }
                    else
                        result += romanDigits[s[i]];
                }
                return result;
            }
            private static Dictionary<char, int> GetRomanDigits()
            {
                Dictionary<char, int> rd = new Dictionary<char, int>();
                rd.Add('I', 1); rd.Add('i', 1);
                rd.Add('V', 5); rd.Add('v', 5);
                rd.Add('X', 10); rd.Add('x', 10);
                rd.Add('L', 50); rd.Add('l', 50);
                rd.Add('C', 100); rd.Add('c', 100);
                rd.Add('D', 500); rd.Add('d', 500);
                rd.Add('M', 1000); rd.Add('m', 1000);
                return rd;
            }
        }
    }
}

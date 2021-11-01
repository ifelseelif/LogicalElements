using System;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new Parser();
            var httpSender = new HttpSender();

            var flag = true;

            Console.WriteLine("Hello, write your commands");
            while (flag)
            {
                Console.Write(">");
                var input = Console.ReadLine();
                if (input == "exit")
                {
                    flag = false;
                    continue;
                }

                var parserResult = parser.Parse(input);
                if (parserResult == null)
                {
                    Console.WriteLine("command not recognized");
                    continue;
                }

                httpSender.Send(parserResult).Wait();
            }
        }
    }
}
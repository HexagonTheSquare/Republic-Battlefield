using System;

namespace CloneArmyPlayground
{
    class Program
    {
        public static void Main(string[] args)
        {
            Program program = new Program();

            bool x0 = true;
            while (x0)
            {
                string command = (string)Console.ReadLine();

                switch (command.ToLower())
                {
                    case "help":
                        Console.WriteLine("begin\nexit\nhelp");
                        break;
                    case "exit":
                        x0 = false;
                        break;
                    case "begin":
                        Operation operation = new Operation();
                        operation.RunWar();
                        break;
                }
            }
        }

        

        
    }
}

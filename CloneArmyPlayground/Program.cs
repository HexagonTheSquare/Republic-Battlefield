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
                        program.CorpsConfiguration();
                        break;
                }

            }
        }


        public static string GetOrdinalSuffix(int number)
        {
            int lastDigit = number % 10;
            int lastTwoDigits = number % 100;

            if (lastDigit == 1 && lastTwoDigits != 11)
            {
                return "st";
            }
            else if (lastDigit == 2 && lastTwoDigits != 12)
            {
                return "nd";
            }
            else if (lastDigit == 3 && lastTwoDigits != 13)
            {
                return "rd";
            }
            else
            {
                return "th";
            }
        }

        public static Corps GenerateCorps(string name, string faction)
        {
            Corps corps = new Corps(name);
            
            switch (faction)
            {
                case "republic":
                    for (int i = 1; i <= 64; i++)
                    {
                        Battalion battalion = new Battalion($"{i}{GetOrdinalSuffix(i)} Battalion");
                        corps.AddSubdivision(battalion);

                        for (int j = 1; j <= 4; j++)
                        {
                            Company company = new Company($"{j}{GetOrdinalSuffix(j)} Company");
                            battalion.AddSubdivision(company);

                            for (int k = 1; k <= 4; k++)
                            {
                                Platoon platoon = new Platoon($"{k}{GetOrdinalSuffix(k)} Platoon");
                                company.AddSubdivision(platoon);

                                for (int l = 1; l <= 4; l++)
                                {
                                    // Set specific values for skill, morale, and discipline
                                    double skill = RandomNumberGeneratorUtil.Next(0, 100) * RandomNumberGeneratorUtil.NextFloat(0.75f, 1);
                                    double morale = RandomNumberGeneratorUtil.Next(0, 100) * RandomNumberGeneratorUtil.NextFloat(0.75f, 1);
                                    double discipline = RandomNumberGeneratorUtil.Next(0, 100) * RandomNumberGeneratorUtil.NextFloat(0.75f, 1);

                                    Squad squad = new Squad($"{l}{GetOrdinalSuffix(l)} Squad", 9, skill, morale, discipline);
                                    platoon.AddSubdivision(squad);
                                }
                            }
                        }
                    }
                    break;
                case "seperatist":
                    for (int i = 1; i <= 96; i++)
                    {
                        Battalion battalion = new Battalion($"{i}{GetOrdinalSuffix(i)} Confederate Battalion");
                        corps.AddSubdivision(battalion);

                        for (int j = 1; j <= 7; j++)
                        {
                            Company company = new Company($"{j}{GetOrdinalSuffix(j)} Company");
                            battalion.AddSubdivision(company);

                            for (int k = 1; k <= 2; k++)
                            {
                                Platoon platoon = new Platoon($"{k}{GetOrdinalSuffix(k)} Platoon");
                                company.AddSubdivision(platoon);

                                for (int l = 1; l <= 7; l++)
                                {
                                    // Set specific values for skill, morale, and discipline
                                    double skill = RandomNumberGeneratorUtil.Next(0, 100) / RandomNumberGeneratorUtil.Next(1, 5);
                                    double morale = RandomNumberGeneratorUtil.Next(0, 100) / RandomNumberGeneratorUtil.Next(1, 5);
                                    double discipline = RandomNumberGeneratorUtil.Next(0, 100) / RandomNumberGeneratorUtil.Next(1, 5);

                                    Squad squad = new Squad($"{l}{GetOrdinalSuffix(l)} Squad", 9, skill, morale, discipline);
                                    platoon.AddSubdivision(squad);
                                }
                            }
                        }
                    }
                    break;
            }
            
            return corps;
        }

        public void CorpsConfiguration()
        {
            Console.WriteLine("Welcome, General!");
            Console.Write("What's the name of your Corps? ");
            string name = Console.ReadLine();

            Corps userCorps = GenerateCorps(name, "republic");

            Console.Write("What's the name of the enemy Corps? ");
            name = Console.ReadLine();

            Corps enemyCorps = GenerateCorps(name, "seperatist");

            Console.WriteLine("Your corps, the {0}, currently has {1} men.", userCorps.Name, userCorps.Manpower);
            Console.WriteLine("The enemy corps, the {0}, currently has {1} men.", enemyCorps.Name, enemyCorps.Manpower);

            foreach (var battalion in userCorps.Subdivisions)
            {
                Console.WriteLine(battalion.Name);
            }
        }




    }
}

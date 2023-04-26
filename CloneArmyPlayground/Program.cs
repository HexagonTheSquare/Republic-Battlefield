using System;

namespace CloneArmyPlayground
{
    class Program
    {
        public static void Main(string[] args)
        {
            Corps reconCorps = GenerateCorps("91st Recon Corps");

            Console.WriteLine($"91st Recon Corps Manpower: {reconCorps.Manpower}");
            Console.WriteLine($"91st Recon Corps Average Skill: {(int)reconCorps.ArmySkill}");
            Console.WriteLine($"91st Recon Corps Average Morale: {(int)reconCorps.ArmyMorale}");
            Console.WriteLine($"91st Recon Corps Average Discipline: {(int)reconCorps.ArmyDiscipline}");

            foreach (var battalion in reconCorps.Subdivisions)
            {
                Console.WriteLine(battalion.Name);
                foreach (var company in battalion.Subdivisions)
                {
                    //Console.WriteLine(company.Name);
                    foreach (var platoon in company.Subdivisions)
                    {
                        foreach (var squad in platoon.Subdivisions)
                        {
                            //
                        }
                    }
                }
            }
            Console.ReadLine();
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
                case "seperatist":
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
                                    double skill = RandomNumberGeneratorUtil.Next(0, 100) / RandomNumberGeneratorUtil.Next(1, 5);
                                    double morale = RandomNumberGeneratorUtil.Next(0, 100) / RandomNumberGeneratorUtil.Next(1, 5);
                                    double discipline = RandomNumberGeneratorUtil.Next(0, 100) / RandomNumberGeneratorUtil.Next(1, 5);

                                    Squad squad = new Squad($"{l}{GetOrdinalSuffix(l)} Squad", 8, skill, morale, discipline);
                                    platoon.AddSubdivision(squad);
                                }
                            }
                        }
                    }
                    break;
            }
            
            return corps;
        }




    }
}

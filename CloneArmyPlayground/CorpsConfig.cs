using System;
using System.Collections.Generic;
using System.Text;

namespace CloneArmyPlayground
{
    public class CorpsConfig
    {
        public static Corps[] CorpsConfiguration()
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

            //foreach(var battalion in userCorps.Subdivisions)
            //{
            //    foreach (var company in battalion.Subdivisions)
            //    {
            //        foreach (var platoon in company.Subdivisions)
            //        {
            //            foreach (var squad in platoon.Subdivisions)
            //            {
            //                Console.WriteLine("{2} {0}, {1} - {3} men", squad.Name, battalion.Name, company.Name.Split(' ')[0], squad.Manpower);
            //            }
            //        }
            //    }
            //}

            return new Corps[] { userCorps, enemyCorps };
        }

        public static Corps GenerateCorps(string name, string faction)
        {
            Corps corps = new Corps(name);

            List<string> companyNames = Util.ReadWordsFromFile("wordlists/CompanyNames.txt");

            switch (faction)
            {
                case "republic":

                    HashSet<string> usedCompanyNames = new HashSet<string>();

                    for (int i = 1; i <= 64; i++)
                    {
                        Battalion battalion = new Battalion($"{i}{Util.GetOrdinalSuffix(i)} Battalion");
                        corps.AddSubdivision(battalion);

                        for (int j = 1; j <= 4; j++)
                        {
                            // Pick a random uniqueword for the company name
                            string companyName;
                            do{
                                companyName = Util.GetRandomWord(companyNames);
                            } while (usedCompanyNames.Contains(companyName));

                            usedCompanyNames.Add(companyName);
                            Company company = new Company($"{companyName} Company");
                            battalion.AddSubdivision(company);
                            int squadCounter = 1;

                            for (int k = 1; k <= 4; k++)
                            {
                                Platoon platoon = new Platoon($"{k}{Util.GetOrdinalSuffix(k)} Platoon");
                                company.AddSubdivision(platoon);

                                for (int l = 1; l <= 4; l++)
                                {
                                    // Set specific values for skill, morale, and discipline
                                    double skill = Util.Next(0, 100) * Util.NextFloat(0.75f, 1);
                                    double morale = Util.Next(0, 100) * Util.NextFloat(0.75f, 1);
                                    double discipline = Util.Next(0, 100) * Util.NextFloat(0.75f, 1);

                                    Squad squad = new Squad($"{squadCounter}{Util.GetOrdinalSuffix(squadCounter)} Squad", 9, skill, morale, discipline);
                                    platoon.AddSubdivision(squad);

                                    squadCounter++;
                                }
                            }
                        }
                    }

                    break;
                case "seperatist":
                    for (int i = 1; i <= 96; i++)
                    {
                        Battalion battalion = new Battalion($"{i}{Util.GetOrdinalSuffix(i)} Confederate Battalion");
                        corps.AddSubdivision(battalion);

                        for (int j = 1; j <= 7; j++)
                        {
                            Company company = new Company($"{j}{Util.GetOrdinalSuffix(j)} Company");
                            battalion.AddSubdivision(company);
                            int squadCounter = 1;

                            for (int k = 1; k <= 2; k++)
                            {
                                Platoon platoon = new Platoon($"{k}{Util.GetOrdinalSuffix(k)} Platoon");
                                company.AddSubdivision(platoon);

                                for (int l = 1; l <= 7; l++)
                                {
                                    // Set specific values for skill, morale, and discipline
                                    double skill = Util.Next(0, 100) / Util.Next(1, 5);
                                    double morale = Util.Next(0, 100) / Util.Next(1, 5);
                                    double discipline = Util.Next(0, 100) / Util.Next(1, 5);

                                    Squad squad = new Squad($"{squadCounter}{Util.GetOrdinalSuffix(squadCounter)} Squad", 9, skill, morale, discipline);
                                    platoon.AddSubdivision(squad);

                                    squadCounter++;
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

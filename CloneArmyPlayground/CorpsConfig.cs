using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CloneArmyPlayground
{
    public class CorpsConfig
    {
        private static HashSet<string> usedCloneIds = new HashSet<string>();
        private static HashSet<string> usedJediNames = new HashSet<string>();

        public static Corps[] CorpsConfiguration()
        {
            Console.WriteLine("Welcome, General!");
            Console.Write("What's the name of your Corps? ");
            string name = Console.ReadLine();
            Corps userCorps = GenerateCorps(name, "republic");

            Console.Write("What's the name of the enemy Corps? ");
            name = Console.ReadLine();
            Corps enemyCorps = GenerateCorps(name, "seperatist");

            Console.WriteLine($"Your corps, the {userCorps.Name}, currently has {userCorps.Manpower} men.");
            Console.WriteLine($"The enemy corps, the {enemyCorps.Name}, currently has {enemyCorps.Manpower} units.");

            return new Corps[] { userCorps, enemyCorps };
        }

        private static string GetCloneIdentifier(Random rand)
        {
            string id;
            do
            {
                int part1 = rand.Next(10, 100);
                int part2 = rand.Next(1000, 10000);
                id = $"CT-{part1}-{part2}";
            } while (usedCloneIds.Contains(id));

            usedCloneIds.Add(id);
            return id;
        }

        private static string GetDroidIdentifier(int index) => $"OOM-{index:D4}";

        private static string GetUniqueJediName(Random rand)
        {
            List<string> firstNames = Util.ReadWordsFromFile("wordlists/JediFirstNames.txt");
            List<string> lastNames = Util.ReadWordsFromFile("wordlists/JediLastNames.txt");

            string fullName;
            do
            {
                string first = firstNames[rand.Next(firstNames.Count)];
                string last = lastNames[rand.Next(lastNames.Count)];
                fullName = $"{first} {last}";
            } while (usedJediNames.Contains(fullName));

            usedJediNames.Add(fullName);
            return fullName;
        }

        public static Corps GenerateCorps(string name, string faction)
        {
            Corps corps = new Corps(name);
            List<string> companyNames = Util.ReadWordsFromFile("wordlists/CompanyNames.txt");
            Random rand = new Random();
            int soldierCounter = 1;

            int brigades = 4;
            int regiments = 4;
            int battalions = faction == "republic" ? 4 : 7;
            int companies = faction == "republic" ? 4 : 2;
            int platoons = faction == "republic" ? 4 : 2;
            int squads = faction == "republic" ? 4 : 7;
            int soldiersPerSquad = faction == "republic" ? 9 : 8;

            // Corps HQ
            Platoon corpsHQ = new Platoon("Corps HQ");
            corps.AddSubdivision(corpsHQ);
            corpsHQ.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Marshal Commander"));
            corpsHQ.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Jedi General"));

            for (int b = 1; b <= brigades; b++)
            {
                Brigade brigade = new Brigade($"{b}{Util.GetOrdinalSuffix(b)} Brigade");
                corps.AddSubdivision(brigade);

                // Brigade HQ
                Platoon brigadeHQ = new Platoon("Brigade HQ");
                brigade.AddSubdivision(brigadeHQ);
                brigadeHQ.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Senior Commander"));
                brigadeHQ.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Jedi General"));

                for (int i = 1; i <= regiments; i++)
                {
                    Regiment regiment = new Regiment($"{i}{Util.GetOrdinalSuffix(i)} Regiment");

                    // Regiment HQ
                    Platoon regHQ = new Platoon("Regiment HQ");
                    regiment.AddSubdivision(regHQ);
                    regHQ.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Regimental Commander"));
                    regHQ.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Jedi Commander"));

                    brigade.AddSubdivision(regiment);

                    for (int j = 1; j <= battalions; j++)
                    {
                        Battalion battalion = new Battalion($"{j}{Util.GetOrdinalSuffix(j)} Battalion");

                        // Battalion HQ
                        Platoon bnHQ = new Platoon("Battalion HQ");
                        battalion.AddSubdivision(bnHQ);
                        bnHQ.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Battalion Commander"));

                        regiment.AddSubdivision(battalion);

                        for (int k = 1; k <= companies; k++)
                        {
                            string companyName = faction == "republic" ? Util.GetUniqueWord(companyNames) + " Company" : $"{k}{Util.GetOrdinalSuffix(k)} Company";
                            Company company = new Company(companyName);

                            // Company HQ
                            Platoon hq = new Platoon("Company HQ");
                            company.AddSubdivision(hq);
                            hq.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Captain"));
                            hq.AddSubdivision(GenerateCommander(rand, faction, ref soldierCounter, "Lieutenant"));

                            battalion.AddSubdivision(company);

                            for (int m = 1; m <= platoons; m++)
                            {
                                Platoon platoon = new Platoon($"{m}{Util.GetOrdinalSuffix(m)} Platoon");
                                company.AddSubdivision(platoon);

                                for (int n = 1; n <= squads; n++)
                                {
                                    Squad squad = new Squad($"{n}{Util.GetOrdinalSuffix(n)} Squad");
                                    platoon.AddSubdivision(squad);

                                    for (int p = 1; p <= soldiersPerSquad; p++)
                                    {
                                        string id = faction == "republic" ? GetCloneIdentifier(rand) : GetDroidIdentifier(soldierCounter++);
                                        string rank, type;

                                        if (faction == "republic")
                                        {
                                            if (p == 1) { rank = "Sergeant"; type = "Squad Leader"; }
                                            else if (p == 2) { rank = "Corporal"; type = "Marksman"; }
                                            else if (p == 3) { rank = "Corporal"; type = "Medic"; }
                                            else if (p == 4) { rank = "Private"; type = "AT Specialist"; }
                                            else if (p == 5) { rank = "Private"; type = "Support Gunner"; }
                                            else { rank = "Private"; type = "Rifleman"; }
                                        }
                                        else
                                        {
                                            if (p == 1) { rank = "Squad Leader"; type = "Tactical Droid"; }
                                            else if (p <= 3) { rank = "B2"; type = "Super Battle Droid"; }
                                            else { rank = "B1"; type = "Battle Droid"; }
                                        }

                                        double skill = Util.Next(50, 100) * Util.NextFloat(0.8f, 1);
                                        double morale = Util.Next(50, 100) * Util.NextFloat(0.75f, 1);
                                        double discipline = Util.Next(50, 100) * Util.NextFloat(0.75f, 1);
                                        string nickname = faction == "republic" ? GenerateNickname(rand) : null;

                                        Soldier s = new Soldier($"{id}", rank, id, type, skill, morale, discipline, nickname);
                                        squad.AddSubdivision(s);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return corps;
        }

        private static string GenerateNickname(Random rand)
        {
            List<string> cloneNicknames = Util.ReadWordsFromFile("wordlists/CloneNicknames.txt");
            return cloneNicknames[rand.Next(cloneNicknames.Count)];
        }

        private static Soldier GenerateCommander(Random rand, string faction, ref int counter, string rank)
        {
            bool isJedi = rank.ToLower().Contains("jedi");
            if (isJedi)
            {
                string name = GetUniqueJediName(rand);
                return new Soldier(name, rank, name, "Jedi", Util.Next(90, 100), 100, 95);
            }
            else
            {
                string id = faction == "republic" ? GetCloneIdentifier(rand) : GetDroidIdentifier(counter++);
                string type = faction == "republic" ? "Clone Officer" : "Tactical Droid";
                return new Soldier($"{id}", rank, id, type, Util.Next(85, 100), 90, 90, GenerateNickname(rand));
            }
        }
    }
}

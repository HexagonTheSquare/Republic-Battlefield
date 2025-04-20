using System;
using System.Collections.Generic;
using static Squad;

namespace CloneArmyPlayground
{
    public class CorpsInspector
    {
        private Corps userCorps;
        private Corps enemyCorps;

        public CorpsInspector(Corps userCorps, Corps enemyCorps)
        {
            this.userCorps = userCorps;
            this.enemyCorps = enemyCorps;
        }

        public void Start()
        {
            Console.WriteLine("\n--- Corps Inspection ---");
            Console.WriteLine("Type 'help' for a list of commands. Type 'exit' to continue to battle.\n");

            while (true)
            {
                Console.Write("> ");
                var input = Console.ReadLine()?.Trim();

                if (input == null || input.ToLower() == "exit") break;

                string lowerInput = input.ToLower();

                switch (lowerInput)
                {
                    case "help":
                        PrintHelp();
                        break;
                    case "view user":
                        InspectUnit(userCorps);
                        break;
                    case "view enemy":
                        InspectUnit(enemyCorps);
                        break;
                    case "tree user":
                        PrintTree(userCorps, 0, 3);
                        break;
                    case "tree enemy":
                        PrintTree(enemyCorps, 0, 3);
                        break;
                    case var str when lowerInput.StartsWith("inspect user "):
                        string userPath = input.Substring("inspect user ".Length);
                        InspectPath(userCorps, userPath);
                        break;
                    case var str when lowerInput.StartsWith("inspect enemy "):
                        string enemyPath = input.Substring("inspect enemy ".Length);
                        InspectPath(enemyCorps, enemyPath);
                        break;
                    default:
                        Console.WriteLine("Unknown command. Type 'help' for command list.");
                        break;
                }
            }
        }

        private void PrintHelp()
        {
            Console.WriteLine("\nAvailable Commands:");
            Console.WriteLine("  view user               - View the detailed stats of your corps");
            Console.WriteLine("  view enemy              - View the detailed stats of enemy corps");
            Console.WriteLine("  tree user               - Show a structured tree of your corps (down to company level)");
            Console.WriteLine("  tree enemy              - Show a structured tree of enemy corps (down to company level)");
            Console.WriteLine("  inspect user [path]     - Inspect any subunit down to individual soldier (e.g. 'Breakaway Company 1st Platoon')");
            Console.WriteLine("  inspect enemy [path]    - Same as above for enemy corps");
            Console.WriteLine("  help                    - Show this help menu");
            Console.WriteLine("  exit                    - Proceed to battle\n");
        }

        private void InspectPath(ArmyUnit root, string path)
        {
            string[] segments = path.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            ArmyUnit current = root;

            for (int i = 0; i < segments.Length; i++)
            {
                ArmyUnit next = null;

                foreach (var sub in current.Subdivisions)
                {
                    if (sub.Name.Equals(string.Join(" ", segments, i, segments.Length - i), StringComparison.OrdinalIgnoreCase))
                    {
                        InspectUnit(sub);
                        return;
                    }
                    if (sub.Name.StartsWith(segments[i], StringComparison.OrdinalIgnoreCase))
                    {
                        next = sub;
                        break;
                    }
                }

                if (next == null)
                {
                    Console.WriteLine($"Could not find subunit '{segments[i]}' under {current.Name}.");
                    return;
                }

                current = next;
            }

            InspectUnit(current);
        }

        private void InspectUnit(ArmyUnit unit)
        {
            Console.WriteLine($"\n--- {unit.Name} ---");
            Console.WriteLine($"Type: {unit.GetType().Name}");
            Console.WriteLine($"Soldiers: {unit.Manpower}");

            if (unit is Soldier soldier)
            {
                Console.WriteLine($"Rank: {soldier.Rank}");
                Console.WriteLine($"Identifier: {soldier.Identifier}");
                Console.WriteLine($"Type: {soldier.Type}");
            }
            else if (unit is Squad)
            {
                foreach (var member in unit.Subdivisions)
                {
                    if (member is Soldier s)
                    {
                        string nickname = string.IsNullOrEmpty(s.Nickname) ? "" : $" \"{s.Nickname}\"";
                        Console.WriteLine($"  - {s.Rank} {s.Name}{nickname} [{s.Type}]");

                    }
                }
            }
            else
            {
                Console.WriteLine($"Order of Battle:");
                foreach (var sub in unit.Subdivisions)
                {
                    DisplaySubunit(sub);
                }
            }

            Console.WriteLine("----------------------\n");
        }

        private void DisplaySubunit(ArmyUnit subunit)
        {
            if (subunit is Soldier soldier)
            {
                Console.WriteLine($" - {soldier.Rank} {soldier.Name}  | {soldier.Type}");
            }
            else
            {
                Console.WriteLine($" - {subunit.Name} | {subunit.Manpower} soldiers");
            }
        }

        private void PrintTree(ArmyUnit root, int indent = 0, int depthLimit = 3)
        {
            string indentStr = new string(' ', indent * 2);
            Console.WriteLine($"{indentStr}- {root.Name} ({root.Manpower} troops)");

            if (indent / 2 >= depthLimit || root is Squad || root is Company)
                return;

            foreach (var sub in root.Subdivisions)
            {
                if (sub is Platoon platoon && platoon.Name.ToLower().Contains("hq"))
                {
                    Console.WriteLine($"{indentStr}  {platoon.Name} ({platoon.Manpower} troops)");
                    foreach (var unit in platoon.Subdivisions)
                    {
                        if (unit is Soldier hqSoldier)
                        {
                            string nickname = string.IsNullOrEmpty(hqSoldier.Nickname) ? "" : $" \"{hqSoldier.Nickname}\"";
                            Console.WriteLine($"{indentStr}    - {hqSoldier.Rank} {hqSoldier.Name}{nickname} [{hqSoldier.Type}]");
                        }
                    }
                }
                else
                {
                    PrintTree(sub, indent + 1, depthLimit);
                }
            }
        }
    }
}

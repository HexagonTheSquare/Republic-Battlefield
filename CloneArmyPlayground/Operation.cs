using System;
using System.Collections.Generic;
using System.Text;

namespace CloneArmyPlayground
{
    public enum BattleStage
    {
        Configuration,
        PreBattle,
        InitialEngagement,
        MainEngagement,
        Climax,
        PostBattle
    }

    public class BattlePhase
    {
        public BattleStage Stage { get; set; }
        public int SubStage { get; set; }

        public BattlePhase(BattleStage stage, int subStage)
        {
            Stage = stage;
            SubStage = subStage;
        }

        public void NextSubStage()
        {
            SubStage++;
        }
    }
    public class Operation
    {

        //Variables
        private bool warIsOver = false;
        private BattlePhase currentPhase;

        public int Intelligence { get; set; }
        public float RepCasualtyRate { get; set; }
        public float SepCasualtyRate { get; set; }
        public float RepMoraleModifier { get; set; }
        public float SepMoraleModifier { get; set; }

        public Operation()
        {
            Intelligence = 0;
            RepCasualtyRate = 0f;
            SepCasualtyRate = 0f;
            RepMoraleModifier = 0f;
            SepMoraleModifier = 0f;
        }

        public int[] RunWar()
        {
            Corps userCorps = null;
            Corps enemyCorps = null;
            currentPhase = new BattlePhase(BattleStage.Configuration, 0);

            PreBattle preBattle = new PreBattle();
            InitialEngagement initialEngagement = new InitialEngagement();
            MainEngagement mainEngagement = new MainEngagement();
            Climax climax = new Climax();
            PostBattle postBattle = new PostBattle();

            while (!warIsOver)
            {
                switch (currentPhase.Stage)
                {
                    case BattleStage.Configuration:
                        Corps[] corps = CorpsConfig.CorpsConfiguration();
                        userCorps = corps[0];
                        enemyCorps = corps[1];

                        // Begin inspection phase
                        var inspector = new CorpsInspector(userCorps, enemyCorps);
                        inspector.Start();

                        currentPhase = new BattlePhase(BattleStage.PreBattle, 0);

                        break;
                    case BattleStage.PreBattle:
                        preBattle.Execute(currentPhase, userCorps, enemyCorps, this);
                        break;
                    case BattleStage.InitialEngagement:
                        initialEngagement.Execute(currentPhase, userCorps, enemyCorps, this);
                        break;
                    case BattleStage.MainEngagement:
                        mainEngagement.Execute(currentPhase, userCorps, enemyCorps, this);
                        break;
                    case BattleStage.Climax:
                        climax.Execute(currentPhase, userCorps, enemyCorps, this);
                        break;
                    case BattleStage.PostBattle:
                        postBattle.Execute(currentPhase, userCorps, enemyCorps, this);

                        warIsOver = CheckIfWarIsOver();
                        if (!warIsOver)
                        {
                            //currentPhase.Stage = BattleStage.PreBattle;
                            //currentPhase.SubStage = 0;
                        }
                        else
                        {
                            return new int[] { userCorps.Manpower, enemyCorps.Manpower };
                        }
                        break;
                }
            }
            return new int[] { userCorps.Manpower, enemyCorps.Manpower };
        }

        private bool CheckIfWarIsOver()
        {
            // TODO
            return (currentPhase.Stage == BattleStage.PostBattle && currentPhase.SubStage == 2);
        }
    }

    public class PreBattle
    {
        public void Execute(BattlePhase currentPhase, Corps userCorps, Corps enemyCorps, Operation operation)
        {
            
            switch (currentPhase.SubStage)
            {
                case 0:
                    // TODO

                    Console.WriteLine("The Nth Battle of [PLANET] has begun, both sides are conducting pre-battle preparations.");

                    // REPUBLIC INTELLIGENCE GATHERING

                    var battalion = userCorps.Subdivisions[Util.Next(0, userCorps.Subdivisions.Count)];
                    var company = battalion.Subdivisions[Util.Next(0, battalion.Subdivisions.Count)];
                    var platoon = company.Subdivisions[Util.Next(0, company.Subdivisions.Count)];
                    Squad activeSquad = (Squad)platoon.Subdivisions[Util.Next(0, platoon.Subdivisions.Count)];

                    Console.WriteLine("{2} {0}, {1} is assigned to scout the enemy forces, they have {3} troopers.", activeSquad.Name, battalion.Name, company.Name.Split(' ')[0], activeSquad.Manpower); //Placeholder

                    switch (Util.NextFloat(0, 1))
                    {
                        case var num when num * activeSquad.ArmySkill >= 75:

                            //Very good outcome

                            operation.Intelligence += 2;

                            Console.WriteLine("The mission goes very well with no casualties.");
                            break;
                        case var num when num * activeSquad.ArmySkill >= 50 && num * activeSquad.ArmySkill < 75:

                            //Good outcome

                            operation.Intelligence += 1;

                            Console.WriteLine("The mission goes well with no casualties.");
                            break;
                        case var num when num * activeSquad.ArmySkill >= 25 && num * activeSquad.ArmySkill < 50:

                            //Bad outcome


                            var casualties = Util.Next(0, Math.Max(0, ((activeSquad.Manpower / 2) * (int)(100 - activeSquad.ArmySkill))/100));
                            operation.Intelligence -= 1;
                            //activeSquad.UpdateManpower(activeSquad.Manpower - casualties);

                            Console.WriteLine("The mission goes poorly and {0} receive {1} casualt{2}.", activeSquad.Name, casualties, Util.GetPluralSuffix(casualties));
                            break;
                        case var num when num * activeSquad.ArmySkill >= 0 && num * activeSquad.ArmySkill < 25:

                            //Very bad outcome


                            var casualtiesMajor = Util.Next(0, Math.Max(0, ((activeSquad.Manpower) * (int)(100 - activeSquad.ArmySkill)) / 100));
                            operation.Intelligence -= 2;
                            //activeSquad.UpdateManpower(activeSquad.Manpower - casualtiesMajor);

                            Console.WriteLine("The mission goes very poorly and {0} receive {1} casualt{2}.", activeSquad.Name, casualtiesMajor, Util.GetPluralSuffix(casualtiesMajor));
                            break;
                    }

                    currentPhase.NextSubStage();
                    break;
                case 1:
                    // Planning and strategy
                    
                    // TODO: decide on whether to use numerical probabilities based on ArmySkill PURELY or if to combine with actual strategies in order to set what the casualty rates would be

                    currentPhase.NextSubStage();
                    break;
                case 2:
                    // Deployment

                    // TODO: add anything at all here

                    // Apply any pre-battle deployment effects on both armies
                    currentPhase.NextSubStage();
                    break;
                default:
                    currentPhase.Stage = BattleStage.InitialEngagement;
                    currentPhase.SubStage = 0;
                    break;
            }
        }
    }

    public class InitialEngagement
    {
        public void Execute(BattlePhase currentPhase, Corps userCorps, Corps enemyCorps, Operation operation)
        {
            
            // Execute the 3 substages
            switch (currentPhase.SubStage)
            {
                case 0:
                    // Skirmishing
                    // Apply casualties based on skill and casualty rate

                    Console.WriteLine("Following the pre-battle stages, the first clash between the two forces happen.");

                    var uBattalion = userCorps.Subdivisions[Util.Next(0, userCorps.Subdivisions.Count)];
                    var uCompany = uBattalion.Subdivisions[Util.Next(0, uBattalion.Subdivisions.Count)];
                    Platoon engagedUserPlatoon = (Platoon)uCompany.Subdivisions[Util.Next(0, uCompany.Subdivisions.Count)];

                    var eBattalion = enemyCorps.Subdivisions[Util.Next(0, enemyCorps.Subdivisions.Count)];
                    var eCompany = eBattalion.Subdivisions[Util.Next(0, eBattalion.Subdivisions.Count)];
                    Platoon engagedEnemyPlatoon = (Platoon)eCompany.Subdivisions[Util.Next(0, eCompany.Subdivisions.Count)];

                    Console.WriteLine("{0} in {1} of the {2} engage in a short skirmish against {3} in {4} of the {5}. It's {6} troopers against {7} droids.", engagedUserPlatoon.Name, uCompany.Name, uBattalion.Name, engagedEnemyPlatoon.Name, eCompany.Name, eBattalion.Name, engagedUserPlatoon.Manpower, engagedEnemyPlatoon.Manpower);
                    int totalUCasualties = 0;
                    int uCasualties = 0;

                    int totalECasualties = 0;
                    int eCasualties = 0;
                    float result = Util.NextFloat(0, 1);
                    Console.WriteLine(result);
                    Console.WriteLine(result * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill)) * 2);
                    switch (result)
                    {
                        case var num when num * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill))*2 >= 75:

                            //Very good outcome

                            foreach(Squad squad in engagedUserPlatoon.Subdivisions)
                            {
                                uCasualties = Util.Next(0, Math.Max(1, ((squad.Manpower / 4) * (int)(100 - squad.ArmySkill)) / 100));
                                totalUCasualties += uCasualties;

                                //squad.UpdateManpower(squad.Manpower - uCasualties);
                            }

                            foreach(Squad squad in engagedEnemyPlatoon.Subdivisions)
                            {
                                eCasualties = Util.Next(0, Math.Max(1, ((squad.Manpower) * (int)(100 - squad.ArmySkill)) / 100));
                                totalECasualties += eCasualties;

                                //squad.UpdateManpower(squad.Manpower - eCasualties);
                            }

                            Console.WriteLine("Your platoon receives {0} casualt{1} and are now at {2} troopers. The enemy platoon receives {3} casualt{4} and are now at {5} droids.", totalUCasualties, Util.GetPluralSuffix(totalUCasualties), engagedUserPlatoon.Manpower, totalECasualties, Util.GetPluralSuffix(totalECasualties), engagedEnemyPlatoon.Manpower);
                            break;
                        case var num when num * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill)) * 2 >= 50 && num * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill)) * 2 < 75:

                            //Good outcome

                            foreach (Squad squad in engagedUserPlatoon.Subdivisions)
                            {
                                uCasualties = Util.Next(0, Math.Max(1, ((squad.Manpower / 3) * (int)(100 - squad.ArmySkill)) / 100));
                                totalUCasualties += uCasualties;

                                //squad.UpdateManpower(squad.Manpower - uCasualties);
                            }

                            foreach (Squad squad in engagedEnemyPlatoon.Subdivisions)
                            {
                                eCasualties = Util.Next(0, Math.Max(1, ((squad.Manpower/2) * (int)(100 - squad.ArmySkill)) / 100));
                                totalECasualties += eCasualties;

                                //squad.UpdateManpower(squad.Manpower - eCasualties);
                            }

                            Console.WriteLine("Your platoon receives {0} casualt{1} and are now at {2} troopers. The enemy platoon receives {3} casualt{4} and are now at {5} droids.", totalUCasualties, Util.GetPluralSuffix(totalUCasualties), engagedUserPlatoon.Manpower, totalECasualties, Util.GetPluralSuffix(totalECasualties), engagedEnemyPlatoon.Manpower);
                            break;
                        case var num when num * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill)) * 2 >= 25 && num * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill)) * 2 < 50:

                            //Bad outcome


                            foreach (Squad squad in engagedUserPlatoon.Subdivisions)
                            {
                                uCasualties = Util.Next(0, Math.Max(1, ((squad.Manpower / 2) * (int)(100 - squad.ArmySkill)) / 100));
                                totalUCasualties += uCasualties;

                                //squad.UpdateManpower(squad.Manpower - uCasualties);
                            }

                            foreach (Squad squad in engagedEnemyPlatoon.Subdivisions)
                            {
                                eCasualties = Util.Next(0, Math.Max(1, ((squad.Manpower/3) * (int)(100 - squad.ArmySkill)) / 100));
                                totalECasualties += eCasualties;

                                //squad.UpdateManpower(squad.Manpower - eCasualties);
                            }

                            Console.WriteLine("Your platoon receives {0} casualt{1} and are now at {2} troopers. The enemy platoon receives {3} casualt{4} and are now at {5} droids.", totalUCasualties, Util.GetPluralSuffix(totalUCasualties), engagedUserPlatoon.Manpower, totalECasualties, Util.GetPluralSuffix(totalECasualties), engagedEnemyPlatoon.Manpower);
                            break;
                        case var num when num * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill)) * 2 >= 0 && num * Math.Max(1, (engagedUserPlatoon.ArmySkill - engagedEnemyPlatoon.ArmySkill)) * 2 < 25:

                            //Very bad outcome


                            foreach (Squad squad in engagedUserPlatoon.Subdivisions)
                            {
                                uCasualties = Util.Next(0, Math.Max(0, ((squad.Manpower) * (int)(100 - squad.ArmySkill)) / 100));
                                totalUCasualties += uCasualties;

                                //squad.UpdateManpower(squad.Manpower - uCasualties);
                            }

                            foreach (Squad squad in engagedEnemyPlatoon.Subdivisions)
                            {
                                eCasualties = Util.Next(0, Math.Max(0, ((squad.Manpower / 4) * (int)(100 - squad.ArmySkill)) / 100));
                                totalECasualties += eCasualties;

                                //squad.UpdateManpower(squad.Manpower - eCasualties);
                            }

                            Console.WriteLine("Your platoon receives {0} casualt{1} and are now at {2} troopers. The enemy platoon receives {3} casualt{4} and are now at {5} droids.", totalUCasualties, Util.GetPluralSuffix(totalUCasualties), engagedUserPlatoon.Manpower, totalECasualties, Util.GetPluralSuffix(totalECasualties), engagedEnemyPlatoon.Manpower);
                            break;
                    }

                    currentPhase.NextSubStage();
                    break;
                case 1:
                    // Artillery bombardment
                    currentPhase.NextSubStage();
                    break;
                case 2:
                    // Air or space superiority 
                    currentPhase.NextSubStage();
                    break;
                default:
                    // Move to the next stage
                    currentPhase.Stage = BattleStage.MainEngagement;
                    currentPhase.SubStage = 0;
                    break;
            }
        }
    }

    public class MainEngagement
    {
        public void Execute(BattlePhase currentPhase, Corps userCorps, Corps enemyCorps, Operation operation)
        {
            Console.WriteLine("Main Engagement");
            Console.WriteLine(currentPhase.SubStage);
            switch (currentPhase.SubStage)
            {
                case 0:
                    // Maneuver
                    // Apply casualties and morale changes based on maneuver success
                    currentPhase.NextSubStage();
                    break;
                case 1:
                    // Flanking and envelopment
                    // Apply casualties and morale changes based on flanking/envelopment success
                    currentPhase.NextSubStage();
                    break;
                case 2:
                    // Breakthrough
                    // Apply casualties and morale changes based on breakthrough success
                    currentPhase.NextSubStage();
                    break;
                case 3:
                    // Reserve deployment
                    // Apply reinforcements or reserve effects to both armies
                    currentPhase.NextSubStage();
                    break;
                default:
                    currentPhase.Stage = BattleStage.Climax;
                    currentPhase.SubStage = 0;
                    break;
            }
        }
    }

    public class Climax
    {
        public void Execute(BattlePhase currentPhase, Corps userCorps, Corps enemyCorps, Operation operation)
        {
            Console.WriteLine("Climax");
            Console.WriteLine(currentPhase.SubStage);
            // Execute the 2 substages
            switch (currentPhase.SubStage)
            {
                case 0:
                    // Decisive engagement
                    // Apply casualties and morale changes based on decisive engagement success
                    int repDecisiveCasualties = (int)(userCorps.Manpower * operation.RepCasualtyRate * (1 - userCorps.ArmySkill / 100f));
                    int sepDecisiveCasualties = (int)(enemyCorps.Manpower * operation.SepCasualtyRate * (1 - userCorps.ArmySkill / 100f));

                    //userCorps.UpdateManpower(userCorps.Manpower - repDecisiveCasualties);
                    //sepArmy.SetArmyUnitCurrentSize(enemyCorps.Manpower - sepDecisiveCasualties);

                    // Adjust morale based on the outcome of the decisive engagement
                    //repArmy.SetArmyUnitMorale((int)(userCorps.ArmyMorale * repMoraleModifier));
                    //sepArmy.SetArmyUnitMorale((int)(enemyCorps.ArmyMorale * sepMoraleModifier));
                    currentPhase.NextSubStage();
                    break;
                case 1:
                    // Morale collapse
                    // Determine if either army's morale has collapsed
                    bool repMoraleCollapse = userCorps.ArmyMorale < 20; // example threshold for morale collapse
                    bool sepMoraleCollapse = enemyCorps.ArmyMorale < 20; // example threshold for morale collapse

                    if (repMoraleCollapse || sepMoraleCollapse)
                    {
                        // Apply additional casualties and effects based on morale collapse
                    }
                    currentPhase.NextSubStage();
                    break;
                default:
                    // Move to the next stage
                    currentPhase.Stage = BattleStage.PostBattle;
                    currentPhase.SubStage = 0;
                    break;
            }
        }
    }

    public class PostBattle
    {
        public void Execute(BattlePhase currentPhase, Corps userCorps, Corps enemyCorps, Operation operation)
        {
            Console.WriteLine("Post-Battle");
            Console.WriteLine(currentPhase.SubStage);
            switch (currentPhase.SubStage)
            {
                case 0:
                    // Pursuit
                    // Apply casualties and effects based on pursuit success
                    int repPursuitCasualties = (int)(userCorps.Manpower * operation.RepCasualtyRate * (1 - userCorps.ArmySkill / 100f));
                    int sepPursuitCasualties = (int)(enemyCorps.Manpower * operation.SepCasualtyRate * (1 - enemyCorps.ArmySkill / 100f));

                    //repArmy.SetArmyUnitCurrentSize(userCorps.Manpower - repPursuitCasualties);
                    //sepArmy.SetArmyUnitCurrentSize(enemyCorps.Manpower - sepPursuitCasualties);
                    currentPhase.NextSubStage();
                    break;
                case 1:
                    // Reorganization
                    currentPhase.NextSubStage();
                    break;
                default:
                    break;
            }
        }
    }
}
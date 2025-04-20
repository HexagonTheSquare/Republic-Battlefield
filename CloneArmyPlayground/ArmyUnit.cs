using System;
using System.Collections.Generic;
using System.Linq;

public abstract class ArmyUnit
{
    public string Name { get; set; }
    public List<ArmyUnit> Subdivisions { get; set; }

    public virtual int Manpower => Subdivisions.Sum(unit => unit.Manpower);
    public virtual double ArmySkill => Subdivisions.Count == 0 ? 0 : Subdivisions.Average(unit => unit.ArmySkill);
    public virtual double ArmyMorale => Subdivisions.Count == 0 ? 0 : Subdivisions.Average(unit => unit.ArmyMorale);
    public virtual double ArmyDiscipline => Subdivisions.Count == 0 ? 0 : Subdivisions.Average(unit => unit.ArmyDiscipline);

    public ArmyUnit(string name)
    {
        Name = name;
        Subdivisions = new List<ArmyUnit>();
    }

    public void AddSubdivision(ArmyUnit unit) => Subdivisions.Add(unit);
    public void RemoveSubdivision(ArmyUnit unit) => Subdivisions.Remove(unit);
}

public class Corps : ArmyUnit { public Corps(string name) : base(name) { } }
public class Brigade : ArmyUnit { public Brigade(string name) : base(name) { } }
public class Regiment : ArmyUnit { public Regiment(string name) : base(name) { } }
public class Vanguard : ArmyUnit { public Vanguard(string name) : base(name) { } }
public class Battalion : ArmyUnit { public Battalion(string name) : base(name) { } }
public class Company : ArmyUnit { public Company(string name) : base(name) { } }
public class Platoon : ArmyUnit { public Platoon(string name) : base(name) { } }
public class Squad : ArmyUnit { public Squad(string name) : base(name) { } }

public class Soldier : ArmyUnit
{
    public string Rank { get; set; }
    public string Identifier { get; set; }
    public string Type { get; set; }
    public string Nickname { get; set; }
    public double Skill { get; set; }
    public double Morale { get; set; }
    public double Discipline { get; set; }

    public override int Manpower => 1;
    public override double ArmySkill => Skill;
    public override double ArmyMorale => Morale;
    public override double ArmyDiscipline => Discipline;

    public Soldier(string name, string rank, string identifier, string type, double skill, double morale, double discipline, string nickname = null)
        : base(name)
    {
        Rank = rank;
        Identifier = identifier;
        Type = type;
        Skill = skill;
        Morale = morale;
        Discipline = discipline;
        Nickname = nickname;
    }
}

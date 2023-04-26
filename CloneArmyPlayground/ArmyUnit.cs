using System;
using System.Collections.Generic;
using System.Linq;

public abstract class ArmyUnit
{
    public string Name { get; set; }
    public List<ArmyUnit> Subdivisions { get; set; }

    public virtual int Manpower
    {
        get { return Subdivisions.Sum(unit => unit.Manpower); }
    }

    public virtual double ArmySkill
    {
        get { return Subdivisions.Count == 0 ? 0 : Subdivisions.Average(unit => unit.ArmySkill); }
    }

    public virtual double ArmyMorale
    {
        get { return Subdivisions.Count == 0 ? 0 : Subdivisions.Average(unit => unit.ArmyMorale); }
    }

    public virtual double ArmyDiscipline
    {
        get { return Subdivisions.Count == 0 ? 0 : Subdivisions.Average(unit => unit.ArmyDiscipline); }
    }

    public ArmyUnit(string name)
    {
        Name = name;
        Subdivisions = new List<ArmyUnit>();
    }

    public void AddSubdivision(ArmyUnit unit)
    {
        Subdivisions.Add(unit);
    }

    public void RemoveSubdivision(ArmyUnit unit)
    {
        Subdivisions.Remove(unit);
    }
}


public class Corps : ArmyUnit
{
    public Corps(string name) : base(name) { }
}

public class Battalion : ArmyUnit
{
    public Battalion(string name) : base(name) { }
}

public class Company : ArmyUnit
{
    public Company(string name) : base(name) { }
}

public class Platoon : ArmyUnit
{
    public Platoon(string name) : base(name) { }
}

public class Squad : ArmyUnit
{
    private int _manpower;
    private double _armySkill;
    private double _armyMorale;
    private double _armyDiscipline;

    public override int Manpower
    {
        get { return _manpower; }
    }

    public override double ArmySkill
    {
        get { return _armySkill; }
    }

    public override double ArmyMorale
    {
        get { return _armyMorale; }
    }

    public override double ArmyDiscipline
    {
        get { return _armyDiscipline; }
    }

    public Squad(string name, int manpower, double skill, double morale, double discipline) : base(name)
    {
        _manpower = manpower;
        _armySkill = skill;
        _armyMorale = morale;
        _armyDiscipline = discipline;
    }

    public void UpdateManpower(int newManpower)
    {
        _manpower = newManpower;
    }
}




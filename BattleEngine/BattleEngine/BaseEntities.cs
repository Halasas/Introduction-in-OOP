using System;
using System.Collections.Generic;
using LAS;
using BattleEngine.Effect;
using BattleEngine.Spell;

namespace BattleEngine.BaseEntities
{
    public enum UnitType
    {
        Human,
        Mutant,
        Vehicle,
        Robot
    }
    public class ChangeableUnit
    {
        public virtual UnitType Type { get;  set; }
        public virtual string Name { get; set; }
        public int HitPoint { get; set; }
        public int Attack { get; set; }
        public int Defence { get; set; }

        public List<IEffect> BaseEffects = new List<IEffect>();
        public List<IAbility> BaseSpells = new List<IAbility>();
        public Range Damage { get; set; }
        public double Initiative { get; set; }
        public int Resistance { get; set; }
        public bool MeleeDamager { get; set; }
        public bool ImmunityToMelee { get; set; }
        public bool ImmunityToRanged { get; set; }
        public ChangeableUnit() { }

        public ChangeableUnit(Unit unit)
        {
            this.Type = unit.Type;
            this.Name = unit.Name;
            this.HitPoint = unit.HitPoint;
            this.Attack = unit.Attack;
            this.Defence = unit.Defence;
            this.Damage = new Range(unit.Damage);
            this.Initiative = unit.Initiative;
            this.Resistance = unit.Resistance;
            this.BaseEffects = unit.BaseEffects;
            this.BaseSpells = unit.BaseSpells;
            this.MeleeDamager = unit.MeleeDamager;
            this.ImmunityToMelee = unit.ImmunityToMelee;
            this.ImmunityToRanged = unit.ImmunityToRanged;
        }
    }
    public class Unit
    {
        public virtual UnitType Type { get; private set; }
        public virtual string Name { get; private set; }
        public int HitPoint { get; private set; }
        public int Attack { get; private set; }
        public int Defence { get; private set; }
        private List<IEffect> baseEffects;
        private List<IAbility> baseSpells;
        private Range damage;
        public Range Damage { get => new Range(damage); private set => damage = value; }
        public List<IEffect> BaseEffects { get => LAS.Functions.cloneEffects(baseEffects); }
        public List<IAbility> BaseSpells { get => LAS.Functions.cloneSpells(baseSpells); }
        public double Initiative { get; private set; }
        public int Resistance { get; private set; }
        public bool MeleeDamager { get; private set; }
        public bool ImmunityToMelee { get; private set; }
        public bool ImmunityToRanged { get; private set; }

        public string toStr()
        {
            return Type.ToString() +
                " " + Name +
                " HP: " + HitPoint.ToString() +
                " A: " + Attack.ToString() +
                " D: " + Defence.ToString() +
                " Dam: " + damage.From.ToString() + "-" + damage.To.ToString() +
                "" +
                "\nInitiative: " + Initiative.ToString() +
                " Resistance " + Resistance.ToString() +
                " \nMelee - " + MeleeDamager.ToString() +
                " \nImmunity To Melee - " + ImmunityToMelee.ToString() +
                " \nImmunity To Ranged - " + ImmunityToRanged.ToString();
        }

        public Unit(
        UnitType Type, string Name,
        int HitPoint, int Attack,
        int Defence, Range Damage,
        double Initiative, int NumOfResists,
        List<IEffect> effects, List<IAbility> spells, 
        bool MeleeDamager,
        bool ImmunityToMelee, bool ImmunityToRanged)
        {
            this.Type = Type;
            this.Name = Name;
            this.HitPoint = HitPoint;
            this.Attack = Attack;
            this.Defence = Defence;
            this.Damage = new Range(Damage);
            this.Initiative = Initiative;
            this.Resistance = NumOfResists;
            this.MeleeDamager = MeleeDamager;
            this.baseEffects = LAS.Functions.cloneEffects(effects);
            this.baseSpells = LAS.Functions.cloneSpells(spells);
            this.ImmunityToMelee = ImmunityToMelee;
            this.ImmunityToRanged = ImmunityToRanged;
        }
        public Unit(ChangeableUnit chunit) : this(
                                    chunit.Type,
                                    chunit.Name,
                                    chunit.HitPoint,
                                    chunit.Attack,
                                    chunit.Defence,
                                    chunit.Damage,
                                    chunit.Initiative,
                                    chunit.Resistance,
                                    chunit.BaseEffects,
                                    chunit.BaseSpells,
                                    chunit.MeleeDamager,
                                    chunit.ImmunityToMelee,
                                    chunit.ImmunityToRanged)
        { }
        public Unit() { }
        public Unit(Unit unit) : this(
                                    unit.Type,
                                    unit.Name,
                                    unit.HitPoint,
                                    unit.Attack,
                                    unit.Defence,
                                    unit.Damage,
                                    unit.Initiative,
                                    unit.Resistance,
                                    unit.baseEffects,
                                    unit.baseSpells,
                                    unit.MeleeDamager,
                                    unit.ImmunityToMelee,
                                    unit.ImmunityToRanged)
        { }
    }

    public class UnitsStack
    {
        public Unit UnitOrigin { get; protected set; }
        public int Count { get; protected set; }
        public UnitsStack(Unit unit)
        {
            if (unit == null)
                throw new Exception("You can't create a stack with incorrect values");
            this.UnitOrigin = new Unit(unit);
            Count = 1;
        }
        public UnitsStack(Unit unit, int count)
        {
            if (unit == null || count <= 0)
                throw new Exception("You can't create a stack with incorrect values");
            this.UnitOrigin = new Unit(unit);
            Count = count;
        }
        public UnitsStack(UnitsStack units) : this(units.UnitOrigin, units.Count) { }
        public bool isAlive() { return Count > 0; }
    }

    public class GenericArmy<T> 
    {
        protected int maxNumOfStacks;
        protected List<T> stacks = new List<T>();
        public List<T> Stacks { get => new List<T>(stacks); }
        public int StackCount { get { return stacks.Count; } }
        public void AddStack(T stack)
        {
            if (StackCount < maxNumOfStacks)
            {
                stacks.Add(stack);
            }
            else
                throw new Exception("Army is full");
        }
        public void RemoveStack(T stack)
        {
            stacks.Remove(stack);
        }
        public void RemoveStack(int ind)
        {
            stacks.RemoveAt(ind);
        }
    }
    
    public class Army : GenericArmy<UnitsStack>
    {
        public Army() : base() { maxNumOfStacks = 6; }
    }
}


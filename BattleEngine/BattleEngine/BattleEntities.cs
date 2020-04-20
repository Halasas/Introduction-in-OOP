using BattleEngine.BaseEntities;
using System.Collections.Generic;
using System;
using BattleEngine.Effect;
using BattleEngine.Spell;
namespace BattleEngine.BattleEntities
{
    public class BattleUnitsStack : UnitsStack
    {
        public BattleArmy Army { get; private set; }
        private int curTime = 0;
        private int curHitPoint;
        public int HitPoint { get => curHitPoint; }
        private int numberOfBody;
        public Unit CurrentUnit { get { return ApplyEffects(curTime); } }

        // управление параметрами происходит с помощью эффектов 
        public void UpdateTime(int Time)
        {
            curTime = Time;
        }
        private List<IEffect> effects;
        public List<IEffect> GetEffects() { return LAS.Functions.cloneEffects(effects); }
        public void AddEffect(IEffect effect) { effects.Add(effect); }
        public void RemoveEffect(IEffect effect) { effects.Remove(effect); }
        public void RemoveEffect(int ind) { effects.RemoveAt(ind); }
        public void RemoveAllEffects() { effects.Clear(); }
        private Unit ApplyEffects(int Time)
        {
            ChangeableUnit chUnit = new ChangeableUnit(UnitOrigin);
            List<IEffect> efs = new List<IEffect>(effects);
            foreach (IEffect effect in efs)
            {
                effect.Update(Time);
                if (!effect.IsActive(Time))
                    effects.Remove(effect);
                else
                    chUnit = effect.Apply(chUnit, Time);
            }
            Unit curUnit = new Unit(chUnit);
            return curUnit;

        }
        private List<IAbility> spells;
        public List<IAbility> GetSpells() { return LAS.Functions.cloneSpells(spells); }
        public void AddSpells(IAbility spell) { spells.Add(spell); }
        public void RemoveSpells(IAbility spell) { spells.Remove(spell); }
        public void RemoveSpells(int ind) { spells.RemoveAt(ind); }
        public void RemoveAllSpells() { spells.Clear(); }

        //получение урона
        public void ApplyDamage(int Damage)
        {
            int curMaxHP = CurrentUnit.HitPoint;
            if(Damage < curHitPoint)
            {
                curHitPoint -= Damage;
                return;
            }
            Damage -= curHitPoint;
            curHitPoint = 0;
            Count--;

            Count -= Damage / curMaxHP;
            if (Count <= 0)
            {
                Count = 0;
                return;
            }
            curHitPoint = curMaxHP - (Damage % curMaxHP);
        }
        //лечение и оживление
        public void ApplyHeal(int Heal, bool revive)
        {
            int curMaxHP = CurrentUnit.HitPoint;
            if(revive)
            {
                int x = Heal / curMaxHP;
                Heal -= x * curMaxHP;
                if (curHitPoint + Heal > curMaxHP)
                {
                    x++;
                    curHitPoint = curHitPoint + Heal - curMaxHP;
                }
                else
                    curHitPoint = curHitPoint + Heal;
                if (x > numberOfBody)
                {
                    Count = numberOfBody;
                    curHitPoint = curMaxHP;
                }
                else
                    Count += numberOfBody;
            }
            else
                curHitPoint = Math.Min(curHitPoint + Heal, curMaxHP);
        }

        //конструкторы
        public BattleUnitsStack(Unit unit, int count, BattleArmy army) : base(unit, count)
        {
            Army = army;
            curHitPoint = unit.HitPoint;
            Count = count;
            numberOfBody = count;
            effects = unit.BaseEffects;
            spells = unit.BaseSpells;
        }
        public BattleUnitsStack(Unit unit, BattleArmy army) : this(unit, 1, army) {}
        public BattleUnitsStack(UnitsStack unitStack, BattleArmy army) :this(unitStack.UnitOrigin, unitStack.Count, army) { }
    }
    public class BattleArmy : GenericArmy<BattleUnitsStack>
    {
        public bool isAlive() { return (StackCount > 0); }
        public bool Surrended { get; set; }
        public void UpdateTime(int Time)
        {
            foreach (BattleUnitsStack stack in stacks)
                stack.UpdateTime(Time);
        }
        public BattleArmy(Army army)
        {
            maxNumOfStacks = 9;
            foreach (var stack in army.Stacks)
            {
                AddStack(new BattleUnitsStack(stack, this));
            }
        }
    }
}

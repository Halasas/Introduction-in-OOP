using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleEngine.BaseEntities;
using BattleEngine.BattleEntities;
using BattleEngine.Effect;

namespace BattleEngine.Spell
{
    public interface IAbility
    {
        void Apply(BattleUnitsStack caster, BattleUnitsStack[] targets, int Time);
        IAbility clone();
        string ToStr();
    }

    public class BlitzkriegAbility : IAbility
    {
        public void Apply(BattleUnitsStack caster, BattleUnitsStack[] targets, int Time)
        {
            foreach (var t in targets)
            { 
                t.AddEffect(new ReduceInitiative(Time, 2, 0.5f * caster.Count));
                t.AddEffect(new ReduceResistance(Time, 2, caster.Count * 1));
            }
        }
        public IAbility clone()
        {
            return new BlitzkriegAbility();
        }

        public string ToStr()
        {
            return "Blitzkrieg - makes targets slower(I -0.5) and scared(R -1) on 2 rounds \nUse format \"num_of_ability target1 target2 ... targetn\"";
        }
    }

    public class AT_Missiles : IAbility
    {
        public void Apply(BattleUnitsStack caster, BattleUnitsStack[] targets, int Time)
        {
            foreach (var t in targets)
            {
                if (targets.Length > 3)
                    throw new Exception("Invalid number of targets ");
                if (t.UnitOrigin.Type == UnitType.Vehicle)
                { 
                    t.ApplyDamage(caster.Count * 1000);
                    t.AddEffect(new ReduceInitiative(Time, 1, 0.5f));
                }
                else
                    t.ApplyDamage(100);
            }
        }

        public IAbility clone()
        {
            return new AT_Missiles();
        }

        public string ToStr()
        {
            return "Special Anti-Tank missiles only 3 missles at time \nUse format \"num_of_ability target1 target2 target3\"";
        }
    }

    public class DropUnit : IAbility
    {
        Unit unit;
        int count;
        public void Apply(BattleUnitsStack caster, BattleUnitsStack[] targets, int Time)
        {
            try
            {
                caster.Army.AddStack(new BattleUnitsStack(unit, count * caster.Count, caster.Army));
            } catch (Exception e)
            {
                Console.WriteLine("You can't have more then 9 stacks");
            }
        }

        public IAbility clone()
        {
            return new DropUnit(unit, count);
        }

        public DropUnit(Unit unit, int count) { this.unit = unit; this.count = count; }

        public string ToStr()
        {
            return "Drop "+ count.ToString() + " " + unit.Name + "s on field\nUse format \"num_of_ability\"";
        }
    }
}

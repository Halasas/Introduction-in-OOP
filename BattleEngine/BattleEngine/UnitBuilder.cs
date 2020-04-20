using BattleEngine.BaseEntities;
using BattleEngine.Effect;
using BattleEngine.Spell;
using LAS;

namespace BattleEngine.Units
{
    public class UnitBuilder
    {
        private ChangeableUnit unit = new ChangeableUnit();
        public UnitBuilder SetRequiredParameters(
            UnitType type,
            string Name,
            int HitPoint,
            int Attack,
            int Defence,
            Range Damage,
            float Initiative)
        {
            unit.Type = type;
            unit.Name = Name;
            unit.HitPoint = HitPoint;
            unit.Attack = Attack;
            unit.Defence = Defence;
            unit.Damage = Damage;
            unit.Initiative = Initiative;
            unit.Resistance = 1;
            unit.MeleeDamager = false;
            unit.ImmunityToMelee = false;
            unit.ImmunityToRanged = false;
            return this;
        }
        public UnitBuilder SetResistance(int value) { unit.Resistance = value; return this; }
        public UnitBuilder SetMelee() { unit.MeleeDamager = true; return this; }
        public UnitBuilder SetMeleeImmunity() { unit.ImmunityToMelee = true; return this; }
        public UnitBuilder SetRangeImmunity() { unit.ImmunityToRanged = true; return this; }
        public UnitBuilder AddEffect(IEffect e) { unit.BaseEffects.Add(e); return this; }
        public UnitBuilder AddAbility(IAbility s) { unit.BaseSpells.Add(s); return this; }
        public Unit GetUnit() { return new Unit(unit); }
        public void Restart() { unit = new ChangeableUnit(); }
    }

}

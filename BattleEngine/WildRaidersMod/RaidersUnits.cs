using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleEngine.BaseEntities;
using BattleEngine.Units;

namespace WildRaidersMod.Units
{
    public class Savage : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Mutant, "Savage", 150, 25, 15, new LAS.Range(150, 200), 1.0f);
            ub.SetMelee();
            return ub.GetUnit();
        }
        public Savage() : base(getUnit()) { }
    }
    public class Raider : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Human, "Raider", 75, 20, 20, new LAS.Range(100, 150), 1.2f);
            return ub.GetUnit();
        }
        public Raider() : base(getUnit()) { }
    }
    public class FatMan : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Mutant, "FatMan", 500, 25, 25, new LAS.Range(450, 600), 0.8f);
            return ub.GetUnit();
        }
        public FatMan() : base(getUnit()) { }
    }
}
 
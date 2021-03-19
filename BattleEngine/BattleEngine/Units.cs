using System.Collections.Generic;
using BattleEngine.BaseEntities;
using BattleEngine.Effect;
using BattleEngine.Spell;
using LAS;
using System.Reflection;
using System.IO;
using System;

namespace BattleEngine.Units
{
    public static class UnitsLibrary
    {
        private static bool isLoaded = false;
        private static List<Unit> Units; 
        public static List<Unit> GetUnits() 
        {
            if (isLoaded)
                return Units;
            isLoaded = true;
            return Units = LoadAllUnits();
        }
        private static List<Unit> LoadAllUnits()
        {
            List<Unit> units = new List<Unit>(GetUnitsFrom(GetTypesFromThis()));
            units.AddRange(GetUnitsFrom(GetTypesFromMods()));
            return units;
        }
        private static Unit[] GetUnitsFrom(Type[] types)
        {
            List<Unit> units = new List<Unit>();
            foreach (var type in types)
                if (type.IsSubclassOf(typeof(Unit)) && ! type.IsAbstract) 
                    units.Add((Unit)Activator.CreateInstance(type));
            return units.ToArray();
        }
        private static Type[] GetTypesFromThis()
        {
            return Assembly.GetExecutingAssembly().GetTypes();
        }
        private static Type[] GetTypesFromMods()
        {
            List<Type> types = new List<Type>();
            if(Directory.Exists("Mods\\")) 
            { 
                foreach(var path in Directory.GetFiles("Mods\\"))
                {
                    Assembly asm = Assembly.LoadFrom(path);
                    types.AddRange(asm.GetTypes());
                }
            }
            return types.ToArray();
        }
    }

    public class Soldier : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Human, "Soldier", 100, 20, 20, new LAS.Range(75, 150), 1f);
            return ub.GetUnit();
        }
        public Soldier() : base(getUnit()) { }
    }

    public class AT_Drone : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Robot, "AT Drone \"Wasp\"", 300, 20, 20, new LAS.Range(75, 150), 1.2f).AddAbility(new AT_Missiles());
            return ub.GetUnit();
        }
        public AT_Drone() : base(getUnit()) { }
    }

    public class Ranger : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Human, "Ranger", 75, 25, 25, new LAS.Range(200, 300), 1.2f)
                .SetMeleeImmunity();
            return ub.GetUnit();
        }
        public Ranger() : base(getUnit()) { }
    }

    public class HeavyTankBizon : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Vehicle, "Heavy Tank \"Bizon\"", 2000, 20, 20, new LAS.Range(300, 500), 0.75f)
                .AddEffect(new EndlessResistance(0, 100))
                .AddAbility(new BlitzkriegAbility());
            return ub.GetUnit();
        }
        public HeavyTankBizon() : base(getUnit()) { }
    }
    public class MotherBase : Unit
    {
        private static Unit getUnit()
        {
            UnitBuilder ub = new UnitBuilder();
            ub.SetRequiredParameters(UnitType.Vehicle, "DropPod \"Mother\"", 1000, 5, 5, new LAS.Range(100, 150), 1.2f)
                .SetMeleeImmunity()
                .AddAbility(new DropUnit(new Soldier(), 10));
            return ub.GetUnit();
        }
        public MotherBase() : base(getUnit()) { }
    }
}

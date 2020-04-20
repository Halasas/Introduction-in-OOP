using System;
using System.Collections.Generic;
using BattleEngine.Effect;
using BattleEngine.Spell;
//How to use it?
//just move into project folder
//click right mouse botton on Solution Explorer / <ProjectName> / Add / *LAS.cs*
//When you need it, just write using LAS in header


namespace LAS //Library of Algorithms and Structures
{
    public static class Functions
    {
        public static List<IEffect> cloneEffects(List<IEffect> effects)
        {
            List<IEffect> clone = new List<IEffect>();
            foreach (var e in effects)
                clone.Add(e.clone());
            return clone;
        }
        public static List<IAbility> cloneSpells(List<IAbility> spells)
        {
            List<IAbility> clone = new List<IAbility>();
            foreach (var e in spells)
                clone.Add(e.clone());
            return clone;
        }
        public static void Swap<T>(ref T a, ref T b)
        {
            T c = a;
            a = b;
            b = c;
        }
    }
    public class Pair <T,V>
    {
        public T First { get; set; }
        public V Second { get; set; }

        public Pair(T t, V v)
        {
            First = t;
            Second = v;
        }
        public Pair() { }
    }

    public class Range
    {
        public int From { get; }
        public int To { get; }

        public override string ToString()
        {
            return From.ToString() + " " + To.ToString();
        }

        public static Range operator +(Range a, Range b)
        {
            return new Range(a.From + b.From, a.To + b.To);
        }
        public static Range operator *(Range a, int b)
        {
            if (a.From * b <= a.To * b)
                return new Range(a.From * b, a.To * b);
            else
                return new Range(a.To * b, a.From * b);
        }
        public static Range operator *(Range a, double b)
        {
            if (a.From * b <= a.To * b)
                return new Range((int)(a.From * b), (int)(a.To * b));
            else
                return new Range((int)(a.To * b), (int)(a.From * b));
        }
        public Range(int _from, int _to)
        {
            if (_from > _to)
                throw new Exception("Wrong format: _from must be less then _to!");
            From = _from;
            To = _to;
        }

        public Range(Range range) : this(range.From, range.To) {}

        public bool InRange(int x) { return (From <= x) && (x <= To); }
    }
}

using BattleEngine.BaseEntities;
using BattleEngine.BattleController;

namespace BattleEngine.Effect
{
    public interface IEffect
    {
        void Update(int Time);
        bool IsActive(int Time);
        ChangeableUnit Apply(ChangeableUnit unit, int Time);
        string ToStr();
        IEffect clone();
    }

    public abstract class AEffect : IEffect
    {
        public int StartTime { get; protected set; }
        public int Duration { get; protected set; }
        public abstract ChangeableUnit Apply(ChangeableUnit unit, int Time);
        public abstract IEffect clone();
        public virtual void Update(int Time)
        {
        }
        public virtual bool IsActive(int curRound)
        {
            int timePassed = curRound - StartTime;
            return timePassed <= Duration;
        }

        public abstract string ToStr();

        public AEffect(int StartTime, int Duration)
        {
            this.StartTime = StartTime;
            this.Duration = Duration;
        }
    }
    public class IncreaseResistance : AEffect, IEffect
    {
        public int Value;
        public IncreaseResistance(int StartTime, int Duration, int Value) : base(StartTime, Duration)
        { this.Value = Value; }

        public override ChangeableUnit Apply(ChangeableUnit unit, int Time)
        {
            unit.Resistance += Value;
            return unit;
        }
        public override IEffect clone()
        {
            return new IncreaseResistance(StartTime, Duration, Value);
        }
        public override string ToStr()
        {
            return "Increased Resistance " + Value.ToString();
        }
    }
    public class ReduceResistance : AEffect, IEffect
    {
        public int Value;
        public ReduceResistance(int StartTime, int Duration, int Value) : base(StartTime, Duration)
        { this.Value = Value; }

        public override ChangeableUnit Apply(ChangeableUnit unit, int Time)
        {
            unit.Resistance -= Value;
            return unit;
        }
        public override IEffect clone()
        {
            return new ReduceResistance(StartTime, Duration, Value);
        }
        public override string ToStr()
        {
            return "Reduced Resistance " + Value.ToString();
        }
    }
    public class MultiplyDefenceEffect : AEffect, IEffect
    {
        public float Value;
        public override ChangeableUnit Apply(ChangeableUnit unit, int Time)
        {
            unit.Defence = (int)(unit.Defence * Value);
            return unit;
        }

        public override IEffect clone()
        {
            return new MultiplyDefenceEffect(StartTime, Duration, Value);
        }

        public MultiplyDefenceEffect(int StartTime, int Duration, float Value)  : base(StartTime,Duration)
        {
            this.Value = Value;
        }

        public override string ToStr()
        {
            return "Defence multiplyed on " + Value.ToString() ;
        }
    }

    public class ReduceInitiative : AEffect, IEffect
    {
        public float Value;

        public ReduceInitiative(int startTime, int duration, float value) : base(startTime, duration)
        {
            Value = value;
        }

        public override ChangeableUnit Apply(ChangeableUnit unit, int Time)
        {
            unit.Initiative -= Value;
            return unit;
        }

        public override IEffect clone()
        {
            return new ReduceInitiative(StartTime, Duration, Value);
        }

        public override string ToStr()
        {
            return "Reduce Initiative by" + Value.ToString(); 
        }
    }
    public class EndlessResistance : AEffect
    {
        public EndlessResistance(int StartTime, int Duration) : base(StartTime, Duration) { }
        public override ChangeableUnit Apply(ChangeableUnit unit, int Time)
        {
            unit.Resistance = 100;
            return unit;
        }

        public override IEffect clone()
        {
            return  new EndlessResistance(StartTime, Duration);
        }

        public override string ToStr()
        {
            return "Unbreakable";
        }
    }
}

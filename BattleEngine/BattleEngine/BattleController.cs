using System;
using System.Collections.Generic;
using BattleEngine.BattleEntities;
using BattleEngine.BaseEntities;
using BattleEngine.Effect;
using BattleEngine.Spell;

namespace BattleEngine.BattleController
{
    public class Battle
    {        
        //поля
        private BattleArmy[] armies = new BattleArmy[2];
        public List<BattleArmy> Armies { get => new List<BattleArmy>(armies); }
        //
        List<BattleUnitsStack> activeQueue = new List<BattleUnitsStack>();
        public List<BattleUnitsStack> ActiveQueue { get => activeQueue; }
        List<BattleUnitsStack> waitingQueue = new List<BattleUnitsStack>();
        public List<BattleUnitsStack> WaitingQueue { get => waitingQueue; }
        List<BattleUnitsStack> finished = new List<BattleUnitsStack>();
        //
        public int Round { get; private set; }

        //методы
        private void finishTurn(BattleUnitsStack stack)
        {
            if (activeQueue.Contains(stack))
                activeQueue.Remove(stack);
            else if (waitingQueue.Contains(stack))
                waitingQueue.Remove(stack);
            else
                throw new Exception("Queues don't contain this stack");
            finished.Add(stack);
            UpdateQueues();
        }
        public void Attack(BattleUnitsStack attacker, BattleUnitsStack defender, bool Melee)
        {
            if (attacker == null || defender == null)
                throw new ArgumentNullException();
            Unit att = attacker.CurrentUnit;
            Unit def = defender.CurrentUnit;

            if ((Melee && !def.ImmunityToMelee) || (!Melee && !def.ImmunityToRanged))
            {
                LAS.Range CalcDamage(LAS.Range Damage, int Attack, int Defence)
                {
                    LAS.Range ansDamage;
                    if (Attack >= Defence)
                        ansDamage = Damage * ((float)attacker.Count * (1.0 + 0.05f * (Attack - Defence)));
                    else
                        ansDamage = Damage * ((float)attacker.Count * (1.0f / (1.0f + 0.05f * (Defence - Attack))));
                    return ansDamage;
                }
                LAS.Range damageRange = CalcDamage(att.Damage, att.Attack, def.Defence);
                int damage = (new Random()).Next(damageRange.From, damageRange.To);
                Console.WriteLine("Attack damage: {0}", damage);
                defender.ApplyDamage(damage);
                if (def.Resistance > 0 && defender.isAlive())
                {
                    defender.AddEffect(new ReduceResistance(Round, 1, 1));
                    damageRange = CalcDamage(def.Damage, def.Attack, att.Defence);
                    damage = (new Random()).Next(damageRange.From, damageRange.To);
                    Console.WriteLine("Resist damage: {0}", damage);
                    attacker.ApplyDamage(damage);
                }
            }
            finishTurn(attacker);
        }

        public void ApplySpell(IAbility spell, BattleUnitsStack caster, BattleUnitsStack[] targets)
        {
            if (caster == null || spell == null || targets == null)
                throw new ArgumentNullException();
            spell.Apply(caster, targets, Round);
            finishTurn(caster);
        }

        public void UpdateQueues()
        {
            foreach (var s in armies[0].Stacks)
                if (!activeQueue.Contains(s) && !waitingQueue.Contains(s) && !finished.Contains(s))
                    waitingQueue.Add(s);
            activeQueue.Sort((x, y) => y.CurrentUnit.Initiative.CompareTo(x.CurrentUnit.Initiative));
            waitingQueue.Sort((x, y) => x.CurrentUnit.Initiative.CompareTo(y.CurrentUnit.Initiative));
        }

        public void RemoveDead()
        {
            foreach (var army in Armies)
                foreach (var s in army.Stacks)
                    if (!s.isAlive())
                    {
                        finished.Remove(s);
                        activeQueue.Remove(s);
                        waitingQueue.Remove(s);
                        army.RemoveStack(s);
                    }
        }

        public void Wait(BattleUnitsStack stack)
        {
            if (stack == null)
                throw new ArgumentNullException();
            if (activeQueue.Contains(stack))
            { 
                activeQueue.Remove(stack);
                waitingQueue.Add(stack);
                UpdateQueues();
            }
        }

        public void Defend(BattleUnitsStack stack)
        {
            stack.AddEffect(new MultiplyDefenceEffect(Round, 1, 1.3f));
            finishTurn(stack);
        }

        public void GiveUp(BattleArmy army)
        {
            army.Surrended = true;
            CheckGameOver();
        }
        public BattleUnitsStack GetNextStack()
        {
            if (activeQueue.Count == 0 && waitingQueue.Count == 0)
            {
                NextRound();
            }
            UpdateQueues();
            if (activeQueue.Count != 0)
                return activeQueue[0];
            else
                return waitingQueue[0];
        }
        public void NextRound()
        {
            if (activeQueue.Count > 0 || waitingQueue.Count > 0)
                throw new Exception("Queues not empty");
            Round++;
            foreach (var army in armies)
                army.UpdateTime(Round);
            foreach(var i in finished)
                activeQueue.Add(i);
            finished.Clear();
            UpdateQueues();
            CheckGameOver();
        }

        public bool CheckGameOver()
        {
            if((armies[0].Surrended || !armies[0].isAlive()) && (armies[1].Surrended || !armies[1].isAlive()))
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Game over: Draw");
                return true;
            }
            else if (armies[0].Surrended || !armies[0].isAlive())
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Game over: Second player win");
                return true;
            }
            else if (armies[0].Surrended || !armies[1].isAlive())
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("Game over: First player win");
                return true;
            }
            return false;
        }

        //конструкторы
        public Battle(BattleArmy first, BattleArmy second)
        {
            if (first == null || second == null)
                throw new ArgumentNullException("First or second army is null");
            armies[0] = first;
            armies[1] = second;
            Round = 0;
            activeQueue.AddRange(first.Stacks);
            activeQueue.AddRange(second.Stacks);
            UpdateQueues();
        }
    }
}

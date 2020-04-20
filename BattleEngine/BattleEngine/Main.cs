using BattleEngine.BaseEntities;
using BattleEngine.BattleEntities;
using System;
using System.Collections.Generic;
using BattleEngine.BattleController;
using BattleEngine.Spell;

namespace BattleEngine
{
    class mainclass
    {
        public static void BuildArmy(Army army, string armyname)
        {
            List<Unit> units = Units.UnitsLibrary.GetUnits();
            int[] prices = { 20, 50, 400, 200, 400 };
            int id = 0;
            foreach (var u in units)
            {
                Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*-*");
                Console.WriteLine("id=" + id++.ToString());
                Console.WriteLine("___________");
                Console.WriteLine(u.toStr());
                Console.WriteLine("EFFECTS:");

                foreach (var e in u.BaseEffects)
                    Console.WriteLine("--" + e.ToStr());
                Console.WriteLine("ABILITIES:");
                foreach (var s in u.BaseSpells)
                    Console.WriteLine("++" + s.ToStr());
                Console.WriteLine();
            }
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("Let's gather {0}", armyname);
            Console.WriteLine("--------------------------------------------------------------------");
            Console.WriteLine("Use format \"id count\" to buy units \n or STOP to stop");
            while (army.StackCount < 5)
            {
                char[] det = { ' ' };
                string[] words = Console.ReadLine().Split();
                int X = -1;
                int Y = -1;
                if (words.Length > 2 || 
                    (words.Length == 1 && words[0] != "STOP") ||
                    (words.Length == 2 && (!int.TryParse(words[0], out X) || !int.TryParse(words[1], out Y))))
                {
                    Error("Invalid format");
                    Console.WriteLine("Use format \"id count\" to buy units \nor STOP to stop");
                    continue;
                }
                else if(words.Length == 1 && words[0] == "STOP")
                {
                    break;
                }
                if (X < units.Count && X >= 0 && Y > 0)
                {
                    army.AddStack(new UnitsStack(units[X], Y));
                    Console.WriteLine(units[X].Name + " " + Y.ToString() + " Added");
                }
                else
                {
                    Error("Invalid format");
                    Console.WriteLine("Use format \"id count\" to buy units \n or STOP to stop");
                }
            }
        }

        public static void ShowArmies(BattleArmy a, BattleArmy b, Battle battle, BattleUnitsStack curstack)
        {
            Console.WriteLine("-------------------------------------------");

            for (int i = 0; i < 9; i++)
            {
                BattleUnitsStack ast = null;
                BattleUnitsStack bst = null;
                string astack = "---";
                if (a.StackCount > i)
                {
                    ast = a.Stacks[i];
                    astack = ast.CurrentUnit.Name + " C:" + ast.Count + " HP:" + ast.HitPoint.ToString() + "/" + ast.CurrentUnit.HitPoint.ToString() + " R:" +
                        ast.CurrentUnit.Resistance;
                }
                string bstack = "---";
                if (b.StackCount > i)
                {
                    bst = b.Stacks[i];
                    bstack = bst.CurrentUnit.Name + " C:" + bst.Count + " HP:" + bst.HitPoint.ToString() + "/" + bst.CurrentUnit.HitPoint.ToString() + " R:" +
                        bst.CurrentUnit.Resistance;
                }
                if (ast != null && ast == curstack)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("{0}) {1,-50} ", i, astack);
                Console.ForegroundColor = ConsoleColor.White;

                if (bst != null && bst == curstack)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("{0}) {1}\n", i + 9, bstack);
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("{0,-5} {1,-40} {2,-5} {3}", "Ini", "active", "Ini", "waiting");
            for (int i = 0; i < 9; i++)
            {
                string astack = "---";
                double ai = -0;
                if (battle.ActiveQueue.Count > i) {
                    ai = battle.ActiveQueue[i].CurrentUnit.Initiative;
                    astack = battle.ActiveQueue[i].CurrentUnit.Name;
                }
                string bstack = "---";
                double bi = -0;
                if (battle.WaitingQueue.Count > i) {
                    bi = battle.WaitingQueue[i].CurrentUnit.Initiative;
                    bstack = battle.WaitingQueue[i].CurrentUnit.Name;
                }
                Console.WriteLine("{0:0.00} {1,-40} {2:0.00} {3}", ai, astack, bi, bstack);
            }
            Console.WriteLine("-------------------------------------------");

        }
        public static BattleUnitsStack[] getStackByID(int[] ids, Battle battle)
        {
            List<BattleUnitsStack> stacks = new List<BattleUnitsStack>();
            foreach(var id in ids)
            {
                if(id > 8 && id - 9 < battle.Armies[1].StackCount)
                    stacks.Add(battle.Armies[1].Stacks[id-9]);
                else if(id < battle.Armies[0].StackCount)
                    stacks.Add(battle.Armies[0].Stacks[id]);
                else
                {
                    Error("Invalid format");
                    return null;
                }
            }
            return stacks.ToArray();
        }

        public static int[] ParseComand(string s)
        {
            List<int> ans = new List<int>();
            foreach(var str in s.Split(' '))
            {
                int a = -1;
                if (int.TryParse(str, out a) && a >= 0 && a <= 17)
                    ans.Add(int.Parse(str));
                else
                    return null;
            }
            return ans.ToArray();
        }

        public static void Error(string s)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            Console.ForegroundColor = ConsoleColor.White;
            return;
        }

        public static void Main()
        {

            Army army1 = new Army();
            BuildArmy(army1, "Army 1");
            Army army2 = new Army();
            BuildArmy(army2, "Army 2");

            BattleArmy battleArmy0 = new BattleArmy(army1);
            BattleArmy battleArmy1 = new BattleArmy(army2);
            Battle battle = new Battle(battleArmy0, battleArmy1);
            while(battle.CheckGameOver() != true) {
                BattleUnitsStack cur_stack = battle.GetNextStack();
                ShowArmies(battleArmy0, battleArmy1, battle, cur_stack);
                if(cur_stack.Army == battleArmy0)
                    Console.WriteLine("Next unit from {2}: {0} - {1}", cur_stack.CurrentUnit.Name, cur_stack.Count, "Army 1");
                if (cur_stack.Army == battleArmy1)
                    Console.WriteLine("Next unit from {2}: {0} - {1}", cur_stack.CurrentUnit.Name, cur_stack.Count, "Army 2");
                if (cur_stack.GetEffects().Count > 0)
                    foreach (var e in cur_stack.GetEffects())
                        Console.WriteLine(e.ToStr());
                Console.WriteLine("You can do:");
                Console.WriteLine("0 target - Attack");
                Console.WriteLine("1 - Wait");
                Console.WriteLine("2 - Defend");
                if (cur_stack.GetSpells().Count > 0)
                {
                    int i = 3;
                    foreach (var e in cur_stack.GetSpells())
                        Console.WriteLine("{0} - {1}", i++, e.ToStr());
                }
                Console.WriteLine("GIVEUP - Give Up");
                string com = Console.ReadLine();
                while(com == "")
                    com = Console.ReadLine();
                Console.WriteLine("^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^");
                if (com[0] == '0')
                {
                    string[] args = com.Split(' ');
                    int target_id = 0;
                    if (args.Length != 2 || !int.TryParse(args[1], out target_id) || target_id < 0 || target_id >= 18)
                    {
                        Error("Invalid format");
                        continue;
                    }
                    else
                    { 
                        BattleUnitsStack target;
                        if (target_id >= 9 && battleArmy1.StackCount > (target_id - 9))
                            target = battleArmy1.Stacks[target_id - 9];
                        else if (target_id <= 8 && battleArmy0.StackCount > (target_id))
                            target = battleArmy0.Stacks[target_id];
                        else
                        {
                            Error("Invalid format");
                            continue;
                        }
                        battle.Attack(cur_stack, target, cur_stack.CurrentUnit.MeleeDamager);
                    }
                }
                else if (com[0] == '1')
                    battle.Wait(cur_stack);
                else if (com[0] == '2')
                    battle.Defend(cur_stack);
                else if (com == "giveup")
                    battle.GiveUp(cur_stack.Army);
                else
                {
                    int[] args = ParseComand(com);
                    if (args == null)
                    {
                        Error("Invalid format");
                        continue;
                    }
                    List<int> targs = new List<int>(args);
                    targs.RemoveAt(0);
                    BattleUnitsStack[] targets = getStackByID(targs.ToArray(), battle);
                    if(targets == null)
                    {
                        continue;
                    }
                    if (cur_stack.GetSpells().Count > args[0] - 3)
                    {
                        try { 
                            IAbility ability = cur_stack.GetSpells()[args[0] - 3];
                            battle.ApplySpell(ability, cur_stack, targets);
                        }
                        catch (Exception e)
                        {
                            Error(e.Message);
                        }
                    }
                    else
                        Error("Invalid format");
                }
                battle.RemoveDead();
            }
        }
    }
}

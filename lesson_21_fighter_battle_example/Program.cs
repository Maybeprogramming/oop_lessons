namespace lesson_21_fighter_battle_example
{
    using static Arena;
    using static InputModule;

    using System.ComponentModel;

    public struct DamageEvent
    {
        public readonly Fighter Attacker;
        public readonly Fighter Victim;
        public readonly int OriginalDamage;
        public readonly bool Ability;
        public readonly bool Evadable;
        public int Damage;

        public DamageEvent(Fighter attacker, Fighter victim, int originalDamage, bool evadable = true, bool ability = false)
        {
            Attacker = attacker;
            Victim = victim;
            OriginalDamage = originalDamage;
            Damage = originalDamage;
            Evadable = evadable;
            Ability = ability;
        }
    }

    public class TaskFourty
    {
        public static void Main(string[] args)
        {
            const string FightCommand = "fight";
            const string ExitCommand = "exit";

            const string InputRequestMessage = "Write \"" + FightCommand + "\" to start new fight or \"" + ExitCommand + "\"" + "to leave";

            string userInput;
            bool leave = false;
            ArenaBarker manager = new ArenaBarker();

            do
            {
                userInput = ReadResponse(InputRequestMessage);

                switch (userInput)
                {
                    case FightCommand:
                        manager.SetupFight();
                        break;

                    case ExitCommand:
                        leave = true;
                        continue;

                    default:
                        Console.Error.WriteLine("Failed to read input");
                        break;
                }
            } while (leave == false);
        }
    }

    public class ArenaBarker
    {
        public void SetupFight()
        {
            Console.Clear();

            BeginFight(GetFighter(), GetFighter());
        }

        private Fighter GetFighter()
        {
            Fighter[] fighters = { new Warrior(), new Paladin(), new Hunter(), new Priest(), new Mage(), new Warlock() };

            for (int i = 0; i < fighters.Length; i++)
            {
                Console.WriteLine(fighters[i] + " - " + i);
            }

            int userChoise = ForceReadInt("Write contestant number!", 0, fighters.Length);

            return fighters[userChoise];
        }
    }

    public static class Arena
    {
        private const int PercentDivier = 100;

        public static event Action<int> Tick;

        private static Fighter _fighter1 = null;
        private static Fighter _fighter2 = null;

        public static bool ArenaOccuped { get; private set; } = false;
        public static int Time { get; private set; }

        public static void BeginFight(Fighter fighter1, Fighter fighter2)
        {
            if (ArenaOccuped)
            {
                return;
            }

            _fighter1 = fighter1;
            _fighter2 = fighter2;

            ProccessFight();
        }

        public static void ProccessFight()
        {
            ArenaOccuped = true;

            while (_fighter1.Alive && _fighter2.Alive)
            {
                Update();
            }

            DeclareVictory();
        }

        public static void DeclareVictory()
        {
            if (_fighter1.Alive == false)
            {
                if (_fighter2.Alive == false)
                {
                    Logger.InformDraw();
                }
                else
                {
                    Logger.InformVictory(_fighter2);
                }
            }
            else
            {
                Logger.InformVictory(_fighter1);
            }

            Clear();
            ArenaOccuped = false;
        }

        public static void Clear()
        {
            _fighter1.Dispose();
            _fighter2.Dispose();

            _fighter1 = null;
            _fighter2 = null;
        }

        public static void Update()
        {
            Time++;
            Tick?.Invoke(1);
        }

        public static void PerformeAttack(Fighter attacker, Fighter victim)
        {
            ApplyDamage(attacker, victim, attacker.Attack);
        }

        public static void ApplyDamage(Fighter attacker, Fighter victim, int originalDamage, bool evadable = true, bool ability = false)
        {
            DamageEvent damageEvent = new DamageEvent(attacker, victim, originalDamage, evadable, ability);

            damageEvent.Damage *= damageEvent.Attacker.GetDamageDealthPercent(damageEvent) / PercentDivier * damageEvent.Victim.GetDamageRecivePercent(damageEvent) / PercentDivier;

            damageEvent.Victim.TakeDamage(damageEvent);

            if (damageEvent.Damage > 0)
            {
                damageEvent.Attacker.OnDamageDealth(damageEvent);
            }
        }

        public static Fighter GetOponentOf(Fighter fighter)
        {
            if (fighter == _fighter1)
            {
                return _fighter2;
            }
            else
            {
                return _fighter1;
            }
        }
    }

    public abstract class Object : IDisposable
    {
        public Object()
        {
            Tick += Update;
        }

        public void Dispose()
        {
            Tick -= Update;
        }

        protected abstract void Update(int deltaTime);
    }

    public class Resource
    {
        public readonly int MaxValue;
        public int Value { get; private set; }
        public int Regeneration { get; private set; }

        public Resource(int maxValue, int startValue, int regeneraion)
        {
            MaxValue = maxValue;
            Value = startValue;
            Regeneration = regeneraion;
        }

        public void ModifyValue(int value)
        {
            Value += value;

            if (Value > MaxValue)
            {
                Value = MaxValue;
            }

            if (Value < 0)
            {
                Value = 0;
            }
        }

        public void SetValue(int value)
        {
            if (value > MaxValue)
            {
                Value = MaxValue;
                return;
            }

            if (value < 0)
            {
                Value = 0;
                return;
            }

            Value = value;
        }
    }

    public abstract class Fighter : Object
    {
        public const int DefaultDamageRecivePercent = 100;

        private readonly Resource HealthPool = null;
        private readonly Resource ManaPool = null;

        private List<Ability> _abilities = new List<Ability>();

        public Fighter(int maxHealth, int maxMana, int attackDamage, int manaStartValue = 0, int healthRegen = 0, int manaRegen = 0)
        {
            _abilities = new List<Ability>();

            Alive = true;

            HealthPool = new Resource(maxHealth, maxHealth, healthRegen);
            ManaPool = new Resource(maxMana, manaStartValue, manaRegen);

            Attack = attackDamage;
        }

        public bool Alive { get; private set; } = true;

        public int Attack { get; private set; } = 0;

        public int Mana => ManaPool.Value;
        public int Health => HealthPool.Value;

        public int MaxMana => ManaPool.MaxValue;
        public int MaxHealth => HealthPool.MaxValue;

        public void GiveAbility(Ability ability)
        {
            _abilities.Add(ability);
        }

        public void TakeDamage(DamageEvent e)
        {
            if (e.Damage < 0)
                return;

            HealthPool.ModifyValue(-e.Damage);

            OnDamageRecive(e);
            Logger.InformDamage(e);

            if (Health == 0)
            {
                Logger.InformDeath(this);
                OnReciveLethalDamage(e);
            }

            if (Health <= 0)
            {
                Alive = false;
            }
        }

        public void Heal(int healing)
        {
            HealthPool.ModifyValue(healing);
        }

        public void GiveMana(int value)
        {
            if (value < 0)
                return;
            ManaPool.ModifyValue(value);
        }

        public void SpendMana(int value)
        {
            if (value < 0)
                return;
            ManaPool.ModifyValue(-value);
        }

        public void CastAbilities()
        {
            foreach (Ability ability in _abilities)
            {
                if (ability.CooldownReady)
                    ability.Cast();
            }
        }

        public virtual int GetDamageRecivePercent(DamageEvent e)
        {
            return DefaultDamageRecivePercent;
        }

        public virtual int GetDamageDealthPercent(DamageEvent e)
        {
            return DefaultDamageRecivePercent;
        }

        public virtual void OnReciveLethalDamage(DamageEvent e)
        {
        }

        public virtual void OnDamageDealth(DamageEvent e)
        {
        }

        public virtual void OnDamageRecive(DamageEvent e)
        {
        }

        protected override void Update(int time)
        {
            HealthPool.ModifyValue(HealthPool.Regeneration);
            ManaPool.ModifyValue(ManaPool.Regeneration);
        }

        public override string ToString()
        {
            //return TypeDescriptor.GetClassName(this);
            return $"{this.GetType().Name} | {Health} / {MaxHealth}";
        }
    }

    public abstract class Ability
    {
        public readonly string Name;
        public readonly Fighter Owner;

        public Ability(Fighter owner, string name, int cooldown, int manaCost)
        {
            Owner = owner;

            Name = name;

            Cooldown = cooldown;
            CooldownEnd = 0;

            Cost = manaCost;
        }

        public int Cooldown { get; private set; }
        public int Cost { get; private set; }

        public int CooldownEnd { get; private set; }

        public bool CooldownReady => CooldownEnd <= Time;

        public void Cast()
        {
            if (Owner.Mana < Cost)
                return;

            Logger.InformCast(this);
            CooldownEnd = Cooldown + Time;
            Owner.SpendMana(Cost);
            OnSpellStart();
        }

        public Fighter GetOwnerTarget()
        {
            return GetOponentOf(Owner);
        }

        protected virtual void OnSpellStart()
        {
        }
    }

    public sealed class Warrior : Fighter
    {
        public Warrior() : base(1000, 10, 50, 0, 10)
        {
        }

        protected override void Update(int time)
        {
            base.Update(time);
            PerformeAttack(this, GetOponentOf(this));
        }
    }

    public sealed class Paladin : Fighter
    {
        private int _abilityCasyHealthLevel = 300;

        public Paladin() : base(800, 20, 35, 20, 3, 1)
        {
            GiveAbility(new LieOfHand(this));
        }

        protected override void Update(int time)
        {
            base.Update(time);
            if (Health < _abilityCasyHealthLevel)
            {
                CastAbilities();
            }
            else
            {
                PerformeAttack(this, GetOponentOf(this));
            }
        }
    }

    public sealed class Hunter : Fighter
    {
        public Hunter() : base(500, 0, 40, 0, 3)
        {
            GiveAbility(new Powershot(this));
        }

        protected override void Update(int time)
        {
            base.Update(time);
            CastAbilities();
            PerformeAttack(this, GetOponentOf(this));
        }
    }

    public sealed class Priest : Fighter
    {
        public Priest() : base(300, 40, 30, 40, 2)
        {
            GiveAbility(new Pray(this));
        }

        protected override void Update(int time)
        {
            base.Update(time);

            if (Health < MaxHealth)
            {
                CastAbilities();
            }
            else
            {
                PerformeAttack(this, GetOponentOf(this));
            }
        }
    }

    public sealed class Mage : Fighter
    {
        public Mage() : base(300, 40, 0, 40, 0, 4)
        {
            GiveAbility(new Fireball(this));
        }
        protected override void Update(int time)
        {
            base.Update(time);
            CastAbilities();
        }
    }

    public sealed class Warlock : Fighter
    {
        public Warlock() : base(700, 10, 70, 10, 1, 2)
        {
            GiveAbility(new Soulstone(this));
        }

        protected override void Update(int time)
        {
            base.Update(time);

            PerformeAttack(this, GetOponentOf(this));
        }

        public override void OnReciveLethalDamage(DamageEvent e)
        {
            CastAbilities();
        }
    }

    public sealed class LieOfHand : Ability
    {
        public LieOfHand(Fighter owner) : base(owner, "Lie of hands", 10, 0)
        {
        }

        protected override void OnSpellStart()
        {
            Owner.Heal(Owner.MaxHealth);
        }
    }

    public sealed class Powershot : Ability
    {
        private int _damage = 10;

        public Powershot(Fighter owner) : base(owner, "Powershot", 4, 0)
        {
        }

        protected override void OnSpellStart()
        {
            ApplyDamage(Owner, GetOwnerTarget(), _damage, ability: true);
        }
    }

    public sealed class Pray : Ability
    {
        private int _healing = 100;

        public Pray(Fighter owner) : base(owner, "Pray", 2, 4)
        {
        }

        protected override void OnSpellStart()
        {
            Owner.Heal(_healing);
        }
    }

    public sealed class Fireball : Ability
    {
        private int _damage = 100;

        public Fireball(Fighter owner) : base(owner, "Fireball", 0, 5)
        {
        }

        protected override void OnSpellStart()
        {
            ApplyDamage(Owner, GetOwnerTarget(), _damage, true, true);
        }
    }

    public sealed class Soulstone : Ability
    {
        private int _healing = 500;

        public Soulstone(Fighter owner) : base(owner, "Soulstone", 10, 0)
        {
        }

        protected override void OnSpellStart()
        {
            Owner.Heal(_healing);
        }
    }

    public static class Logger
    {
        public static void InformCast(Ability ability)
        {
            Console.WriteLine($"{ability.Owner} casted {ability}!");
        }

        public static void InformDamage(DamageEvent e)
        {
            Console.WriteLine($"{e.Attacker} attacked {e.Victim}: -{e.Damage}HP ({e.Victim.Health} left)");
        }

        public static void InformDeath(Fighter victim)
        {
            Console.WriteLine(victim + " died.");
        }

        public static void InformVictory(Fighter winner)
        {
            Console.WriteLine(winner + " won!");
        }

        public static void InformDraw()
        {
            Console.WriteLine("Draw!");
        }
    }

    public static class InputModule
    {
        public static string ReadResponse(string message)
        {
            Console.WriteLine(message);

            return Console.ReadLine();
        }

        public static int ForceReadInt(string message = "Write number.", int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            Console.WriteLine(message);

            int result;

            while (int.TryParse(Console.ReadLine(), out result) == false || result < minValue || result >= maxValue)
            {
                Console.Error.WriteLine("Failed to read. Try again.");
            }

            return result;
        }

        public static int ReadChoose(string message, params string[] responses)
        {
            string choice = ReadResponse(message);

            for (int i = 0; i < responses.Length; i++)
            {
                if (responses[i].Equals(choice))
                {
                    return i;
                }
            }

            Console.Error.WriteLine("Failed to read choose.");
            return -1;
        }
    }

}
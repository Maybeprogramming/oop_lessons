using System.Text;

namespace lesson_15_events_practice_01
{
    public delegate void HandlerDamage(int damage, int health, string name);
    public delegate void HandlerNotDamage(string name);
    public delegate void HandlerAttack(Fighter fighterAttack, Fighter fighterTakeDamage);
    public delegate void HandlerPrintBar(Fighter fighter1, Fighter fighter2);

    class Program
    {
        private static event HandlerPrintBar? EventPrintBar;

        static void Main()
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 110;
            Random random = new Random();

            UserInterface userInterface = new();

            Fighter fighter1 = new(random.Next(100,200), random.Next(10,40), "Иван");
            InitializationEvents(userInterface, fighter1);

            Fighter fighter2 = new(random.Next(100, 200), random.Next(10, 40), "Олег");
            InitializationEvents(userInterface, fighter2);

            EventPrintBar += userInterface.OnPrintBar;
            int Counter = 0;
            bool isAlive = true;

            Console.WriteLine("Начать битву?");
            EventPrintBar?.Invoke(fighter1, fighter2);
            Console.ReadKey();

            while (isAlive == true)
            {
                Console.Clear();
                EventPrintBar?.Invoke(fighter1, fighter2);

                Console.Write($"{++Counter} раунд:\n");
                fighter1.Attack(fighter2);
                fighter2.Attack(fighter1);

                EventPrintBar?.Invoke(fighter1, fighter2);

                if (fighter1.IsDead == true || fighter2.IsDead == true)
                {
                    isAlive = false;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения боя...");
                Console.ReadLine();
            }

            Console.WriteLine("Бой закончен!!!");

            userInterface.PrintBaseScreen();

            Console.ReadLine();
        }

        private static void InitializationEvents(UserInterface userInterface, Fighter fighter)
        {
            fighter.AttackDamage += userInterface.OnAttackDamage;
            fighter.EventTakeDamage += userInterface.OnTakeDamage;
            fighter.EventNotDamage += userInterface.OnNotTakeDamage;
            fighter.EventAttack += userInterface.OnAttack;
            fighter.FighterDead += userInterface.OnDeadFighter;
        }
    }

    class FighterEventArgs : EventArgs
    {
        public FighterEventArgs(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }

        public string Name { get; private set; }
        public int Health { get; private set; }
        public int Damage { get; private set; }
    }

    public class Fighter
    {
        public event HandlerDamage? EventTakeDamage;
        public event HandlerNotDamage? EventNotDamage;
        public event HandlerAttack? EventAttack;

        public event Action action;

        public event EventHandler? AttackDamage;
        public event EventHandler? FighterDead;
        private FighterEventArgs? eventArgs;

        public Fighter(int health, int damage, string name)
        {
            Health = health;
            Damage = damage;
            Name = name;
        }

        public int Health { get; private set; }
        public int Damage { get; private set; }
        public string Name { get; private set; }
        public bool IsDead
        {
            get => Health <= 0;
        }

        public bool TakeDamage(int damage)
        {
            if (IsDead == false)
            {
                Health -= damage;

                EventTakeDamage?.Invoke(damage, Health, Name);

                if (Health < 0)
                {
                    FighterDead?.Invoke(this, new EventArgs());
                }

                return true;
            }
            else
            {
                EventNotDamage?.Invoke(Name);
                return false;
            }
        }

        public void Attack(Fighter fighter)
        {
            eventArgs = new FighterEventArgs(fighter.Name, fighter.Health, fighter.Damage);

            AttackDamage?.Invoke(this, eventArgs);

            EventAttack?.Invoke(this, fighter);
            fighter.TakeDamage(Damage);
        }

    }
    class UserInterface
    {
        private int currentCursorPositionLeft;
        private int currentCursorPositionTop;

        public void OnTakeDamage(int damage, int health, string name)
        {
            Console.WriteLine($"Боец {name} получил {damage} урона, теперь у него {health} здоровья");
        }

        public void OnNotTakeDamage(string name)
        {
            Console.WriteLine($"({name}) Невозможно убить то что мертво...");
        }

        internal void OnAttack(Fighter fighterAttack, Fighter fighterTakeDamage)
        {
            Console.WriteLine($"Боец {fighterAttack.Name} атаковал {fighterTakeDamage.Name} и нанёс ему {fighterAttack.Damage} урона");
        }

        public void OnAttackDamage(object? sender, EventArgs e)
        {
            Fighter? fighter = (Fighter)sender;
            FighterEventArgs? eventArgs = (FighterEventArgs)e;

            Console.WriteLine($"Боец {fighter.Name} атаковал ({eventArgs.Name}) и нанёс ему {fighter.Damage} урона");
        }

        public void OnPrintBar(Fighter fighter1, Fighter fighter2)
        {
            currentCursorPositionLeft = Console.CursorLeft;
            currentCursorPositionTop = Console.CursorTop;

            Console.CursorLeft = 80;
            Console.CursorTop = 0;

            for (int i = 0; i < 4; i++)
            {
                Console.CursorLeft = 80;

                if (i == 0)
                {
                    Console.CursorTop = i;
                    Console.Write($"Статистика:");
                }
                else if (i == 1)
                {
                    ClearString(i);
                    PrintFighterStats(fighter1);
                }
                else if (i == 2)
                {
                    Console.CursorTop = i;
                    Console.Write(new string('-', 20));
                }
                else if (i == 3)
                {
                    ClearString(i);
                    PrintFighterStats(fighter2);
                }
            }

            Console.CursorLeft = currentCursorPositionLeft;
            Console.CursorTop = currentCursorPositionTop;
        }

        private void PrintFighterStats(Fighter fighter)
        {
            Console.Write($"Боец: {fighter.Name}. HP: {fighter.Health}");
        }

        private void ClearString(int topCursorPosition)
        {
            Console.CursorTop = topCursorPosition;
            Console.Write(new string(' ', 20));
            Console.CursorLeft = 80;
        }

        public void OnDeadFighter(object? sender, EventArgs e)
        {
            Fighter? fighter = (Fighter)sender;

            currentCursorPositionLeft = Console.CursorLeft;
            currentCursorPositionTop = Console.CursorTop;
            Console.CursorLeft = 80;
            Console.CursorTop = 6;

            ClearString(6);
            Console.CursorTop = 6;
            Console.Write($"{fighter.Name} погиб...");

            Console.CursorLeft = currentCursorPositionLeft;
            Console.CursorTop = currentCursorPositionTop;
        }

        public void PrintBaseScreen()
        {
            StringBuilder sb = new();
            sb.Append("    A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |\n");
            sb.Append("012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789\n");
            sb.Append("                                                                     | Статистика:       |\n");
            sb.Append("                                                                     | Боец: < Олег >    |\n");
            sb.Append("                                                                     | HP:   < 200 >     |\n");
            sb.Append("                                                                     | DMG:  < 50 >      |\n");
            sb.Append("                                                                     |-------------------|\n");
            sb.Append("                                                                     | Боец: < Олег >    |\n");
            sb.Append("                                                                     | HP:   < 200 >     |\n");
            sb.Append("                                                                     | DMG:  < 50 >      |\n");
            sb.Append("                                                                     |-------------------|\n");

            Console.Write(sb);
        }
    }
}

//  Title = Битва героев
//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Меню:                                                                | Статистика:       |
//2 1. Выбрать бойцов для битвы                                          | Боец: <не выбран> |
//3 2. Выйти из бойцовского клуба                                        | HP:   < --- >     |
//4                                                                      | DMG:  < --- >     |
//5                                                                      |-------------------|
//6                                                                      | Боец: <не выбран> |
//7                                                                      | HP:   < --- >     |
//8                                                                      | DMG:  < --- >     |
//9                                                                      |-------------------|
//0 

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Меню выбора бойцов                                                   | Статистика:       |
//2 1. Выбрать бойцов для битвы                                          | Боец: < Олег >    |
//3 2. Выйти из бойцовского клуба                                        | HP:   < 200 >     |
//4                                                                      | DMG:  < 50 >      |
//5                                                                      |-------------------|
//6                                                                      | Боец: < Олег >    |
//7                                                                      | HP:   < 200 >     |
//8                                                                      | DMG:  < 50 >      |
//9                                                                      |-------------------|
//0 
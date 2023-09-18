namespace lesson_15_events_practice_01
{
    public delegate void HandlerDamage(int damage, int health, string name);
    public delegate void HandlerNotDamage(string name);
    public delegate void HandlerAttack(Fighter fighterAttack, Fighter fighterTakeDamage);
    public delegate void HandlerPrintBar(Fighter fighter1, Fighter fighter2);

    class Program
    {
        public static event HandlerPrintBar EventPrintBar;

        static void Main()
        {
            Fighter fighter1 = new Fighter(140, 20, "Иван");
            Fighter fighter2 = new Fighter(140, 30, "Олег");
            EventPrintBar += UserInterface.OnPrintBar;
            int Counter = 0;
            bool isAlive = true;

            Console.WriteLine("Старт битвы");
            Console.ReadKey();

            while (isAlive == true)
            {
                EventPrintBar?.Invoke(fighter1, fighter2);

                Console.Write($"{++Counter} раунд:\n");
                fighter1.Attack(fighter2);
                fighter2.Attack(fighter1);


                if (fighter1.Health < 0 || fighter2.Health < 0)
                {
                    isAlive = false;
                }

                Console.WriteLine("Нажмите любую клавишу для продолжения...");
                Console.ReadLine();
            }

            EventPrintBar?.Invoke(fighter1, fighter2);
            Console.WriteLine("Бой закончен!!!");
            Console.ReadKey();
        }
    }

    public class Fighter
    {
        public event HandlerDamage EventTakeDamage;
        public event HandlerNotDamage EventNotDamage;
        public event HandlerAttack EventAttack;

        public Fighter(int health, int damage, string name)
        {
            Health = health;
            Damage = damage;
            Name = name;
            EventTakeDamage += UserInterface.OnTakeDamage;
            EventNotDamage += UserInterface.OnNotTakeDamage;
            EventAttack += UserInterface.OnAttack;
        }

        public int Health { get; private set; }
        public int Damage { get; private set; }
        public string Name { get; private set; }

        public bool TakeDamage(int damage)
        {
            if (Health > 0)
            {
                Health -= damage;

                EventTakeDamage?.Invoke(damage, Health, Name);

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
            EventAttack?.Invoke(this, fighter);
            fighter.TakeDamage(Damage);
        }

    }

    class UserInterface
    {
        private static int currentCursorPositionLeft;
        private static int currentCursorPositionTop;

        public static void OnTakeDamage(int damage, int health, string name)
        {
            Console.WriteLine($"Боец {name} получил {damage} урона, теперь у него {health} здоровья");
        }

        public static void OnNotTakeDamage(string name)
        {
            Console.WriteLine($"({name}) Невозможно убить то что мертво...");
        }

        internal static void OnAttack(Fighter fighterAttack, Fighter fighterTakeDamage)
        {
            Console.WriteLine($"Боец {fighterAttack.Name} атаковал {fighterTakeDamage.Name} и нанёс ему {fighterAttack.Damage} урона");
        }

        public static void OnPrintBar(Fighter fighter1, Fighter fighter2)
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

                if (i == 1)
                {
                    ClearString(i);
                    PrintFighterStats(fighter1);
                }

                if (i == 2)
                {
                    Console.CursorTop = i;
                    Console.Write(new string('-', 20));
                }

                if (i == 3)
                {
                    ClearString(i);
                    PrintFighterStats(fighter2);
                }
            }

            Console.CursorLeft = currentCursorPositionLeft;
            Console.CursorTop = currentCursorPositionTop;
        }

        private static void PrintFighterStats(Fighter fighter)
        {
            Console.Write($"Боец: {fighter.Name}. HP: {fighter.Health}");
        }

        private static void ClearString(int i)
        {
            Console.CursorTop = i;
            Console.Write(new string(' ', 20));
            Console.CursorLeft = 80;
        }
    }
}
namespace lesson_15_events_practice_01
{
    public delegate void Handler(int damage, int health);

    class Program
    {
        static void Main()
        {
            Fighter fighter = new Fighter(100, 20);
            fighter.TakeDamage(10);
            fighter.TakeDamage(20);
            fighter.TakeDamage(30);

            Console.ReadKey();
        }
    }

    class Fighter
    {
        public event Handler PrintUI;

        public Fighter(int health, int damage)
        {
            Health = health;
            Damage = damage;
            PrintUI += UserInterface.OnUpdate;
        }

        public int Health { get; private set; }
        public int Damage { get; private set; }

        public bool TakeDamage(int damage)
        {
            if (Health > 0)
            {
                Health -= damage;
                
                PrintUI.Invoke(damage, Health);

                return true;
            }
            else
            {
                Console.WriteLine("Невозможно убить то что мертво...");
                return false;
            }
        }
    }

    class UserInterface
    {
        int figterFirstHealth;
        int fighterSecondHealth;

        int currentCursorPositionLeft;
        int currentCursorPositionTop;

        public static event Handler PrinteMessage;

        public static void OnUpdate(int damage, int health)
        {
            Console.WriteLine($"Боец получил {damage} урона, теперь у него {health} здоровья");
        }
    }


}
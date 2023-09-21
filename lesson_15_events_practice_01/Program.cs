using System.Text;

namespace lesson_15_events_practice_01
{
    delegate void FighterDamageHandler(int damage, int health, string name);
    delegate void HandlerNotDamage(string name);
    delegate void HandlerAttack(Fighter fighterAttack, Fighter fighterTakeDamage);
    delegate void HandlerPrintBar(Fighter fighter1, Fighter fighter2);

    class Program
    {
        private static event HandlerPrintBar? EventPrintBar;

        static void Main()
        {
            Console.WindowHeight = 40;
            Console.WindowWidth = 110;
            Random random = new Random();

            UserInterface userInterface = new();

            Fighter fighter1 = new(random.Next(100, 200), random.Next(10, 40), "Иван");
            SubscribsToEvents(userInterface, fighter1);

            Fighter fighter2 = new(random.Next(100, 200), random.Next(10, 40), "Олег");
            SubscribsToEvents(userInterface, fighter2);

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

        private static void SubscribsToEvents(UserInterface userInterface, Fighter fighter)
        {
            fighter.AttackDamage += userInterface.OnAttackDamage;
            fighter.EventTakeDamage += userInterface.OnTakeDamage;
            fighter.EventNotDamage += userInterface.OnNotTakeDamage;
            fighter.EventAttack += userInterface.OnAttack;
            fighter.ActionAttack += userInterface.OnAttack;
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

    class Fighter
    {
        public event FighterDamageHandler? EventTakeDamage;
        public event HandlerNotDamage? EventNotDamage;
        public event HandlerAttack? EventAttack;

        public event Action<Fighter, Fighter> ActionAttack;

        public event Action action;

        public event EventHandler<FighterEventArgs>? AttackDamage;
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

        public void Attack(Fighter target)
        {
            eventArgs = new FighterEventArgs(target.Name, target.Health, target.Damage);
            AttackDamage?.Invoke(this, eventArgs);

            //EventAttack?.Invoke(this, target);
            //ActionAttack?.Invoke(this, target);

            target.TakeDamage(Damage);
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

        public void OnAttackDamage(object? sender, FighterEventArgs e)
        {
            if (sender is Fighter)
            {
                Console.WriteLine($"Боец {((Fighter)sender).Name} атаковал ({e.Name}) и нанёс ему {e.Damage} урона");
            }

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

            var equalNumbers = bool (int i, int j) => { return i > j; };

            bool isEqual = equalNumbers(10, 2);

            Console.WriteLine(isEqual);
        }
    }
}

//  Title = Битва героев
//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//
//0 Меню:                                                                | Статистика:       |
//1 1. Выбрать бойцов для битвы                                          | Боец: <не выбран> |
//2 2. Выйти из бойцовского клуба                                        | HP:   < --- >     |
//3                                                                      | DMG:  < --- >     |
//4                                                                      |-------------------|
//5                                                                      | Боец: <не выбран> |
//6                                                                      | HP:   < --- >     |
//7                                                                      | DMG:  < --- >     |
//8                                                                      |-------------------|
//9 

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Список доступных бойцов:                                             | Статистика:       |
//2 1. Кларк HP <200> DMG: <35>                                          | Боец: <не выбран> |
//3 2. Флеш HP <180> DMG: <40>                                           | HP:   < --- >     |
//4 3. Валькирия HP <200> DMG: <30>                                      | DMG:  < --- >     |
//5 4. Алькатрон HP <250> DMG: <25>                                      |-------------------|
//6 5. Халк HP <300> DMG: <50>                                           | Боец: <не выбран> |
//7 6. Бэтмен HP <150> DMG: <45>                                         | HP:   < --- >     |
//8 7. Король-Лич HP <500> DMG: <100>                                    | DMG:  < --- >     |
//9 8. Сплинтер HP <100> DMG: <100>                                      |-------------------|
//0 9. Неуязвимый HP <2000> DMG: <300>
//1 10. Асока HP <300> DMG: <150>
//2

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Список доступных бойцов:                                             | Статистика:       |
//2 1. Кларк HP <200> DMG: <35>                                          | Боец: <Валькирия> |
//3 2. Флеш HP <180> DMG: <40>                                           | HP:   < 200 >     |
//4 3. Валькирия HP <200> DMG: <30>                                      | DMG:  < 30 >      |
//5 4. Алькатрон HP <250> DMG: <25>                                      |-------------------|
//6 5. Халк HP <300> DMG: <50>                                           | Боец: <не выбран> |
//7 6. Бэтмен HP <150> DMG: <45>                                         | HP:   < --- >     |
//8 7. Король-Лич HP <500> DMG: <100>                                    | DMG:  < --- >     |
//9 8. Сплинтер HP <100> DMG: <100>                                      |-------------------|
//0 9. Неуязвимый HP <2000> DMG: <300>
//1 10. Асока HP <300> DMG: <150>
//2
//3 Введите номер для выбора первого бойца: 3
//4 Вы выбрали - Валькирия! Хороший выбор!

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Список доступных бойцов:                                             | Статистика:       |
//2 1. Кларк HP <200> DMG: <35>                                          | Боец: <Валькирия> |
//3 2. Флеш HP <180> DMG: <40>                                           | HP:   < 200 >     |
//4 3. Валькирия HP <200> DMG: <30>                                      | DMG:  < 30 >      |
//5 4. Алькатрон HP <250> DMG: <25>                                      |-------------------|
//6 5. Халк HP <300> DMG: <50>                                           | Боец: <Асока>     |
//7 6. Бэтмен HP <150> DMG: <45>                                         | HP:   < 300 >     |
//8 7. Король-Лич HP <500> DMG: <100>                                    | DMG:  < 150 >     |
//9 8. Сплинтер HP <100> DMG: <100>                                      |-------------------|
//0 9. Неуязвимый HP <2000> DMG: <300>
//1 10. Асока HP <300> DMG: <150>
//2
//3 Введите номер для выбора первого бойца: 3
//4 Вы выбрали - Валькирия! Хороший выбор!
//5
//6 Введите номер для выбора второго бойца: 10
//7 Вы выбрали - Асока! Да прибудет с вами сила!
//8

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Список доступных бойцов:                                             | Статистика:       |
//2 1. Кларк HP <200> DMG: <35>                                          | Боец: <Валькирия> |
//3 2. Флеш HP <180> DMG: <40>                                           | HP:   < 200 >     |
//4 3. Валькирия HP <200> DMG: <30>                                      | DMG:  < 30 >      |
//5 4. Алькатрон HP <250> DMG: <25>                                      |-------------------|
//6 5. Халк HP <300> DMG: <50>                                           | Боец: <Асока>     |
//7 6. Бэтмен HP <150> DMG: <45>                                         | HP:   < 300 >     |
//8 7. Король-Лич HP <500> DMG: <100>                                    | DMG:  < 150 >     |
//9 8. Сплинтер HP <100> DMG: <100>                                      |-------------------|
//0 9. Неуязвимый HP <2000> DMG: <300>                                   
//1 10. Асока HP <300> DMG: <150>                                        
//2                                                                      
//3 Введите номер для выбора первого бойца: 3                             
//4 Вы выбрали - Валькирия! Хороший выбор!                                
//5                                                                       
//6 Введите номер для выбора второго бойца: 10                            
//7 Вы выбрали - Асока! Да прибудет с вами сила!                         
//8

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Начало великой битвы героев!                                         | Статистика:       |
//2 Нажмите любую клавишу чтобы продолжить...                            | Боец: <Валькирия> |
//3                                                                      | HP:   < 200 >     |
//4                                                                      | DMG:  < 30 >      |
//5                                                                      |-------------------|
//6                                                                      | Боец: <Асока>     |
//7                                                                      | HP:   < 300 >     |
//8                                                                      | DMG:  < 150 >     |
//9                                                                      |-------------------|

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Раунд 1:                                                             | Статистика:       |
//2 1. Валькирия атакует Асока и наносит 30 урона                        | Боец: <Валькирия> |
//3 2. Асока атакует Валькирия и наносит 150 урона                       | HP:   < 50 >     |
//4                                                                      | DMG:  < 30 >      |
//5                                                                      |-------------------|
//6                                                                      | Боец: <Асока>     |
//7                                                                      | HP:   < 270 >     |
//8                                                                      | DMG:  < 150 >     |
//9                                                                      |-------------------|

//      A    |     B   |     C   |     D   |     E   |     F   |     G   |     J   |     L   |
//  012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789
//1 Раунд 1:                                                             | Статистика:       |
//2 1. Валькирия атакует Асока и наносит 30 урона                        | Боец: <Валькирия> |
//3 2. Асока атакует Валькирия и наносит 150 урона                       | Боец мёртв!       |
//4                                                                      |                   |
//5 Раунд 2:                                                             |-------------------|
//6 1. Валькирия атакует Асока и наносит 30 урона                        | Боец: <Асока>     |
//7 2. Асока атакует Валькирия и наносит 150 урона                       | Победитель!       |
//8 3. Боец Валькирия пала в битве героев                                |                   |
//9 4. Боец Асока получил титул чемпиона!                                |-------------------|
//0                                                                                           
//1 Нажмите любую клавишу чтобы продолжить...                                                
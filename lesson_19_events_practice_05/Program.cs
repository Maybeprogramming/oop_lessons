using System.Dynamic;

namespace lesson_19_events_practice_05
{
    class Program
    {
        static void Main()
        {
            Console.WindowWidth = 100;
            Console.CursorVisible = false;

            BattleField battleField = new();

            battleField.BeginBattle();
        }
    }

    class BattleField
    {
        private List<Fighter> _availableFighters;
        private List<Fighter> _selectedFighters;
        private bool isSelectedFighters = false;

        public BattleField()
        {
            _availableFighters = FillFightersList();
            _selectedFighters = new();
        }

        public void BeginBattle()
        {
            ListBar fightersList = new ListBar(_availableFighters);
            ListBar.SelectedElement += ChooseFighter;

            // ВЕРНИСЬ СЮДА
            StatsBar statsBar = new StatsBar();
            statsBar.SetPosition(new Point(60, 0));
            statsBar.Drow();
            /// Да сюда!

            KeyControl keyboardControl = new KeyControl();
            keyboardControl.Enable();

            while (isSelectedFighters == false)
            {
                Display.Print($"Список бойцов для выбора:", new Point(0, 0), ConsoleColor.Green);
                fightersList.SetPosition(new Point(0, 1));
                fightersList.Drow();
                keyboardControl.WaitReadKey();

                if (_selectedFighters.Count == 2)
                    DispayChooseFighters();

                Task.Delay(20).Wait();
            }

            //
            statsBar.SetPosition(new Point(60, 0));
            statsBar.Drow();
            new Point(0, 4);
            //

            DownloadArena();

            Fighter fighter1 = _selectedFighters[0];
            Fighter fighter2 = _selectedFighters[1];

            while (fighter1.IsAlive == true && fighter2.IsAlive == true)
            {
                if (fighter2.IsAlive == true)
                {
                    fighter1.Attack(fighter2);
                }

                if (fighter1.IsAlive == true)
                {
                    fighter2.Attack(fighter1);
                }

                Task.Delay(200).Wait();
            }

            CheckVictory(fighter1, fighter2);

            Console.WriteLine("\n\nПрограмма завершена!!!");
            Console.ReadLine();
        }

        public void ChooseFighter(int numberFighter)
        {
            if (numberFighter >= 0 && numberFighter < _availableFighters.Count)
            {
                if (_selectedFighters.Count < 2)
                {
                    _selectedFighters.Add(_availableFighters[numberFighter]);

                    if (_selectedFighters.Count == 2)
                    {
                        Display.Print($"\nВы выбрали двух бойцов, нажмите любую клавишу для продолжения...", new Point(), ConsoleColor.Green);
                        Console.ReadLine();
                    }
                }
            }
        }

        private void DownloadArena()
        {
            int delayCicle = 10;
            int delayMiliseconds = 50;
            char oneSymbol = '|';
            char twoSymbol = '/';
            char threeSymbol = '-';
            char fourSymbol = '\\';
            ConsoleColor textColor = ConsoleColor.Green;

            Display.Print($"\nПодготовка арены для боя: ", new Point(), textColor);

            while (true)
            {
                if (delayCicle == 0)
                {
                    Display.Print($"{oneSymbol}\n", new Point(), textColor);
                    return;
                }

                Display.Print($"{oneSymbol}", new Point(), textColor);
                Task.Delay(delayMiliseconds).Wait();
                Display.Print($"{twoSymbol}", new Point(), textColor);
                Task.Delay(delayMiliseconds).Wait();
                Console.Write($"{threeSymbol}", new Point(), textColor);
                Task.Delay(delayMiliseconds).Wait();
                Display.Print($"{fourSymbol}", new Point(), textColor);
                delayCicle--;
            }
        }

        private void DispayChooseFighters()
        {
            Console.Clear();
            Display.Print($"Выбранные бойцы:\n", new Point(), ConsoleColor.Green);

            foreach (var fighter in _selectedFighters)
            {
                Display.Print(fighter.Name + "\n");
            }

            isSelectedFighters = true;
            ListBar.SelectedElement -= ChooseFighter;
            Display.Print("Нажмите любую клавишу чтобы начать схватку...", new Point(), ConsoleColor.Green);
            Console.ReadLine();
        }

        public void CheckVictory(Fighter fighter1, Fighter fighter2)
        {
            if (fighter1.IsAlive == false && fighter2.IsAlive == false)
            {
                Display.Print($"Ничья! Оба бойца пали");
            }
            else if (fighter1.IsAlive == true && fighter2.IsAlive == false)
            {
                Display.Print($"{fighter1.Name} - победил в этом сражении");
            }
            else if (fighter1.IsAlive == false && fighter2.IsAlive == true)
            {
                Display.Print($"{fighter2.Name} - победил в этом сражении");
            }
        }

        private List<Fighter> FillFightersList()
        {
            Random random = new Random();
            List<string> nameList = new()
            {
                "Валькирия",
                "Алькатрон",
                "Железный человек",
                "Тор",
                "Халк",
                "Черная вдова",
                "Стрела",
                "Человек муравей",
                "Алая ведьма",
                "Черная пантера",
                "Доктор Стрэнж",
                "Грут",
                "Вижен",
                "Звездный лорд",
                "Зимний солдат",
                "Локи",
                "Гамора",
                "Дедпул",
                "Дракс",
                "Ракета"
            };
            _availableFighters = new List<Fighter>();

            for (int i = 0; i < nameList.Count; i++)
            {
                _availableFighters.Add(new Fighter(nameList[i], random.Next(20, 30), random.Next(5, 15), random.Next(100, 150)));
            }

            return _availableFighters;
        }
    }

    class Fighter
    {
        private int _health;
        public Fighter(string name, int damage, int armor, int health)
        {
            Name = name;
            Damage = damage;
            Armor = armor;
            Health = health;
        }

        public string Name { get; private set; }
        public int Damage { get; private set; }
        public int Armor { get; private set; }
        public int Health
        {
            get { return _health; }
            private set
            {
                if (value <= 0)
                {
                    _health = 0;
                }
                else
                {
                    _health = value;
                }
            }
        }
        public bool IsAlive { get => Health > 0; }

        public void TakeDamage(int damage)
        {
            if (Health > 0)
            {
                Health -= damage - Armor;
            }

            Console.Write($"{Name} получил {damage} урона. Осталось здоровья: {Health}\n");
        }

        public void Attack(Fighter target)
        {
            if (IsAlive == true && target.IsAlive == true)
            {
                Console.Write($"{Name} атаковал {target.Name}! ");
                target.TakeDamage(Damage);
            }
        }

        public string ShowInfo()
        {
            return $"{Name}| STATS: {Health} HP, {Damage} DMG, {Armor} ARMOR";
        }
    }

    class UserInterface
    {
        public string Text { get; private set; }
        public ConsoleColor ColorText { get; set; } = ConsoleColor.White;
        public Point Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public virtual void Drow()
        {
            Display.Print(Text, Position, ColorText);
        }

        public virtual void SetPosition(Point point)
        {
            Position = point;
        }
    }

    class ListBar : UserInterface
    {
        private static List<Fighter> _list;
        private static int _activeElement = 0;
        private static int _elementsCount;
        public static event Action<int>? SelectedElement;

        public ListBar(List<Fighter> list)
        {
            _list = list;
            _elementsCount = _list.Count;
            BackColor = ConsoleColor.Yellow;
            TextColor = ConsoleColor.Red;
        }

        public ConsoleColor BackColor { get; private set; }
        public ConsoleColor TextColor { get; private set; }

        public override void Drow()
        {
            ConsoleColor defaultBackgroundColor = Console.BackgroundColor;
            ConsoleColor defaultTextColor = Console.ForegroundColor;
            int number = 0;

            for (int i = 0; i < _elementsCount; i++)
            {
                if (i == _activeElement)
                {
                    Console.BackgroundColor = BackColor;

                    UpdateString(_list[i], ref number, i, ConsoleColor.Red);

                    Console.BackgroundColor = defaultBackgroundColor;
                    Console.ForegroundColor = defaultTextColor;
                }
                else
                {
                    UpdateString(_list[i], ref number, i);
                }
            }
        }

        public static void OnChangeActiveElement(object? sender, KeyEventArgs e)
        {
            if (e.Key == ConsoleKey.UpArrow)
            {
                _activeElement = ChangeNegative(_activeElement);
            }
            else if (e.Key == ConsoleKey.DownArrow)
            {
                _activeElement = ChangePositive(_activeElement);
            }
            else if (e.Key == ConsoleKey.Enter)
            {
                ClearOneString();
                Display.Print($"\nВы выбрали: {_list[_activeElement].Name}!");
                SelectedElement?.Invoke(_activeElement);
            }
        }

        private void UpdateString(Fighter fighter, ref int number, int currentPosition, ConsoleColor textColor = ConsoleColor.White)
        {
            Display.Print($"{++number}. {fighter.Name} | Stats: ", new Point(Position.X, Position.Y + currentPosition), textColor);
            Display.Print($"{fighter.Health}", new Point(), ConsoleColor.Green);
            Display.Print($" HP ", new Point(), textColor);
            Display.Print($"{fighter.Damage}", new Point(), ConsoleColor.Red);
            Display.Print($" DMG ", new Point(), textColor);
            Display.Print($"{fighter.Armor}", new Point(), ConsoleColor.Blue);
            Display.Print($" ARMOR\n", new Point(), textColor);
        }

        private static void ClearOneString()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.WriteLine("\n" + new string(' ', 100));
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }

        private static int ChangePositive(int activeElement)
        {
            if (activeElement < _elementsCount - 1)
            {
                return ++activeElement;
            }
            else
            {
                return 0;
            }
        }

        private static int ChangeNegative(int activeElement)
        {
            if (activeElement <= 0)
            {
                return _elementsCount - 1;
            }
            else
            {
                return --activeElement;
            }
        }
    }

    class StatsBar : UserInterface
    {
        private Fighter? _firstFighter = null;
        private Fighter? _secondFighter = null;
        private Fighter?[] fighters;

        public StatsBar(Fighter? firstFighter = null, Fighter? secondFighter = null)
        {
            _firstFighter = firstFighter;
            _secondFighter = secondFighter;
        }

        public override void Drow()
        {
            Display.Print("Stats Bar", Position, ColorText);
        }

        public void OnChanged(object sender, StatsBarEventsArgs e)
        {

        }
    }

    class StatsBarEventsArgs : EventArgs
    {
        public StatsBarEventsArgs(Fighter? fighter)
        {
            Fighter = fighter;
        }

        public Fighter? Fighter { get; }
    }
    class KeyControl
    {
        public event EventHandler<KeyEventArgs>? UpArrowKeyPressed;
        public event EventHandler<KeyEventArgs>? DownArrowKeyPressed;
        public event EventHandler<KeyEventArgs>? EnterKeyPressed;

        public void WaitReadKey()
        {
            ConsoleKeyInfo consoleKey;
            consoleKey = Console.ReadKey();

            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    UpArrowKeyPressed?.Invoke(this, new KeyEventArgs(consoleKey.Key));
                    break;

                case ConsoleKey.DownArrow:
                    DownArrowKeyPressed?.Invoke(this, new KeyEventArgs(consoleKey.Key));
                    break;

                case ConsoleKey.Enter:
                    EnterKeyPressed?.Invoke(this, new KeyEventArgs(consoleKey.Key));
                    break;
            }
        }

        public void Enable()
        {
            UpArrowKeyPressed += ListBar.OnChangeActiveElement;
            DownArrowKeyPressed += ListBar.OnChangeActiveElement;
            EnterKeyPressed += ListBar.OnChangeActiveElement;
        }

        public void Disable()
        {
            UpArrowKeyPressed -= ListBar.OnChangeActiveElement;
            DownArrowKeyPressed -= ListBar.OnChangeActiveElement;
            EnterKeyPressed -= ListBar.OnChangeActiveElement;
        }
    }

    public class KeyEventArgs : EventArgs
    {
        public KeyEventArgs(ConsoleKey key)
        {
            Key = key;
        }

        public ConsoleKey Key { get; }
    }

    struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
            X = x;
            Y = y;
        }
    }

    static class Display
    {
        public static void Print(string text, Point point = new Point(), ConsoleColor consoleColor = ConsoleColor.White)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.Write($"{text}");
            Console.ForegroundColor = defaultColor;
        }
    }
}
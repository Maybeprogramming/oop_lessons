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
        private event EventHandler<FighterEventsArgs> _changedSelectionFighters;
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
            ListBar fightersListBar = new ListBar(_availableFighters, new Point(0, 0));
            fightersListBar.SelectedElement += ChooseFighter;

            // ВЕРНИСЬ СЮДА
            StatsBar statsBar = new StatsBar(_selectedFighters, new Point(60, 0));
            _changedSelectionFighters += statsBar.OnChanged;
            statsBar.Drow();
            /// Да сюда!

            KeyControl keyboardControl = new KeyControl();
            keyboardControl.Enable(fightersListBar);

            while (isSelectedFighters == false)
            {
                fightersListBar.Drow();
                keyboardControl.WaitReadKey();


                if (_selectedFighters.Count == 2)
                {
                    DispayChooseFighters();

                    //Console.Clear();
                    //isSelectedFighters = true;
                    //_changedSelectionFighters.Invoke(this, new FighterEventsArgs(null));
                    //Display.Print("Нажмите любую клавишу чтобы начать схватку...", ConsoleColor.Green);
                    //Console.ReadLine();
                }

                Task.Delay(50).Wait();
            }

            //
            statsBar.Drow();
            //

            fightersListBar.SelectedElement -= ChooseFighter;

            OnChangeStatsFighters(statsBar);
            DownloadArena();

            Fighter fighter1 = _selectedFighters[0];
            Fighter fighter2 = _selectedFighters[1];

            while (fighter1.IsAlive == true && fighter2.IsAlive == true)
            {
                fighter1.Attack(fighter2);
                fighter2.Attack(fighter1);

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
                    _changedSelectionFighters?.Invoke(this, new FighterEventsArgs(_availableFighters[numberFighter]));

                    if (_selectedFighters.Count == 2)
                    {
                        Console.SetCursorPosition(0, 22);
                        Display.Print($"\nВы выбрали двух бойцов, нажмите любую клавишу для продолжения...", ConsoleColor.Green);
                        Console.ReadKey();
                    }
                }
            }
        }

        private void OnChangeStatsFighters(StatsBar statsBar)
        {
            foreach (var fighter in _selectedFighters)
            {
                fighter.FighterChanged += statsBar.OnChanged;
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

            Display.Print($"\nПодготовка арены для боя: ", textColor);

            while (true)
            {
                if (delayCicle == 0)
                {
                    Display.Print($"{oneSymbol}\n", textColor);
                    return;
                }

                Display.Print($"{oneSymbol}", textColor);
                Task.Delay(delayMiliseconds).Wait();
                Display.Print($"{twoSymbol}", textColor);
                Task.Delay(delayMiliseconds).Wait();
                Display.Print($"{threeSymbol}", textColor);
                Task.Delay(delayMiliseconds).Wait();
                Display.Print($"{fourSymbol}", textColor);
                delayCicle--;
            }
        }

        private void DispayChooseFighters()
        {
            Console.Clear();
            _changedSelectionFighters.Invoke(this, new FighterEventsArgs(null));
            Display.Print($"Выбранные бойцы:\n", ConsoleColor.Green);

            foreach (var fighter in _selectedFighters)
            {
                Display.Print(fighter.Name + "\n");
            }

            isSelectedFighters = true;
            Display.Print("Нажмите любую клавишу чтобы начать схватку...", ConsoleColor.Green);
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
                Display.Print($"Победил в этом сражении - {fighter1.ShowInfo()}");
            }
            else if (fighter1.IsAlive == false && fighter2.IsAlive == true)
            {
                Display.Print($"Победил в этом сражении - {fighter2.ShowInfo()}");
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

        public event EventHandler<FighterEventsArgs> FighterChanged;

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

            FighterChanged?.Invoke(this, new FighterEventsArgs(this));
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
        public UserInterface(Point position)
        {
            Position = position;
        }

        public string Text { get; private set; }
        public ConsoleColor ColorText { get; set; } = ConsoleColor.White;
        public Point Position { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public virtual void Drow()
        {
            Console.SetCursorPosition(Position.X, Position.Y);
            Display.Print(Text, ColorText);
        }

        public virtual void SetPosition(Point position)
        {
            Position = position;
        }

        protected virtual void ClearOneString()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.WriteLine("\n" + new string(' ', 100));
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }
    }

    class ListBar : UserInterface
    {
        private List<Fighter> _list;
        private int _activeElement = 0;
        private int _elementsCount;
        public event Action<int>? SelectedElement;

        public ListBar(List<Fighter> list, Point position) : base(position)
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

            Console.SetCursorPosition(Position.X, Position.Y);
            Display.Print($"Список бойцов для выбора:", ConsoleColor.Green);

            for (int i = 0; i < _elementsCount; i++)
            {
                if (i == _activeElement)
                {
                    Console.BackgroundColor = BackColor;
                    Console.SetCursorPosition(Position.X, Position.Y + i + 1);

                    UpdateString(_list[i], ref number, i, ConsoleColor.Red);

                    Console.BackgroundColor = defaultBackgroundColor;
                    Console.ForegroundColor = defaultTextColor;
                }
                else
                {
                    Console.SetCursorPosition(Position.X, Position.Y + i + 1);
                    UpdateString(_list[i], ref number, i);
                }
            }
        }

        public void OnChangeActiveElement(object? sender, KeyEventArgs e)
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
                //ClearOneString();
                //Display.Print($"\nВы выбрали: {_list[_activeElement].Name}!");
                SelectedElement?.Invoke(_activeElement);
            }
        }

        private void UpdateString(Fighter fighter, ref int number, int currentPosition, ConsoleColor textColor = ConsoleColor.White)
        {
            Display.Print($"{++number}. {fighter.Name} | Stats: ", textColor);
            Display.Print($"{fighter.Health}", ConsoleColor.Green);
            Display.Print($" HP ", textColor);
            Display.Print($"{fighter.Damage}", ConsoleColor.Red);
            Display.Print($" DMG ", textColor);
            Display.Print($"{fighter.Armor}", ConsoleColor.Blue);
            Display.Print($" ARMOR\n", textColor);
        }

        private int ChangePositive(int activeElement)
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

        private int ChangeNegative(int activeElement)
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
        private List<Fighter?> _fighters;

        public StatsBar(List<Fighter?> fighters, Point position) : base(position)
        {
            _fighters = fighters;
        }

        public override void Drow()
        {
            Point tempPosition = new Point(Console.CursorLeft, Console.CursorTop);

            Console.SetCursorPosition(Position.X, Position.Y);
            Display.Print($"Статы бойцов:", ConsoleColor.Red);

            if (_fighters.Count == 0)
            {
                Console.SetCursorPosition(Position.X, Position.Y + 1);
                Console.Write(new string(' ', 30));

                Console.SetCursorPosition(Position.X, Position.Y + 1);
                Display.Print($"Бойцы - не выбраны!", ConsoleColor.White);
            }

            for (int i = 0; i < _fighters.Count; i++)
            {
                if (_fighters[i] != null)
                {
                    Console.SetCursorPosition(Position.X, Position.Y + i + 1);
                    Console.Write(new string(' ', 30));

                    Console.SetCursorPosition(Position.X, Position.Y + i + 1);
                    Display.Print($"{i + 1}). {_fighters[i].Name}, Здоровье [{_fighters[i].Health}]", ConsoleColor.White);
                }
            }

            Console.SetCursorPosition(tempPosition.X, tempPosition.Y);
        }

        public void OnChanged(object sender, FighterEventsArgs e)
        {
            Drow();
        }
    }

    class FighterEventsArgs : EventArgs
    {
        public FighterEventsArgs(Fighter? fighter)
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

        public void Enable(ListBar listBar)
        {
            UpArrowKeyPressed += listBar.OnChangeActiveElement;
            DownArrowKeyPressed += listBar.OnChangeActiveElement;
            EnterKeyPressed += listBar.OnChangeActiveElement;
        }

        public void Disable(ListBar listBar)
        {
            UpArrowKeyPressed -= listBar.OnChangeActiveElement;
            DownArrowKeyPressed -= listBar.OnChangeActiveElement;
            EnterKeyPressed -= listBar.OnChangeActiveElement;
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

        public Point(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
    }

    static class Display
    {
        public static void Print(string text, ConsoleColor consoleColor = ConsoleColor.White)
        {
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.Write($"{text}");
            Console.ForegroundColor = defaultColor;
        }
    }
}
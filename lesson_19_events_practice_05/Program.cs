﻿namespace lesson_19_events_practice_05
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

        public BattleField()
        {
            _availableFighters = FillFightersList();
            _selectedFighters = new();
        }

        public void BeginBattle()
        {
            bool isSelectedFighters = false;
            ListBar controlList = new ListBar(_availableFighters);

            KeyboardControl keyboardControl = new KeyboardControl();
            keyboardControl.OnEnable();

            while (isSelectedFighters == false)
            {
                Display.Print($"Список бойцов для выбора:", new Point(0, 0));
                controlList.SetPosition(new Point(0, 1));
                controlList.Drow();

                keyboardControl.WaitReadKey();

                if (_selectedFighters.Count == 2)
                {
                    Console.Clear();
                    Display.Print($"Выбранные бойцы:\n");

                    foreach (var fighter in _selectedFighters)
                    {
                        Display.Print(fighter.ShowInfo() + "\n");
                    }

                    isSelectedFighters = true;
                }

                Task.Delay(20).Wait();
            }

            Console.WriteLine("\nПрограмма завершена!!!");
            Console.ReadLine();
        }

        public void ChooseFighter(int numberFighter)
        {

        }

        public void CheckVictory()
        {

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
        public int Health { get; private set; }
        public bool IsAlive { get => Health > 0; }

        public void TakeDamage(int damage)
        {
            if (Health > 0)
            {
                Health -= damage - Armor;
            }
            else
            {
                Health = 0;
            }
        }

        public void Attack(Fighter target)
        {
            if (target.IsAlive == true)
            {
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

                    Display.Print($"{++number}. {_list[i].Name} | Stats: ", new Point(Position.X, Position.Y + i), TextColor);
                    Display.Print($"{_list[i].Health}", new Point(), ConsoleColor.Green);
                    Display.Print($" HP ", new Point(), TextColor);
                    Display.Print($"{_list[i].Damage}", new Point(), ConsoleColor.Red);
                    Display.Print($" DMG ", new Point(), TextColor);
                    Display.Print($"{_list[i].Armor}", new Point(), ConsoleColor.Blue);
                    Display.Print($" ARMOR\n", new Point(), TextColor);

                    Console.BackgroundColor = defaultBackgroundColor;
                    Console.ForegroundColor = defaultTextColor;
                }
                else
                {
                    Display.Print($"{++number}. {_list[i].Name} | Stats: ", new Point(Position.X, Position.Y + i));
                    Display.Print($"{_list[i].Health}", new Point(), ConsoleColor.Green);
                    Display.Print($" HP ");
                    Display.Print($"{_list[i].Damage}", new Point(), ConsoleColor.Red);
                    Display.Print($" DMG ");
                    Display.Print($"{_list[i].Armor}", new Point(), ConsoleColor.Blue);
                    Display.Print($" ARMOR\n");
                }
            }
        }

        public static void OnChangeActiveElement(object? sender, KeyboardEventArgs e)
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
                Display.Print($"Вы выбрали: {_list[_activeElement].Name}!");
            }
        }

        private static void ClearOneString()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.WriteLine(new string(' ', 50));
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

    class KeyboardControl
    {
        public event EventHandler<KeyboardEventArgs>? UpArrowKeyPressed;
        public event EventHandler<KeyboardEventArgs>? DownArrowKeyPressed;
        public event EventHandler<KeyboardEventArgs>? EnterKeyPressed;

        public void WaitReadKey()
        {
            ConsoleKeyInfo consoleKey;
            consoleKey = Console.ReadKey();

            switch (consoleKey.Key)
            {
                case ConsoleKey.UpArrow:
                    UpArrowKeyPressed?.Invoke(this, new KeyboardEventArgs(consoleKey.Key));
                    break;

                case ConsoleKey.DownArrow:
                    DownArrowKeyPressed?.Invoke(this, new KeyboardEventArgs(consoleKey.Key));
                    break;

                case ConsoleKey.Enter:
                    EnterKeyPressed?.Invoke(this, new KeyboardEventArgs(consoleKey.Key));
                    break;
            }
        }

        public void OnEnable()
        {
            UpArrowKeyPressed += ListBar.OnChangeActiveElement;
            DownArrowKeyPressed += ListBar.OnChangeActiveElement;
            EnterKeyPressed += ListBar.OnChangeActiveElement;
        }

        public void OnDisable()
        {
            UpArrowKeyPressed -= ListBar.OnChangeActiveElement;
            DownArrowKeyPressed -= ListBar.OnChangeActiveElement;
            EnterKeyPressed -= ListBar.OnChangeActiveElement;
        }
    }

    public class KeyboardEventArgs : EventArgs
    {
        public KeyboardEventArgs(ConsoleKey key)
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
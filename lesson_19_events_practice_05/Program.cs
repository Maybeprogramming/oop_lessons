namespace lesson_19_events_practice_05
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;
            KeyboardControl keyboardControl = new KeyboardControl(); 
            keyboardControl.OnEnable();

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

            List<Fighter> fightersSelected = new();
            List<Fighter> fighterList = new();

            for (int i = 0; i < nameList.Count; i++)
            {
                fighterList.Add(new Fighter(nameList[i]));
            }

            bool isSelectedFighters = false;
            FightersListBar controlList = new FightersListBar(fighterList);

            while (isSelectedFighters == false)
            {
                Display.Print($"Список бойцов для выбора:", new Point(0, 0));
                controlList.SetPosition(new Point(0, 1));
                controlList.Drow();

                keyboardControl.WaitReadKey();

                if (fightersSelected.Count == 2)
                {
                    Console.Clear();
                    Display.Print($"Выбранные бойцы:\n");

                    foreach (var fighter in fightersSelected)
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
    }

    class Fighter
    {
        public string Name { get; private set; }

        public Fighter(string name)
        {
            Name = name;
        }

        public string ShowInfo()
        {
            return $"{Name}";
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

    class FightersListBar : UserInterface
    {
        private static List<string> _list;
        private static int _selectedElement = 0;

        public FightersListBar(List<Fighter> list)
        {
            _list = list.Select(e => e.Name).ToList();
            BackColor = ConsoleColor.Yellow;
            TextColor = ConsoleColor.Red;
        }

        public ConsoleColor BackColor { get; set; }
        public ConsoleColor TextColor { get; set; }

        public override void Drow()
        {
            ConsoleColor defaultBackgroundColor = Console.BackgroundColor;
            ConsoleColor defaultTextColor = Console.ForegroundColor;
            int number = 0;

            for (int i = 0; i < _list.Count; i++)
            {
                if (i == _selectedElement)
                {
                    Console.BackgroundColor = BackColor;
                    Display.Print($"{++number}. {_list[i]}\n", new Point(Position.X, Position.Y + i), TextColor);
                    Console.BackgroundColor = defaultBackgroundColor;
                    Console.ForegroundColor = defaultTextColor;
                }
                else
                {
                    Display.Print($"{++number}. {_list[i]}\n", new Point(Position.X, Position.Y + i));
                }
            }
        }

        public static void OnPressKey(object? sender, KeyboardEventArgs e)
        {
            if (e.Key == ConsoleKey.UpArrow)
            {
                _selectedElement = ChangeNegative(_list.Count, _selectedElement);
            }
            else if (e.Key == ConsoleKey.DownArrow)
            {
                _selectedElement = ChangePositive(_list.Count, _selectedElement);
            }
            else if (e.Key == ConsoleKey.Enter)
            {
                ClearOneString();
                Display.Print($"Вы выбрали: {_list[_selectedElement]}!");
            }
        }

        private static void ClearOneString()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.WriteLine(new string(' ', 40));
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }

        private static int ChangePositive(int listCount, int counter)
        {
            if (counter < listCount - 1)
            {
                return ++counter;
            }
            else
            {
                return counter = 0;
            }
        }

        private static int ChangeNegative(int listCount, int counter)
        {
            if (counter <= 0)
            {
                return counter = listCount - 1;
            }
            else
            {
                return --counter;
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
            UpArrowKeyPressed += FightersListBar.OnPressKey;
            DownArrowKeyPressed += FightersListBar.OnPressKey;
            EnterKeyPressed += FightersListBar.OnPressKey;
        }

        public void OnDisable()
        {
            UpArrowKeyPressed -= FightersListBar.OnPressKey;
            DownArrowKeyPressed -= FightersListBar.OnPressKey;
            EnterKeyPressed -= FightersListBar.OnPressKey;
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
namespace lesson_19_events_practice_05
{
    class Program
    {
        static void Main()
        {
            Console.CursorVisible = false;

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
            int numberSelected = 0;

            ControlList controlList = new ControlList(fighterList);

            while (isSelectedFighters == false)
            {
                Display.Print($"Список бойцов для выбора:", new Point(0, 0));
                controlList.SetPosition(new Point(0, 1));
                controlList.Drow();

                ConsoleKeyInfo consoleKey;
                consoleKey = Console.ReadKey();

                switch (consoleKey.Key)
                {
                    case ConsoleKey.UpArrow: 
                        numberSelected = ChangeNegative(fighterList.Count, numberSelected); 
                        break;

                    case ConsoleKey.DownArrow: 
                        numberSelected = ChangePositive(fighterList.Count, numberSelected); 
                        break;

                    case ConsoleKey.Enter: 
                        SelectHero(numberSelected, fighterList, fightersSelected); 
                        break;
                }

                controlList.SelectElement(numberSelected);

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

        private static void SelectHero(int numberSelected, List<Fighter> listHero, List<Fighter> selectedFighters)
        {
            if (selectedFighters.Count < 2)
            {
                selectedFighters.Add(listHero[numberSelected]);
                ClearOneString();
                Display.Print($"\nВы выбрали: {listHero[numberSelected].ShowInfo()}!!!");
            }
        }

        private static void ClearOneString()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.WriteLine("\n" + new string(' ', 40));
            Console.CursorLeft = left;
            Console.CursorTop = top;
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

    class ControlList : UserInterface
    {
        private List<string> _list;
        private int _selectedElement = 0;

        public ControlList(List<Fighter> list)
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

        public void SelectElement(int number)
        {
            _selectedElement = number;
            Drow();
        }
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
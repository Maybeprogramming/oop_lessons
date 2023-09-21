namespace lesson_16_events_practice_02
{
    class Program
    {
        public static event Action<string>? selectedHero;

        static void Main()
        {
            Console.CursorVisible = false;

            int numberSelected = 0;

            List<string> list = new()
            {
                "Валькирия",
                "Алькатрон",
                "Железный человек",
                "Тор",
                "Халк"
            };

            selectedHero += OnSelectedHero;

            while (true)
            {
                Console.WriteLine("Меню:");
                PrintList(numberSelected, list);

                ConsoleKeyInfo consoleKey;
                consoleKey = Console.ReadKey();

                switch (consoleKey.Key)
                {
                    case ConsoleKey.UpArrow: numberSelected = ChangeNegative(numberSelected); break;
                    case ConsoleKey.DownArrow: numberSelected = ChangePositive(list.Count, numberSelected); break;
                    case ConsoleKey.Enter: LetsGoToDance(numberSelected, list); break;
                }

                Console.CursorLeft = 0;
                Console.CursorTop = 0;

                Task.Delay(20).Wait();
            }
        }

        private static void LetsGoToDance(int numberSelected, List<string> list)
        {
            ClearOneString();
            Console.WriteLine($"\nВы выбрали: {list[numberSelected]}!!!");
            selectedHero?.Invoke(list[numberSelected]);
        }

        private static void ClearOneString()
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;
            Console.WriteLine("\n" + new string(' ', 40));
            Console.CursorLeft = left;
            Console.CursorTop = top;
        }

        public static void OnSelectedHero(string hero)
        {
            Console.CursorTop = 10;

            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            Console.WriteLine("\n" + new string(' ', 40));

            Console.CursorLeft = left;
            Console.CursorTop = top;

            Console.WriteLine("\n" + hero + " здоровье: " + new Random().Next(50, 100));
        }

        static void PrintList(int selectString, List<string> list, ConsoleColor backColor = ConsoleColor.Yellow, ConsoleColor textColor = ConsoleColor.Red)
        {
            ConsoleColor defaultBackgroundColor = Console.BackgroundColor;
            ConsoleColor defaultTextColor = Console.ForegroundColor;
            int counter = 0;

            for (int i = 0; i < list.Count; i++)
            {
                if (i == selectString)
                {
                    Console.BackgroundColor = backColor;
                    Console.ForegroundColor = textColor;
                    Console.WriteLine(++counter + ". " + list[i]);
                    Console.BackgroundColor = defaultBackgroundColor;
                    Console.ForegroundColor = defaultTextColor;
                }
                else
                {
                    Console.WriteLine(++counter + ". " + list[i]);
                }
            }
        }

        static int ChangePositive(int listCount, int counter)
        {
            if (counter < listCount - 1)
            {
                return ++counter;
            }
            else
            {
                return counter;
            }
        }

        static int ChangeNegative(int counter)
        {
            if (counter <= 0)
            {
                return counter;
            }
            else
            {
                return --counter;
            }
        }
    }
}
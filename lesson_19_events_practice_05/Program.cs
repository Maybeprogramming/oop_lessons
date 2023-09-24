using System.Security.Principal;

namespace lesson_19_events_practice_05
{
    class Program
    {
        static void Main()
        {
            UserInterface str = new UserInterface(new Point(10,10), 10, 1, "Валькирия");
            str.ColorText = ConsoleColor.Green;
            str.Drow();

            Console.ReadLine();
        }
    }

    class UserInterface
    {
        public UserInterface(Point point, int width = 10, int height = 1, string name = "default")
        {
            Point = point;
            Width = width; 
            Height = height;
            Text = name;
        }

        public string Text { get; private set; }
        public ConsoleColor ColorText { get; set; } = ConsoleColor.White;
        public Point Point { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public virtual void Drow()
        {
            Display.Print(Text, Point, ColorText);
        }
    }

    class ControlList : UserInterface
    {
        private List<string> _list;

        public ControlList(List<string> list, Point point, int width = 10, int height = 1, string name = "ControlList") : base(point, width, height, name)
        {
            _list = list;
        }

        public override void Drow()
        {
            int number = 0;

            foreach (var element in _list)
            {
                Display.Print($"{++number}. {element}", Point);
            }
        }
    }

    struct Point
    {
        public int X { get => Console.CursorLeft; }
        public int Y { get => Console.CursorTop; }

        public Point(int x, int y)
        {
            Console.CursorLeft = x;
            Console.CursorTop = y;
        }
    }

    static class Display 
    { 
        public static void Print (string text, Point point, ConsoleColor consoleColor = ConsoleColor.White)
        { 
            Point printPoint = point;
            ConsoleColor defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.Write($"{text}");
            Console.ForegroundColor = defaultColor;
        }
    }

}
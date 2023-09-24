﻿using System.Diagnostics.Metrics;
using System.Security.Principal;

namespace lesson_19_events_practice_05
{
    class Program
    {
        static void Main()
        {
            List<string> list = new()
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

            ControlList controlList = new ControlList(list, new Point(0,0));
            controlList.SelectElement(19);

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
        private int _selectedElement = 0;

        public ControlList(List<string> list, Point point, int width = 10, int height = 1, string name = "ControlList") : base(point, width, height, name)
        {
            _list = list;
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
                    Display.Print($"{++number}. {_list[i]}\n", new Point(Point.X, i), TextColor);
                    Console.BackgroundColor = defaultBackgroundColor;
                    Console.ForegroundColor = defaultTextColor;
                }
                else
                {
                    Display.Print($"{++number}. {_list[i]}\n", new Point(Point.X, i));
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
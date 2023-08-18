namespace oop_lesson_04
{
    class Program
    {
        static void Main()
        {
            Renderer renderer = new Renderer();
            Player player = new Player(55, 10);

            renderer.Draw(player.X, player.Y);
        }
    }

    class Renderer
    {
        public void Draw(int x, int y, char character = '@')
        {
            Console.CursorVisible = false;
            Console.SetCursorPosition(x, y);
            Console.Write(character);

            Console.ReadKey(true);
        }
    }

    class Player
    {
        public Player(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}

//Инкапсуляция
//- подразумевает сокрытие данных и состояния от неправильного внешнего воздействия
// Console.ReadKey(true); - уберет надпись в консоли - "Для продолжения нажмите любую клавишу..."

//Свойства в c#
//При помощи свойств достигли защиты объекта от неправильного внешнего воздействия
namespace lesson_12_class_and_struct
{
    class Program
    {
        static void Main()
        {
            Vector2 targetPosition = new Vector2(10, 10);
            Vector2 playerPosition = targetPosition;
            playerPosition.X += 15;

            Console.WriteLine(targetPosition.X);

            Console.WriteLine(new string('-', 30));
            GameObject bullet = new GameObject();

            Console.WriteLine(bullet.Position.X);

            //Распространенная ошибка работы со структурами
            //bullet.Position.X = 50;

            //А надо работать вот так:
            Vector2 newPosition = bullet.Position;
            newPosition.X = 50;
            bullet.Position = newPosition;

            Console.WriteLine(bullet.Position.X);

            Console.ReadLine();
        }
    }

    struct Vector2
    {
       public int X, Y;

        public Vector2(int x, int y) 
        {  
            X = x; 
            Y = y; 
        }

        public Vector2(int x): this()
        {
            X = x;
        }
    }

    class GameObject
    {
        public Vector2 Position { get; set; }
    }
}

/// <summary>
/// У структуры все члены по умолчанию имеют 
/// публичный модификатор доступа в отличии от класса
/// 
/// У структур по умолчанию есть конструкторы, только это более сложные вещи
/// 
/// В структуре можно определять свой конструктор,
/// но его нужно обязательно проинициализировать.
/// 
/// Класс - ссылочный тип
/// Структура - тип значение
/// 
/// 
/// </summary>
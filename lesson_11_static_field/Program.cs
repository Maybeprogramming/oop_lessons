namespace lesson_11_static_field
{
    class Program
    {
        static void Main()
        {
            User.Idetifications = 10;
            User user1 = new User();
            User user2 = new User();
            user1.ShowInfo();
            user2.ShowInfo();

            Console.WriteLine(new string ('-', 30));

            Console.WriteLine("Привет Мир!");
            MyClass instance = new MyClass();
            Console.WriteLine(MyClass.StaticField);

            Console.ReadLine();
        }
    }

    //Статические члены и классы

    class User
    {
        public static int Idetifications;
        public int Identification;

        public User()
        {
            Identification = ++Idetifications;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Identification}");
        }
    }

    //Статический конструктор
    //Техника ленивого инициализации
    //Т.е. вызываем конструктор в момент первого взаимодействия с типом
    //Т.е. когда происходит декларирование типа
    //А вызов обычного конструктора - взаимодействие с типом

    class MyClass
    {
        public static float StaticField;

        static MyClass()
        {
            StaticField = 10;
            Console.WriteLine($"Статический конструктор ({StaticField})") ;
        }

        public MyClass()
        {
            Console.WriteLine($"Обычный конструктор");
        }
    }
}

///СТАТИКА ЭТО ПЛОХО!
///<summary> 
/// Задача ООП это:
/// 1. выделение сущностей
/// 2. защита их состояния
/// 3. построение абстракций
/// и всё это основывается на композиции и взаимосвязанности
/// 
/// Всё глобальное можно разбить на композицию объектов
/// 
/// При описании типов можно использовать структуры
/// Схожая с классом но отличается симантикой
/// Объект класса это сущность и он не равен другому объекту, 
/// даже если все поля имеют одинаковые значения
/// 
/// А объект структуры это объект значения
/// и он равен другому объекту если значения всех их полей совпадают
/// </summary>

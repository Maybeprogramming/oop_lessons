namespace lesson_10_abstract_class
{
    class Program
    {
        static void Main()
        {
            Vehicle[] vehicles =
            {
                new Car(),
                new Train()
            };


            foreach (var vehicle in vehicles)
            {
                vehicle.Move();
            }
        }
    }

    abstract class Vehicle
    {
        protected float Speed;

        public abstract void Move();

        public float GetCyrrentSpeed()
        {
            return Speed;
        }
    }

    class Car : Vehicle
    {
        public override void Move()
        {
            Console.WriteLine("Машина едет по асфальту");
        }
    }

    class Train : Vehicle
    {
        public override void Move()
        {
            Console.WriteLine("Поезд едет по рельсам");
        }
    }
}

//Абстрактные классы
// - это как чертежи для будущих классов, от них мы наследуемся,
//Экземпляр абстрактного класса нельзя!!!
//Один или более нереализованных методов как и в интерфейсах,
//А так же методы которые имеют реализацию
//Необходимо реализовать все абстрактные методы
//В чём отличие от интерфейса?
//- в интерфейсе нельзя сделать поле (но можно сделать свойство)
//- также в интерфейсе нельзя создать реализованные методы
//Почему не обычный класс?
//Хочу сделать виртуальный метод и оставить его пустым,
//не нужна реализация в базовом классе, а в дочернем нужна
//то в этом случае точно нужен абстрактный класс

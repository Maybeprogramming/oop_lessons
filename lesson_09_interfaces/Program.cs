namespace lesson_09_interfaces
{
    class Program
    {
        static void Main()
        {
            IMovable car = new Car();
            IMovable human = new Human();
        }
    }

    interface IMovable
    {
        void Move();

        void ShowMoveSpeed();
    }

    interface IBurnable
    {
        void Burn();
    }

    class Vehicle { }

    class Car : Vehicle, IMovable, IBurnable
    {
        public void Burn()
        {

        }

        public void Move()
        {

        }

        public void ShowMoveSpeed()
        {

        }
    }

    class Human : IMovable
    {
        public void Move()
        {

        }

        public void ShowMoveSpeed()
        {

        }
    }
}

//Интерфейсы в С#
//Представляет собой некий набор методов не имеющих реализаций
//Методы интерфейсов должны быть полностью реализованы в классах наследниках
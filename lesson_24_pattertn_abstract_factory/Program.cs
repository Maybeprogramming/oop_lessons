namespace lesson_24_pattertn_abstract_factory
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }

    abstract class AbstractFactory
    {
        public abstract Fighter CreateFighter();
        public abstract Vihicle CreateVihicle();
    }
    class ConcreteFactory1 : AbstractFactory
    {
        public override Fighter CreateFighter()
        {
            return new Sniper();
        }

        public override Vihicle CreateVihicle()
        {
            return new Helicopter();
        }
    }
    class ConcreteFactory2 : AbstractFactory
    {
        public override Fighter CreateFighter()
        {
            return new Stormtrooper();
        }

        public override Vihicle CreateVihicle()
        {
            return new Tank();
        }
    }

    abstract class Fighter
    { }

    abstract class Vihicle
    { }

    class Sniper : Fighter
    { }

    class Helicopter : Vihicle
    { }

    class Stormtrooper : Fighter
    { }

    class Tank : Vihicle
    { }
}
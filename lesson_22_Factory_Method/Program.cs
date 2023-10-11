namespace lesson_22_Factory_Method
{
    class Program
    {
        static void Main()
        {
        }
    }

    abstract class Product { }
    class ConcreteProductA : Product { }
    class ConcreteProductB : Product { }

    abstract class  Creator
    {
        public abstract Product FactoryMethod();
    }

    class ConcreteCreatorA : Creator
    {
        public override Product FactoryMethod()
        {
            return new ConcreteProductA();
        }
    }

    class ConcreteCreatorB : Creator 
    { 
        public override Product FactoryMethod() 
        { 
            return new ConcreteProductB();
        } 
    }
}

#region Теория Патерна - Фабричный метод

//  Порождающие паттерны
//  Фабричный метод (Factory Method)

//  Фабричный метод(Factory Method)
//  -это паттерн, который определяет интерфейс для создания объектов некоторого класса,
//  но непосредственное решение о том, объект какого класса создавать происходит в подклассах.
//  То есть паттерн предполагает, что базовый класс делегирует создание объектов классам-наследникам.

//  --- Когда надо применять паттерн ---
//  1. Когда заранее неизвестно, объекты каких типов необходимо создавать
//  2. Когда система должна быть независимой от процесса создания новых объектов и расширяемой:
//  в нее можно легко вводить новые классы, объекты которых система должна создавать.
//  3. Когда создание новых объектов необходимо делегировать из базового класса классам наследникам

//  Участники:
//  1. Абстрактный класс Product определяет интерфейс класса, объекты которого надо создавать.

//  2. Конкретные классы ConcreteProductA и ConcreteProductB представляют реализацию класса Product.
//  Таких классов может быть множество

//  3. Абстрактный класс Creator определяет абстрактный фабричный метод FactoryMethod(),
//  который возвращает объект Product.

//  4. Конкретные классы ConcreteCreatorA и ConcreteCreatorB
//  - наследники класса Creator, определяющие свою реализацию метода FactoryMethod().
//  Причем метод FactoryMethod() каждого отдельного класса-создателя
//  возвращает определенный конкретный тип продукта.
//  Для каждого конкретного класса продукта определяется свой конкретный класс создателя.

//  Таким образом, класс Creator делегирует создание объекта Product своим наследникам.
//  А классы ConcreteCreatorA и ConcreteCreatorB могут самостоятельно выбирать какой конкретный тип продукта им создавать.

#endregion
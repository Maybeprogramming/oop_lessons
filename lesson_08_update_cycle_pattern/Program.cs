namespace lesson_08_update_cycle_pattern
{
    class Program
    {
        static void Main()
        {
            Behaviour[] behaviours =
            {
                new Walker(),
                new Jumper()
            };

            while (true)
            {
                foreach (var behaviour in behaviours)
                {
                    behaviour.Update();
                    Task.Delay(500).Wait();
                }
            }
        }
    }

    class Behaviour
    {
        public virtual void Update() { }
    }

    class Walker : Behaviour
    {
        public override void Update()
        {
            Console.WriteLine("Иду");
        }
    }

    class Jumper : Behaviour
    {
        public override void Update()
        {
            Console.WriteLine("Прыгаю");
        }
    }
}


//Паттерн - цикл обновления
//Примудрости полиморфизма - полиморфизм подтипов отчасти относится к "Ad hoc" полиморфизму ("специально для этого")
//Когда мы исполняем примерно одинаковый код для разных подтипов, т.е. ->
//У нас есть один интерфейс и множество реализаций

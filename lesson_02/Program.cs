namespace oop_lesson_02
{
    class Program
    {
        static void Main()
        {
            Performer worker1 = new Performer("Илья");
            Performer worker2 = new Performer("Костя");

            Task[] tasks =
                {
                    new Task(worker1, "Выкопать яму"),
                    new Task(worker2, "Вывезти грунт"),
                };

            Board shedule = new Board(tasks);

            shedule.ShowAllTasks();
        }
    }

    class Performer
    {
        public string Name;

        public Performer(string name)
        {
            Name = name;
        }
    }

    class Board
    {
        public Task[] Tasks;

        public Board(Task[] tasks)
        {
            Tasks = tasks;
        }

        public void ShowAllTasks()
        {
            for (int i = 0; i < Tasks.Length; i++)
            {
                Tasks[i].ShowInfo();
            }
        }
    }

    class Task
    {
        public Performer Worker;
        public string Descriprtion;

        public Task(Performer worker, string descriprtion)
        {
            Worker = worker;
            Descriprtion = descriprtion;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Ответственный: {Worker.Name}\nОписание задачи: {Descriprtion}.\n");
        }
    }
}

//Связь Has - a
//Каждый класс имеет по одному созданному классу
//Task имеет в себе класс Performer
//Board имеет в себе класс Task
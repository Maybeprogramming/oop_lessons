namespace oop_lesson_01
{
    class Program
    {
        static void Main()
        {
            Table[] tables = { new Table(1, 4), new Table(2, 8), new Table(3, 10) };
            bool isOpen = true;

            while (isOpen == true)
            {
                Console.WriteLine("Администрирование кафе. \n");

                foreach (Table table in tables)
                {
                    table.ShowInfo();
                }

                Console.WriteLine($"\nВведите номер стола: ");
                int wishTable = Convert.ToInt32(Console.ReadLine()) - 1;
                Console.WriteLine($"\nВведите количество мест для брони");
                int desirePlaces = Convert.ToInt32(Console.ReadLine());

                bool isReservationCompleted = tables[wishTable].Reserve(desirePlaces);

                if (isReservationCompleted == true)
                {
                    Console.WriteLine("Бронь прошла успешно!");
                }
                else
                {
                    Console.WriteLine("Бронь не прошла. Недостаточно мест.");
                }


                Console.ReadKey();
                Console.Clear();
            }
        }
    }

    class Table
    {
        public int Number;
        public int MaxPlaces;
        public int FreePlaces;

        public Table(int number, int maxPlaces)
        {
            Number = number;
            MaxPlaces = maxPlaces;
            FreePlaces = maxPlaces;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Стол №: {Number}. Свободно мест: {FreePlaces} из {MaxPlaces}");
        }

        public bool Reserve(int places)
        {
            if (FreePlaces >= places)
            {
                FreePlaces -= places;
                return true;
            }

            return false;
        }
    }
}
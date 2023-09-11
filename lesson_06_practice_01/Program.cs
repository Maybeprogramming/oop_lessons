using System.Security.Cryptography.X509Certificates;

namespace lesson_06_practice_01
{
    class Program
    {
        static void Main()
        {
            Fighter[] fighters =
            {
                new Fighter("Джон", 500,50,0),
                new Fighter("Марк", 250,25,20),
                new Fighter("Алекс", 150,100,10),
                new Fighter("Джек", 300,75,5)
            };

            int fighterNumber;

            for (int i = 0; i < fighters.Length; i++)
            {
                Console.Write((i + 1) + ". ");
                fighters[i].ShowStats();
            }

            Console.WriteLine("\n** " + new string('-', 50) + " **");
            Console.Write("\nВыберите номер первого бойца: ");
            fighterNumber = Convert.ToInt32(Console.ReadLine()) - 1;
            Fighter firstFighter = fighters[fighterNumber];


            Console.Write("\nВыберите номер второго бойца: ");
            fighterNumber = Convert.ToInt32(Console.ReadLine()) - 1;
            Fighter secondFighter = fighters[fighterNumber];
            Console.WriteLine("\n** " + new string('-', 50) + " **");

            while (firstFighter.Health > 0 && secondFighter.Health > 0)
            {
                firstFighter.TakeDamage(secondFighter.Damage);
                secondFighter.TakeDamage(firstFighter.Damage);
                firstFighter.ShowCurrentHealth();
                secondFighter.ShowCurrentHealth();
            }

            Console.ReadLine();
        }
    }

    class Fighter
    {
        private string _name;
        private int _health;
        private int _damage;
        private int _armor;

        public int Health => _health;
        public int Damage => _damage;

        public Fighter(string name, int health, int damage, int armor)
        {
            _name = name;
            _health = health;
            _damage = damage;
            _armor = armor;
        }

        public void ShowStats()
        {
            Console.WriteLine($"Боец - {_name}, Здоровье: {_health}, наносимый урон: {_damage}, броня: {_armor}");
        }

        public void ShowCurrentHealth()
        {
            Console.WriteLine($"{_name} здоровье: {_health}");
        }

        public void TakeDamage(int damage)
        {
            _health -= damage - _armor;
        }
    }
}

//Практика: Бой героев

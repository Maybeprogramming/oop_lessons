namespace lesson_23_pattern_Strategy
{
    using WeaponStrategy;

    class Program
    {
        static void Main()
        {
            Hero hero = new Hero("Васелёк");
            hero.Attack();

            hero.SetWeapon(new WeaponOne());
            hero.Attack();

            hero.SetWeapon(new WeaponTwo());
            hero.Attack();

            hero.SetWeapon(new WeaponThree());
            hero.Attack();

            Console.ReadKey();
        }
    }

    class Hero
    {
        private string _name;
        public IWeapon _weapon;

        public Hero(string name)
        {
            _name = name;
        }

        public void SetWeapon (IWeapon weapon)
        {
            _weapon = weapon;
        }

        public void Attack()
        {
            Console.WriteLine($">>>");

            if (_weapon is null)
            {
                Console.WriteLine($"Герой не может атаковать!");
                return;
            }

            Console.WriteLine($"{_name} грозно посмотрел в сторону врага");
            Console.Write($"{_name} ");

            _weapon.Shoot();

            Console.WriteLine($"{_name} радуется своему успеху!");
        }
    }

}

namespace WeaponStrategy
{
    interface IWeapon
    {
        void Shoot();
    }

    class WeaponOne : IWeapon
    {
        public void Shoot()
        {
            Console.WriteLine($"Атакую первым оружием");
        }
    }

    class WeaponTwo : IWeapon
    {
        public void Shoot()
        {
            Console.WriteLine($"Атакую втором оружием");
        }
    }

    class WeaponThree : IWeapon
    {
        public void Shoot()
        {
            Console.WriteLine($"Атакую третьим оружием");
        }
    }
}

// Паттерн - Стратегия!
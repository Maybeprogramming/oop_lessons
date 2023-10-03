namespace lesson_20_fighter_battle
{
    internal class Program
    {
        static void Main()
        {

            
        }
    }

    class BatteDield
    {
        List<Fighter> fighters = new List<Fighter>();

        public void StartBattle()
        {
            while (fighters.Count < 3)
            {
                switch (Console.ReadLine())
                {
                    case "0":
                        ChooseFighter(Fighter.Create());
                        break;

                    case "1":
                        ChooseFighter(Warrior.Create());
                        break;

                    case "2":
                        ChooseFighter(Assasign.Create());
                        break;
                    default:
                        Console.WriteLine("Нет такой команды!");
                        break;
                }
            }

            foreach(var fighter in fighters)
            {
                Console.WriteLine(fighter);
            }

            Console.ReadKey();

        }

        void ChooseFighter(Func<Fighter> Create) => fighters.Add(Create.Invoke());
        void ChooseFighter(Func<Warrior> Create) => fighters.Add(Create.Invoke());
        void ChooseFighter(Func<Assasign> Create) => fighters.Add(Create.Invoke());
    }

    class Fighter : IDamageable, IDamageProvider, IHealable
    {
        internal static Func<Fighter> Create()
        {
            return () => new Fighter();
        }

        public void Attack(Fighter target)
        {
            throw new NotImplementedException();
        }

        public void Healing(int healingPoint)
        {
            throw new NotImplementedException();
        }

        public void TakeDamage(int damage)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"{this.GetType().Name}";
        }
    }

    class Warrior : Fighter
    {

    }

    class Assasign: Fighter
    {

    }

    class Ability
    {
        private string? _name;
    }

    interface IDamageable
    {
        void TakeDamage(int damage);
    }

    interface IDamageProvider
    {
        void Attack(Fighter target);
    }

    interface IHealable
    {
        void Healing(int healingPoint);
    }
}
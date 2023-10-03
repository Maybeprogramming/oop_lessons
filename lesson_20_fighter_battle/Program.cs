namespace lesson_20_fighter_battle
{
    internal class Program
    {
        static void Main()
        {
            BattleField battleField = new BattleField();
            battleField.StartBattle();


            Console.ReadKey();
        }
    }

    class BattleField
    {
        List<Fighter> fighters;

        public void StartBattle()
        {
            fighters = new List<Fighter>();

            while (fighters.Count < 5)
            {
                Console.Clear();

                Console.WriteLine($"Доступные бойцы:\n" +
                    $"0 - Figter\n" +
                    $"1 - Warrior\n" +
                    $"2 - Assasign\n" +
                    $"3 - Hunter\n" +
                    $"4 - Wizzard\n" +
                    $"Введите номер для выбора бойца: ");

                switch (Console.ReadLine())
                {
                    case "0":
                        ChooseFighter(new Fighter());
                        break;

                    case "1":
                        ChooseFighter(new Warrior());
                        break;

                    case "2":
                        ChooseFighter(new Assasign());
                        break;

                    case "3":
                        ChooseFighter(new Hunter());
                        break;

                    case "4":
                        ChooseFighter(new Wizzard());
                        break;

                    default:
                        Console.WriteLine("Нет такой команды!");
                        break;
                }
            }

            foreach (var fighter in fighters)
            {
                Console.WriteLine(fighter);
            }

            Console.ReadKey();

            StartBattle();
        }

        void ChooseFighter(Fighter fighter) => fighters.Add(fighter);
        void ChooseFighter(Warrior warrior) => fighters.Add(warrior);
        void ChooseFighter(Assasign assasign) => fighters.Add(assasign);
        void ChooseFighter(Hunter hunter) => fighters.Add(hunter);
        void ChooseFighter(Wizzard wizzard) => fighters.Add(wizzard);
    }

    class Fighter : IDamageable, IDamageProvider, IHealable
    {
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

    class Assasign : Fighter
    {

    }

    class Hunter : Fighter
    {

    }

    class Wizzard : Fighter
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
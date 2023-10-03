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
        private List<Fighter> fighters;

        public void StartBattle()
        {
            fighters = new List<Fighter>();

            while (fighters.Count < 2)
            {
                Console.Clear();

                Console.WriteLine(
                    $"Доступные бойцы:\n" +
                    $"0 - Figter\n" +
                    $"1 - Warrior\n" +
                    $"2 - Assasign\n" +
                    $"3 - Hunter\n" +
                    $"4 - Wizzard\n" +
                    $"Введите номер для выбора бойца:");

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

        private void ChooseFighter(Fighter fighter) => fighters.Add(fighter);
        private void ChooseFighter(Warrior warrior) => fighters.Add(warrior);
        private void ChooseFighter(Assasign assasign) => fighters.Add(assasign);
        private void ChooseFighter(Hunter hunter) => fighters.Add(hunter);
        private void ChooseFighter(Wizzard wizzard) => fighters.Add(wizzard);
    }

    class Fighter : IDamageable, IDamageProvider, IHealable
    {
        public Fighter()
        {
            Health = Generator.GetInt(100, 200);
            Damage = Generator.GetInt(20, 50);
            Name = Generator.GetName();
        }

        public int Health { get; private set; }
        public int Damage { get; private set; }
        public bool IsAlive => Health > 0;
        public string Name { get; private set; }

        public void Attack(Fighter target)
        {
            if (target.TryTakeDamage(Damage))
            {
                Console.WriteLine($"{GetType().Name} ({Name}) ударил {target.GetType().Name} ({target.Name})");
            }
            else
            {
                Console.WriteLine($"{GetType().Name} ({Name}) не смог ударить {target.GetType().Name} ({target.Name})");
            }
        }

        public void Healing(int healingPoint)
        {
            Health += healingPoint;
            Console.WriteLine($"{GetType().Name} ({Name}) подлечил здоровье на ({healingPoint}) ед.");

        }

        public bool TryTakeDamage(int damage)
        {
            if (Health > 0)
            {
                Health -= damage;
                Console.WriteLine($"{GetType().Name} ({Name}) получил удар ({damage}) ед., осталось здоровья ({Health})");
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{GetType().Name}";
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
        bool TryTakeDamage(int damage);
    }

    interface IDamageProvider
    {
        void Attack(Fighter target);
    }

    interface IHealable
    {
        void Healing(int healingPoint);
    }

    static class Generator
    {
        private static Random _random = new Random();
        private static string[] _names =
            {
                "Варвар",
                "Алконафт",
                "Миледи",
                "Вульфич",
                "Страйк",
                "Герандич",
                "Фрея",
                "Крыса",
                "Нинка",
                "Царь",
                "Забота",
                "Жарить",
                "Овощ",
                "Имба",
                "Нагибатель",
                "Топчик",
                "Холивар",
                "Членчик",
                "Пирожок",
                "Котейка",
                "Оливер",
                "Викрам",
                "Архидея",
                "Вагинометр",
                "Зимник",
                "Волкодав",
                "Богатырь",
                "Вафлич",
                "Вурдолакыч",
                "Зяблик",
                "Кудахта",
                "Чувайка",
                "Мордорка",
                "Куряха",
                "Смоляха",
                "Крендель",
                "Остряк",
                "Крушила",
                "Очкович",
                "Щавель",
                "Днище",
                "Нубичка",
                "Жираф",
                "Лизун",
                "Сосальщик",
                "Подгузник",
                "Тряпка"
            };
        public static string GetName()
        {
            return _names[_random.Next(0, _names.Length - 1)];
        }
        public static int GetInt(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }
    }
}
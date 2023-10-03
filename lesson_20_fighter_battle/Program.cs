using System.Xml.Linq;

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
        private List<Fighter> _fighters;

        public void StartBattle()
        {
            _fighters = new List<Fighter>();

            while (_fighters.Count < 2)
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

            Console.WriteLine("Готовые к бою отважные герои:");

            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.WriteLine($"{i+1}. {_fighters[i].GetType().Name} ({_fighters[i].Name})");
            }

            Console.WriteLine("Начать битву?\nДля продолжения нажмите любую клавишу...\n\n");
            Console.ReadKey();

            while (_fighters[0].IsAlive == true && _fighters[1].IsAlive == true)
            {
                _fighters[0].Attack(_fighters[1]);
                _fighters[1].Attack(_fighters[0]);
            }

            if (_fighters[0].IsAlive == false && _fighters[1].IsAlive == false)
            {
                Console.WriteLine("Ничья! Оба героя пали на поле боя!");
            }
            else if (_fighters[0].IsAlive == true && _fighters[1].IsAlive == false)
            {
                Console.WriteLine($"Победитель - {_fighters[0]} ({_fighters[0].Name})!");
            }
            else if (_fighters[0].IsAlive == false && _fighters[1].IsAlive == true)
            {
                Console.WriteLine($"Победитель - {_fighters[1]} ({_fighters[1].Name})!");
            }

            Console.ReadKey();
            StartBattle();
        }

        private void ChooseFighter(Fighter fighter) => _fighters.Add(fighter);
        private void ChooseFighter(Warrior warrior) => _fighters.Add(warrior);
        private void ChooseFighter(Assasign assasign) => _fighters.Add(assasign);
        private void ChooseFighter(Hunter hunter) => _fighters.Add(hunter);
        private void ChooseFighter(Wizzard wizzard) => _fighters.Add(wizzard);
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
                //Console.WriteLine($"{GetType().Name} ({Name}) получил удар ({damage}) ед., осталось здоровья ({Health})");
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
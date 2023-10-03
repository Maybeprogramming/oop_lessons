using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace lesson_20_fighter_battle
{
    internal class Program
    {
        static void Main()
        {
            Console.WindowWidth = 100;

            BattleField battleField = new BattleField();
            battleField.BeginBattle();

            Console.ReadKey();
        }
    }

    class BattleField
    {
        private List<Fighter> _fighters;

        public void BeginBattle()
        {
            _fighters = new List<Fighter>();

            while (_fighters.Count < 2)
            {
                Console.Clear();

                Console.WriteLine(
                    $"Доступные классы героев:\n" +
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
            AnnounceFightresNames();

            Console.WriteLine("Начать битву?\nДля продолжения нажмите любую клавишу...\n\n");
            Console.ReadKey();

            StartFighting();
            CheckVictory();

            Console.ReadKey();
            BeginBattle();
        }

        private void AnnounceFightresNames()
        {
            for (int i = 0; i < _fighters.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_fighters[i].GetType().Name} ({_fighters[i].Name}): DMG: {_fighters[i].Damage}, HP: {_fighters[i].Health}");
            }
        }

        private void StartFighting()
        {
            while (_fighters[0].IsAlive == true && _fighters[1].IsAlive == true)
            {
                _fighters[0].Attack(_fighters[1]);
                _fighters[1].Attack(_fighters[0]);
                Console.WriteLine(new string('-', 80));
            }
        }

        private void CheckVictory()
        {
            if (_fighters[0].IsAlive == false && _fighters[1].IsAlive == false)
            {
                Console.WriteLine("\nНичья! Оба героя пали на поле боя!");
            }
            else if (_fighters[0].IsAlive == true && _fighters[1].IsAlive == false)
            {
                Console.WriteLine($"\nПобедитель - {_fighters[0]} ({_fighters[0].Name})!");
            }
            else if (_fighters[0].IsAlive == false && _fighters[1].IsAlive == true)
            {
                Console.WriteLine($"\nПобедитель - {_fighters[1]} ({_fighters[1].Name})!");
            }
        }

        private void ChooseFighter(Fighter fighter) => _fighters.Add(fighter);
        private void ChooseFighter(Warrior warrior) => _fighters.Add(warrior);
        private void ChooseFighter(Assasign assasign) => _fighters.Add(assasign);
        private void ChooseFighter(Hunter hunter) => _fighters.Add(hunter);
        private void ChooseFighter(Wizzard wizzard) => _fighters.Add(wizzard);
    }

    class Fighter : IDamageable, IDamageProvider, IHealable
    {
        protected int _health;
        public Fighter()
        {
            Health = Generator.NextInt(150, 301);
            Damage = Generator.NextInt(10, 21);
            Name = Generator.NextName();
        }

        public int Health { get => _health; protected set => SetHealth(value); }
        public int Damage { get; protected set; }
        public bool IsAlive => Health > 0;
        public string Name { get; protected set; }

        public virtual void Attack(Fighter target)
        {
            if (IsAlive == false) return;
            else
            {
                Console.WriteLine($"{GetType().Name} ({Name}) произвёл удар в сторону {target.GetType().Name} ({target.Name})");

                if (target.IsAlive == true)
                    target.TryTakeDamage(Damage);
            }
        }

        public virtual void Healing(int healingPoint)
        {
            Health += healingPoint;
            Console.WriteLine($"{GetType().Name} ({Name}) подлечил здоровье на ({healingPoint}) ед. Здоровье : ({Health})");
        }

        public virtual bool TryTakeDamage(int damage)
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

        private void SetHealth(int value)
        {
            if (value > 0)
            {
                _health = value;
            }
            else
            {
                _health = 0;
            }
        }
    }

    class Warrior : Fighter
    {
        private int _missDamagePercent = 30;
        private int _maxPercent = 100;

        public override bool TryTakeDamage(int damage)
        {
            int missChance = Generator.NextInt(0, _maxPercent + 1);

            if (missChance < _missDamagePercent)
            {
                Console.WriteLine($"{GetType().Name} ({Name}) увернулся от удара, осталось здоровья ({Health})");
                return false;
            }

            return base.TryTakeDamage(damage);
        }
    }

    class Assasign : Fighter
    {
        public override void Attack(Fighter target)
        {
            if (IsAlive == false || target.IsAlive == false) return;

            Console.WriteLine($"{GetType().Name} ({Name}) произвёл удар в сторону {target.GetType().Name} ({target.Name})");

            if (target.TryTakeDamage(Damage))
            {
                int damageDivider = 10;
                int healingPoint = Damage / damageDivider;
                Healing(healingPoint);
            }
        }
    }

    class Hunter : Fighter
    {
        private int _critPercent = 30;
        private int _maxPercent = 100;
        private int _damageModifyPercent = 150;

        public override void Attack(Fighter target)
        {
            if (IsAlive == false) return;
            else
            {
                int currentDamage = CalculateCriteDamage();
                Console.WriteLine($"{GetType().Name} ({Name}) произвёл удар в сторону {target.GetType().Name} ({target.Name})");

                if (target.IsAlive == true)
                    target.TryTakeDamage(currentDamage);
            }
        }

        private int CalculateCriteDamage()
        {
            int critChance = Generator.NextInt(0, _maxPercent + 1);

            Console.WriteLine($"{critChance} шанс");

            if (critChance < _critPercent)
            {
                return Damage * _damageModifyPercent / _maxPercent;
            }

            return Damage;
        }
    }

    class Wizzard : Fighter
    {
        private int _mana;
        private int _manMana = 50;
        private int _maxMana = 100;
        private int _castingManaCost = 20;
        private int _regenerationManaCount = 10;

        public Wizzard()
        {
            _mana = Generator.NextInt(_manMana, _maxMana);
        }

        public int Mana { get => _mana; }

        public override void Attack(Fighter target)
        {
            if (_mana >= _castingManaCost)
            {
                _mana -= _castingManaCost;
                base.Attack(target);
            }
            else
            {
                _mana += _regenerationManaCount;
                Console.WriteLine($"{GetType().Name} ({ Name}) не хватает маны для удара {target.GetType().Name} ({target.Name})");
            }
        }

        public override bool TryTakeDamage(int damage)
        {
            _mana += _regenerationManaCount;
            return base.TryTakeDamage(damage);
        }
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
        public static string NextName()
        {
            return _names[_random.Next(0, _names.Length - 1)];
        }
        public static int NextInt(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
namespace oop_lesson_03
{
    class Program
    {
        static void Main()
        {
            Knight warrior1 = new Knight(100, 10, 5);
            Barbarian warrior2 = new Barbarian(100, 7, 5, 2);


            warrior1.TakeDamage(50);
            warrior2.TakeDamage(25);

            Console.Write("Рыцарь: ");
            warrior1.ShowInfo();

            Console.Write("Барбариан: ");
            warrior2.ShowInfo();

            Console.ReadKey();
        }
    }

    class Warrior
    {
        protected int Health;
        protected int Armor;
        protected int Damage;

        public Warrior(int health, int armor, int damage)
        {
            Health = health;
            Armor = armor;
            Damage = damage;
        }

        public void TakeDamage(int damage)
        {
            Health -= damage - Armor;
        }

        public void ShowInfo()
        {
            Console.WriteLine($"{Health}");
        }
    }

    class Knight : Warrior
    {
        public Knight(int health, int armor, int damage) : base(health, armor, damage) { }

        public void Pray()
        {
            Armor += 2;
        }
    }

    class Barbarian : Warrior
    {
        public int AttackSpeed;

        public Barbarian(int health, int armor, int damage, int attackSpeed) : base(health, armor, damage * attackSpeed)
        {
            AttackSpeed = attackSpeed;
        }

        public void Shout()
        {
            Armor -= 2;
            Health += 10;
        }
    }
}

//Связь Is - a
//Отношения наследования - генерализация или обобщения
//Когда мы работаем с классами и объектами - мы можем связывать их не только ссылками между объектами
//Например мы рассмотрели связь Has - a, где объект одного типа может содержать в себе объект другого типа
//Дополнительно можем связывать объекты наследованием
//И рыцарь и Барбариан являются войном - это и есть связь Is - a
//А это значит что они содержат в себе тоже самое что и воин
//и поведение и состояние (и методы и поля)
//При этом рыцарь и барбариан имеют уникальное поведение
//Когда базовый класс имеет конструктор то мы обязательно должны вызвать в производном классе

//Узнали про модификатор доступа protected
//Закрывает доступ к полям/методам из вне, но разрешает доступ в классах наследниках

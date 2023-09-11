namespace lesson_07_virtual_methods
{
    class Program
    {
        static void Main()
        {
            NonPlayerCharacter[] characters =
            {
                new NonPlayerCharacter(),
                new Farmer(),
                new Knight(),
                new Child()               
            };

            foreach (NonPlayerCharacter character in characters)
            {
                character.ShowDescription();
                Console.WriteLine(new string ('-', 40));
            }

            Console.ReadLine();
        }
    }

    class NonPlayerCharacter
    {
        public virtual void ShowDescription()
        {
            Console.WriteLine("Я простой НПЦ, умею только гулять");
        }
    }

    class Farmer: NonPlayerCharacter
    {
        public override void ShowDescription()
        {
            base.ShowDescription();
            Console.WriteLine("А ещё я фермер и умею работать на поле");
        }
    }

    class Knight : NonPlayerCharacter
    {
        public override void ShowDescription()
        {
            Console.WriteLine("Я рыцарь моё дело только сражение");
        }
    }

    class Child : NonPlayerCharacter
    {

    }
}

//Вирутальные методы
namespace lesson_20_fighter_battle
{
    internal class Program
    {
        List<Fighter> fighters = new List<Fighter>();

        static void Main()
        {

            switch (Console.ReadLine())
            {
                case "0":
                    CreateFighter(Fighter.Create());
                    break;

                case "1":
                    break;

                case "2":
                    break;
            }
        }

        private void CreateFighter(Func<Fighter> Create) => fighters.Add(Create.Invoke());

    }

    class Fighter : IDamageable, IDamageProvider, IHealable
    {
        internal static Func<Fighter> Create()
        {
            throw new NotImplementedException();
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
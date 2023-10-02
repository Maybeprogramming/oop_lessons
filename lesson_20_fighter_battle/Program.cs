namespace lesson_20_fighter_battle
{
    internal class Program
    {
        static void Main()
        {

        }
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
    }

    class Ability
    {
        private string _name;
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
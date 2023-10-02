namespace lesson_20_fighter_battle
{
    internal class Program
    {
        static void Main()
        {

        }
    }

    class Warrior: IDamageable
    {

    }

    class Ability
    {
        private string _name;

    }

    interface IDamageable
    {
       public void TakeDamage(int damage) { }
    }
}
namespace oop_lesson_05
{
    class Program
    {
        static void Main()
        {
            Cart cart = new Cart();

            cart.ShowProducts();

            List<Product> products = new List<Product>();

            for (int i = 0; i < cart.GetProductsCount(); i++)
            {
                products.Add(cart.GetProductByIndex(i));
            }

            products.RemoveAt(0);
            Console.WriteLine();

            cart.ShowProducts();
            Console.ReadLine();
        }
    }

    class Cart
    {
        private List<Product> _products = new List<Product>();

        #region Ошибка 1
        //Так делать свойство нельзя!!!
        public List<Product> Products2 { get { return _products; } private set { } }
        //
        #endregion

        public Cart()
        {
            _products.Add(new Product("Яблоко"));
            _products.Add(new Product("Банан"));
            _products.Add(new Product("Апельсин"));
            _products.Add(new Product("Груша"));
        }

        public void ShowProducts()
        {
            foreach (Product product in _products)
            {
                Console.WriteLine($"{product.Name}");
            }
        }

        #region Ошибка 2
        //Так делать нельзя!!!!!
        public List<Product> GetProducts()
        {
            return _products;
        }
        #endregion

        public Product GetProductByIndex(int index)
        {
            return _products.ElementAt(index);
        }

        public int GetProductsCount()
        {
            return _products.Count();
        }
    }

    class Product
    {
        public string Name { get; private set; }

        public Product(string name)
        {
            Name = name;
        }
    }
}

//Это ломает инкапсуляцию
//Ошибки возникают так как коллекции ссылаются на одну и ту же область памяти
//Для избежания этой ошибки надо выделить дополнительную память под коллекцию, создав экземпляр
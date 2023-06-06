using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp95
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandProductAddProduct = "1";
            const string CommandShowProducts = "2";
            const string CommandBuyGoods = "3";
            const string CommandShowPlayerBag = "4";
            const string CommandExitProgramm = "5";

            string userInput;
            bool isOpen = true;

            Salesman salesman = new Salesman();
            Player player = new Player();


            while (isOpen)
            {
                Console.WriteLine("\nДобро пожаловать в магазин");

                Console.WriteLine($"{CommandProductAddProduct} - добавить товар");
                Console.WriteLine($"{CommandShowProducts} - показать товары");
                Console.WriteLine($"{CommandBuyGoods} - купить товар");
                Console.WriteLine($"{CommandShowPlayerBag} - показать сумку игрока");
                Console.WriteLine($"{CommandExitProgramm} - выход из программы");
                userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandProductAddProduct:
                        salesman.AddItem();
                        break;

                    case CommandShowProducts:
                        salesman.ShowAllProducts();
                        break;

                    case CommandBuyGoods:
                        player.Purchase(salesman.Sell());
                        break;

                    case CommandShowPlayerBag:
                        player.ShowBag();
                        break;

                    case CommandExitProgramm:
                        isOpen = false;
                        break;

                    default:
                        Console.WriteLine("Ошибка выбора меню.Выберите подходящие");
                        Console.ReadKey();
                        break;
                }

            }
        }
    }

    class Salesman
    {
        private List<Product> _products = new List<Product>();
        Player Player = new Player();

        public void ShowAllProducts()
        {
            for (int i = 0; i < _products.Count; i++)
            {
                _products[i].ShowProducts();
            }
        }

        public void AddItem()
        {
            Console.Write("Пропишите название товара");
            string productName = Console.ReadLine();
            int priceOfProduct = GetPriceOfProduct("Пропишите цену за товар");
            int countOfProduct = GetPriceOfProduct("Пропишите количество товара");

            _products.Add(new Product(productName, countOfProduct, priceOfProduct));
        }

        private int GetPriceOfProduct(string text)
        {
            Console.Write(text);
            int.TryParse(Console.ReadLine(), out int number);
            return number;
        }

        public Product Sell ()
        {
            bool isSold = true;

            while (isSold)
            {
                string name = ParseName("Введите название товара, который хотите купить", "Такого товара не существует");
                int count = ParseNumber("Введите количество товара, который хотите купить", "Столько товара нет");             

                if (BuyOpporttunity(name, count))
                {                  
                    _products[GetIndex(name)].Count -= count;
                    Product tempProduct = new Product(name, count, _products[GetIndex(name)].Price);
                    return tempProduct;
                }
            }
            return null;
        }

        private string ParseName(string text,string textError)
        {
            string name = " ";
            bool isParsed = true;

            while (isParsed)
            {
                Console.Write(text);
                string userInput = Console.ReadLine();

                for (int i = 0; i<_products.Count; i++)
                {
                    if (_products[i].Name == userInput)
                    {
                        return _products[i].Name;
                    }
                }

                Console.Write(textError);
                Console.ReadLine();
                Console.Clear();
            }
            return name;
        }

        private int ParseNumber (string text, string textError)
        {
            int number = 0;
            bool isParsed = true;

            while (isParsed)
            {
                Console.Write(text);
                string userInput = Console.ReadLine();

                if(int.TryParse(userInput, out number))
                {
                    isParsed = false;
                }
                else
                {
                    Console.WriteLine(textError);
                }
                Console.ReadKey();
                Console.Clear();
            }
            return number;
        }

        private bool BuyOpporttunity(string name, int count)
        {
            for (int i = 0; i < _products.Count; i++)
            {
                if (_products[i].Name == name)
                {
                    if (_products[i].Count >= count && count > 0)
                    {
                        if (_products[i].Price * count <= Player.Money)
                        {
                            Console.WriteLine("Успешно купили товар");
                            return true;                         
                        }
                        else
                        {
                            Console.WriteLine("Недостаточно денег");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Недостаточно товара");
                    }
                }
            }
            return false;
        }

        private int GetIndex(string name)
        {
            int index = -1;

            for (int i = 0; i < _products.Count; i++)
            {
                if (name == _products[i].Name)
                {
                    index = i;
                }
            }
            return index;
        }
    }

    class Player
    {
        private List<Product> _bag  = new List<Product>();
        public int Money { get; private set; } = 500;
        public int bag { get; private set; }

        public void Purchase (Product product)
        {
            _bag.Add(product);
            Money -= product.Price * product.Count;
        }

        public void ShowBag()
        {
            foreach (var product in _bag)
            {
                Console.WriteLine($"Назавние - {product.Name}, количество - {product.Count}, денег - {Money}");
            }
        }

    }

    class Product
    {
        public string Name { get; private set; }
        public int Count { get; set; }

        public int Price { get; private set; }
        public Product(string name, int count, int price)
        {
            Name = name;
            Count = count;
            Price = price;
        }

        public void ShowProducts()
        {
            Console.WriteLine($"{Name} - название, цена - {Price}, количество - {Count}");
    }
    }

}

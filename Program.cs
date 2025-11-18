using System;
using System.Collections.Generic;
using System.Linq;

namespace laboratorna7
{
    // 1. ІНКАПСУЛЯЦІЯ — приховування стану + контроль доступу
    class Client
    {
        private int loyaltyPoints;    // Приховане поле
        public string Name { get; set; }
        public decimal Discount { get; private set; } // Змінити може тільки клас

        public Client(string name) => Name = name;

        public void EarnPoints(int points)
        {
            if (points <= 0) return;
            loyaltyPoints += points;

            Discount = loyaltyPoints switch
            {
                >= 1000 => 0.20m,
                >= 500 => 0.15m,
                >= 200 => 0.10m,
                >= 100 => 0.05m,
                _ => 0m
            };

            Console.WriteLine($"[ІНКАПСУЛЯЦІЯ] {Name}: +{points} балів → {loyaltyPoints} | Знижка: {Discount:P0}");
        }
    }

    // 2. НАСЛІДУВАННЯ — повторне використання коду через ієрархію
    class Product
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public decimal Price { get; set; }

        public Product(string name, string brand, decimal price)
        {
            Name = name;
            Brand = brand;
            Price = price;
        }

        // Простий метод успадковується всіма нащадками
        public void ShowBasicInfo()
        {
            Console.WriteLine($"Товар: {Name} | Бренд: {Brand} | Ціна: {Price:C}");
        }
    }

    class Dress : Product          // Чисте наслідування
    {
        public string Size { get; set; }
        public string Style { get; set; }

        public Dress(string name, string brand, decimal price, string size, string style)
            : base(name, brand, price)
        {
            Size = size;
            Style = style;
        }
    }

    class Lipstick : Product       //  Ще один нащадок
    {
        public string Shade { get; set; }
        public bool IsMatte { get; set; }

        public Lipstick(string name, string brand, decimal price, string shade, bool matte)
            : base(name, brand, price)
        {
            Shade = shade;
            IsMatte = matte;
        }
    }

    // 3. ПОЛІМОРФІЗМ — однаковий виклик, різна поведінка
    class DisplayableItem
    {
        // Віртуальний метод може бути перевизначений
        public virtual void Display()
        {
            Console.WriteLine("Це звичайний товар.");
        }
    }

    class Clothing : DisplayableItem
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }

        // перевизначення — поліморфізм
        public override void Display()
        {
            Console.WriteLine($"ОДЯГ → {Name}, розмір: {Size}, ціна: {Price:C}");
        }
    }

    class Cosmetic : DisplayableItem
    {
        public string Product { get; set; }
        public string Shade { get; set; }
        public decimal Price { get; set; }

        // Інша реалізація того ж методу
        public override void Display()
        {
            Console.WriteLine($"КОСМЕТИКА → {Product}, відтінок: {Shade}, ціна: {Price:C}");
        }
    }

    // 4. АБСТРАКЦІЯ — приховування деталей, обов’язковий контракт
    abstract class BeautyService
    {
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Абстрактний метод КОЖЕН нащадок ПОВИНЕН його реалізувати
        public abstract void Perform();

        // Звичайний метод вже має реалізацію
        public void ShowInfo()
        {
            Console.WriteLine($"Послуга: {Name} | Вартість: {Price:C}");
        }
    }

    class Manicure : BeautyService
    {
        public Manicure()
        {
            Name = "Класичний манікюр";
            Price = 450m;
        }

        public override void Perform()
        {
            Console.WriteLine("Виконуємо манікюр: обробка, покриття, дизайн...");
        }
    }

    class Facial : BeautyService
    {
        public Facial()
        {
            Name = "Догляд за обличчям";
            Price = 1200m;
        }

        public override void Perform()
        {
            Console.WriteLine("Очищення → маска → масаж → зволоження. Шкіра сяє!");
        }
    }

    // Кошик — використовує поліморфізм (DisplayableItem)
    class Cart
    {
        private readonly List<DisplayableItem> items = new();

        public void Add(DisplayableItem item)
        {
            items.Add(item);
            Console.WriteLine("Додано до кошика");
        }

        public void ShowAll()
        {
            Console.WriteLine("\nКОШИК (демонстрація ПОЛІМОРФІЗМУ):");
            Console.WriteLine(new string('═', 55));
            foreach (var item in items)
                item.Display(); //  Один виклик але різний результат
            Console.WriteLine(new string('═', 55));
        }

        public decimal Total => items.Sum(x => x is Clothing c ? c.Price : ((Cosmetic)x).Price);
    }

    // ГОЛОВНА ПРОГРАМА — чітка демонстрація ВСІХ 4 принципів
    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("═══════════════════════════════════════════=");
            Console.WriteLine("   FASHION & BEAUTY STORE — 4 ПРИНЦИПИ ООП  ");
            Console.WriteLine("════════════════════════════════════════════\n");

            // 1. ІНКАПСУЛЯЦІЯ
            var client = new Client("Софія");
            Console.WriteLine("1. ІНКАПСУЛЯЦІЯ — створено клієнта з прихованими балами\n");

            // 2. НАСЛІДУВАННЯ
            var dress = new Dress("Сукня-сорочка", "Zara", 2190m, "M", "Оверсайз");
            dress.ShowBasicInfo();
            Console.WriteLine("  Наслідування: метод ShowBasicInfo() від Product\n");

            // 3. ПОЛІМОРФІЗМ
            var cart = new Cart();
            cart.Add(new Clothing { Name = "Біла блузка", Size = "S", Price = 1490m });
            cart.Add(new Cosmetic { Product = "Помада Dior 999", Shade = "Червоний", Price = 1780m });
            cart.Add(new Clothing { Name = "Штани палаццо", Size = "38", Price = 2590m });
            cart.ShowAll();

            // 4. АБСТРАКЦІЯ
            Console.WriteLine("4. АБСТРАКЦІЯ — працюємо тільки через загальний контракт:");
            BeautyService service1 = new Manicure();
            BeautyService service2 = new Facial();

            service1.ShowInfo();
            service1.Perform();

            service2.ShowInfo();
            service2.Perform();
            Console.WriteLine("  Не знаємо деталей реалізації, тільки контракт!\n");

            // Фінал: нарахування балів
            decimal total = cart.Total;
            Console.WriteLine($"Загальна сума покупок: {total:C}");
            client.EarnPoints((int)(total / 10));

            Console.WriteLine("\nДякуємо за покупку, Юліана! До зустрічі! ");
            Console.ReadKey();
        }
    }
}
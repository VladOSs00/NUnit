using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class CarsWorkShop
    {
        /// <summary>
        /// Количество денег
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// Наименование услуг
        /// </summary>
        private Dictionary<string, Service> Services { get; set; }

        /// <summary>
        /// Подсчет деталей
        /// </summary>
        private Dictionary<string, Service> NumberDetails { get; set; }

        /// <summary>
        /// Покупка новых деталей
        /// </summary>
        /// <param name="name">Название деталей</param>
        /// <param name="price">Результат покупки деталей</param>
        /// <returns></returns>
        public string ByuDetails(string name, int count)
        {
            if (!Services.ContainsKey(name))
            {
                return "Такой детали не существует!";
            }

            var service = Services[name];
            int price = service.Price * count;

            if (price > Money)
            {
                return "Недостаточно средст для покупки";
            }
            else
            {
                service.CoutDetails -= count;
                Services[name] = service;
                Money -= price;

                var soldProd = NumberDetails[name];
                soldProd.CoutDetails += count;
                soldProd.Price += price;
                NumberDetails[name] = soldProd;

                return "Успех";
            }

        }

        /// <summary>
        /// Доставка товара
        /// </summary>
        /// <param name="name">Название деталей</param>
        /// <param name="count">Количество</param>
        public void Delivery(string name, int count)
        {
            var service = Services[name];
            service.CoutDetails += count;
            Services[name] = service;
        }

        /// <summary>
        /// Добавление новых деталей
        /// </summary>
        /// <param name="name">Название детали</param>
        /// <param name="service">Деталь</param>
        public void NewDetails(string name, Service service)
        {
            if (!Services.ContainsKey(name))
                Services.Add(name, service);
        }

        /// <summary>
        /// Подсчет проданых деталей
        /// </summary>
        /// <param name="name">Наименование деталей</param>
        /// <returns>Количество проданых деталей</returns>
        public int CountingSoldPart(string name)
        {
            return NumberDetails[name].CoutDetails;
        }

        /// <summary>
        /// Изменение цены деталей
        /// </summary>
        /// <param name="name">Наименование деталей</param>
        /// <param name="newPrice">Новая деталей</param>
        public void PriceChange(string name, int newPrice)
        {
            var service = Services[name];
            service.Price = newPrice;
            Services[name] = service;
        }

        /// <summary>
        /// Подсчет суммы проданных деталей
        /// </summary>
        /// <param name="name">Наименование деталей</param>
        /// <returns>Общая сумма проданны деталей</returns>
        public int CountingMoney(string name)
        {
            return NumberDetails[name].Price;
        }


        /// <summary>
        /// Количество деталей на складе
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Service CountingDetails(string name)
        {
            return Services[name];
        }

        /// <summary>
        /// Удаление деталей
        /// </summary>
        /// <param name="name"></param>
        public void DeleteDetails(string name)
        {
            if (Services.ContainsKey(name))
                Services.Remove(name);
        }

        /// <summary>
        /// Проверка на существование деталей
        /// </summary>
        /// <param name="name">Название деталей</param>
        /// <returns>Существует ли товар на складе</returns>
        public bool ContainsDetails(string name)
        {
            return Services.ContainsKey(name);
        }

        /// <summary>
        /// Cервис
        /// </summary>
        public struct Service
        {
            public int Price;
            public int CoutDetails;
        }



        [TestFixture]
        class CarsWorkShopTest
        {
            CarsWorkShop cws;

            [SetUp]
            public void SetUp()
            {

                cws = new CarsWorkShop();
                cws.Money = 1000;
                cws.Services = new Dictionary<string, Service>();
                cws.NumberDetails = new Dictionary<string, Service>();
                cws.Services = new Dictionary<string, Service>();
                cws.Services.Add("Замена двигателя", new Service { Price = 500, CoutDetails = 9 });
                cws.Services.Add("Замена фар", new Service { Price = 100, CoutDetails = 5 });
                cws.Services.Add("Замена руля", new Service { Price = 200, CoutDetails = 3 });
                cws.Services.Add("Замена шин", new Service { Price = 450, CoutDetails = 4 });
                cws.Services.Add("Замена тормозов", new Service { Price = 30, CoutDetails = 6 });
                cws.Services.Add("Замена трансмиссии", new Service { Price = 350, CoutDetails = 7 });

                foreach (var item in cws.Services.Keys)
                {
                    cws.NumberDetails.Add(item, new Service { CoutDetails = 0, Price = 0 });
                }
            }


            [Test]
            public void TestByuDetails()
            {
                Assert.AreEqual(1000, cws.Money);
                Assert.AreEqual("Успех", cws.ByuDetails("Замена тормозов", 5));
                Assert.AreEqual(1, cws.CountingDetails("Замена тормозов").CoutDetails);
                Assert.AreEqual(850, cws.Money);

            }

            [Test]
            public void TestNotBuy()
            {
                Assert.AreEqual("Такой детали не существует!", cws.ByuDetails("Замена масла", 2));
                Assert.AreEqual("Недостаточно средст для покупки", cws.ByuDetails("Замена двигателя", 31));
            }


            [Test]
            public void TestDelivery()
            {
                Assert.AreEqual(3, cws.CountingDetails("Замена руля").CoutDetails);
                Assert.DoesNotThrow(() => { cws.Delivery("Замена руля", 10); });
                Assert.AreEqual(13, cws.CountingDetails("Замена руля").CoutDetails);

            }


            [Test]
            public void TestDeleteDetails()
            {
                cws.DeleteDetails("Замена фар");
                Assert.IsFalse(cws.ContainsDetails("Замена фар"));
            }

            [Test]
            public void TestPriceChange()
            {
                Assert.AreEqual(500, cws.CountingDetails("Замена двигателя").Price);
                Assert.DoesNotThrow(() => { cws.PriceChange("Замена двигателя", 20); });
                Assert.AreEqual(20, cws.CountingDetails("Замена двигателя").Price);

            }

            [Test]
            public void TestCountingMoney()
            {
                cws.ByuDetails("Замена фар", 5);
                Assert.AreEqual(500, cws.CountingMoney("Замена фар"));
            }

            [Test]
            public void TestCountingSoldPart()
            {
                cws.ByuDetails("Замена фар", 5);
                Assert.AreEqual(5, cws.CountingSoldPart("Замена фар"));
            }


            [Test]
            public void TestNewDetails()
            {
                cws.NewDetails("Замена КПП", new Service { Price = 20, CoutDetails = 50 });
                Assert.IsTrue(cws.ContainsDetails("Замена КПП"));

            }
       
        
        }
    }
}
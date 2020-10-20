using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class CarsWorkShop
    {
        /// <summary>
        /// ���������� �����
        /// </summary>
        public int Money { get; set; }

        /// <summary>
        /// ������������ �����
        /// </summary>
        private Dictionary<string, Service> Services { get; set; }

        /// <summary>
        /// ������� �������
        /// </summary>
        private Dictionary<string, Service> NumberDetails { get; set; }

        /// <summary>
        /// ������� ����� �������
        /// </summary>
        /// <param name="name">�������� �������</param>
        /// <param name="price">��������� ������� �������</param>
        /// <returns></returns>
        public string ByuDetails(string name, int count)
        {
            if (!Services.ContainsKey(name))
            {
                return "����� ������ �� ����������!";
            }

            var service = Services[name];
            int price = service.Price * count;

            if (price > Money)
            {
                return "������������ ������ ��� �������";
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

                return "�����";
            }

        }

        /// <summary>
        /// �������� ������
        /// </summary>
        /// <param name="name">�������� �������</param>
        /// <param name="count">����������</param>
        public void Delivery(string name, int count)
        {
            var service = Services[name];
            service.CoutDetails += count;
            Services[name] = service;
        }

        /// <summary>
        /// ���������� ����� �������
        /// </summary>
        /// <param name="name">�������� ������</param>
        /// <param name="service">������</param>
        public void NewDetails(string name, Service service)
        {
            if (!Services.ContainsKey(name))
                Services.Add(name, service);
        }

        /// <summary>
        /// ������� �������� �������
        /// </summary>
        /// <param name="name">������������ �������</param>
        /// <returns>���������� �������� �������</returns>
        public int CountingSoldPart(string name)
        {
            return NumberDetails[name].CoutDetails;
        }

        /// <summary>
        /// ��������� ���� �������
        /// </summary>
        /// <param name="name">������������ �������</param>
        /// <param name="newPrice">����� �������</param>
        public void PriceChange(string name, int newPrice)
        {
            var service = Services[name];
            service.Price = newPrice;
            Services[name] = service;
        }

        /// <summary>
        /// ������� ����� ��������� �������
        /// </summary>
        /// <param name="name">������������ �������</param>
        /// <returns>����� ����� �������� �������</returns>
        public int CountingMoney(string name)
        {
            return NumberDetails[name].Price;
        }


        /// <summary>
        /// ���������� ������� �� ������
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Service CountingDetails(string name)
        {
            return Services[name];
        }

        /// <summary>
        /// �������� �������
        /// </summary>
        /// <param name="name"></param>
        public void DeleteDetails(string name)
        {
            if (Services.ContainsKey(name))
                Services.Remove(name);
        }

        /// <summary>
        /// �������� �� ������������� �������
        /// </summary>
        /// <param name="name">�������� �������</param>
        /// <returns>���������� �� ����� �� ������</returns>
        public bool ContainsDetails(string name)
        {
            return Services.ContainsKey(name);
        }

        /// <summary>
        /// C�����
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
                cws.Services.Add("������ ���������", new Service { Price = 500, CoutDetails = 9 });
                cws.Services.Add("������ ���", new Service { Price = 100, CoutDetails = 5 });
                cws.Services.Add("������ ����", new Service { Price = 200, CoutDetails = 3 });
                cws.Services.Add("������ ���", new Service { Price = 450, CoutDetails = 4 });
                cws.Services.Add("������ ��������", new Service { Price = 30, CoutDetails = 6 });
                cws.Services.Add("������ �����������", new Service { Price = 350, CoutDetails = 7 });

                foreach (var item in cws.Services.Keys)
                {
                    cws.NumberDetails.Add(item, new Service { CoutDetails = 0, Price = 0 });
                }
            }


            [Test]
            public void TestByuDetails()
            {
                Assert.AreEqual(1000, cws.Money);
                Assert.AreEqual("�����", cws.ByuDetails("������ ��������", 5));
                Assert.AreEqual(1, cws.CountingDetails("������ ��������").CoutDetails);
                Assert.AreEqual(850, cws.Money);

            }

            [Test]
            public void TestNotBuy()
            {
                Assert.AreEqual("����� ������ �� ����������!", cws.ByuDetails("������ �����", 2));
                Assert.AreEqual("������������ ������ ��� �������", cws.ByuDetails("������ ���������", 31));
            }


            [Test]
            public void TestDelivery()
            {
                Assert.AreEqual(3, cws.CountingDetails("������ ����").CoutDetails);
                Assert.DoesNotThrow(() => { cws.Delivery("������ ����", 10); });
                Assert.AreEqual(13, cws.CountingDetails("������ ����").CoutDetails);

            }


            [Test]
            public void TestDeleteDetails()
            {
                cws.DeleteDetails("������ ���");
                Assert.IsFalse(cws.ContainsDetails("������ ���"));
            }

            [Test]
            public void TestPriceChange()
            {
                Assert.AreEqual(500, cws.CountingDetails("������ ���������").Price);
                Assert.DoesNotThrow(() => { cws.PriceChange("������ ���������", 20); });
                Assert.AreEqual(20, cws.CountingDetails("������ ���������").Price);

            }

            [Test]
            public void TestCountingMoney()
            {
                cws.ByuDetails("������ ���", 5);
                Assert.AreEqual(500, cws.CountingMoney("������ ���"));
            }

            [Test]
            public void TestCountingSoldPart()
            {
                cws.ByuDetails("������ ���", 5);
                Assert.AreEqual(5, cws.CountingSoldPart("������ ���"));
            }


            [Test]
            public void TestNewDetails()
            {
                cws.NewDetails("������ ���", new Service { Price = 20, CoutDetails = 50 });
                Assert.IsTrue(cws.ContainsDetails("������ ���"));

            }


        }
    }
}
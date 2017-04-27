using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyStoreDomain.Abstract;
using MyStoreDomain.Entities;
using MyStoreWebUI.Controllers;
using MyStoreWebUI.Models;
using MyStoreWebUI.HtmlHelpers;

namespace SportsStore.UnitTests 
{

    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void Can_Paginate()
        {

            // przygotowanie
            Mock<IitemsRepository> mock = new Mock<IitemsRepository>();
            mock.Setup(m => m.Myitems).Returns(new MyItem[] {
                new MyItem {NumberID = 1, Name1 = "P1"},
                new MyItem {NumberID = 2, Name1 = "P2"},
                new MyItem {NumberID = 3, Name1 = "P3"},
                new MyItem {NumberID = 4, Name1 = "P4"},
                new MyItem {NumberID = 5, Name1 = "P5"}
            });

            // utworzenie kontrolera i ustawienie 3-elementowej strony
            HomeController controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            // działanie
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // asercje
            MyItem[] prodArray = result.MyItems.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name1, "P4");
            Assert.AreEqual(prodArray[1].Name1, "P5");
        }


        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // przygotowanie - definiowanie metody pomocniczej HTML — potrzebujemy tego,
            // aby użyć metody rozszerzającej
            HtmlHelper myHelper = null;

            // przygotowanie - tworzenie danych PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // przygotowanie - konfigurowanie delegatu z użyciem wyrażenia lambda
            Func<int, string> pageUrlDelegate = i => "Strona" + i;

            // działanie
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // asercje
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Strona1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Strona2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Strona3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {

            // przygotowanie
            Mock<IitemsRepository> mock = new Mock<IitemsRepository>();
            mock.Setup(m => m.Myitems).Returns(new MyItem[] {
                new MyItem {NumberID = 1, Name1 = "P1"},
                new MyItem {NumberID = 2, Name1 = "P2"},
                new MyItem {NumberID = 3, Name1 = "P3"},
                new MyItem {NumberID = 4, Name1 = "P4"},
                new MyItem {NumberID = 5, Name1 = "P5"}
            });

            // przygotowanie
            HomeController controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            // działanie
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;


            // asercje
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {

            // przygotowanie
            // - utworzenie imitacji repozytorium
            Mock<IitemsRepository> mock = new Mock<IitemsRepository>();
            mock.Setup(m => m.Myitems).Returns(new MyItem[] {
                new MyItem {NumberID = 1, Name1 = "P1", Category = "Cat1"},
                new MyItem {NumberID = 2, Name1 = "P2", Category = "Cat2"},
                new MyItem {NumberID = 3, Name1 = "P3", Category = "Cat1"},
                new MyItem {NumberID = 4, Name1 = "P4", Category = "Cat2"},
                new MyItem {NumberID = 5, Name1 = "P5", Category = "Cat3"}
            });

            // przygotowanie - utworzenie kontrolera i ustawienie 3-elementowej strony
            HomeController controller = new HomeController(mock.Object);
            controller.PageSize = 3;

            // działanie
            MyItem[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).MyItems.ToArray();

            // asercje
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name1 == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name1 == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {

            // przygotowanie
            // - utworzenie imitacji repozytorium
            Mock<IitemsRepository> mock = new Mock<IitemsRepository>();
            mock.Setup(m => m.Myitems).Returns(new MyItem[] {
                new MyItem {NumberID = 1, Name1 = "P1", Category = "Jabłka"},
                new MyItem {NumberID = 2, Name1 = "P2", Category = "Jabłka"},
                new MyItem {NumberID = 3, Name1 = "P3", Category = "Śliwki"},
                new MyItem {NumberID = 4, Name1 = "P4", Category = "Pomarańcze"},
            });

            // przygotowanie - utworzenie kontrolera
            NavController target = new NavController(mock.Object);// tu juz sie robi objekt z klasy nav

            // działanie - pobranie zbioru kategorii
            string[] results = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // asercje
            Assert.AreEqual(results.Length, 3);
            Assert.AreEqual(results[0], "Jabłka");
            Assert.AreEqual(results[1], "Pomarańcze");
            Assert.AreEqual(results[2], "Śliwki");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {

            // przygotowanie
            // - utworzenie imitacji repozytorium
            Mock<IitemsRepository> mock = new Mock<IitemsRepository>();
            mock.Setup(m => m.Myitems).Returns(new MyItem[] {
                new MyItem {NumberID = 1, Name1 = "P1", Category = "Jabłka"},
                new MyItem {NumberID = 4, Name1 = "P2", Category = "Pomarańcze"},
            });

            // przygotowanie - utworzenie kontrolera 
            NavController target = new NavController(mock.Object);

            // przygotowanie - definiowanie kategorii do wybrania
            string categoryToSelect = "Jabłka";

            // działanie
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // asercje
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            // przygotowanie
            // - utworzenie imitacji repozytorium
            Mock<IitemsRepository> mock = new Mock<IitemsRepository>();
            mock.Setup(m => m.Myitems).Returns(new MyItem[] {
                new MyItem {NumberID = 1, Name1 = "P1", Category = "Cat1"},
                new MyItem {NumberID = 2, Name1 = "P2", Category = "Cat2"},
                new MyItem {NumberID = 3, Name1 = "P3", Category = "Cat1"},
                new MyItem {NumberID = 4, Name1 = "P4", Category = "Cat2"},
                new MyItem {NumberID = 5, Name1 = "P5", Category = "Cat3"}
            });

            // przygotowanie - utworzenie kontrolera i ustawienie 3-elementowej strony
            HomeController target = new HomeController(mock.Object);
            target.PageSize = 3;

            // działanie - testowanie liczby produktów dla różnych kategorii
            int res1 = ((ProductsListViewModel)target
                .List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target
                .List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target
                .List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target
                .List(null).Model).PagingInfo.TotalItems;

            // asercje
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }

    }
}
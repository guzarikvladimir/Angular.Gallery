using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Angular.Tests
{
    [TestFixture]
    public class UnitTest1
    {
        [Test]
        public void Login()
        {
            IWebDriver driver = new ChromeDriver();
            string login = "admin@gmail.com";
            driver.Navigate().GoToUrl("http://localhost:54287/Angular/gallery");
            driver.FindElement(By.Id("emailAddress")).SendKeys(login);
            driver.FindElement(By.Id("password")).SendKeys("admin");
            driver.FindElement(By.Id("login")).Click();
            string expected = "Hello, " + login + "!";
            var actual = "Hello, " + driver.FindElement(By.Id("hello")).GetAttribute("value") + "!";
            Assert.AreEqual(login, actual);
        }

        [Test]
        public void JsTest()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:54287/Angular/home");
            driver.FindElement(By.Id("goEdit")).Click();
            driver.FindElement(By.Id("editArea")).Clear();
            string test = "some value";
            driver.FindElement(By.Id("editArea")).SendKeys(test);
            driver.FindElement(By.Id("applyEdit")).Click();
            var actual = driver.FindElement(By.Id("editArea")).GetAttribute("value");
            Assert.AreEqual(test, actual);
        }
    }
}

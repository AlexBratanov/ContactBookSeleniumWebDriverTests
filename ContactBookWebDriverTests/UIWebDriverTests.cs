using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome; // install WebDiver and Selenium Chrome. also check the browser vertion. to be the same.
using System;
using System.Linq;

namespace ContactBookWebDriverTests
{
    public class UITests
    {
        private const string url = "https://contactbook.nakov.repl.co/";// change the url 
        private WebDriver driver;

        [SetUp]
        public void OpenBrowser()
        {
            this.driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        [TearDown]
        public void CloseBrowser()
        {
            this.driver.Quit();
        }

        [Test]
        public void firstLastNames()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.LinkText("Contacts")).Click();
            Assert.That(driver.FindElement(By.CssSelector("#contact1 .fname > td")).Text, Is.EqualTo("Steve"));
            Assert.That(driver.FindElement(By.CssSelector("#contact1 .lname > td")).Text, Is.EqualTo("Jobs"));
        }
        [Test]
        public void findAlbert()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a:nth-child(3) > .icon")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys("albert");
            driver.FindElement(By.Id("search")).Click();
            Assert.That(driver.FindElement(By.CssSelector("#contact3 .fname > td")).Text, Is.EqualTo("Albert"));
            Assert.That(driver.FindElement(By.CssSelector(".lname > td")).Text, Is.EqualTo("Einstein"));
        }
        [Test]
        public void emptyResults()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a:nth-child(3) > .icon")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys("invalid2635");
            driver.FindElement(By.Id("search")).Click();
            Assert.That(driver.FindElement(By.Id("searchResult")).Text, Is.EqualTo("No contacts found."));
            driver.FindElement(By.CssSelector("html")).Click();
        }
        [Test]
        public void createContactInvalidData()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a:nth-child(2) > .icon")).Click();
            driver.FindElement(By.Id("firstName")).SendKeys("Test1");
            driver.FindElement(By.Id("create")).Click();
            Assert.That(driver.FindElement(By.CssSelector(".err")).Text, Is.EqualTo("Error: Last name cannot be empty!"));
        }
        [Test]
        public void createNewValidContact()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("a:nth-child(2) > .icon")).Click();
            driver.FindElement(By.Id("firstName")).SendKeys("Test3");
            driver.FindElement(By.Id("lastName")).SendKeys("LastTest3");
            driver.FindElement(By.Id("email")).SendKeys("Test@test.com");
            driver.FindElement(By.Id("phone")).SendKeys("222333666");
            driver.FindElement(By.Id("create")).Click();
            var allContacts = driver.FindElements(By.CssSelector("table.contact-entry"));
            var lastContact = allContacts.Last();

            var firstNameLabel = lastContact.FindElement(By.CssSelector("tr.fname > td")).Text;
            var lastNameLabel = lastContact.FindElement(By.CssSelector("tr.lname > td")).Text;

            Assert.That(firstNameLabel, Is.EqualTo("Test3"));
            Assert.That(lastNameLabel, Is.EqualTo("LastTest3"));
        }
    }
}
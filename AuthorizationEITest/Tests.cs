using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;

namespace AuthorizationEITest
{
    public class TestsEI
    {
        #region

        private IWebDriver DriverEI;
        private WebDriverWait wait;

        private By LKButton = By.XPath("//div[@class='main-menu__top main-menu__top_individual'] //span[@class='text-icon' and text()='Личный кабинет']");
        private By ContinueAnywayButton = By.XPath("//button[@class='detailsLink']");
        private By DemoLKButton = By.XPath("//a[@class='button button__medium button__100 button__outline-white']");
        private By ImageElement = By.XPath("//*[@xmlns='http://www.w3.org/2000/svg']");
        private By TitleSumElement = By.XPath("//span[@class='nowrap main-page_title_sum']");

        private int ExpectedSize = 32;
        private int MaxExpectedAmount = 200000;

        private int MaxWaitingTime = 5;

        #endregion

        [SetUp]
        public void Setup()
        {
            DriverEI = new InternetExplorerDriver();
            DriverEI.Navigate().GoToUrl("https://www.nalog.ru/");
            DriverEI.Manage().Window.Maximize();
            DriverEI.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(MaxWaitingTime);
        }

        [Test]
        public void AuthorizationTest()
        {
            FindElementClick(LKButton);

            new WebDriverWait(DriverEI, TimeSpan.FromSeconds(MaxWaitingTime)).Until(
            d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            if (DriverEI.Url == "https://lkfl2.nalog.ru/lkfl/static/browserOutOfDate/browserOutOfDate.html")
            {
                FindElementClick(ContinueAnywayButton);
            }

            FindElementClick(DemoLKButton);

            WaitingElement(ImageElement);
            var image_element = DriverEI.FindElement(ImageElement);
            var title_sum_element = DriverEI.FindElement(TitleSumElement);

            Assert.AreEqual(image_element.Size.Width, ExpectedSize);
            Assert.AreEqual(image_element.Size.Height, ExpectedSize);

            int title_sum = Convert.ToInt32(title_sum_element.Text.Split('.').First().Replace(" ", ""));

            if (title_sum <= MaxExpectedAmount)
            {
                Assert.Pass();
            }
        }

        [TearDown]
        public void TearDown()
        {
            DriverEI.Close();
        }

        public void WaitingElement(By xpath)
        {
            WebDriverWait wait = new WebDriverWait(DriverEI, TimeSpan.FromSeconds(MaxWaitingTime));
            wait.Until(ExpectedConditions.ElementToBeClickable(xpath));
        }

        public void FindElementClick(By xpath_button)
        {
            WaitingElement(xpath_button);
            var _button = DriverEI.FindElement(xpath_button);
            _button.SendKeys(Keys.Enter);
        }
    }
}

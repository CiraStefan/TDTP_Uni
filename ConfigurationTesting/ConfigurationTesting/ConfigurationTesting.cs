using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Support.UI;
using System.Xml.Linq;
using Xunit;
using Assert = Xunit.Assert;

namespace ConfigurationTesting
{
    public class ConfigurationTests
    {
        [Theory]
        [InlineData("chrome")]
        [InlineData("firefox")]
        [InlineData("edge")]
        [InlineData("safari")]
        public void DifferentBrowsers_UserIsAbleToSearchForAnItem_AddItemToBasket(string browserType)
        {
            IWebDriver driver;

            switch (browserType)
            {
                case "chrome":
                    driver = new ChromeDriver();
                    break;

                case "firefox":
                    driver = new FirefoxDriver();
                    break;

                case "edge":
                    driver = new EdgeDriver();
                    break;

                case "safari":
                    driver = new SafariDriver();
                    break;

                default:
                    return;
            }

            driver.Url = "https://www.remedius.ro";
            driver.Manage().Window.Size = new System.Drawing.Size(1200, 800);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement searchElem = wait.Until(e => e.FindElement(By.Name("q")));
            searchElem.SendKeys("miere" + Keys.Enter);
            Thread.Sleep(1000);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("window.scrollBy(0, 300);");
            jsExecutor.ExecuteScript("document.querySelector('.cookie-policy-overlay__buttons button[data-bs-dismiss=\"offcanvas\"]').click();");

            Thread.Sleep(1000);

            IWebElement adaugaButon = wait.Until(e => e.FindElement(By.LinkText("Adauga in cos")));
            adaugaButon.Click();

            string expectedValue = "1";
            var emailLocator = RelativeBy.WithLocator(By.TagName("input")).Above(By.LinkText("Detalii Livrare"));

            string actualValue = wait.Until(e => e.FindElement(By.CssSelector("[class*='form-control'][class*='cart__item-quantity']")).GetAttribute("value"));

            Assert.Equal(expectedValue, actualValue);
            driver.Quit();
        }

        [Theory]
        [InlineData(PhoneTypes.IphoneXR)]
        [InlineData(PhoneTypes.NestHub)]
        [InlineData(PhoneTypes.GalxyFold)]
        [InlineData(PhoneTypes.IphoneSE)]
        [InlineData(PhoneTypes.IpadMini)]
        public void DifferentResolutions_UserIsAbleToSearchForItem_AddItemToBasket(PhoneTypes type)
        {
            int width;
            int height;
            switch (type)
            {
                case PhoneTypes.IphoneXR:
                    width = 414;
                    height = 896;
                    break;

                case PhoneTypes.NestHub:
                    width = 1024;
                    height = 600;
                    break;

                case PhoneTypes.GalxyFold:
                    width = 280;
                    height = 653;
                    break;

                case PhoneTypes.IphoneSE:
                    width = 375;
                    height = 667;
                    break;

                case PhoneTypes.IpadMini:
                    width = 800;
                    height = 1024;
                    break;

                default:
                    return;
            }

            IWebDriver driver = new ChromeDriver();
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            driver.Url = "https://www.remedius.ro";
            driver.Manage().Window.Size = new System.Drawing.Size(width, height);
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement searchElem = wait.Until(e => e.FindElement(By.Name("q")));
            jsExecutor.ExecuteScript("arguments[0].click();", searchElem);
            jsExecutor.ExecuteScript("arguments[0].value = 'miere';", searchElem);
            jsExecutor.ExecuteScript("arguments[0].dispatchEvent(new Event('input'));", searchElem);
            jsExecutor.ExecuteScript("arguments[0].dispatchEvent(new Event('change'));", searchElem);
            jsExecutor.ExecuteScript("arguments[0].form.submit();", searchElem);

            Thread.Sleep(1000);

            jsExecutor.ExecuteScript("document.querySelector('.cookie-policy-overlay__buttons button[data-bs-dismiss=\"offcanvas\"]').click();");

            jsExecutor.ExecuteScript("window.scrollBy(0, 300);");

            Thread.Sleep(1000);

            IWebElement adaugaButon = wait.Until(e => e.FindElement(By.LinkText("Adauga in cos")));
            adaugaButon.Click();

            string expectedValue = "1";
            var emailLocator = RelativeBy.WithLocator(By.TagName("input")).Above(By.LinkText("Detalii Livrare"));

            string actualValue = wait.Until(e => e.FindElement(By.CssSelector("[class*='form-control'][class*='cart__item-quantity']")).GetAttribute("value"));

            Assert.Equal(expectedValue, actualValue);
            driver.Quit();
        }
    }
}
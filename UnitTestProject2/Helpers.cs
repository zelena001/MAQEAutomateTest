using System;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomateTestForFactools
{
    public class Helpers
    {
        public void ClickElement(IWebDriver driver, By itemPath)
        {
            WaitToBeClickable(driver, itemPath, 10);
            var targetElement = driver.FindElement(itemPath);
            targetElement.Click();
        }

        public void SetElementValue(IWebDriver driver, By itemPath, string value)
        {
            WaitToBeClickable(driver, itemPath, 10);
            var targetElement = driver.FindElement(itemPath);
            targetElement.Clear();
            targetElement.SendKeys(value);
        }
        
        public void WaitToBeClickable(IWebDriver driver, By itemPath, int timeInSec)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSec));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(itemPath));
        }

        public void AssertElementValue(IWebDriver driver, By itemPath, string expectedValue, string exceptionHandlerMsg)
        {
            WaitToBeClickable(driver, itemPath, 5);
            string actualValue = driver.FindElement(itemPath).GetAttribute("value");
            Assert.AreEqual(expectedValue, actualValue, exceptionHandlerMsg +" Expected value " + actualValue + " to be equal to " + expectedValue);
        }
        public void AssertElementText(IWebDriver driver, By itemPath, string expectedText, string exceptionHandlerMsg)
        {
            string actualText = driver.FindElement(itemPath).Text.Trim();
            Assert.AreEqual(expectedText, actualText, exceptionHandlerMsg + " Expected text " + actualText + " to be equal to " + expectedText);
        }

        public void SelectProductVariant(IWebDriver driver, int variantIndex, int variantChoice)
        {
            int variant = variantIndex - 1;
            int choice = variantChoice + 1;
            By Variant = By.XPath("//*[@id=\"product-variants-"+variant+"\"]/div");
            By Choice = By.XPath("//*[@id=\"product-variants-" + variant + "\"]/div/ul/li[" + choice + "]");
            WaitToBeClickable(driver, Variant, 10);
            ClickElement(driver, Variant);
            ClickElement(driver, Choice);
        }

        public void WaitToBeNotExist(IWebDriver driver, By itemPath, int timeInSec)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSec));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(itemPath));
        }

        public void AssertStringToContainElementText(IWebDriver driver, By itemPath, string MainMessage, string exceptionHandlerMsg)
        {
            if ((MainMessage).Contains((driver.FindElement(itemPath).Text)))
            {
                bool contain = true;
                Assert.AreEqual(true, contain);
            }
            else
            {
                bool contain = false;
                Assert.AreEqual(true, contain, exceptionHandlerMsg +" Expect" + MainMessage + " To Contain " + (driver.FindElement(itemPath).Text));
            }
        }
    }
}

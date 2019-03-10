﻿using System;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using OpenQA.Selenium.Support.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomateTestForFactools
{
    class Helpers
    {
        public string nonInt = "!@#$%^&*()_+-=QWERTYUIOP{}|[]\\ASDFGHJKL:;'\"ZXCVBNM<>?,./";
        public void ClickElement(IWebDriver driver, By elementPath)
        {
            var targetElement = driver.FindElement(elementPath);
            targetElement.Click();
        }
        public void WaitToClick(IWebDriver driver, By itemPath, int timeInSec)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSec));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(itemPath));
        }
        public void SetElementValue(IWebDriver driver, By elementPath, string value)
        {
            var targetElement = driver.FindElement(elementPath);
            targetElement.Clear();
            targetElement.SendKeys(value);
            //Assert.AreEqual(value, targetElement.Text, "can't insert value, caurrent value is "+targetElement.Text);
        }
        
        public void WaitToBeClickable(IWebDriver driver, By itemPath, int timeInSec)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeInSec));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(itemPath));
        }
        public void AssertElementValue(IWebDriver driver, By itemPath, string expectedValue, string exceptionHandlerMsg)
        {
            //enable var x for debuggin
            //var x = driver.FindElement(itemPath);
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
        //public string GetElementText(IWebDriver driver, By itemPath, string returnText)
        //{
        //    string objectText = "2";
        //    return x;

        //}
    }
}

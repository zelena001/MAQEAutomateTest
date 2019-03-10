using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace AutomateTestForFactools
{
    [TestClass]
    public class CartPage
    {
        static private IWebDriver _driver;
        private const string FacToolProductUrl = "https://factools.qa.maqe.com/p/20";
        private const string FacToolProductUrl2 = "https://factools.qa.maqe.com/p/21";
        private Helpers _helpers = new Helpers();

        //Header Obhect Locator
        private By MiniCart = By.XPath("//*[@data-test=\"mini-cart\"]");
        private By CartBadgeAmount = By.XPath("//*[@data-test=\"cart-badge-amount\"]");

        //Item Detail Page Object Locator
        private By ProdName = By.XPath("//*[@data-test=\"product-name\"]");
        private By ProdPrice = By.XPath("//*[@data-test=\"product-price\"]");
        private By ProdSku = By.XPath("//*[@data-test=\"product-sku\"]");
        private By ProdQuantity = By.XPath("//*[@data-test=\"amount-quantity\"]");
        private By ProdAddToCartBtn = By.XPath("//*[@data-test=\"add-to-cart\"]");
        private By ProdCartNotiBtn = By.XPath("//*[@href=\"https://factools.qa.maqe.com/cart\"]");
        //expected Error Handler when adding more than 99 for item amount from Item Detail Screen
        private By expectedErrorMsg = By.XPath("//*[@data-test=\"exception\"]");

        //Cart Page Object Locator
        private By CartProdSku = By.XPath("//div[5]/div[1]//*[@data-test=\"cart-product-sku\"]");
        private By CartProdPrice = By.XPath("//div[5]/div[1]//*[@data-test=\"cart-product-sku-price\"]");
        private By CartProdAmount = By.XPath("//div[5]/div[1]//*[@data-test=\"cart-product-sku-amount\"]");
        private By CartProdTotal = By.XPath("//div[5]/div[1]//*[@data-test=\"cart-product-sku-price-total\"]");
        private By CartProdSku2 = By.XPath("//div[5]/div[2]//*[@data-test=\"cart-product-sku\"]");
        private By CartProdPrice2 = By.XPath("//div[5]/div[2]//*[@data-test=\"cart-product-sku-price\"]");
        private By CartProdAmount2 = By.XPath("//div[5]/div[2]//*[@data-test=\"cart-product-sku-amount\"]");
        private By CartProdTotal2 = By.XPath("//div[5]/div[2]//*[@data-test=\"cart-product-sku-price-total\"]");
        private By CartTotal = By.XPath("//*[@data-test=\"cart-price-subtotal\"]");
        private By CartIncreaseItem = By.XPath("//*[@data-test=\"cart-product-increase\"]");
        private By CartDecreaseItem = By.XPath("//*[@data-test=\"cart-product-decrease\"]");
        private By CartRemoveItem = By.XPath("//*[@data-test=\"cart-product-remove\"]");


        [TestInitialize]
        public void TestSetUp()
        {
            
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();

        }
        [TestCleanup]
        public void TestCleanUp()
        {
            _driver.Quit();
        }

        [TestMethod]
        public void AddNewLineItemWith99QuantityIntoTheCartSuccessfully()
        {
            //arrange
            int quantity = 99;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);
            
            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity,quantity.ToString());
            string productSku1 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);

            //act 
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //assert
            _helpers.WaitToBeClickable(_driver,CartProdAmount, 5 );
            _helpers.AssertElementText(_driver, CartProdPrice, pricePerUnit, "Validate Line Item price detect incorrect behavior");
            _helpers.AssertElementValue(_driver, CartProdAmount, quantity.ToString(), "Validate Line Amount detect incorrect behavior");
            _helpers.AssertStringToContainElementText(_driver, CartProdSku, productSku1, "Product SKU for first item doesn't show correct in Cart Screen");
            

         }

        [TestMethod]
        public void AddNewLineItemWith100QuantityIntoTheCartShouldBeLimitTo2DigitsByUI()
        {
            //arrange
            string quantity = "100";
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            //act
            _driver.FindElement(ProdQuantity).Clear();
            _driver.FindElement(ProdQuantity).SendKeys(quantity);
            //assert
            _helpers.AssertElementValue(_driver, ProdQuantity, "10", "Validate Line Item Quantity Input detect incorrect behavior");

        }

        [TestMethod]
        public void AddNewLineItemWith0QuantityIntoTheCartShouldBeDefaultTo1()
        {
            //arrange
            string quantity = "0";
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            //act
            _driver.FindElement(ProdQuantity).Clear();
            _driver.FindElement(ProdQuantity).SendKeys(quantity);
            //assert
            _helpers.AssertElementValue(_driver, ProdQuantity, "1", "Validate Line Item Quantity Input detect incorrect behavior");
        }

        [TestMethod]
        public void AddNewLineItemWithEmptyQuantityIntoTheCartShouldBeDefaultTo1()
        {
            //arrange
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            //act
            _driver.FindElement(ProdQuantity).Clear();
            _driver.FindElement(ProdQuantity).SendKeys(Keys.Tab);
            //assert
            _helpers.AssertElementValue(_driver, ProdQuantity, "1", "Validate Line Item Quantity Input detect incorrect behavior");
        }

        [TestMethod]
        public void AddNewLineItemWithWithNonIntQuantityIntoTheCartShouldBeBeValidateByUI()
        {
            //arrange
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _driver.FindElement(ProdQuantity).Clear();
            _helpers.SetElementValue(_driver, ProdQuantity, "7");
            //act
            _helpers.SetElementValue(_driver, ProdQuantity, _helpers.nonInt);
            _driver.FindElement(ProdQuantity).SendKeys(Keys.Tab);
            //assert
            _helpers.AssertElementValue(_driver, ProdQuantity, "7", "Validate Line Item Quantity Input detect incorrect behavior");

            // It's possible to input non-int into the field, but I wasn't be able to retrieve value if it's not int. Workaround is to put constant value and expect constant value as in the script
        }

        [TestMethod]
        public void IncreaseLineItemAmountToBe99Correctly()
        {
            //arrange
            int quantity = 98;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartIncreaseItem, 5);
            _helpers.ClickElement(_driver, CartIncreaseItem);

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "99", "Validate Line Amount detect incorrect behavior");

        }

        [TestMethod]
        public void IncreaseLineItemAmountToBe100TheLineItemQuantityShouldNotExceed99()
        {
            //arrange
            int quantity = 99;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartIncreaseItem, 5);
            _helpers.ClickElement(_driver, CartIncreaseItem);

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "99", "Validate Line Amount detect incorrect behavior");

        }

        [TestMethod]
        public void DecreaseLineItemAmountToBe1Correctly()
        {
            //arrange
            int quantity = 2;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartDecreaseItem, 5);
            _helpers.ClickElement(_driver, CartDecreaseItem);

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "1", "Validate Line Amount detect incorrect behavior");

        }

        [TestMethod]
        public void DecreaseLineItemAmountToBe0LineItemQUantityShouldNotBeLowerThan1()
        {
            //arrange
            int quantity = 1;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartDecreaseItem, 5);
            _helpers.ClickElement(_driver, CartDecreaseItem);

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "1", "Validate Line Amount detect incorrect behavior");

        }

        [TestMethod]
        public void SetLineItemAmountToBe99Correctly()
        {
            //arrange
            int quantity = 98;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.SetElementValue(_driver, CartProdAmount, "99");

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "99", "Validate Line Amount detect incorrect behavior");

        }

        [TestMethod]
        public void SetLineItemAmountToBe100TheLineItemQuantityShouldNotExceed2Digits()
        {
            //arrange
            int quantity = 98;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.SetElementValue(_driver, CartProdAmount, "100");

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "10", "Validate Line Amount detect incorrect behavior");

        }

        [TestMethod]
        public void SetLineItemAmountToBe1Correctly()
        {
            //arrange
            int quantity = 2;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.SetElementValue(_driver, CartProdAmount, "1");

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "1", "Validate Line Amount detect incorrect behavior");
            
        }

        [TestMethod]
        public void SetLineItemAmountToBe0LineItemQUantityShouldNotBeLowerThan1()
        {
            //arrange
            int quantity = 2;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.SetElementValue(_driver, CartProdAmount, "0");

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.AssertElementValue(_driver, CartProdAmount, "1", "Validate Line Amount detect incorrect behavior");
            
        }

        [TestMethod]
        public void SetLineItemAmountToBeNonIntQuantityIntoTheCartShouldBeBeValidateByUI()
        {
            //arrange
            int quantity = 2;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            _helpers.SetElementValue(_driver, CartProdAmount, _helpers.nonInt);

            //assert
            _helpers.AssertElementValue(_driver, CartProdAmount, "2", "Validate Line Item Quantity Input detect incorrect behavior");

            // It's possible to input non-int into the field, but I wasn't be able to retrieve value if it's not int. Workaround is to put constant value and expect constant value as in the script
        }

        [TestMethod]
        public void AddExistingLineItemToCauseAmountToExceed99InCartScreen()
        {
            //arrange
            int quantity = 50;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);


            //act 
            _helpers.ClickElement(_driver, ProdAddToCartBtn);

            //assert
            _helpers.AssertElementText(_driver, expectedErrorMsg, "Maximum Amount for each Item per Order is 99, You can manage your Order in your Cart", "Validate Line Item Quantity Input detect incorrect behavior");

            // The Xpath and Expected Error Message is assumption, which doesn't exist in current web application
        }

        [TestMethod]
        public void AddExistingLineItemWithSimilarVariantIntoTheCartSuccessfully()
        {
            //arrange
            int quantity = 1;
            int totalQuantity = quantity * 2;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.SelectProductVariant(_driver, 1, 0);
            _helpers.SelectProductVariant(_driver, 1, 1);
            Thread.Sleep(500);
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);

            //act 
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);

            //assert

            _helpers.AssertElementText(_driver, CartProdPrice, pricePerUnit, "Validate Line Item price detect incorrect behavior");
            _helpers.AssertElementValue(_driver, CartProdAmount, totalQuantity.ToString(), "Validate Line Amount detect incorrect behavior");
            Assert.IsTrue((productSku).Contains((_driver.FindElement(CartProdSku).Text)));

        }

        [TestMethod]
        public void AddExistingLineItemWithDifferentVariantIntoTheCartSuccessfully()
        {
            //arrange
            int quantity = 1;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku1 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit1 = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.SelectProductVariant(_driver, 1, 2);
            Thread.Sleep(1000);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            Thread.Sleep(1000);
            // the Web not update information, it need delay or it will pull invalid number
            string productSku2 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit2 = _driver.FindElement(ProdPrice).Text;

            //act 
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);

            //assert

            
            _helpers.AssertElementValue(_driver, CartProdAmount, quantity.ToString(), "Validate Line Amount detect incorrect behavior");
            _helpers.AssertStringToContainElementText(_driver, CartProdSku, productSku1, "Product SKU for first item doesn't show correct in Cart Screen");

            _helpers.AssertElementValue(_driver, CartProdAmount2, quantity.ToString(), "Validate Line Amount detect incorrect behavior");
            _helpers.AssertStringToContainElementText(_driver, CartProdSku2, productSku2, "Product SKU for Second item doesn't show correct in Cart Screen");

        }

        [TestMethod]
        public void AddTwoItemIntoTheCartSuccessfully()
        {
            //arrange
            int quantity = 1;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku1 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit1 = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            Thread.Sleep(1000);
            _driver.Navigate().GoToUrl(FacToolProductUrl2);
            Thread.Sleep(2000);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);
            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku2 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit2 = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);

            //act 
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);

            //assert
            _helpers.AssertElementValue(_driver, CartProdAmount, quantity.ToString(), "Validate Line Amount detect incorrect behavior");
            _helpers.AssertStringToContainElementText(_driver, CartProdSku, productSku1, "Product SKU for first item doesn't show correct in Cart Screen");

            _helpers.AssertElementValue(_driver, CartProdAmount2, quantity.ToString(), "Validate Line Amount detect incorrect behavior");
            _helpers.AssertStringToContainElementText(_driver, CartProdSku2, productSku2, "Product SKU for Second item doesn't show correct in Cart Screen");

        }

        [TestMethod]
        public void RemoveItemFromTheCartSuccessfully()
        {
            //arrange
            int quantity = 2;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartRemoveItem, 5);
            _helpers.ClickElement(_driver, CartRemoveItem);

            //assert
            _helpers.WaitToBeNotExist(_driver, CartProdTotal,5);
            _helpers.AssertElementText(_driver, CartTotal, "0.00", "Validate Line Amount detect incorrect behavior");
        }

        [TestMethod]
        public void AddNewLineItemIntoTheCartCalculateCorrectLineItemValue()
        {
            //arrange
            int quantity = 99;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku1 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);

            //act 
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //assert
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);
            
            decimal priceTag = Convert.ToDecimal(pricePerUnit);
            decimal lineItemTotal = quantity * priceTag;
            decimal roundedDecinal = decimal.Round(lineItemTotal, 2);
            _helpers.AssertElementText(_driver, CartProdTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");
            _helpers.AssertElementText(_driver, CartTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");

        }

        [TestMethod]
        public void IncreaseLineItemAmountRecalculatedCorrectLineItemValue()
        {
            //arrange
            int quantity = 98;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartIncreaseItem, 5);
            _helpers.ClickElement(_driver, CartIncreaseItem);

            //assert
            Thread.Sleep(1000);
            decimal priceTag = Convert.ToDecimal(pricePerUnit);
            decimal lineItemTotal = (quantity+1) * priceTag;
            decimal roundedDecinal = decimal.Round(lineItemTotal, 2);
            _helpers.AssertElementText(_driver, CartProdTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");
            _helpers.AssertElementText(_driver, CartTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");
        }

        [TestMethod]
        public void DecreaseLineItemAmountRecalculatedCorrectLineItemValue()
        {
            //arrange
            int quantity = 98;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartDecreaseItem, 5);
            _helpers.ClickElement(_driver, CartDecreaseItem);

            //assert
            Thread.Sleep(1000);
            decimal priceTag = Convert.ToDecimal(pricePerUnit);
            decimal lineItemTotal = (quantity - 1) * priceTag;
            decimal roundedDecinal = decimal.Round(lineItemTotal, 2);
            _helpers.AssertElementText(_driver, CartProdTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");
            _helpers.AssertElementText(_driver, CartTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");
        }


        [TestMethod]
        public void SetLineItemAmountRecalculatedCorrectLineItemValue()
        {
            //arrange
            int quantity = 98;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku = _driver.FindElement(ProdSku).Text;
            string pricePerUnit = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);

            //act 
            _helpers.WaitToBeClickable(_driver, CartIncreaseItem, 5);
            _helpers.SetElementValue(_driver, CartProdAmount, "99");

            //assert
            Thread.Sleep(1000);
            decimal priceTag = Convert.ToDecimal(pricePerUnit);
            decimal lineItemTotal = 99* priceTag;
            decimal roundedDecinal = decimal.Round(lineItemTotal, 2);
            _helpers.AssertElementText(_driver, CartProdTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");
            _helpers.AssertElementText(_driver, CartTotal, roundedDecinal.ToString("0,000.00"), "Line Item Total is not correct, ");

        }

        [TestMethod]
        public void TotalCartValueCalculatedCorrectly()
        {
            //arrange
            int quantity = 1;
            _driver.Navigate().GoToUrl(FacToolProductUrl);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);

            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku1 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit1 = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            Thread.Sleep(1000);
            _driver.Navigate().GoToUrl(FacToolProductUrl2);
            Thread.Sleep(2000);
            _helpers.WaitToBeClickable(_driver, ProdQuantity, 30);
            _helpers.SelectProductVariant(_driver, 1, 1);
            _helpers.SetElementValue(_driver, ProdQuantity, quantity.ToString());
            string productSku2 = _driver.FindElement(ProdSku).Text;
            string pricePerUnit2 = _driver.FindElement(ProdPrice).Text;
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);

            //act 
            _helpers.WaitToBeClickable(_driver, ProdAddToCartBtn, 5);
            _helpers.ClickElement(_driver, ProdAddToCartBtn);
            _helpers.WaitToBeClickable(_driver, ProdCartNotiBtn, 5);
            _helpers.ClickElement(_driver, ProdCartNotiBtn);
            _helpers.WaitToBeClickable(_driver, CartProdAmount, 5);

            //assert
            decimal priceTag1 = Convert.ToDecimal(pricePerUnit1);
            decimal priceTag2 = Convert.ToDecimal(pricePerUnit2);
            decimal lineItemTotal1 = quantity * priceTag1;
            decimal lineItemTotal2 = quantity * priceTag2;
            decimal CartValue = lineItemTotal1 + lineItemTotal2;
            decimal roundedDecinal = decimal.Round(CartValue, 2);
            _helpers.AssertElementText(_driver, CartTotal, roundedDecinal.ToString("0.00"), "Cart Total is not correct, ");
        }
    }

}

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiTests
{
    [TestClass]
    public class BasicTests : TestSession
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [TestMethod]
        public void ShowToastNotification()
        {
            var showNotificationButton = WaitForElementByAutomationId("ShowNotification");
            showNotificationButton.Click();
        }
    }
}

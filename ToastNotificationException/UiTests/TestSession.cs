using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace UiTests
{
    public class TestSession
    {
        private const string WinAppDriverUrl = "http://127.0.0.1:4723";
        private const string ContosoIncAppId = "ContosoInc.ToastNotificationException_t3hjarr2snmzm!App";

        protected static WindowsDriver<WindowsElement> Session;

        public static void Setup(TestContext context)
        {
            if (Session != null)
                return;

            var appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", ContosoIncAppId);

            try
            {
                Session = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), appCapabilities);
            }
            catch
            {
                Session = new WindowsDriver<WindowsElement>(new Uri(WinAppDriverUrl), appCapabilities);
            }

            Thread.Sleep(TimeSpan.FromSeconds(10));

            var allWindowHandles = Session.WindowHandles;
            Session.SwitchTo().Window(allWindowHandles[0]);

            // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
            Session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        public WindowsElement WaitForElementByAutomationId(string automationId, int timeoutInSeconds = 10, bool shouldReturnNull = false)
        {
            WindowsElement element = null;

            var wait = new DefaultWait<WindowsDriver<WindowsElement>>(Session)
            {

                Timeout = TimeSpan.FromSeconds(timeoutInSeconds),
                Message = $"Element with AutomationId \"{automationId}\" was not found within period of {timeoutInSeconds}s."
            };

            wait.IgnoreExceptionTypes(typeof(InvalidOperationException));

            try
            {
                wait.Until(s =>
                {
                    element = s.FindElementByAccessibilityId(automationId);
                    return element != null;
                });
            }
            catch
            {
                if (shouldReturnNull)
                    return null;

                throw;
            }

            return element;
        }
    }
}

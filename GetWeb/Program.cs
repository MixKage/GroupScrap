using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.IO;
using System.Threading;
using CommandLine;

namespace GetWeb
{
    class Options
    {
        [Option('t', "timeout", Default = 10, HelpText = "Maximum waiting time for a response from the webinar.")]
        public double Timeout { get; set; }

        [Option('f', "OutFilePath", Default = "ans.html", HelpText = "Path to output file.")]
        public string OutFilePath { get; set; }

        [Value(0, MetaName = "url", HelpText = "URL to webinar.")]
        public string URL { get; set; }
    }


    class Program
    {
        static Options CurrentOptions;
        static int Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                   .WithParsed<Options>(o =>
                   { CurrentOptions = o; });

            var options = new ChromeOptions();
            options.AddArgument("mute-audio");//Запуск без звука

            string res = "";

            IWebDriver driver = new ChromeDriver(options);
            try
            {
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Navigate().GoToUrl(CurrentOptions.URL);

                do
                {
                    string pageSource = driver.PageSource;
                    if (pageSource.Contains("Подключиться"))
                    {
                        res = PrepairDirect(driver);
                        break;
                    }
                    else if (pageSource.Contains("Посмотреть запись"))
                    {
                        res = PrepairRecord(driver);
                        break;
                    }
                    else if (pageSource.Contains("зарегистрироваться"))
                    {
                        res = PrepairStream(driver);
                        break;
                    }
                } while (true);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return 1;
            }
            finally
            {
                driver?.Dispose();
            }

            File.WriteAllText(CurrentOptions.OutFilePath, res);
            return 0;
        }

        static void SetSliderPercentage(IWebDriver driver, string sliderHandleXpath, string sliderTrackXpath, int percentage)
        {
            var sliderHandle = driver.FindElement(By.XPath(sliderHandleXpath));
            var sliderTrack = driver.FindElement(By.XPath(sliderTrackXpath));
            var width = int.Parse(sliderHandle.GetCssValue("Width").Replace("px", ""));
            var dx = (int)(percentage / 100.0 * width);
            new Actions(driver)
                        .MoveToElement(sliderHandle, dx, 0)
                        .Click()
                        .Build()
                        .Perform();
        }
        static string PrepairRecord(IWebDriver driver)
        {
            driver.FindElement(By.LinkText("ПОСМОТРЕТЬ ЗАПИСЬ")).Click(); // Войти в конференциею
            driver.FindElement(By.ClassName("autoplay-video-allow-btn")).Click();

            SetSliderPercentage(driver, "/html/body/div[1]/div/div/div[2]/div/div", "/html/body/div[1]/div/div/div[2]/div[1]/div/div[2]", 90);

            driver.FindElement(By.XPath("/html/body/div[1]/div/div/div[1]/main/div[1]/div[2]/button[3]")).Click(); //click участники

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.body.style.zoom='5%'");

            while (!driver.PageSource.Contains("Row.P_PARTICIPANTS.15")) { } //Костыль переделать

            return driver.PageSource;
        }
        static string PrepairStream(IWebDriver driver)
        {
            driver.FindElement(By.LinkText("ЗАРЕГИСТРИРОВАТЬСЯ")).Click(); // Войти в конференциею
            driver.FindElement(By.Id("email")).SendKeys("1854pro2@gmail.com\tВроде\tЧеловек\t\t\t\t\n"); // Ввод данных
            do
            {
                var tmp = driver.PageSource;
                if (tmp.Contains("Отправить письмо повторно"))
                    throw new Exception("Укажите ссылку с почты");
                if (tmp.Contains("id=\"name\""))
                    break;
            } while (true);

            driver.FindElement(By.Id("name")).SendKeys("\t\n"); // Подтверждение данных

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            driver.FindElement(By.XPath("/html/body/div[1]/div/div/main/div[1]/div[2]/button[4]")).Click(); //click участники
            js.ExecuteScript("document.body.style.zoom='5%'");

            while (!driver.PageSource.Contains("Вроде")) { } //Костыль переделать

            return driver.PageSource;
        }
        static string PrepairDirect(IWebDriver driver)
        {
            driver.FindElement(By.Id("name")).SendKeys("\t\n"); // Подтверждение данных
            driver.FindElement(By.XPath("/html/body/div[1]/div/div/main/div[1]/div[2]/button[4]")).Click(); //click участники

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("document.body.style.zoom='5%'");

            while (!driver.PageSource.Contains("Вроде")) { } //Костыль переделать

            return driver.PageSource;
        }
    }
}

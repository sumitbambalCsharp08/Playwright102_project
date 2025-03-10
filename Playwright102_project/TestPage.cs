using Microsoft.Playwright;
using SeleniumTests.Tests;

namespace Playwright102_project
{
    public class Tests
    {
        private IPage _page;
        private IBrowser _browser;
        private IBrowserContext _context;
        private IPlaywright _playwright;
        private TestLocators _testLocator;

        [SetUp]
        public async Task Setup()
        {
            string user, accessKey;
            user = Environment.GetEnvironmentVariable("sumit_bambal");
            accessKey = Environment.GetEnvironmentVariable("wE91sD1nmqnNV05GRTBG0snnv61ovDozF7ZcMELVYcXsMetMN2");

            Dictionary<string, object> capabilities = new Dictionary<string, object>();
            Dictionary<string, string> ltOptions = new Dictionary<string, string>();

            ltOptions.Add("name", "Playwright Test");
            ltOptions.Add("build", "Playwright C-Sharp tests on Hyperexecute");
            ltOptions.Add("platform", Environment.GetEnvironmentVariable("HYPEREXECUTE_PLATFORM"));
            ltOptions.Add("user", user);
            ltOptions.Add("accessKey", accessKey);

            capabilities.Add("browserName", "chrome");
            capabilities.Add("browserVersion", "latest");
            capabilities.Add("LT:Options", ltOptions);
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = false });
            _context = await _browser.NewContextAsync();
            _page = await _context.NewPageAsync();
            _testLocator = new TestLocators(_page);
        }

        [Test]
        public async Task TestScenario1()
        {
            await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
            await _testLocator.ClickAsync(_testLocator.SimpleFrmDemo);
            await _testLocator.EnterTextAsync(_testLocator.SimplefrmIp, "Welcome to LambdaTest");
            await _testLocator.ClickAsync(_testLocator.GetCheckedValue);
            Assert.That(_page.Url, Does.Contain("simple-form-demo"));
            Assert.That(await _testLocator.SampleMsg.InnerTextAsync(), Is.EqualTo("Welcome to LambdaTest"));
        }
       
        
        [Test]
        public async Task TestScenario2()
        {
            await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
            await _testLocator.ClickAsync(_testLocator.DragAndDrop);
            await _page.EvaluateAsync(@"() => {
                document.body.style.zoom = '0.50'; 
            }");
            var dragger = _testLocator.Slider;
            var box = await dragger.BoundingBoxAsync();

            if (box != null)
            {
                await _page.Mouse.MoveAsync(box.X + box.Width / 2, box.Y + box.Height / 2);
                await _page.Mouse.DownAsync();
                await _page.Mouse.MoveAsync(box.X + box.Width / 2 + 107, box.Y + box.Height / 2);
                await _page.Mouse.UpAsync();
            }
           
            var textContent = await _testLocator.SliderValue.TextContentAsync();
            Assert.That(textContent, Is.EqualTo("95"));
        }

        [Test]
        public async Task TestScenario3()
        {
            await _page.GotoAsync("https://www.lambdatest.com/selenium-playground/");
            await _page.SetViewportSizeAsync(1920, 1080);
            await _testLocator.ClickAsync(_testLocator.InputForm);
            await _testLocator.ClickAsync(_testLocator.SubmitForm);
            var validationMessage = await _page.EvaluateAsync<string>("document.activeElement.validationMessage");
            Assert.That(validationMessage, Is.EqualTo("Please fill out this field."));
            await _testLocator.EnterTextAsync(_testLocator.Name, "Lambda");
            await _testLocator.EnterTextAsync(_testLocator.Email, "lambda@email.com");
            await _testLocator.EnterTextAsync(_testLocator.Password, "PaSsWoRd");
            await _testLocator.EnterTextAsync(_testLocator.Company, "LambdaTest");
            await _testLocator.EnterTextAsync(_testLocator.Website, "https://www.lambdatest.com");
            await _testLocator.Country.SelectOptionAsync(new SelectOptionValue { Label = "United States" });
            await _testLocator.EnterTextAsync(_testLocator.City, "City");
            await _testLocator.EnterTextAsync(_testLocator.Address1, "Address1");
            await _testLocator.EnterTextAsync(_testLocator.Address2, "Address2");
            await _testLocator.EnterTextAsync(_testLocator.State, "State");
            await _testLocator.EnterTextAsync(_testLocator.Zip, "12345");
            await _testLocator.ClickAsync(_testLocator.SubmitForm);
            Assert.Multiple(async () =>
            {
                Assert.That(await _testLocator.SuccessMsg.IsVisibleAsync(), Is.True);
                Assert.That(await _testLocator.SuccessMsg.InnerTextAsync(), Is.EqualTo("Thanks for contacting us, we will get back to you shortly."));
            });
        }

        [TearDown]
        public async Task Close()
        {
            await _context.CloseAsync();
            await _browser.CloseAsync();
            _playwright.Dispose();
        }
    }
}
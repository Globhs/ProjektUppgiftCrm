namespace ProjektUppgiftCrm.Tests;

using Microsoft.Playwright;

using Microsoft.Playwright.MSTest;

[TestClass]

public class LoginTest : PageTest

{

    private IPlaywright _playwright;

    private IBrowser _browser;

    private IBrowserContext _browserContext;

    private IPage _page;

    [TestInitialize]

    public async Task Setup()

    {

        _playwright = await Microsoft.Playwright.Playwright.CreateAsync();

        _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions

        {

            Headless = true

        });

        _browserContext = await _browser.NewContextAsync();

        _page = await _browserContext.NewPageAsync();

    }

    [TestCleanup]

    public async Task Cleanup()

    {

        await _browserContext.CloseAsync();

        await _browser.CloseAsync();

        _playwright.Dispose();

    }

    public async Task Login()
    {
        await _page.GotoAsync("http://localhost:5173/");
        await _page.GetByText("Login").ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("m@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).ClickAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("abc123");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Login" }).ClickAsync();
        await Expect(_page.GetByRole(AriaRole.Button, new() { Name = "Logout" })).ToBeVisibleAsync();
    }

    [TestMethod]
    public async Task CheckIssues()
    {
        await Login();
        await _page.GetByText("Issues").ClickAsync();
    }

    [TestMethod]
    public async Task CheckEmployees()
    {
        await Login();
        await _page.GetByText("Employees").ClickAsync();
    }

    [TestMethod]
    public async Task AddEmployee() //expected to fail :(
    {
        await Login();
        await _page.GetByText("Employees").ClickAsync();
        await _page.GetByText("Add Employee").ClickAsync();
        await Expect(_page.GetByRole(AriaRole.Button, new() { Name = "Create New Employee" })).ToBeVisibleAsync();
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Firstname" }).FillAsync("Pelle");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Lastname" }).FillAsync("Kanin");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Email" }).FillAsync("abc@email.com");
        await _page.GetByRole(AriaRole.Textbox, new() { Name = "Password" }).FillAsync("abc123");
        await _page.GetByRole(AriaRole.Radio, new() { Name = "USER" }).ClickAsync();
        await _page.GetByText("Create New Employee").ClickAsync();
    }

    [TestMethod]
    public async Task ChangeIssueStatus()
    {
        await Login();
        await _page.GetByText("Issues").ClickAsync();
        await _page.Locator("button.subjectEditButton").First.ClickAsync();
        await _page.Locator("select.stateSelect").First.SelectOptionAsync("CLOSED");
        await _page.Locator("button.stateUpdateButton").First.ClickAsync();
        await _page.Locator("button.subjectEditButton").First.ClickAsync();
        await _page.Locator("select.stateSelect").First.SelectOptionAsync("NEW");
        await _page.Locator("button.stateUpdateButton").First.ClickAsync();
        await _page.GetByRole(AriaRole.Button, new() { Name = "Logout" }).ClickAsync();
    }

    [TestMethod]
    public async Task RegisterUser()
    {
        await _page.GotoAsync("http://localhost:5173/");
        await _page.GetByText("Register").ClickAsync();
        await _page.Locator("input[name='email']").FillAsync("KasperTest@email.com");
        await _page.Locator("input[name='password']").FillAsync("test123");
        await _page.Locator("input[name='username']").FillAsync("KasperTest");
        await _page.Locator("input[name='company']").FillAsync("KasparInc");
        await _page.GetByRole(AriaRole.Button, new() { Name = "Skapa Konto" }).ClickAsync();
    }
}


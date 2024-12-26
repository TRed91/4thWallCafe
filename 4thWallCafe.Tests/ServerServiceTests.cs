using _4thWallCafe.App.Services;
using _4thWallCafe.Core.Entities;
using _4thWallCafe.Core.Models;
using _4thWallCafe.Tests.MockRepos;

namespace _4thWallCafe.Tests;

public class ServerServiceTests
{
    private ServerService _service = new ServerService(new MockServerRepo());

    [Test]
    public void AddsServer_Successfully()
    {
        var server = new ServerForm
        {
            LastName = "Tester",
            FirstName = "Tester",
            DoB = DateTime.Today.AddYears(-20),
        };
        var result = _service.AddServer(server);
        
        Assert.That(result.Ok, Is.True);
    }

    [Test]
    public void AddsServer_Fail_Under18()
    {
        var server = new ServerForm
        {
            LastName = "Tester",
            FirstName = "Tester",
            DoB = DateTime.Today.AddYears(-17),
        };
        
        var result = _service.AddServer(server);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.Not.Null);
        Assert.That(result.Message, Is.Not.Empty);
    }

    [Test]
    public void UpdatesServer_Successfully()
    {
        var server = new ServerForm
        {
            LastName = "Tester",
            FirstName = "Tester",
            DoB = DateTime.Today.AddYears(-19),
        };
        int id = 1;
        var result = _service.UpdateServer(id, server);
        Assert.That(result.Ok, Is.True);
        Assert.That(result.Message, Is.Empty);
    }

    [Test]
    public void UpdatesServer_Fail_Under18()
    {
        var server = new ServerForm
        {
            LastName = "Tester",
            FirstName = "Tester",
            DoB = DateTime.Today.AddYears(-16),
        };
        int id = 1;
        var result = _service.UpdateServer(id, server);
        Assert.That(result.Ok, Is.False);
        Assert.That(result.Message, Is.Not.Empty);
        Assert.That(result.Message, Is.Not.Null);
    }
}
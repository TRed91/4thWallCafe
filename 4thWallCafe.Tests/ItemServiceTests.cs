using _4thWallCafe.App.Services;
using _4thWallCafe.Tests.MockRepos;

namespace _4thWallCafe.Tests;

public class ItemServiceTests
{
    private ItemService _service = new ItemService(new MockItemRepo());
    
    // methods in the ItemService only read and write data.
    // There's no logic that requires testing.
}
using _4thWallCafe.App.Services;
using _4thWallCafe.Tests.MockRepos;

namespace _4thWallCafe.Tests;

public class CafeOrderServiceTests
{
    private CafeOrderService _cafeOrderService = new CafeOrderService(new MockCafeOrderRepo());
    
    // methods in the CafeOrderService only read and write data.
    // There's no logic that requires testing.
}
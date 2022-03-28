using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CommunityDrivenSocialPlatform.UnitTests
{
    public class IntegrationTestBuilder
    {
        [Fact]
        public async void RunIntergrationTests()
        {
            IdentityControllerTests identityControllerTests = new IdentityControllerTests();
            await identityControllerTests.RunTest();

            UserControllerTests userControllerTests = new UserControllerTests();
            await userControllerTests.RunTest();

            SubthreadControllerTests subthreadControllerTests = new SubthreadControllerTests();
            await subthreadControllerTests.RunTest();
        }
    }
}

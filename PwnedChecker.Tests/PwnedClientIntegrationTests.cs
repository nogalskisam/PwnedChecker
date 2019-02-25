using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PwnedChecker.Tests
{
    [TestFixture]
    public class PwnedClientIntegrationTests
    {
        private IPwnedClient GetSUT()
        {
            return new PwnedClient();
        }

        /// <summary>
        /// Use a password we know has been breached more than once here.
        /// </summary>
        [TestCase("password")]
        public async Task PwnedClient_ChecksPassword_Successfully(string password)
        {
            // Arrange
            var client = GetSUT();

            // Act
            var expected = await client.CheckPassword(password);

            // Assert
            Assert.IsTrue(expected > 0);
        }
    }
}

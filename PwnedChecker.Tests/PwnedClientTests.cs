using Flurl.Http.Testing;
using NUnit.Framework;
using PwnedChecker.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PwnedChecker.Tests
{
    [TestFixture]
    public class PwnedClientTests
    {
        private HttpTest _httpTest;

        [SetUp]
        public void SetUp()
        {
            _httpTest = new HttpTest();
        }

        [TearDown]
        public void TearDown()
        {
            _httpTest.Dispose();
        }

        public IPwnedClient GetSut()
        {
            return new PwnedClient();
        }

        [TestCase((int)HttpStatusCode.Unauthorized)]
        [TestCase((int)HttpStatusCode.BadRequest)]
        [TestCase((int)HttpStatusCode.NotFound)]
        [TestCase((int)HttpStatusCode.InternalServerError)]
        public void CheckPassword_ThrowsPwnedCheckerException_WhenServiceUnavailable(int statusCode)
        {
            // Arrange
            var sut = GetSut();

            _httpTest.RespondWith(status: statusCode);

            // Act
            // Assert
            Assert.ThrowsAsync<PwnedCheckerException>(async () => await sut.CheckPassword("password"));
        }
        
        [Test]
        public async Task CheckPassword_ReturnsCountOfBreaches()
        {
            // Arrange
            var result = @"E305CC99CD84C2FA6705DADB1A1F1CC28FC:8
E373574DC7B5BABE5513AC018CFAEDB7F5D:8
E3A3DD55211BC0E621F57A4E0449A5BC34A:1511
E3BE53CC21EED5E20A29A582D9E53729CD8:6";

            var sut = GetSut();

            _httpTest.RespondWith(result);

            // Act
            var actual = await sut.CheckPassword("Abcdefg1");

            // Assert
            Assert.AreEqual(1511, actual);
        }

        [Test]
        public async Task CheckPassword_ReturnsZeroBreaches_WhenNoBreaches()
        {
            // Arrange
            var result = @"E305CC99CD84C2FA6705DADB1A1F1CC28FC:8
E373574DC7B5BABE5513AC018CFAEDB7F5D:8
E3A3DD55211BC0E621F57A4E0449A5BC34A:1511
E3BE53CC21EED5E20A29A582D9E53729CD8:6";

            var sut = GetSut();

            _httpTest.RespondWith(result);

            // Act
            var actual = await sut.CheckPassword("Hihjhkhi123A");

            // Assert
            Assert.AreEqual(0, actual);
        }
    }
}

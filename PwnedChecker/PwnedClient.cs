using Flurl;
using Flurl.Http;
using PwnedChecker.Exceptions;
using PwnedChecker.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PwnedChecker
{
    public class PwnedClient : IPwnedClient
    {
        private IFlurlClient _flurlClient;

        /// <summary>
        /// Creates a new PwnedClient to call Troy Hunt's HaveIBeenPwned v2 API
        /// </summary>
        public PwnedClient()
        {
            _flurlClient = new FlurlClient("https://api.pwnedpasswords.com/");
        }

        /// <summary>
        /// Check if a password has been pwned
        /// </summary>
        /// <param name="password">The password to check</param>
        /// <returns>Number of instances the password has been pwned</returns>
        public async Task<int> CheckPassword(string password)
        {
            int result = 0;

            var passwordHashBytes = PasswordHasher.HashPassword(password);
            var passwordHash = PasswordHasher.ConvertHashToString(passwordHashBytes);
            var passwordHashPrefix = passwordHash.Substring(0, 5);

            var response = await CheckIfPwned(passwordHashPrefix).ConfigureAwait(false);

            if (response.Any())
            {
                var passwordHashSuffix = passwordHash.Substring(6);

                var breaches = response
                                    .Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                                    .Where(r => r.ToLowerInvariant().Contains(passwordHashSuffix.ToLowerInvariant()))
                                    .ToList();


                breaches.ForEach(fe =>
                {
                    var breach = fe.Split(':');

                    result += int.Parse(breach[1]);
                });

            }

            return result;
        }

        private async Task<string> CheckIfPwned(string passwordHashPrefix)
        {
            try
            {
                var results = await _flurlClient.BaseUrl
                                                    .AppendPathSegment("range")
                                                    .AppendPathSegment(passwordHashPrefix)
                                                    .GetStringAsync();

                return results;
            }
            catch (FlurlHttpException ex)
            {
                throw new PwnedCheckerException("An exception occurred while checking passwords against HaveIBeenPwned", ex.InnerException);
            }
        }
    }
}

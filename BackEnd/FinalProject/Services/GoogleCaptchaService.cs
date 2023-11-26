using FinalProject.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace FinalProject.Services
{
    public class GoogleCaptchaService
    {
        private readonly IOptions<GoogleRecaptchaConfig> _config;

        public GoogleCaptchaService(IOptions<GoogleRecaptchaConfig> config)
        {
            _config = config;
        }

        public async Task<bool> VerifyToken(string token)
        {
            try
            {

                string url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.Value.SecretKey}&response={token}";

                using (HttpClient client = new())
                {
                    var httpResponse = await client.GetAsync(url);
                    if (httpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }

                    string responseContent = await httpResponse.Content.ReadAsStringAsync();
                    GoogleCaptchaResponse captchaResponse =  JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseContent);
                    return captchaResponse.Success && captchaResponse.Score >= 0.5;
                };

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


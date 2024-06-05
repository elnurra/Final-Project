using FinalProject.Models;
using FinalProject.Services.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace FinalProject.Services
{
    public class GoogleCaptchaService: IGoogleCaptchaService
    {
        private readonly IConfiguration _config;

        public GoogleCaptchaService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> VerifyToken(string token)
        {
            try
            {
                string url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.GetSection("GoogleRecaptcha:SecretKey").Value}&response={token}";
                using (HttpClient client = new())
                {
                    var httpResponse = await client.GetAsync(url);
                    if (httpResponse.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }
                    string responseContent = await httpResponse.Content.ReadAsStringAsync();
                    GoogleCaptchaResponse? captchaResponse =  JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseContent);
                    return captchaResponse!.Success && captchaResponse.Score >= 0.5;
                };

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}


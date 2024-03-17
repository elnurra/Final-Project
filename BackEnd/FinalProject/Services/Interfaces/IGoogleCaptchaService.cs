namespace FinalProject.Services.Interfaces
{
    public interface IGoogleCaptchaService
    {
        public Task<bool> VerifyToken(string token);
    }
}

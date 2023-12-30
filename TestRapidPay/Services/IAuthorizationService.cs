using TestRapidPay.Models;

namespace TestRapidPay.Services
{
    public interface IAuthorizationService
    {
        Task<AuthorizationResponse> ReturnToken(User authorization);
    }
}

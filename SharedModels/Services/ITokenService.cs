using System.Threading.Tasks;
using IdentityModel.Client;

namespace SharedModels.Services
{
  public interface ITokenService
  {
    Task<TokenResponse> GetToken(string scope);
  }
}
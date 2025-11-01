
using aknaIdentityApi.Domain.Dtos.Requests;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository
    {
        Task<long> AddCompanyAsync(UserRegisterRequest request);

        Task<long> AddCompanyAsync(CompanyRegisterRequest request);

    }
}

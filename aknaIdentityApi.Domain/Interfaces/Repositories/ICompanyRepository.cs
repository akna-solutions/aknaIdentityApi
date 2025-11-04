
using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        Task<long> AddCompanyAsync(UserRegisterRequest request);
        Task<long> AddCompanyAsync(CompanyRegisterRequest request);
        Task<Company?> GetByIdAsync(long id);
        Task<IEnumerable<Company>> GetAllAsync();
        Task<IEnumerable<Company>> GetByStatusAsync(CompanyStatus status);
        Task<Company?> GetByTaxNumberAsync(string taxNumber);
        Task<bool> ApproveCompanyAsync(long id, string approvedBy);
        Task<bool> RejectCompanyAsync(long id, string rejectionReason, string rejectedBy);
    }
}

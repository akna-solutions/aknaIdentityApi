using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;


namespace aknaIdentityApi.Infrastructure.Repositories
{
    public class CompanyRepository : BaseRepository<User>, ICompanyRepository
    {
        public CompanyRepository(AknaIdentityDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Şirket kaydını sağlar
        /// </summary>
        /// <param name="request">UserRegisterRequest</param>
        /// <returns></returns>
        public async Task<long> AddCompanyAsync(UserRegisterRequest request)
        {
            Company company = new Company
            {
                Name = request.CompanyName ?? string.Empty,
                Address = request.CompanyAddress ?? string.Empty,
                City = request.CompanyCity,
                Country = request.CompanyCountry ?? "TR",
                UseEArsiv = request.UseEArsiv ?? false,
                UseEFatura = request.UseEFatura ?? false,
                TaxNumber = request.CompanyTaxNumber ?? string.Empty,
                MersisNo = request.CompanyMersisNo ?? string.Empty,
                PhoneNumber = request.CompanyPhoneNumber ?? string.Empty,
                Email = request.CompanyEmail ?? string.Empty,
                Website = request.CompanyWebsite ?? string.Empty,
                CreatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system",
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            return company.Id;
        }
    }
}

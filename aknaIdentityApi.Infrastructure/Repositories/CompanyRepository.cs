using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;


namespace aknaIdentityApi.Infrastructure.Repositories
{
    public class CompanyRepository : BaseRepository<Company>, ICompanyRepository
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
                LegalRepresentativeName = request.Name ?? string.Empty,
                LegalRepresentativeEmail = request.Email ?? string.Empty,
                LegalRepresentativePhone = request.PhoneNumber,
                LegalRepresentativeSurname = request.Surname ?? string.Empty,
                LegalRepresentativeTitle = "Founder",
                CompanyType = Domain.Enums.CompanyType.SoleProprietorship,
                Status = Domain.Enums.CompanyStatus.Pending,
                RejectionReason = "",
                ApprovedDate = System.DateTime.MinValue,
                ApprovedBy = "",
                FoundationDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system",
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            return company.Id;
        }

        /// <summary>
        /// Şirket kaydını sağlar
        /// </summary>
        /// <param name="request">UserRegisterRequest</param>
        /// <returns></returns>
        public async Task<long> AddCompanyAsync(CompanyRegisterRequest request)
        {
            Company company = new Company
            {
                Name = request.Name ?? string.Empty,
                Address = request.Address ?? string.Empty,
                City = request.City,
                Country = request.Country ?? "TR",
                UseEArsiv = request.UseEArsiv,
                UseEFatura = request.UseEFatura,
                TaxNumber = request.TaxNumber ?? string.Empty,
                MersisNo = request.MersisNo ?? string.Empty,
                PhoneNumber = request.PhoneNumber ?? string.Empty,
                Email = request.Email ?? string.Empty,
                Website = request.Website ?? string.Empty,
                LegalRepresentativeName = request.LegalRepresentativeName ?? string.Empty,
                LegalRepresentativeEmail = request.LegalRepresentativeEmail ?? string.Empty,
                LegalRepresentativePhone = request.LegalRepresentativePhone,
                LegalRepresentativeSurname = request.LegalRepresentativeSurname ?? string.Empty,
                LegalRepresentativeTitle = request.LegalRepresentativeTitle,
                CompanyType = request.CompanyType,
                FoundationDate = DateTime.UtcNow,
                Status = Domain.Enums.CompanyStatus.Pending,
                RejectionReason = "",
                ApprovedDate = System.DateTime.MinValue,
                ApprovedBy = "",
                CreatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system",
            };

            await context.Companies.AddAsync(company);
            await context.SaveChangesAsync();
            return company.Id;
        }

        public new async Task<Company?> GetByIdAsync(long id)
        {
            return await context.Companies
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<Company>> GetAllAsync()
        {
            return await context.Companies
                .Where(c => !c.IsDeleted)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Company>> GetByStatusAsync(CompanyStatus status)
        {
            return await context.Companies
                .Where(c => c.Status == status && !c.IsDeleted)
                .OrderByDescending(c => c.CreatedDate)
                .ToListAsync();
        }

        public async Task<Company?> GetByTaxNumberAsync(string taxNumber)
        {
            return await context.Companies
                .FirstOrDefaultAsync(c => c.TaxNumber == taxNumber && !c.IsDeleted);
        }

        public async Task<bool> ApproveCompanyAsync(long id, string approvedBy)
        {
            var company = await context.Companies.FindAsync(id);
            if (company == null || company.IsDeleted)
            {
                return false;
            }

            company.Status = CompanyStatus.Approved;
            company.ApprovedDate = DateTime.UtcNow;
            company.ApprovedBy = approvedBy;
            company.UpdatedDate = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RejectCompanyAsync(long id, string rejectionReason, string rejectedBy)
        {
            var company = await context.Companies.FindAsync(id);
            if (company == null || company.IsDeleted)
            {
                return false;
            }

            company.Status = CompanyStatus.Rejected;
            company.RejectionReason = rejectionReason;
            company.UpdatedDate = DateTime.UtcNow;
            await context.SaveChangesAsync();

            return true;
        }
    }
}

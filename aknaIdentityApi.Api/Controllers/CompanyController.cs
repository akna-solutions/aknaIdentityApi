using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aknaIdentityApi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository companyRepository;

        public CompanyController(ICompanyRepository companyRepository)
        {
            this.companyRepository = companyRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCompanies()
        {
            var companies = await companyRepository.GetAllAsync();
            return Ok(new { data = companies, success = true });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyById(long id)
        {
            var company = await companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound(new { message = "Company not found", success = false });
            }

            return Ok(new { data = company, success = true });
        }

        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetCompaniesByStatus(CompanyStatus status)
        {
            var companies = await companyRepository.GetByStatusAsync(status);
            return Ok(new { data = companies, success = true });
        }

        [HttpGet("tax-number/{taxNumber}")]
        public async Task<IActionResult> GetCompanyByTaxNumber(string taxNumber)
        {
            var company = await companyRepository.GetByTaxNumberAsync(taxNumber);
            if (company == null)
            {
                return NotFound(new { message = "Company not found", success = false });
            }

            return Ok(new { data = company, success = true });
        }

        [HttpPost("{id}/approve")]
        public async Task<IActionResult> ApproveCompany(long id, [FromBody] ApprovalRequest request)
        {
            var result = await companyRepository.ApproveCompanyAsync(id, request.ApprovedBy);
            if (!result)
            {
                return NotFound(new { message = "Company not found", success = false });
            }

            return Ok(new { message = "Company approved successfully", success = true });
        }

        [HttpPost("{id}/reject")]
        public async Task<IActionResult> RejectCompany(long id, [FromBody] RejectionRequest request)
        {
            var result = await companyRepository.RejectCompanyAsync(id, request.RejectionReason, request.RejectedBy);
            if (!result)
            {
                return NotFound(new { message = "Company not found", success = false });
            }

            return Ok(new { message = "Company rejected successfully", success = true });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(long id)
        {
            var company = await companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return NotFound(new { message = "Company not found", success = false });
            }

            company.IsDeleted = true;
            company.UpdatedDate = DateTime.UtcNow;
            await companyRepository.UpdateAsync(company);

            return Ok(new { message = "Company deleted successfully", success = true });
        }
    }

    public class ApprovalRequest
    {
        public string ApprovedBy { get; set; } = string.Empty;
    }

    public class RejectionRequest
    {
        public string RejectionReason { get; set; } = string.Empty;
        public string RejectedBy { get; set; } = string.Empty;
    }
}

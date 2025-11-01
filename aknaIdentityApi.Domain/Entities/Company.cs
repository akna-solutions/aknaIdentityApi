using aknaIdentityApi.Domain.Base;
using aknaIdentityApi.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace aknaIdentityApi.Domain.Entities
{
    [Table("Companies")]
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public CompanyType CompanyType { get; set; }
        public DateTime FoundationDate { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; } = "TR";
        public bool UseEArsiv { get; set; }
        public bool UseEFatura { get; set; }
        public string TaxNumber { get; set; }
        public string MersisNo { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string? EmployeeCount { get; set; }

        // Legal Representative Information
        public string LegalRepresentativeName { get; set; }
        public string LegalRepresentativeSurname { get; set; }
        public string? LegalRepresentativeTitle { get; set; }
        public string LegalRepresentativeEmail { get; set; }
        public string? LegalRepresentativePhone { get; set; }
        public CompanyStatus Status { get; set; } = CompanyStatus.Pending;
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedBy { get; set; }
        public string? RejectionReason { get; set; }
    }
}

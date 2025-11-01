using System.ComponentModel.DataAnnotations;
using aknaIdentityApi.Domain.Enums;

namespace aknaIdentityApi.Domain.Dtos.Requests
{
    public class CompanyRegisterRequest
    {
        // Company Basic Information
        [Required(ErrorMessage = "Şirket adı gereklidir")]
        [MaxLength(200, ErrorMessage = "Şirket adı en fazla 200 karakter olabilir")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Şirket tipi gereklidir")]
        public CompanyType CompanyType { get; set; }

        [Required(ErrorMessage = "Vergi numarası gereklidir")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Vergi numarası 10 haneli olmalıdır")]
        public string TaxNumber { get; set; }

        [MaxLength(50, ErrorMessage = "Mersis numarası en fazla 50 karakter olabilir")]
        public string? MersisNo { get; set; }

        [Required(ErrorMessage = "Kuruluş tarihi gereklidir")]
        public DateTime FoundationDate { get; set; }

        // Contact Information
        [Required(ErrorMessage = "Email gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [MaxLength(100, ErrorMessage = "Email en fazla 100 karakter olabilir")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telefon numarası gereklidir")]
        [MaxLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        public string PhoneNumber { get; set; }

        [Url(ErrorMessage = "Geçerli bir web sitesi adresi giriniz")]
        [MaxLength(200, ErrorMessage = "Web sitesi adresi en fazla 200 karakter olabilir")]
        public string? Website { get; set; }

        [Required(ErrorMessage = "Adres gereklidir")]
        [MaxLength(500, ErrorMessage = "Adres en fazla 500 karakter olabilir")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Şehir gereklidir")]
        [MaxLength(100, ErrorMessage = "Şehir en fazla 100 karakter olabilir")]
        public string City { get; set; }

        [MaxLength(2, ErrorMessage = "Ülke kodu en fazla 2 karakter olabilir")]
        public string Country { get; set; } = "TR";

        // Legal Representative Information
        [Required(ErrorMessage = "Yasal temsilci adı gereklidir")]
        [MaxLength(100, ErrorMessage = "Yasal temsilci adı en fazla 100 karakter olabilir")]
        public string LegalRepresentativeName { get; set; }

        [Required(ErrorMessage = "Yasal temsilci soyadı gereklidir")]
        [MaxLength(100, ErrorMessage = "Yasal temsilci soyadı en fazla 100 karakter olabilir")]
        public string LegalRepresentativeSurname { get; set; }

        [MaxLength(100, ErrorMessage = "Yasal temsilci ünvanı en fazla 100 karakter olabilir")]
        public string? LegalRepresentativeTitle { get; set; }

        [Required(ErrorMessage = "Yasal temsilci emaili gereklidir")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz")]
        [MaxLength(100, ErrorMessage = "Email en fazla 100 karakter olabilir")]
        public string LegalRepresentativeEmail { get; set; }

        [MaxLength(20, ErrorMessage = "Telefon numarası en fazla 20 karakter olabilir")]
        public string? LegalRepresentativePhone { get; set; }

        // Business Information
        [MaxLength(20, ErrorMessage = "Çalışan sayısı en fazla 20 karakter olabilir")]
        public string? EmployeeCount { get; set; }

        // E-Invoice/Archive Settings
        public bool UseEArsiv { get; set; }
        public bool UseEFatura { get; set; }

        // System Information (from frontend)
        [MaxLength(100, ErrorMessage = "Davet token en fazla 100 karakter olabilir")]
        public string? InviteToken { get; set; }

        // Documents
        public List<DocumentDto>? Documents { get; set; } = new List<DocumentDto>();
    }
}
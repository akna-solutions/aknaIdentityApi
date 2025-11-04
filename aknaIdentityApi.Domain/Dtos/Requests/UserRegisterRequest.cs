
using aknaIdentityApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace aknaIdentityApi.Domain.Dtos.Requests
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Surname is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Surname must be between 2 and 100 characters")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Turkish Republic ID Number is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Turkish Republic ID Number must be 11 characters")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Turkish Republic ID Number must contain only digits")]
        public string TurkishRepublicIdNumber { get; set; }

        [Required(ErrorMessage = "Birth date is required")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public GenderType Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(500, ErrorMessage = "Address cannot exceed 500 characters")]
        public string Address { get; set; }

        [StringLength(10, ErrorMessage = "Blood type cannot exceed 10 characters")]
        public string BloodType { get; set; }

        [Required(ErrorMessage = "User type is required")]
        public UserType UserType { get; set; }

        [Required(ErrorMessage = "Device ID is required")]
        [StringLength(255, ErrorMessage = "Device ID cannot exceed 255 characters")]
        public string DeviceId { get; set; } = default!;

        [Required(ErrorMessage = "Device type is required")]
        [StringLength(50, ErrorMessage = "Device type cannot exceed 50 characters")]
        public string DeviceType { get; set; }

        [StringLength(100, ErrorMessage = "Device model cannot exceed 100 characters")]
        public string? DeviceModel { get; set; }

        [Required(ErrorMessage = "IP address is required")]
        [StringLength(45, ErrorMessage = "IP address cannot exceed 45 characters")]
        public string IPAddress { get; set; }

        [StringLength(255, ErrorMessage = "Push token cannot exceed 255 characters")]
        public string? PushToken { get; set; }

        public long? CompanyId { get; set; }
        public long? UserId { get; set; }
        public List<DocumentDto>? Documents { get; set; }

        [StringLength(200, ErrorMessage = "Company name cannot exceed 200 characters")]
        public string? CompanyName { get; set; }

        [StringLength(500, ErrorMessage = "Company address cannot exceed 500 characters")]
        public string? CompanyAddress { get; set; }

        [StringLength(100, ErrorMessage = "Company city cannot exceed 100 characters")]
        public string? CompanyCity { get; set; }

        [StringLength(2, ErrorMessage = "Company country code must be 2 characters")]
        public string? CompanyCountry { get; set; } = "TR";

        public bool? UseEArsiv { get; set; }
        public bool? UseEFatura { get; set; }

        [StringLength(20, ErrorMessage = "Company tax number cannot exceed 20 characters")]
        public string? CompanyTaxNumber { get; set; }

        [StringLength(20, ErrorMessage = "Company MERSIS number cannot exceed 20 characters")]
        public string? CompanyMersisNo { get; set; }

        [Phone(ErrorMessage = "Invalid company phone number format")]
        [StringLength(20, ErrorMessage = "Company phone number cannot exceed 20 characters")]
        public string? CompanyPhoneNumber { get; set; }

        [EmailAddress(ErrorMessage = "Invalid company email format")]
        [StringLength(150, ErrorMessage = "Company email cannot exceed 150 characters")]
        public string? CompanyEmail { get; set; }

        [Url(ErrorMessage = "Invalid company website URL format")]
        [StringLength(200, ErrorMessage = "Company website cannot exceed 200 characters")]
        public string? CompanyWebsite { get; set; }

    }
}

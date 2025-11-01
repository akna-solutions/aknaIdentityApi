
using aknaIdentityApi.Domain.Enums;

namespace aknaIdentityApi.Domain.Dtos
{
    public class DocumentDto
    {
        public DocumentCategory DocumentCategory { get; set; } // Identity, License, Insurance, etc.
        public int DocumentNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string FileUrl { get; set; }
    }
}


using aknaIdentityApi.Domain.Enums;

namespace aknaIdentityApi.Domain.Dtos
{
    public class DocumentDto
    {
        public DocumentCategory DocumentCategory { get; set; } // Identity, License, Insurance, etc.
        public string DocumentType { get; set; } // K1, K2, CE, etc.
        public int DocumentNumber { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string FileUrl { get; set; }
    }
}

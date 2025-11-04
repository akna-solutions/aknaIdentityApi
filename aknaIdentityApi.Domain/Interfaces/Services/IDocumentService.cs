using aknaIdentityApi.Domain.Dtos;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;

namespace aknaIdentityApi.Domain.Interfaces.Services
{
    public interface IDocumentService
    {
        Task<Document> CreateDocumentAsync(Document document);
        Task AddDocumentsAsync(long userId, long companyId, List<DocumentDto> documents);
        Task<Document?> GetDocumentByIdAsync(long id);
        Task<IEnumerable<Document>> GetDocumentsByUserIdAsync(long userId);
        Task<IEnumerable<Document>> GetDocumentsByCompanyIdAsync(long companyId);
        Task<IEnumerable<Document>> GetDocumentsByCategoryAsync(DocumentCategory category);
        Task<IEnumerable<Document>> GetExpiringSoonDocumentsAsync(int daysThreshold);
        Task<Document> UpdateDocumentAsync(Document document);
        Task<bool> DeleteDocumentAsync(long id);
        Task<bool> VerifyDocumentAsync(long documentId);
    }
}

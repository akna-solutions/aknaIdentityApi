using aknaIdentityApi.Domain.Dtos;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Task AddDocumentsAsync(long userId, long companyId, List<DocumentDto> documents);
        Task<IEnumerable<Document>> GetByUserIdAsync(long userId);
        Task<IEnumerable<Document>> GetByCompanyIdAsync(long companyId);
        Task<IEnumerable<Document>> GetByCategoryAsync(DocumentCategory category);
        Task<IEnumerable<Document>> GetExpiringSoonAsync(int daysThreshold);
        Task<bool> VerifyDocumentAsync(long documentId);
    }
}
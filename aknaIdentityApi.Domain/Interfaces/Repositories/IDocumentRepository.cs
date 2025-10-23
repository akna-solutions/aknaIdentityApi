using aknaIdentityApi.Domain.Dtos;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;

namespace aknaIdentityApi.Domain.Interfaces.Repositories
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        Task AddDocumentsAsync(long userId, long companyId, List<DocumentDto> documents);
    }
}
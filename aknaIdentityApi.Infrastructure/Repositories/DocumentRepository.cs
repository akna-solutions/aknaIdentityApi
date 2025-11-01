using aknaIdentityApi.Domain.Dtos;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;

namespace aknaIdentityApi.Infrastructure.Repositories
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(AknaIdentityDbContext context) : base(context)
        {
        }

        /// <summary>
        /// Kullanıcı için belge kaydını sağlar
        /// </summary>
        /// <param name="userId">Kullanıcı ID</param>
        /// <param name="companyId">Şirket ID</param>
        /// <param name="documents">Belge listesi</param>
        /// <returns></returns>
        public async Task AddDocumentsAsync(long userId, long companyId, List<DocumentDto> documents)
        {
            if (documents == null || !documents.Any())
                return;

            var documentEntities = documents.Select(doc => new Document
            {
                UserId = userId,
                CompanyId = companyId,
                DocumentCategory = doc.DocumentCategory,
                DocumentNumber = doc.DocumentNumber,
                ExpirationDate = doc.ExpirationDate,
                FileUrl = doc.FileUrl,
                IsVerified = false, // Varsayılan olarak doğrulanmamış
                CreatedDate = DateTime.UtcNow,
                CreatedUser = "system",
                UpdatedDate = DateTime.UtcNow,
                UpdatedUser = "system",
                IsDeleted = false
            }).ToList();

            await context.Documents.AddRangeAsync(documentEntities);
            await context.SaveChangesAsync();
        }

    }
}
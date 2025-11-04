using aknaIdentityApi.Domain.Dtos;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Document>> GetByUserIdAsync(long userId)
        {
            return await context.Documents
                .Where(d => d.UserId == userId && !d.IsDeleted)
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByCompanyIdAsync(long companyId)
        {
            return await context.Documents
                .Where(d => d.CompanyId == companyId && !d.IsDeleted)
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetByCategoryAsync(DocumentCategory category)
        {
            return await context.Documents
                .Where(d => d.DocumentCategory == category && !d.IsDeleted)
                .OrderByDescending(d => d.CreatedDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Document>> GetExpiringSoonAsync(int daysThreshold)
        {
            var thresholdDate = DateTime.UtcNow.AddDays(daysThreshold);
            return await context.Documents
                .Where(d => !d.IsDeleted && d.ExpirationDate <= thresholdDate && d.ExpirationDate >= DateTime.UtcNow)
                .OrderBy(d => d.ExpirationDate)
                .ToListAsync();
        }

        public async Task<bool> VerifyDocumentAsync(long documentId)
        {
            var document = await context.Documents.FindAsync(documentId);
            if (document == null || document.IsDeleted)
            {
                return false;
            }

            document.IsVerified = true;
            document.UpdatedDate = DateTime.UtcNow;
            document.UpdatedUser = "system";
            await context.SaveChangesAsync();

            return true;
        }

    }
}
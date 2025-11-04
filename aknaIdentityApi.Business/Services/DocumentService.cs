using aknaIdentityApi.Domain.Dtos;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;
using aknaIdentityApi.Domain.Interfaces.UnitOfWorks;

namespace aknaIdentityApi.Business.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IUnitOfWork unitOfWork;

        public DocumentService(IDocumentRepository documentRepository, IUnitOfWork unitOfWork)
        {
            this.documentRepository = documentRepository;
            this.unitOfWork = unitOfWork;
        }

        public async Task<Document> CreateDocumentAsync(Document document)
        {
            document.CreatedDate = DateTime.UtcNow;
            document.UpdatedDate = DateTime.UtcNow;
            document.IsDeleted = false;
            document.IsVerified = false;

            await documentRepository.AddAsync(document);
            await unitOfWork.CommitAsync();

            return document;
        }

        public async Task AddDocumentsAsync(long userId, long companyId, List<DocumentDto> documents)
        {
            await documentRepository.AddDocumentsAsync(userId, companyId, documents);
        }

        public async Task<Document?> GetDocumentByIdAsync(long id)
        {
            return await documentRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByUserIdAsync(long userId)
        {
            return await documentRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByCompanyIdAsync(long companyId)
        {
            return await documentRepository.GetByCompanyIdAsync(companyId);
        }

        public async Task<IEnumerable<Document>> GetDocumentsByCategoryAsync(DocumentCategory category)
        {
            return await documentRepository.GetByCategoryAsync(category);
        }

        public async Task<IEnumerable<Document>> GetExpiringSoonDocumentsAsync(int daysThreshold)
        {
            return await documentRepository.GetExpiringSoonAsync(daysThreshold);
        }

        public async Task<Document> UpdateDocumentAsync(Document document)
        {
            var existingDocument = await documentRepository.GetByIdAsync(document.Id);
            if (existingDocument == null)
            {
                throw new KeyNotFoundException($"Document with ID {document.Id} not found");
            }

            document.UpdatedDate = DateTime.UtcNow;
            await documentRepository.UpdateAsync(document);
            await unitOfWork.CommitAsync();

            return document;
        }

        public async Task<bool> DeleteDocumentAsync(long id)
        {
            var document = await documentRepository.GetByIdAsync(id);
            if (document == null)
            {
                return false;
            }

            document.IsDeleted = true;
            document.UpdatedDate = DateTime.UtcNow;
            await documentRepository.UpdateAsync(document);
            await unitOfWork.CommitAsync();

            return true;
        }

        public async Task<bool> VerifyDocumentAsync(long documentId)
        {
            return await documentRepository.VerifyDocumentAsync(documentId);
        }
    }
}

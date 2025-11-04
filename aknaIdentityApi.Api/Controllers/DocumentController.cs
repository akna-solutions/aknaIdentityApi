using aknaIdentityApi.Domain.Dtos;
using aknaIdentityApi.Domain.Entities;
using aknaIdentityApi.Domain.Enums;
using aknaIdentityApi.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aknaIdentityApi.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/documents")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService documentService;

        public DocumentController(IDocumentService documentService)
        {
            this.documentService = documentService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromBody] Document document)
        {
            var createdDocument = await documentService.CreateDocumentAsync(document);
            return CreatedAtAction(nameof(GetDocumentById), new { id = createdDocument.Id }, new { data = createdDocument, success = true });
        }

        [HttpPost("bulk")]
        public async Task<IActionResult> AddDocuments([FromBody] BulkDocumentRequest request)
        {
            await documentService.AddDocumentsAsync(request.UserId, request.CompanyId, request.Documents);
            return Ok(new { message = "Documents added successfully", success = true });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(long id)
        {
            var document = await documentService.GetDocumentByIdAsync(id);
            if (document == null)
            {
                return NotFound(new { message = "Document not found", success = false });
            }

            return Ok(new { data = document, success = true });
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetDocumentsByUserId(long userId)
        {
            var documents = await documentService.GetDocumentsByUserIdAsync(userId);
            return Ok(new { data = documents, success = true });
        }

        [HttpGet("company/{companyId}")]
        public async Task<IActionResult> GetDocumentsByCompanyId(long companyId)
        {
            var documents = await documentService.GetDocumentsByCompanyIdAsync(companyId);
            return Ok(new { data = documents, success = true });
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetDocumentsByCategory(DocumentCategory category)
        {
            var documents = await documentService.GetDocumentsByCategoryAsync(category);
            return Ok(new { data = documents, success = true });
        }

        [HttpGet("expiring/{daysThreshold}")]
        public async Task<IActionResult> GetExpiringSoonDocuments(int daysThreshold)
        {
            var documents = await documentService.GetExpiringSoonDocumentsAsync(daysThreshold);
            return Ok(new { data = documents, success = true });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDocument(long id, [FromBody] Document document)
        {
            if (id != document.Id)
            {
                return BadRequest(new { message = "ID mismatch", success = false });
            }

            try
            {
                var updatedDocument = await documentService.UpdateDocumentAsync(document);
                return Ok(new { data = updatedDocument, success = true });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message, success = false });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(long id)
        {
            var result = await documentService.DeleteDocumentAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Document not found", success = false });
            }

            return Ok(new { message = "Document deleted successfully", success = true });
        }

        [HttpPost("{id}/verify")]
        public async Task<IActionResult> VerifyDocument(long id)
        {
            var result = await documentService.VerifyDocumentAsync(id);
            if (!result)
            {
                return NotFound(new { message = "Document not found", success = false });
            }

            return Ok(new { message = "Document verified successfully", success = true });
        }
    }

    public class BulkDocumentRequest
    {
        public long UserId { get; set; }
        public long CompanyId { get; set; }
        public List<DocumentDto> Documents { get; set; } = new();
    }
}

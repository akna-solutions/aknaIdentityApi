
using aknaIdentityApi.Domain.Dtos.Requests;
using aknaIdentityApi.Domain.Interfaces.Repositories;
using aknaIdentityApi.Domain.Interfaces.Services;

namespace aknaIdentityApi.Business.Services
{
    /// <summary>
    /// AuthenticationService
    /// </summary>
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository userRepository;
        private readonly IDeviceInfoRepository deviceInfoRepository;
        private readonly ICompanyRepository companyRepository;
        private readonly IDocumentRepository documentRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="deviceInfoRepository"></param>
        /// <param name="companyRepository"></param>
        /// <param name="documentRepository"></param>
        public AuthenticationService(IUserRepository userRepository, IDeviceInfoRepository deviceInfoRepository, ICompanyRepository companyRepository, IDocumentRepository documentRepository)
        {
            this.userRepository = userRepository;
            this.deviceInfoRepository = deviceInfoRepository;
            this.companyRepository = companyRepository;
            this.documentRepository = documentRepository;
        }

        /// <summary>
        /// RegisterAsync
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public async Task RegisterAsync(UserRegisterRequest request)
        {
            if (request.UserType.ToString() == "IndividualCarrier")
            {
                request.CompanyId = await companyRepository.AddCompanyAsync(request);
            }
            var userId = await userRepository.AddUserAsync(request);
            request.UserId = userId;

            await deviceInfoRepository.AddDeviceInfoAsync(request);

            await documentRepository.AddDocumentsAsync(userId, request.CompanyId ?? 0, request.Documents);
        }
    }
}


using Microsoft.AspNetCore.DataProtection;
using System.Web;

namespace MotoVerse.Core.Features.Shared.EncryptionProvider.Commands.Handlers;

public class EncryptionProviderCommandHandler :
    ResponseHandler,
    IRequestHandler<EncryptionProviderCommand, Response<string>>,
    IRequestHandler<DecryptionProviderCommand, Response<string>>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly IDataProtector _protector;

    #endregion

    #region Constructors
    public EncryptionProviderCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
        IDataProtectionProvider dataProtectionProvider) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _protector = dataProtectionProvider.CreateProtector("MotoVerseSecureKeyMotoVerseSecureKey");
    }
    #endregion

    #region Handle Functions
    public async Task<Response<string>> Handle(EncryptionProviderCommand request, CancellationToken cancellationToken)
    {

        var protectiobData = HttpUtility.UrlEncode(_protector.Protect(request.Data));
        if (protectiobData is not null)
            return Success<string>(protectiobData);

        return BadRequest<string>(_stringLocalizer[SharedResourcesKeys.EncryptDataFailed]);
    }

    public async Task<Response<string>> Handle(
        DecryptionProviderCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var decodedData =
                HttpUtility.UrlDecode(request.Data);

            var unprotectedData =
                _protector.Unprotect(decodedData);

            return Success<string>(unprotectedData);
        }
        catch (Exception)
        {
            return BadRequest<string>(
                _stringLocalizer[
                    SharedResourcesKeys.DecryptDataFailed]);
        }
    }

    #endregion
}

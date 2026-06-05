
using Microsoft.AspNetCore.DataProtection;
using MotoVerse.Core.Features.Shared.UploadImage.Query.Models;

namespace MotoVerse.Core.Features.Shared.UploadImage.Query.Handlers;

public class UploadImageQueryHandler :
    ResponseHandler,
    IRequestHandler<GetImageQuery, Response<string>>
{
    #region Fields
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;
    private readonly IDataProtector _protector;

    #endregion

    #region Constructors
    public UploadImageQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
        IDataProtectionProvider dataProtectionProvider) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _protector = dataProtectionProvider.CreateProtector("MotoVerseSecureKeyMotoVerseSecureKey");
    }
    #endregion

    #region Handle Functions
    public async Task<Response<string>> Handle(GetImageQuery request, CancellationToken cancellationToken)
    {
        var imagePath = $"{request.Request.Scheme}://{request.Request.Host}/images/{request.FolderName}/{request.ImageName}";

        return new Response<string>(imagePath);
    }




    #endregion
}

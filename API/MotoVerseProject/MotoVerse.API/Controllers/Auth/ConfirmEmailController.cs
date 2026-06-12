using MotoVerse.Core.Features.Auth.ConfirmEmailFeature.Command.Models;
using MotoVerse.Core.Features.ConfirmEmailFeature.Query.Models;

namespace MotoVerse.API.Controllers.Auth;

public class ConfirmEmailController : AppControllerBase
{

    [HttpGet(Routing.ConfirmEmail.Confirm)]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery query)
    {

        var response = await _mediator.Send(query);

        if (response.Succeeded)
        {
            return Redirect("http://localhost:4200/confirm-email?success=true");
        }

        return Redirect("http://localhost:4200/login?success=false");
    }
    [HttpGet(Routing.ConfirmEmail.SendConfirm)]
    public async Task<IActionResult> ConfirmEmail([FromQuery] SendConfirmEmailQuery query)
    {
        var response = await _mediator.Send(query);
        return NewResult(response);
    }

}

/*
 https://localhost:7081/api/V1/ConfirmEmail/confirm-email?
Email= 
CfDJ8CO21ebm-QRBhdociZ2m8YF5d40mpn8O3X3X3BpyjwVpl1f-vq4mFXqBGf8jNJepo-rEqlvrKg0pI_hr8obXy8fFFBUWHr0FASa2kAerfKstzD9W5LSuVqYHfddZHuVQnEU3hMWktlw0yfNrE2mp87I
&
code=
CfDJ8CO21ebm-QRBhdociZ2m8YF-pRETah812zZ6Aujy8ly9ricPC2BQ0WBRDne0ZjY4r6oK9kDDjREaTsoDjWYtytXOUgv1lA9wX9YPn9Mb8osGy6TWJ_p3dJ8lg7ByNuKyZnEO2SjJBveIORbcpGJv64bLqz47Wj1gx2mGz4N-EYSIrwn3ccP7LTmCb0NKlNP4_IIaCOKWT-sRt1-WARcxHdBxhOEmB3c8P3fB4D4dfQc-iWdWfPv8PAX834GhfD3inT2_juFGbmyfp0P7lN9BTZX5SRfcp8jQGYsVIXG2WwVZVlGQcFBrTPA5odSUL9VsYyItF4JKP6RPW1OQuNyMy61PoRxjrDw7k4h0KU4U8Diq2BT5B0xVfJjZZh-qYb8iPcwza3Xxm_-rJVIc5ZAQUI12nI7mEZMDZPItloLaUMhDC9ZC9nd3SVTsY6w5IudDeEJ0RGgFggCRPsftDbnkPLk


 */
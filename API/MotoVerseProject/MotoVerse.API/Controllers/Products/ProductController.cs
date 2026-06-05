using MotoVerse.Core.Features.ProductFeature.Commands.Models;
using MotoVerse.Core.Features.ProductFeature.Queries.Models;
using MotoVerse.Core.Features.ProductFetatures.ProductFeature.Queries.Models;

namespace MotoVerse.API.Controllers.Products;

public class ProductController : AppControllerBase
{
    #region Handlers Function

    //----------------------- Create : 
    [HttpPost(Routing.ProductRouting.Add)]
    public async Task<IActionResult> Add([FromForm] AddProductCommand addProductCommand)
    {
        var product = await _mediator.Send(addProductCommand);

        return NewResult(product);
    }

    //----------------------- Read : 
    [HttpGet(Routing.ProductRouting.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetProductListQuery());

        return NewResult(products);
    }
    //get-by-id?id=
    [HttpGet(Routing.ProductRouting.GetById)]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));

        return NewResult(product);
    }
    [HttpGet(Routing.ProductRouting.GetRecommendations)]
    public async Task<IActionResult> GetRecommendationProducts([FromRoute] string id)
    {
        var product = await _mediator.Send(new GetRecommendationProductQuery(id));

        return NewResult(product);
    }

    //[AllowAnonymous]
    [HttpGet(Routing.ProductRouting.Paginated)]
    public async Task<IActionResult> Paginated([FromQuery] GetProductPaginatedListQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
    //----------------------- Updte : 
    [HttpPut(Routing.ProductRouting.Edit)]
    public async Task<IActionResult> Edit([FromForm] EditProductCommand addProductCommand)
    {
        var product = await _mediator.Send(addProductCommand);

        return NewResult(product);
    }
    //----------------------- Delete : 

    [HttpDelete(Routing.ProductRouting.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        var productDeleted = await _mediator.Send(new DeleteProductCommand(id));

        return NewResult(productDeleted);
    }
    #endregion


}

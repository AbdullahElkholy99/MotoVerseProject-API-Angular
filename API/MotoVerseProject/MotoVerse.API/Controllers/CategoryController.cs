namespace MotoVerse.API.Controllers;

//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class CategoryController : AppControllerBase
{
    #region Handlers Function

    //----------------------- Create : 
    [HttpPost(Routing.CategoryRouting.Add)]
    public async Task<IActionResult> Add([FromForm] AddCategoryCommand addCategoryCommand)
    {
        var token = Request.Headers["Authorization"].ToString();
        var Category = await _mediator.Send(addCategoryCommand);

        return NewResult(Category);
    }

    //----------------------- Read : 
    [HttpGet(Routing.CategoryRouting.GetAll)]
    public async Task<IActionResult> GetAll()
    {
        var token = Request.Headers["Authorization"].ToString();
        var categories = await _mediator.Send(new GetCategoryListQuery());

        return NewResult(categories);
    }
    //get-by-id?id=
    [HttpGet(Routing.CategoryRouting.GetById)]
    public async Task<IActionResult> GetById(string id)
    {
        var Category = await _mediator.Send(new GetCategoryByIdQuery(id));

        return NewResult(Category);
    }

    //[AllowAnonymous]
    [HttpGet(Routing.CategoryRouting.Paginated)]
    public async Task<IActionResult> Paginated([FromQuery] GetCategoryPaginatedListQuery query)
    {
        var response = await _mediator.Send(query);
        return Ok(response);
    }
    //----------------------- Updte : 
    [HttpPut(Routing.CategoryRouting.Edit)]
    public async Task<IActionResult> Edit([FromForm] EditCategoryCommand addCategoryCommand)
    {
        var Category = await _mediator.Send(addCategoryCommand);

        return NewResult(Category);
    }
    //----------------------- Delete : 

    [HttpDelete(Routing.CategoryRouting.Delete)]
    public async Task<IActionResult> Delete(string id)
    {
        var CategoryDeleted = await _mediator.Send(new DeleteCategoryCommand(id));

        return NewResult(CategoryDeleted);
    }
    #endregion


}

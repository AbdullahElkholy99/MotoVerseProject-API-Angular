
using MotoVerse.Core.Features.CategoryFeature.Commands.Models;
using MotoVerse.Core.Features.CurrentUserFeature.Queries.Models;

namespace MotoVerse.Core.Features.CategoryFeature.Commands.Handlers;

internal class CategoryCommandHandler :
    ResponseHandler,
    IRequestHandler<AddCategoryCommand, Response<string>>,
    IRequestHandler<DeleteCategoryCommand, Response<string>>,
    IRequestHandler<EditCategoryCommand, Response<string>>
{

    #region Fields
    private readonly IRepositoryManager _repositoryManager;
    private readonly IStringLocalizer<SharedResources> _sharedResources;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    #endregion

    #region CTOR
    public CategoryCommandHandler(
        IStringLocalizer<SharedResources> localizer, IRepositoryManager repositoryManager, IMediator mediator, IMapper mapper) : base(localizer)
    {
        _sharedResources = localizer;
        _repositoryManager = repositoryManager;
        _mediator = mediator;
        _mapper = mapper;
    }
    #endregion

    #region Method Handlers:
    public async Task<Response<string>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager.CategoryRepository.BeginTransactionAsync();

        try
        {
            var category = _mapper.Map<Category>(request);

            var adminId = (await _mediator.Send(new GetUserIdQuery())).Data;
            if (adminId is null)
                return BadRequest<string>("Added Category Failed");


            category.Id = Guid.NewGuid().ToString();
            category.AdminId = adminId;

            if (request.ImageFile is not null)
            {
                category.ImagePath = (
                    await _mediator.Send(new UploadImageCommand()
                    {
                        ImageFile = request.ImageFile,
                        FolderName = "Category",
                    })
                ).Data;
            }

            await _repositoryManager.CategoryRepository.AddAsync(category);

            await _repositoryManager.CategoryRepository.SaveChangesAsync();

            await _repositoryManager.CategoryRepository.CommitAsync();

            return Created("Added Category Successfully");
        }
        catch (Exception)
        {
            await _repositoryManager.CategoryRepository.RollBackAsync();

            return BadRequest<string>("Added Category Failed");
        }
    }

    public async Task<Response<string>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager.CategoryRepository.BeginTransactionAsync();

        try
        {
            var category =
                await _repositoryManager.CategoryRepository.GetByIdAsync(request.Id);

            if (category is null)
                return BadRequest<string>("This Category Not Exist!");

            if (request.ImageFile is not null)
            {
                var imagePath = (
                    await _mediator.Send(new UpdateImageCommand()
                    {
                        FolderName = "Category",
                        NewFile = request.ImageFile,
                        OldImageName = category.ImagePath ?? ""
                    })
                ).Data;

                request.ImagePath = imagePath;
            }

            var adminId = (await _mediator.Send(new GetUserIdQuery())).Data;
            if (adminId is null)
                return BadRequest<string>("Added Category Failed");

            // mapping 
            category.AdminId = adminId;
            category.NameEn = request.NameEn;
            category.NameAr = request.NameEn;
            category.Description = request.Description;


            await _repositoryManager.CategoryRepository.UpdateAsync(category);

            await _repositoryManager.CategoryRepository.SaveChangesAsync();

            await _repositoryManager.CategoryRepository.CommitAsync();

            return Success($"Edit Successfull Category Id : {category.Id}");
        }
        catch (Exception)
        {
            await _repositoryManager.CategoryRepository.RollBackAsync();

            return BadRequest<string>("Edit Failed");
        }
    }

    public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager.CategoryRepository.BeginTransactionAsync();

        try
        {
            var categoryExisting =
                await _repositoryManager.CategoryRepository.GetByIdAsync(request.Id);

            if (categoryExisting is null)
                return BadRequest<string>("This Category Not Exist!");

            if (categoryExisting.ImagePath is not null)
            {
                var isDeleted = (
                    await _mediator.Send(new DeleteImageCommand()
                    {
                        folderName = "Category",
                        imageName = categoryExisting.ImagePath,
                    })
                ).Data;

                if (isDeleted)
                    categoryExisting.ImagePath = null;
            }

            await _repositoryManager.CategoryRepository.DeleteAsync(categoryExisting);

            await _repositoryManager.CategoryRepository.SaveChangesAsync();

            await _repositoryManager.CategoryRepository.CommitAsync();

            return Deleted<string>(
                $"Deleted Successfull Category Id : {categoryExisting.Id}"
            );
        }
        catch (Exception)
        {
            await _repositoryManager.CategoryRepository.RollBackAsync();

            return BadRequest<string>("Deleted Failed");
        }
    }
    #endregion
}

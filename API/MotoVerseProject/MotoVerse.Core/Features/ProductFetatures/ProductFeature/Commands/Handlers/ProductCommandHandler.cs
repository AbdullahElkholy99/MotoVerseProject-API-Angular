using MotoVerse.Core.Features.Shared.UploadImage.Commands.Models;

namespace MotoVerse.Core.Features.ProductFeature.Commands.Handlers;

internal class ProductCommandHandler :
    ResponseHandler,
    IRequestHandler<AddProductCommand, Response<string>>,
    IRequestHandler<DeleteProductCommand, Response<string>>,
    IRequestHandler<EditProductCommand, Response<string>>
{

    #region Fields

    private readonly IRepositoryManager _repositoryManager;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly IStringLocalizer<SharedResources> _stringLocalizer;

    #endregion

    #region CTOR

    public ProductCommandHandler(
        IStringLocalizer<SharedResources> stringLocalizer,
        IRepositoryManager repositoryManager,
        IMediator mediator,
        IMapper mapper) : base(stringLocalizer)
    {
        _stringLocalizer = stringLocalizer;
        _repositoryManager = repositoryManager;
        _mediator = mediator;
        _mapper = mapper;
    }

    #endregion

    #region Method Handlers

    public async Task<Response<string>> Handle(
        AddProductCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager.ProductRepository.BeginTransactionAsync();

        try
        {
            var product = _mapper.Map<Product>(request);

            product.Id = Guid.NewGuid().ToString();

            if (request.ImageFile is not null)
            {
                var image = (
                    await _mediator.Send(new UploadImageCommand()
                    {
                        ImageFile = request.ImageFile,
                        FolderName = "Product",
                    })
                ).Data;

                product.ImagePath = image;
            }

            await _repositoryManager.ProductRepository.AddAsync(product);

            await _repositoryManager.ProductRepository.SaveChangesAsync();

            await _repositoryManager.ProductRepository.CommitAsync();

            return Created("Added Successfully");
        }
        catch (Exception)
        {
            await _repositoryManager.ProductRepository.RollBackAsync();

            return BadRequest<string>("Added Failed");
        }
    }

    public async Task<Response<string>> Handle(
        EditProductCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager.ProductRepository.BeginTransactionAsync();

        try
        {
            // Check if product exists
            var product =
                await _repositoryManager.ProductRepository.GetByIdAsync(request.Id);

            if (product is null)
                return BadRequest<string>("This Product Not Exist!");

            if (request.ImageFile is not null)
            {
                var imagePath = (
                    await _mediator.Send(new UpdateImageCommand()
                    {
                        FolderName = "Product",
                        NewFile = request.ImageFile,
                        OldImageName = product.ImagePath ?? ""
                    })
                ).Data;

                request.ImagePath = imagePath;
            }

            // Mapping
            _mapper.Map(request, product);

            // Update
            await _repositoryManager.ProductRepository.UpdateAsync(product);

            await _repositoryManager.ProductRepository.SaveChangesAsync();

            await _repositoryManager.ProductRepository.CommitAsync();

            return Success(
                $"Edit Successfully Product Name : {product.NameAr}"
            );
        }
        catch (Exception)
        {
            await _repositoryManager.ProductRepository.RollBackAsync();

            return BadRequest<string>("Edit Failed");
        }
    }

    public async Task<Response<string>> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager.ProductRepository.BeginTransactionAsync();

        try
        {
            // Check if product exists
            var productExisting =
                await _repositoryManager.ProductRepository.GetByIdAsync(request.Id);

            if (productExisting is null)
                return BadRequest<string>("This Product Not Exist!");

            // Delete image
            if (productExisting.ImagePath is not null)
            {
                var isDeleted = (
                    await _mediator.Send(new DeleteImageCommand()
                    {
                        folderName = "Product",
                        imageName = productExisting.ImagePath,
                    })
                ).Data;

                if (isDeleted)
                    productExisting.ImagePath = null;
            }

            // Delete product
            await _repositoryManager.ProductRepository.DeleteAsync(productExisting);

            await _repositoryManager.ProductRepository.SaveChangesAsync();

            await _repositoryManager.ProductRepository.CommitAsync();

            return Deleted<string>(
                $"Deleted Successfully Product Name : {productExisting.NameAr}"
            );
        }
        catch (Exception)
        {
            await _repositoryManager.ProductRepository.RollBackAsync();

            return BadRequest<string>("Deleted Failed");
        }
    }

    #endregion
}
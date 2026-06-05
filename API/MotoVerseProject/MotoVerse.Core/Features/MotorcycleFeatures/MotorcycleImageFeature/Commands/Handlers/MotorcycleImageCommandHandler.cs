using MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Commands.Models;
using MotoVerse.Core.Features.Shared.UploadImage.Commands.Models;
using MotoVerse.Entities.Models.Motorcycles;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleImageFeature.Commands.Handlers;

internal class MotorcycleImageCommandHandler :
    ResponseHandler,
    IRequestHandler<AddMotorcycleImageCommand, Response<string>>,
    IRequestHandler<EditMotorcycleImageCommand, Response<string>>,
    IRequestHandler<DeleteMotorcycleImageCommand, Response<string>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;

    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    #endregion

    #region CTOR

    public MotorcycleImageCommandHandler(
        IRepositoryManager repositoryManager,
        IMediator mediator,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer)
        : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _mediator = mediator;
        _mapper = mapper;
    }

    #endregion

    public async Task<Response<string>> Handle(
        AddMotorcycleImageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var imagePath =
                (await _mediator.Send(new UploadImageCommand
                {
                    ImageFile = request.Image,
                    FolderName = "MotorcycleImages"
                })).Data;

            var motorcycleImage = new MotorcycleImage
            {
                Id = Guid.NewGuid().ToString(),

                ImageUrl = imagePath,

                MotorcycleId = request.MotorcycleId
            };

            await _repositoryManager
                .MotorcycleImageRepository
                .AddAsync(motorcycleImage);

            await _repositoryManager
                .MotorcycleImageRepository
                .SaveChangesAsync();

            return Created("Added Successfully");
        }
        catch (Exception)
        {
            return BadRequest<string>("Add Failed");
        }
    }

    public async Task<Response<string>> Handle(
        EditMotorcycleImageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var motorcycleImage =
                await _repositoryManager
                    .MotorcycleImageRepository
                    .GetByIdAsync(request.Id);

            if (motorcycleImage is null)
                return NotFound<string>();

            if (request.Image is not null)
            {
                var imagePath =
                    (await _mediator.Send(new UploadImageCommand
                    {
                        ImageFile = request.Image,
                        FolderName = "MotorcycleImages"
                    })).Data;

                motorcycleImage.ImageUrl = imagePath;
            }

            motorcycleImage.MotorcycleId =
                request.MotorcycleId;

            await _repositoryManager
                .MotorcycleImageRepository
                .UpdateAsync(motorcycleImage);

            await _repositoryManager
                .MotorcycleImageRepository
                .SaveChangesAsync();

            return Success("Updated Successfully");
        }
        catch (Exception)
        {
            return BadRequest<string>("Update Failed");
        }
    }

    public async Task<Response<string>> Handle(DeleteMotorcycleImageCommand request, CancellationToken cancellationToken)
    {

        //await using var transaction = await _repositoryManager.MotorcycleRepository.BeginTransactionAsync();
        try
        {
            var motorcycleImage = await _repositoryManager.MotorcycleImageRepository.GetByIdAsync(request.Id);

            if (motorcycleImage is null)
                return NotFound<string>();

            await _mediator.Send(new DeleteImageCommand()
            {
                folderName = "Motorcycle",
                imageName = motorcycleImage.ImageUrl
            });

            await _repositoryManager.MotorcycleImageRepository.DeleteAsync(motorcycleImage);

            await _repositoryManager.MotorcycleImageRepository.SaveChangesAsync();

            //await transaction.CommitAsync();

            return Deleted<string>("Deleted Successfully");
        }
        catch (Exception)
        {
            //await transaction.RollbackAsync();
            return BadRequest<string>("Delete Failed");
        }
    }
}
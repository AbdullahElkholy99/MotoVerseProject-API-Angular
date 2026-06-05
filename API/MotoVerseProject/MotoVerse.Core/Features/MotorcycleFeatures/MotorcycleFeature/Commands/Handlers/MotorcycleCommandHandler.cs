

using Hangfire;

namespace MotoVerse.Core.Features.MotorcycleFeatures.MotorcycleFeature.Commands.Handlers;

public class MotorcycleCommandHandler :
    ResponseHandler,
    IRequestHandler<AddMotorcycleCommand, Response<string>>,
    IRequestHandler<EditMotorcycleCommand, Response<string>>,
    IRequestHandler<DeleteMotorcycleCommand, Response<string>>
{
    #region Fields

    private readonly IRepositoryManager _repositoryManager;

    private readonly IMediator _mediator;

    private readonly IMapper _mapper;

    #endregion

    #region CTOR

    public MotorcycleCommandHandler(
        IRepositoryManager repositoryManager,
        IMediator mediator,
        IMapper mapper,
        IStringLocalizer<SharedResources> localizer
    ) : base(localizer)
    {
        _repositoryManager = repositoryManager;
        _mediator = mediator;
        _mapper = mapper;
    }

    #endregion

    #region Add Motorcycle

    public async Task<Response<string>> Handle(
        AddMotorcycleCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager
                .MotorcycleRepository
                .BeginTransactionAsync();

        string? mainImage = null;

        var uploadedImages = new List<string>();

        try
        {
            var motorcycle = new Motorcycle
            {
                Id = Guid.NewGuid().ToString(),

                NameAr = request.NameAr,
                NameEn = request.NameEn,

                Brand = request.Brand,
                Model = request.Model,

                Year = request.Year,

                Color = request.Color,
                PlateNumber = request.PlateNumber,

                EngineCC = request.EngineCC,

                PricePerDay = request.PricePerDay,

                Description = request.Description,

                Status = request.Status,

                OwnerId = request.OwnerId
            };

            // Main Image
            if (request.ImageFile is not null)
            {
                mainImage = await UploadMainImage(request.ImageFile);

                motorcycle.ImagePath = mainImage;
            }

            // Multiple Images
            if (request.Images is not null)
            {
                var images = await UploadImages(request.Images);

                motorcycle.Images = images;

                uploadedImages = images
                    .Select(x => x.ImageUrl)
                    .ToList();
            }

            await _repositoryManager
                .MotorcycleRepository
                .AddAsync(motorcycle);

            await _repositoryManager
                .MotorcycleRepository
                .SaveChangesAsync();

            await transaction.CommitAsync();

            return Created("Motorcycle Added Successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            // Delete uploaded files
            if (mainImage is not null) await DeleteImage(mainImage);
            if (uploadedImages is not null)
                foreach (var image in uploadedImages)
                {
                    await DeleteImage(image);
                }

            return BadRequest<string>(ex.Message);
        }
    }

    #endregion

    #region Edit Motorcycle

    #region Edit Motorcycle

    public async Task<Response<string>> Handle(
        EditMotorcycleCommand request,
        CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager
                .MotorcycleRepository
                .BeginTransactionAsync();

        string? newMainImage = null;

        var uploadedImages = new List<string>();

        try
        {
            Motorcycle? motorcycle =
                await _repositoryManager
                    .MotorcycleRepository
                    .GetByIdWithIncludeAsync(
                        request.Id,
                        x => x.Images);

            if (motorcycle is null)
                return NotFound<string>("Motorcycle Not Found");

            // =========================
            // Upload New Main Image
            // =========================

            if (request.ImageFile is not null)
            {
                newMainImage =
                    await UploadMainImage(request.ImageFile);
            }

            // =========================
            // Upload New Multiple Images
            // =========================

            List<MotorcycleImage>? newImages = null;

            if (request.Images?.Length > 0)
            {
                newImages =
                    await UploadImages(request.Images);

                uploadedImages =
                    newImages
                        .Select(x => x.ImageUrl)
                        .ToList();
            }

            // =========================
            // Mapping
            // =========================

            motorcycle.NameAr = request.NameAr;

            motorcycle.NameEn = request.NameEn;

            motorcycle.Brand = request.Brand;

            motorcycle.Model = request.Model;

            motorcycle.Year = request.Year;

            motorcycle.Color = request.Color;

            motorcycle.PlateNumber = request.PlateNumber;

            motorcycle.EngineCC = request.EngineCC;

            motorcycle.PricePerDay = request.PricePerDay;

            motorcycle.Description = request.Description;

            motorcycle.Status = request.Status;

            motorcycle.OwnerId = request.OwnerId;

            // =========================
            // Replace Main Image
            // =========================

            if (newMainImage is not null)
            {
                var oldMainImage = motorcycle.ImagePath;

                motorcycle.ImagePath = newMainImage;

                await DeleteImage(oldMainImage);
            }

            // =========================
            // Replace Multiple Images
            // =========================

            if (newImages is not null)
            {
                foreach (var oldImage in motorcycle.Images.ToList())
                {
                    await DeleteImage(oldImage.ImageUrl);

                    await _repositoryManager
                        .MotorcycleImageRepository
                        .DeleteAsync(oldImage);
                }

                motorcycle.Images = newImages;
            }

            // =========================
            // Save Changes
            // =========================

            await _repositoryManager
                .MotorcycleRepository
                .SaveChangesAsync();

            await transaction.CommitAsync();

            return Success("Updated Successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            // Delete Uploaded Main Image
            if (newMainImage is not null)
            {
                await DeleteImage(newMainImage);
            }

            // Delete Uploaded Multiple Images
            foreach (var image in uploadedImages)
            {
                await DeleteImage(image);
            }

            return BadRequest<string>(ex.Message);
        }
    }

    #endregion
    #endregion

    #region Delete Motorcycle

    public async Task<Response<string>> Handle(DeleteMotorcycleCommand request, CancellationToken cancellationToken)
    {
        await using var transaction =
            await _repositoryManager
                .MotorcycleRepository
                .BeginTransactionAsync();

        try
        {
            var motorcycle =
                await _repositoryManager
                    .MotorcycleRepository
                    .GetByIdWithIncludeAsync(
                        request.Id,
                        x => x.Images);

            if (motorcycle is null)
                return NotFound<string>("Motorcycle Not Found");

            // Store image paths before deleting
            var images = motorcycle.Images
                .Select(x => x.ImageUrl)
                .ToList();

            var mainImage = motorcycle.ImagePath;

            // Delete image records
            foreach (var image in motorcycle.Images.ToList())
            {
                await _repositoryManager
                    .MotorcycleImageRepository
                    .DeleteAsync(image);
            }

            // Delete motorcycle
            await _repositoryManager
                .MotorcycleRepository
                .DeleteAsync(motorcycle);

            await _repositoryManager
                .MotorcycleRepository
                .SaveChangesAsync();

            await transaction.CommitAsync();

            // Delete files after commit
            foreach (var image in images)
            {
                await DeleteImage(image);
            }

            await DeleteImage(mainImage);

            return Deleted<string>("Deleted Successfully");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            return BadRequest<string>(ex.Message);
        }
    }

    #endregion

    #region Helpers

    private async Task<List<MotorcycleImage>> UploadImages(
        IFormFile[] images)
    {
        var imagesDto = new List<MotorcycleImage>();

        foreach (var image in images.ToList())
        {
            var imagePath =
                (
                    await _mediator.Send(
                        new UploadImageCommand
                        {
                            ImageFile = image,
                            FolderName = "Motorcycle"
                        })
                ).Data;

            imagesDto.Add(new MotorcycleImage
            {
                Id = Guid.NewGuid().ToString(),
                ImageUrl = imagePath
            });
        }

        return imagesDto;
    }

    private async Task<string?> UploadMainImage(IFormFile image)
    {
        if (image is null)
            return null;

        return (
            await _mediator.Send(
                new UploadImageCommand
                {
                    ImageFile = image,
                    FolderName = "Motorcycle"
                })
        ).Data;
    }

    private async Task DeleteImage(string? imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            return;

        BackgroundJob.Enqueue<IMediator>(mediator =>
              mediator.Send(new DeleteImageCommand
              {
                  folderName = "Motorcycle",
                  imageName = imagePath
              })
          );
    }

    #endregion

    #region Mapping 
    private Motorcycle MapToMotorcycle(EditMotorcycleCommand command, Motorcycle motorcycle)
    {
        motorcycle.NameAr = command.NameAr;
        motorcycle.NameEn = command.NameEn;
        motorcycle.Brand = command.Brand;

        motorcycle.Model = command.Model;

        motorcycle.Year = command.Year;

        motorcycle.Color = command.Color;

        motorcycle.PlateNumber = command.PlateNumber;

        motorcycle.EngineCC = command.EngineCC;

        motorcycle.PricePerDay = command.PricePerDay;

        motorcycle.Description = command.Description;

        motorcycle.Status = command.Status;

        motorcycle.OwnerId = command.OwnerId;

        return motorcycle;
    }
    #endregion
}

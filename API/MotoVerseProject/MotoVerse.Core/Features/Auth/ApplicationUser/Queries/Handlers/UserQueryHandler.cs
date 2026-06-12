

namespace MotoVerse.Core.Features.ApplicationUser.Queries.Handlers
{
    public class UserQueryHandler : ResponseHandler,
         IRequestHandler<GetUserPaginationQuery, PaginatedResult<GetUserPaginationReponse>>,
         IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>,
         IRequestHandler<GetUserInfoQuery, Response<GetUserInfoResponse>>
    {
        #region Fields
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IStringLocalizer<SharedResources> _sharedResources;
        private readonly UserManager<User> _userManager;
        private readonly IRepositoryManager _repositoryManager;
        #endregion

        #region Constructors
        public UserQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IMapper mapper,
                                  UserManager<User> userManager,
                                  IRepositoryManager repositoryManager,
                                  IMediator mediator) : base(stringLocalizer)
        {
            _mapper = mapper;
            _sharedResources = stringLocalizer;
            _userManager = userManager;
            _repositoryManager = repositoryManager;
            _mediator = mediator;
        }
        #endregion

        #region Handle Functions
        public async Task<PaginatedResult<GetUserPaginationReponse>> Handle(GetUserPaginationQuery request, CancellationToken cancellationToken)
        {
            var customers = _repositoryManager.CustomerRepository.GetTableNoTracking();

            var pageNumber = request.PageNumber == 0 ? 1 : request.PageNumber;

            var pageSize = request.PageSize == 0 ? 10 : request.PageSize;

            int count = await customers.AsNoTracking().CountAsync();

            if (count == 0)
                return PaginatedResult<GetUserPaginationReponse>.Success(new List<GetUserPaginationReponse>(), count, pageNumber, pageSize);

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;

            var items =
                await customers
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new GetUserPaginationReponse
                {
                    Id = u.Id,
                    Address = u.Address,
                    IsActive = u.IsActive,
                    Email = u.Email ?? "",
                    ImagePath = u.ImagePath != null ? "https://localhost:7081/" + u.ImagePath : "https://localhost:7081/images/users/defualt.png",
                    Phone = u.PhoneNumber ?? "",
                    OrderCount = u.Orders.Count(),
                    RentalCount = u.Bookings.Count(),
                    DisplayName = u.DisplayName
                }).ToListAsync();


            return PaginatedResult<GetUserPaginationReponse>.Success(items, count, pageNumber, pageSize);
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id==request.Id);
            var customer = await _repositoryManager.CustomerRepository
                .GetByIdAsync(request.Id);
            if (customer is null) return NotFound<GetUserByIdResponse>(_sharedResources[SharedResourcesKeys.NotFound]);

            var customerDTO = new GetUserByIdResponse
            {
                Id = customer.Id,
                Address = customer.Address,
                IsActive = customer.IsActive,
                Email = customer.Email ?? "",
                ImagePath = customer.ImagePath != null ? "https://localhost:7081/" + customer.ImagePath : "https://localhost:7081/images/users/defualt.png",
                PhoneNumber = customer.PhoneNumber ?? "",
                OrderCount = customer.Orders.Count(),
                RentalCount = customer.Bookings.Count(),
                DisplayName = customer.DisplayName,
                CreatedAt = customer.CreatedAt
            };
            return Success(customerDTO);
        }

        public async Task<Response<GetUserInfoResponse>> Handle(GetUserInfoQuery request, CancellationToken cancellationToken)
        {
            var userId = (await _mediator.Send(new GetUserIdQuery())).Data;
            if (userId is null)
                return NotFound<GetUserInfoResponse>(_sharedResources[SharedResourcesKeys.NotFound]);

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
                return NotFound<GetUserInfoResponse>(_sharedResources[SharedResourcesKeys.NotFound]);


            var result = new GetUserInfoResponse
            {
                Email = user.Email,
                Name = user.DisplayName,
                PhoneNumber = user.PhoneNumber
            };

            return Success<GetUserInfoResponse>(result);

        }
        #endregion
    }
}

namespace MotoVerse.Core.Mapping.Products;

public static partial class QueryAuth
{
    public static User MapToUser(this AddCustomerCommand addUser)
    {
        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Address = addUser.Address,
            Email = addUser.Email,
            DisplayName = addUser.DisplayName,
            PhoneNumber = addUser.PhoneNumber,
            UserName = addUser.Email
        };
    }
    public static User MapToChangeUser(this EditUserCommand editUser, User oldUser)
    {

        oldUser.Id = editUser.Id;
        oldUser.Address = editUser.Address;
        oldUser.Email = editUser.Email;
        oldUser.DisplayName = editUser.DisplayName;
        oldUser.PhoneNumber = editUser.PhoneNumber;

        return oldUser;
    }
}


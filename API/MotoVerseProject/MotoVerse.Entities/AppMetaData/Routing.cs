
namespace MotoVerse.Data.AppMetaData;

public static class Routing
{
    public const string root = "api";
    public const string version1 = "V1";
    public const string Rule = $"{root}/{version1}";

    public const string add = "add";

    public const string getById = "get-by-id/{id}";
    public const string getAll = "get-all";
    public const string paginated = "paginated";

    public const string edit = "edit";

    public const string delete = "delete/{id}";

    public static class CategoryRouting
    {
        public const string Prefix = $"{Rule}/Category";

        public const string Add = $"{Prefix}/{add}";


        public const string GetAll = $"{Prefix}/{getAll}";
        public const string GetById = $"{Prefix}/{getById}";
        public const string Paginated = $"{Prefix}/{paginated}";

        public const string Edit = $"{Prefix}/{edit}";

        public const string Delete = $"{Prefix}/{delete}";
    }
    public static class ProductRouting
    {
        public const string Prefix = $"{Rule}/Product";

        public const string Add = $"{Prefix}/{add}";


        public const string GetAll = $"{Prefix}/{getAll}";
        public const string GetById = $"{Prefix}/{getById}";
        public const string GetRecommendations = Prefix + "/get-recommendations/{id}";
        public const string Paginated = $"{Prefix}/{paginated}";

        public const string Edit = $"{Prefix}/{edit}";

        public const string Delete = $"{Prefix}/{delete}";
    }
    public static class ReviewProductRouting
    {
        public const string Prefix = $"{Rule}/ReviewProduct";

        public const string Add = $"{Prefix}/{add}";


        public const string GetAll = $"{Prefix}/{getAll}";
        public const string GetById = $"{Prefix}/{getById}";
        public const string Paginated = $"{Prefix}/{paginated}";

        public const string Edit = $"{Prefix}/{edit}";

        public const string Delete = $"{Prefix}/{delete}";
    }

    public static class ApplicationUserRouting
    {
        public const string Prefix = Rule + "/User";
        public const string Create = Prefix + "/Create";
        public const string Paginated = Prefix + "/Paginated";
        public const string GetByID = $"{Prefix}/{getById}";
        public const string Edit = Prefix + "/Edit";
        public const string Delete = Prefix + "/Delete/{id}";
        public const string ChangePassword = Prefix + "/Change-Password";
    }
    public static class Authentication
    {
        public const string Prefix = Rule + "/Authentication";
        public const string SignIn = Prefix + "/SignIn";
        public const string Logout = Prefix + "/Logout";
        public const string RefreshToken = Prefix + "/Refresh-Token";
        public const string ValidateToken = Prefix + "/Validate-Token";


    }
    public static class ManagePassword
    {
        public const string Prefix = Rule + "/ManagePassword";

        public const string SendResetPasswordCode = Prefix + "/SendResetPasswordCode";
        public const string ConfirmResetPasswordCode = Prefix + "/ConfirmResetPasswordCode";
        public const string ResetPassword = Prefix + "/ResetPassword";

    }
    public static class ConfirmEmail
    {
        public const string Prefix = Rule + "/ConfirmEmail";

        public const string Confirm = Prefix + "/confirm-email";
        public const string SendConfirm = Prefix + "/send-confirm-email";

    }
    public static class AuthorizationRouting
    {
        public const string Prefix = Rule + "AuthorizationRouting";
        public const string Roles = Prefix + "/Roles";
        public const string Claims = Prefix + "/Claims";
        public const string Create = Roles + "/Create";
        public const string Edit = Roles + "/Edit";
        public const string Delete = Roles + "/Delete/{id}";
        public const string RoleList = Roles + "/Role-List";
        public const string GetRoleById = Roles + "/Role-By-Id/{id}";
        public const string ManageUserRoles = Roles + "/Manage-User-Roles/{userId}";
        public const string ManageUserClaims = Claims + "/Manage-User-Claims/{userId}";
        public const string UpdateUserRoles = Roles + "/Update-User-Roles";
        public const string UpdateUserClaims = Claims + "/Update-User-Claims";
    }
    public static class EmailsRoute
    {
        public const string Prefix = Rule + "EmailsRoute";
        public const string SendEmail = Prefix + "/SendEmail";
    }


    #region Motorcycles
    public static class MotorcycleRouting
    {
        public const string Prefix = $"{Rule}/Motorcycle";

        public const string Add = $"{Prefix}/{add}";

        public const string GetAll = $"{Prefix}/{getAll}";

        public const string GetById = $"{Prefix}/{getById}";

        public const string Paginated = $"{Prefix}/{paginated}";

        public const string Edit = $"{Prefix}/{edit}";

        public const string Delete = $"{Prefix}/{delete}";
    }

    public static class BookingRouting
    {
        public const string Prefix = $"{Rule}/Booking";

        public const string Add = $"{Prefix}/{add}";

        public const string GetAllForCustomer = $"{Prefix}/get-all-for-customer";
        public const string GetAll = $"{Prefix}/{getAll}";

        public const string GetById = $"{Prefix}/{getById}";

        public const string Paginated = $"{Prefix}/{paginated}";

        public const string Edit = $"{Prefix}/{edit}";

        public const string Delete = $"{Prefix}/{delete}";
    }
    public static class BookingStatusRouting
    {
        public const string Prefix = $"{Rule}/BookingStatus";

        public const string Reject = Prefix + "/reject/{id}";
        public const string Cancel = Prefix + "/cancel/{id}";
        public const string ReCancel = Prefix + "/re-cancel/{id}";
        public const string CancelByCustomer = Prefix + "/cancel-by-customer/{id}";
        public const string Approve = Prefix + "/approve/{id}";
        public const string Complete = Prefix + "/complete/{id}";
        public const string Activate = Prefix + "/activate/{id}";
        public const string Pay = Prefix + "/Pay/{id}";

    }

    public static class MotorcycleImageRouting
    {
        public const string Prefix = $"{Rule}/MotorcycleImage";

        public const string Add = $"{Prefix}/{add}";

        public const string GetAll = $"{Prefix}/{getAll}";

        public const string GetById = $"{Prefix}/{getById}";

        public const string Paginated = $"{Prefix}/{paginated}";

        public const string Edit = $"{Prefix}/{edit}";

        public const string Delete = $"{Prefix}/{delete}";
    }
    public static class BasketRouting
    {
        public const string Prefix = $"{Rule}/Basket";

        public const string Add = $"{Prefix}/{add}";

        public const string GetAll = $"{Prefix}/{getAll}";

        public const string GetById = $"{Prefix}/{getById}";

        public const string Edit = $"{Prefix}/{edit}";

        public const string Delete = $"{Prefix}/{delete}";
        public const string DeleteItem = $"{Prefix}/DeleteItem";
    }

    #endregion
}

namespace MotoVerse.Data.Results
{
    public class ManageUserRolesResult
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<UserRoles> UserRoles { get; set; }
    }
    public class UserRoles
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool HasRole { get; set; }
    }
}

namespace UConnBleedBlue.Models
{
    public class UserValidationService
    {
        private readonly List<string> _validUsers = new() { "KennyD", "BrianH", "TonyC" };

        public bool IsValidUser(string username) =>
            _validUsers.Any(u => string.Equals(u, username, StringComparison.OrdinalIgnoreCase));
    }
}

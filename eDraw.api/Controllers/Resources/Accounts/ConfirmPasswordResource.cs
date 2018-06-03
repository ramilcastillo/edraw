
namespace eDraw.api.Controllers.Resources.Accounts
{
    public class ConfirmPasswordResource
    {
        public string PasswordResetToken { get; set; }
        public string NewPassword { get; set; }
    }
}

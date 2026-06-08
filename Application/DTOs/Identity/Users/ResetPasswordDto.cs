namespace StockFlow.Application.DTOs.Identity.Users
{
    public class ResetPasswordDto
    {
        public string NewPassword { get; set; } = string.Empty;

        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
using Azure;
using System.ComponentModel.DataAnnotations;

namespace SmartDiary.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Имя пользователя обязательно")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Имя должно содержать от 3 до 100 символов")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Некорректный формат email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? Avatar { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? Settings { get; set; }

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
    }
}

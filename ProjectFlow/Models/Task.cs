using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Models
{
    public class Task
    {
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Поле має бути заповненим!")]
        [StringLength(100, MinimumLength = 15, ErrorMessage = "Введіть від 16 до 100 символів!")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd.MM.yy}")]
        public DateTime DueDate { get; set; }  

        public bool IsCompleted { get; set; } = false;

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd.MM.yy}")]
        public DateTime? UpdatedAt { get; set; }

        // Зв'язок з проектом
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        // Зв'язок з користувачем (виконавець задачі)
        public string? AssignedUserId { get; set; }
        public IdentityUser? AssignedUser { get; set; }
    }
}

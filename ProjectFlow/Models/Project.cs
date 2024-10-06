using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ProjectFlow.Models
{
    public class Project
    {
        [HiddenInput]
        public int ProjectId { get; set; }

        [Required(ErrorMessage = "Поле має бути заповненим!")]
        [StringLength(100, MinimumLength = 15, ErrorMessage = "Введіть від 16 до 100 символів!")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd.MM.yy}")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: dd.MM.yy}")]
        public DateTime? UpdatedAt { get; set; }

        // Зв'язок з користувачем (власником проекту)
        public string? OwnerId { get; set; }  // Зміна типу на string
        public IdentityUser? Owner { get; set; }

        // Колекція задач
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs.Task
{
    public class CreateTaskDto
    {
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
    }
}
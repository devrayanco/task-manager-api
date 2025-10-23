using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagerApi.Models.Enums;
using TaskStatusEnum = TaskManagerApi.Models.Enums.TaskStatus;

namespace TaskManagerApi.Models
{
    public class Task
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public TaskStatusEnum Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
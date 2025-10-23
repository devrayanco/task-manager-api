namespace TaskManagerApi.DTOs.Task;
using TaskStatusEnum = TaskManagerApi.Models.Enums.TaskStatus;


public class TaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
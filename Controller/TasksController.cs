using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs.Task;
using TaskManagerApi.Models.Enums;
using DbTask = TaskManagerApi.Models.Task;
using TaskStatusEnum = TaskManagerApi.Models.Enums.TaskStatus;

namespace TaskManagerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        private int GetUserId()
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new InvalidOperationException("Não foi possível encontrar o ID do usuário no token.");
            }
            return int.Parse(userIdString);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks()
        {
            var userId = GetUserId();
            var tasks = await _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.Status)
                .ThenByDescending(t => t.CreatedAt)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Status = t.Status.ToString(),
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(int id)
        {
            var userId = GetUserId();
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return NotFound(new { Message = "Tarefa não encontrada." });
            }
            return Ok(new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt
            });
        }

        [HttpPost]
        public async Task<ActionResult<TaskDto>> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            var userId = GetUserId();
            var task = new DbTask
            {
                Title = createTaskDto.Title,
                UserId = userId
            };
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            var taskDto = new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Status = task.Status.ToString(),
                CreatedAt = task.CreatedAt
            };
            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, taskDto);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] string newStatus)
        {
            var userId = GetUserId();
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound(new { Message = "Tarefa não encontrada." });
            }

            if (!Enum.TryParse<TaskStatusEnum>(newStatus, true, out var statusEnum))
            {
                return BadRequest(new { Message = $"Status '{newStatus}' é inválido." });
            }

            task.Status = statusEnum;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/title")]
        public async Task<IActionResult> UpdateTaskTitle(int id, [FromBody] string newTitle)
        {
            var userId = GetUserId();
            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
            {
                return NotFound(new { Message = "Tarefa não encontrada." });
            }

            if (string.IsNullOrWhiteSpace(newTitle))
            {
                return BadRequest(new { Message = "O título não pode ser vazio." });
            }

            task.Title = newTitle;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = GetUserId();
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (task == null)
            {
                return NotFound(new { Message = "Tarefa não encontrada." });
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
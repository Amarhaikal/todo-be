using AutoMapper;
using FirstApi.DTOs;
using FirstApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly FirstApiContext _context;
        private readonly IMapper _mapper;
        public TodoController(FirstApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //GET: api/Todo
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            try
            {
                var todos = await _context.Todos
                    .Include(t => t.LevelPriority)
                    .ToListAsync();

                if (todos == null)
                {
                    var notFoundResponse = new ApiResponse<List<Todo>>
                    {
                        Status = 200,
                        Message = "No Data",
                        Data = todos
                    };
                    return NotFound(notFoundResponse);
                }

                var newTodo = _mapper.Map<List<TodoDto>>(todos);

                var response = new ApiResponse<List<TodoDto>>
                {
                    Status = 200,
                    Message = "Data retrieved successfully",
                    Data = newTodo
                };
                return Ok(response);


            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<Todo>>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, errorResponse);
            }
        }

        //GET: api/Todo/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo(int id)
        {
            try
            {
                var todo = await _context.Todos
                    .Include(t => t.LevelPriority)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (todo == null)
                {
                    var notFoundResponse = new ApiResponse<Todo>
                    {
                        Status = 200,
                        Message = "No Data",
                        Data = null
                    };
                    return NotFound(notFoundResponse);
                }

                var newTodo = _mapper.Map<TodoDto>(todo);
                var response = new ApiResponse<TodoDto>
                {
                    Status = 200,
                    Message = "Data retrieved",
                    Data = newTodo
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<Todo>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, errorResponse);
            }
        }

        //POST: api/Todo
        [HttpPost]
        public async Task<IActionResult> CreateTodo(Todo todo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var invalidResponse = new ApiResponse<List<Todo>>
                    {
                        Status = 500,
                        Message = $"Invalid Request",
                        Data = null
                    };
                    return StatusCode(500, invalidResponse);
                }

                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();

                // Reload the todo with the LevelPriority relationship
                var savedTodo = await _context.Todos
                    .Include(t => t.LevelPriority)
                    .FirstOrDefaultAsync(t => t.Id == todo.Id);

                var todoDto = new TodoDto
                {
                    Id = savedTodo.Id,
                    Task = savedTodo.Task,
                    IsCompleted = savedTodo.IsCompleted,
                    LevelPriority = savedTodo.LevelPriority != null ? new LevelPriorityDto
                    {
                        Id = savedTodo.LevelPriority.Id,
                        Code = savedTodo.LevelPriority.Code,
                        Name = savedTodo.LevelPriority.Name
                    } : null
                };

                var response = new ApiResponse<TodoDto>
                {
                    Status = 201,
                    Message = $"Save successfully",
                    Data = todoDto
                };
                return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<Todo>>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}

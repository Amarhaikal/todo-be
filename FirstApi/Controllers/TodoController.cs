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
    public class TodoController : BaseApiController
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
                    return GetListNotFoundResponse();
                }

                var newTodo = _mapper.Map<List<TodoDto>>(todos);

                return GetListSuccessResponse(newTodo);

            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex.Message);
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
                    return GetDetailsNotFoundResponse();
                }

                var newTodo = _mapper.Map<TodoDto>(todo);
                return GetDetailsSuccessResponse(newTodo);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex.Message);
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
                    return InvalidDataToSaveResponse();
                }

                _context.Todos.Add(todo);
                await _context.SaveChangesAsync();

                // Reload the todo with the LevelPriority relationship
                var savedTodo = await _context.Todos
                    .Include(t => t.LevelPriority)
                    .FirstOrDefaultAsync(t => t.Id == todo.Id);

                if (savedTodo == null)
                {
                    return CreateFailedResponse();
                }

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

                return CreateSuccessResponse(todoDto, nameof(GetTodo), new { id = savedTodo });
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex.Message);
            }
        }

        //PUT
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(Todo todo, int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return InvalidDataToSaveResponse();
                }

                if (todo.Id != id)
                {
                    var IdNotMatchResponse = new ApiResponse<Todo>
                    {
                        Status = 500,
                        Message = "ID not match",
                        Data = null
                    };
                }

                var todoExist = await _context.Todos.FindAsync(id);
                if (todoExist == null)
                {
                    var noExistResponse = new ApiResponse<Todo>
                    {
                        Status = 500,
                        Message = $"Todo {id} not found",
                        Data = null
                    };
                    return NotFound(noExistResponse);
                }

                todoExist.Task = todo.Task;
                todoExist.IsCompleted  = todo.IsCompleted ;
                todoExist.LevelPriorityId  = todo.LevelPriorityId ;

                _context.Todos.Update(todoExist);
                await _context.SaveChangesAsync();

                var updatedTodo = await _context.Todos.Include(t => t.LevelPriority).FirstOrDefaultAsync(t => t.Id == id);
                var todoDto = _mapper.Map<TodoDto>(updatedTodo);

                return UpdateSuccessResponse(todoDto);
            }
            catch (Exception ex)
            {
                return ExceptionResponse(ex.Message);
            }
        }
    }
}

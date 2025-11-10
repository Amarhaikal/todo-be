using FirstApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelPriorityController : ControllerBase
    {
        private readonly FirstApiContext _context;
        public LevelPriorityController(FirstApiContext context)
        {
            _context = context;
        }

        //GET: api/LevelPriority
        [HttpGet]
        public async Task<IActionResult> GetLevelPriorities()
        {
            try
            {
                var levelPriorities = await _context.LevelPriorities.ToListAsync();

                if (levelPriorities == null)
                {
                    var notFoundResponse = new ApiResponse<List<LevelPriority>>
                    {
                        Status = 200,
                        Message = "No Data",
                        Data = levelPriorities
                    };
                    return NotFound(notFoundResponse);
                }
                var response = new ApiResponse<List<LevelPriority>>
                {
                    Status = 200,
                    Message = "Data retrieved successfully",
                    Data = levelPriorities
                };
                return Ok(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<LevelPriority>>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, errorResponse);
            }
        }

        //GET: api/LevelPriority/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLevelPriority(int id)
        {
            try
            {
                var levelPriorities = await _context.LevelPriorities.FindAsync(id);
                if (levelPriorities == null)
                {
                    var notFoundResponse = new ApiResponse<List<LevelPriority>>
                    {
                        Status = 200,
                        Message = $"Level Priority {id} Not Found",
                        Data = null
                    };
                    return NotFound(notFoundResponse);
                }
                var response = new ApiResponse<LevelPriority>
                {
                    Status = 200,
                    Message = $"Level Priority {id} Found",
                    Data = levelPriorities
                };
                return NotFound(response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<LevelPriority>>
                {
                    Status = 500,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null
                };
                return StatusCode(500, errorResponse);

            }
        }

        //POST: api/LevelPriority
        [HttpPost]
        public async Task<IActionResult> Create(LevelPriority levelPriority)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var badRequestResponse = new ApiResponse<LevelPriority>
                    {
                        Status = 400,
                        Message = "Invalid data provided",
                        Data = null
                    };
                    return BadRequest(badRequestResponse);

                }
                var levelPriorityExisted = await _context.LevelPriorities.FirstOrDefaultAsync(lp => lp.Code == levelPriority.Code);
                if (levelPriorityExisted != null)
                {
                    var conflictResponse = new ApiResponse<LevelPriority>
                    {
                        Status = 500,
                        Message = $"Code {levelPriority.Code} already exist",
                        Data = null
                    };

                    return Conflict(conflictResponse);
                }

                _context.LevelPriorities.Add(levelPriority);
                await _context.SaveChangesAsync();

                var response = new ApiResponse<LevelPriority>
                {
                    Status = 201,
                    Message = "Level priority created successfully",
                    Data = levelPriority
                };

                return CreatedAtAction(nameof(GetLevelPriority), new { id = levelPriority.Id }, response);

            }
            catch (Exception ex)
            {
                var errorResponse = new ApiResponse<List<LevelPriority>>
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

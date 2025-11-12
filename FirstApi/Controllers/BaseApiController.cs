using FirstApi.Model;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected IActionResult GetListSuccessResponse<T>(T data, string? message = null)
        {
            var response = new ApiResponse<T>
            {
                Status = 200,
                Message = message ?? "Data retrieved successfully",
                Data = data
            };
            return Ok(response);
        }

        protected IActionResult GetListNotFoundResponse(string? message = null)
        {
            var response = new ApiResponse<string>
            {
                Status = 200,
                Message = message ?? "No data to retrieve",
                Data = null
            };
            return NotFound(response);
        }

        protected IActionResult GetDetailsSuccessResponse<T>(T data, string? message = null)
        {
            var response = new ApiResponse<T>
            {
                Status = 200,
                Message = message ?? "Data found successfully",
                Data = data
            };
            return Ok(response);
        }

        protected IActionResult GetDetailsNotFoundResponse(string? message = null)
        {
            var response = new ApiResponse<string>
            {
                Status = 200,
                Message = message ?? "Data not found",
                Data = null
            };
            return NotFound(response);
        }

        protected IActionResult CreateSuccessResponse<T>(T data, string actionName, object routeValues, string? message = null)
        {
            var response = new ApiResponse<T>
            {
                Status = 201,
                Message = message ?? "Data created successfully",
                Data = data
            };
            return CreatedAtAction(actionName, routeValues, response);
        }

        protected IActionResult CreateFailedResponse(string? message = null)
        {
            var response = new ApiResponse<string>
            {
                Status = 500,
                Message = message ?? "Data save failed",
                Data = null
            };
            return StatusCode(500, response);
        }

        protected IActionResult UpdateSuccessResponse<T>(T data, string? message = null)
        {
            var response = new ApiResponse<T>
            {
                Status = 201,
                Message = message ?? "Data updated successfully",
                Data = data
            };
            return Ok(response);
        }

        protected IActionResult UpdateFailedResponse(string? message = null)
        {
            var response = new ApiResponse<string>
            {
                Status = 500,
                Message = message ?? "Data update failed",
                Data = null
            };
            return StatusCode(500, response);
        }

        protected IActionResult InvalidDataToSaveResponse(string? message = null)
        {
            var response = new ApiResponse<string>
            {
                Status = 500,
                Message = message ?? "Invalid data to save",
                Data = null
            };
            return StatusCode(500, response);
        }

        protected IActionResult ExceptionResponse(string message)
        {
            var response = new ApiResponse<string>
            {
                Status = 500,
                Message = $"An error occurred: {message}",
                Data = null
            };
            return StatusCode(500, response);
        }


    }
}
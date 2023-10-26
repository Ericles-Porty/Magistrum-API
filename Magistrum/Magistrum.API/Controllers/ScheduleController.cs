using Magistrum.API.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Magistrum.API.Entities;
using Magistrum.API.ViewModels;

namespace Magistrum.API.Controllers;
[ApiController]
[Route("api/v1/[controller]")]
public class ScheduleController : ControllerBase
{
    private readonly ILogger<ScheduleController> _logger;
    private readonly IScheduleService _scheduleService;

    public ScheduleController(ILogger<ScheduleController> logger, IScheduleService scheduleService)
    {
        _logger = logger;
        _scheduleService = scheduleService;
    }

    /// <summary>
    /// Get all schedules
    /// </summary>
    /// <returns>Json Array of all Schedules</returns>
    /// <response code="200">Returns the Schedules</response>
    /// <response code="500">If there is an error getting the Schedules</response>
    [ProducesResponseType(typeof(Schedule[]), 200)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    [HttpGet(Name = "GetAllSchedule")]
    public async Task<IActionResult> GetAllSchedule()
    {
        try
        {
            var schedules = await _scheduleService.GetAllAsync();
            return Ok(schedules);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(new ErrorModel
            {
                Message = ex.Message,
                HttpStatus = 500,
                ErrorCode = "SCHEDULE_GENERIC_ERROR"
            });
        }
    }

    /// <summary>
    /// Get a schedule by id
    /// </summary>
    /// <param name="id">Schedule id</param>
    /// <returns>Json object of a Schedule</returns>
    /// <response code="200">Returns the Schedule</response>
    /// <response code="404">If the Schedule is not found</response>
    /// <response code="500">If there is an error getting the Schedule</response>
    [HttpGet("{id}", Name = "GetSchedule")]
    [ProducesResponseType(typeof(Schedule), 200)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> GetSchedule(int id)
    {
        try
        {
            var schedule = await _scheduleService.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound(new ErrorModel
                {
                    Message = "Schedule not found",
                    HttpStatus = 404,
                    ErrorCode = "SCHEDULE_NOT_FOUND"
                });
            }
            return Ok(schedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(new ErrorModel
            {
                Message = ex.Message,
                HttpStatus = 500,
                ErrorCode = "SCHEDULE_GENERIC_ERROR"
            });
        }
    }

    /// <summary>
    /// Create a new Schedule
    /// </summary>
    /// <param name="scheduleCreateViewModel">ScheduleCreateViewModel object</param>
    /// <returns>Json object of the created Schedule</returns>
    /// <response code="201">Returns the created Schedule</response>
    /// <response code="400">If the Schedule is null</response>
    /// <response code="500">If there is an error creating the Schedule</response>
    [HttpPost(Name = "Create")]
    [ProducesResponseType(typeof(Schedule), 201)]
    [ProducesResponseType(typeof(ErrorModel), 400)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> Create([FromBody] ScheduleCreateViewModel scheduleCreateViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel
                {
                    Message = "Schedule is invalid",
                    HttpStatus = 400,
                    ErrorCode = "SCHEDULE_IS_INVALID"
                });
            }

            var mapedSchedule = new Schedule
            {
                StartTime = scheduleCreateViewModel.StartTime,
                EndTime = scheduleCreateViewModel.EndTime,
            };

            var createdSchedule = await _scheduleService.CreateAsync(mapedSchedule);
            return CreatedAtRoute("GetSchedule", new { id = createdSchedule.Id }, createdSchedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(new ErrorModel
            {
                Message = ex.Message,
                HttpStatus = 500,
                ErrorCode = "SCHEDULE_GENERIC_ERROR"
            });
        }
    }

    /// <summary>
    /// Update a Schedule
    /// </summary>
    /// <param name="id">Schedule id</param>
    /// <param name="schedule">Schedule object</param>
    /// <returns>Json object of the updated Schedule</returns>
    /// <response code="200">Returns the updated Schedule</response>
    /// <response code="404">If the Schedule is not found</response>
    /// <response code="500">If there is an error updating the Schedule</response>
    [HttpPut("{id}", Name = "Update")]
    [ProducesResponseType(typeof(Schedule), 200)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(typeof(ErrorModel), 500)]
    public async Task<IActionResult> Update(int id, [FromBody] Schedule schedule)
    {
        try
        {
            if (id != schedule.Id)
            {
                return BadRequest(new ErrorModel
                {
                    Message = "Schedule id does not match",
                    HttpStatus = 400,
                    ErrorCode = "SCHEDULE_ID_MISMATCH"
                });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorModel
                {
                    Message = "Schedule is invalid",
                    HttpStatus = 400,
                    ErrorCode = "SCHEDULE_IS_INVALID"
                });
            }

            var updatedSchedule = await _scheduleService.UpdateAsync(id, schedule);

            if (updatedSchedule == null)
            {
                return NotFound(new ErrorModel
                {
                    Message = "Schedule not found",
                    HttpStatus = 404,
                    ErrorCode = "SCHEDULE_NOT_FOUND"
                });
            }

            return Ok(updatedSchedule);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return BadRequest(new ErrorModel
            {
                Message = ex.Message,
                HttpStatus = 500,
                ErrorCode = "SCHEDULE_GENERIC_ERROR"
            });
        }
    }

    /// <summary>
    /// Delete a Schedule
    /// </summary>
    /// <param name="id">Schedule id</param>
    /// <returns>Json object of the deleted Schedule</returns>
    /// <response code="204"> Returns confirmation of deleted Schedule</response>
    /// <response code="404">If the Schedule is not found</response>
    /// <response code="500">If there is an error deleting the Schedule</response>
    [HttpDelete("{id}", Name = "Delete")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(ErrorModel), 404)]
    [ProducesResponseType(typeof(ErrorModel), 500)]

    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _scheduleService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return NotFound(new ErrorModel
            {
                Message = ex.Message,
                HttpStatus = 500,
                ErrorCode = "SCHEDULE_GENERIC_ERROR"
            });
        }
    }
}
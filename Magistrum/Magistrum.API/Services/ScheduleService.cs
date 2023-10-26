using Magistrum.API.Entities;
using Magistrum.API.Repositories.IRepositories;
using Magistrum.API.Services.IServices;

public class ScheduleService : IScheduleService
{
    private readonly IScheduleRepository _repository;
    private readonly ILogger<ScheduleService> _logger;
    public ScheduleService(IScheduleRepository repository, ILogger<ScheduleService> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Schedule> CreateAsync(Schedule schedule)
    {
        try
        {
            var createdSchedule = await _repository.CreateAsync(schedule);
            return createdSchedule;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ScheduleService.CreateAsync");
            return null!;
        }
    }

    public async Task DeleteAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ScheduleService.DeleteAsync");
        }
    }

    public async Task<IEnumerable<Schedule>> GetAllAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ScheduleService.GetAllAsync");
            return Enumerable.Empty<Schedule>();
        }
    }

    public async Task<Schedule> GetByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ScheduleService.GetByIdAsync");
            return null!;
        }
    }

    public async Task<Schedule> UpdateAsync(int id, Schedule schedule)
    {
        if (id != schedule.Id)
        {
            return null!;
        }

        try
        {
            var updatedSchedule = await _repository.UpdateAsync(id, schedule);
            return updatedSchedule;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ScheduleService.UpdateAsync");
            return null!;
        }
    }
}
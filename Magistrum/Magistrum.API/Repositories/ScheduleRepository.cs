

using Magistrum.API.Data;
using Magistrum.API.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using Magistrum.API.Entities;

namespace Magistrum.API.Repositories
{
    public class ScheduleRepository : IScheduleRepository
    {
        private readonly MagistrumContext _context;

        private readonly ILogger<ScheduleRepository> _logger;

        public ScheduleRepository(MagistrumContext context, ILogger<ScheduleRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Schedule> CreateAsync(Schedule schedule)
        {
            try
            {
                await _context.Schedules.AddAsync(schedule);
                await _context.SaveChangesAsync();
                return schedule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ScheduleRepository.CreateAsync");
                return null!;
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var schedule = await _context.Schedules.FindAsync(id);

                if (schedule == null)
                {
                    return;
                }

                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ScheduleRepository.DeleteAsync");
            }

        }

        public async Task<IEnumerable<Schedule>> GetAllAsync()
        {
            try
            {
                return await _context.Schedules
                    .AsNoTracking()
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ScheduleRepository.GetAllAsync");
                return Enumerable.Empty<Schedule>();
            }
        }

        public async Task<Schedule> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Schedules
                    .FindAsync(id) ?? null!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ScheduleRepository.GetByIdAsync");
                return null!;
            }
        }

        public async Task<Schedule> UpdateAsync(int id, Schedule schedule)
        {
            try
            {
                _context.Entry(schedule).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return schedule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ScheduleRepository.UpdateAsync");
                return null!;
            }
        }
    }

}

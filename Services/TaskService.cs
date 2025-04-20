using Microsoft.EntityFrameworkCore;
using TaskManager_api.Data;
using TaskManager_api.Models;

namespace TaskManager_api.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TaskItem>> GetTasksByUserAsync(string username, string role)
        {
            if (role == "Admin")
                return await _context.Tasks.ToListAsync();

            return await _context.Tasks
                .Where(t => t.AssignedTo == username)
                .ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id) => await _context.Tasks.FindAsync(id);

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task UpdateAsync(int id, TaskItem updatedTask)
        {
            var existing = await _context.Tasks.FindAsync(id);
            if (existing == null) return;

            existing.Title = updatedTask.Title;
            existing.Description = updatedTask.Description;
            existing.DueDate = updatedTask.DueDate;
            existing.IsCompleted = updatedTask.IsCompleted;
            existing.AssignedTo = updatedTask.AssignedTo;

            await _context.SaveChangesAsync();
        }
        
        public async Task DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AssignTaskAsync(int id, string assignedTo)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task != null)
            {
                task.AssignedTo = assignedTo;
                await _context.SaveChangesAsync();
            }
        }
    }
}

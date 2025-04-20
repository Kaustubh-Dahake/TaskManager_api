//using Microsoft.EntityFrameworkCore;
//using TaskManager_api.Models;

//namespace TaskManager_api.Data
//{
//    public class AppDbContext : DbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//        public DbSet<TaskItem> Tasks { get; set; }
//    }
//}


using Microsoft.EntityFrameworkCore;
using TaskManager_api.Models;
using System;
using System.Linq;

namespace TaskManager_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TaskItem> Tasks { get; set; }

        public void LogData()
        {
            var tasks = Tasks.ToList();
            Console.WriteLine("Current Tasks in Memory:");
            foreach (var task in tasks)
            {
                Console.WriteLine($"Title: {task.Title}, AssignedTo: {task.AssignedTo}, DueDate: {task.DueDate}");
            }
        }
    }
}

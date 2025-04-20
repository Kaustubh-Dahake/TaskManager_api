namespace TaskManager_api.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public string AssignedTo { get; set; }
    }
}


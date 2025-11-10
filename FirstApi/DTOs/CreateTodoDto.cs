namespace FirstApi.DTOs
{
    public class CreateTodoDto
    {
        public string Task { get; set; } = null!;
        public LevelPriorityDto? LevelPriority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
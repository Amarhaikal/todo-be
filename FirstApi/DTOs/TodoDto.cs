namespace FirstApi.DTOs
{
    public class TodoDto
    {
        public int Id { get; set; }
        public string Task { get; set; } = null!;
        public LevelPriorityDto? LevelPriority { get; set; }
        public bool IsCompleted { get; set; }
    }
}
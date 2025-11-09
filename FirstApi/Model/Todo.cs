using System.ComponentModel.DataAnnotations.Schema;

namespace FirstApi.Model
{
    public class Todo
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("task")]
        public string Task { get; set; } = null!;
        [Column("level_priority_id")]
        public int? LevelPriorityId { get; set; }
        [ForeignKey("LevelPriorityId")]
        public LevelPriority? LevelPriority { get; set; }
        [Column("isCompleted")]
        public bool IsCompleted { get; set; }
    }
}

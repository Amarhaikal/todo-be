using System.ComponentModel.DataAnnotations.Schema;

namespace FirstApi.Model
{
    public class LevelPriority
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("code")]
        public string Code { get; set; } = null!;
        [Column("name")]
        public string Name { get; set; } = null!;
    }
}

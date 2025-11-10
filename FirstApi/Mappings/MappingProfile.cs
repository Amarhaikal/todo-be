using AutoMapper;
using FirstApi.Model;
using FirstApi.DTOs;

namespace FirstApi.Mappings
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            //Entity to DTO mappings
            CreateMap<Todo, TodoDto>();
            CreateMap<LevelPriority, LevelPriorityDto>();

            //DTO to Entity mappings
            CreateMap<CreateTodoDto, Todo>();
        }
    }
}
using AutoMapper;
using TaskManager.Domain;

namespace TaskManager
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProjectEntity, ProjectViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RowKey))
                                                        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PartitionKey));
            CreateMap<TaskEntity, TaskViewModel>().ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RowKey))
                                                  .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.PartitionKey));
        }
    }
}

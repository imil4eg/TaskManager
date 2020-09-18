using AutoMapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Domain;

namespace TaskManager
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectViewModel>
    {
        private readonly IGenericRepository<ProjectEntity> _repository;
        private readonly ITableQueries<ProjectEntity, ProjectViewModel> _queries;
        private readonly IMapper _mapper;

        public UpdateProjectCommandHandler(IGenericRepository<ProjectEntity> repository,
            ITableQueries<ProjectEntity, ProjectViewModel> queries,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _queries = queries ?? throw new ArgumentNullException(nameof(queries));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ProjectViewModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var entity = await _queries.GetEntityAsync(request.Id.ToString(), request.Name, cancellationToken);

            if (entity == null)
            {
                entity = new ProjectEntity(Guid.NewGuid(), request.Name)
                {
                    Code = request.Code
                };
            }
            else
            {
                entity.Code = request.Code;
            }

            var updatedEntity = await _repository.UpdateOrInsertAsync(entity, cancellationToken);

            return _mapper.Map<ProjectEntity, ProjectViewModel>(updatedEntity);
        }
    }
}

using AutoMapper;
using TodoList.Application.DTO.Project;
using TodoList.Domain.Abstractions;
using ProjectEntity = TodoList.Domain.Entities.Project;

namespace TodoList.Application.UseCases.Project
{
    public class ProjectService
    {
        private readonly IRepository<ProjectEntity> _projectRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(
            IRepository<ProjectEntity> projectRepo,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _projectRepo = projectRepo;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ProjectDTO?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var project = await _projectRepo.Get(id, cancellationToken);

            return project is null
                ? null
                : _mapper.Map<ProjectDTO>(project);
        }

        public async Task<IReadOnlyCollection<ProjectDTO>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            var projects = await _projectRepo.GetAll(cancellationToken);

            return _mapper.Map<List<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO> CreateAsync(
            CreateProjectDTO dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var project = _mapper.Map<ProjectEntity>(dto);
            var createdProject = await _projectRepo.Add(project, cancellationToken);

            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<ProjectDTO>(createdProject);
        }

        public async Task<ProjectDTO?> UpdateAsync(
            Guid id,
            CreateProjectDTO dto,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(dto);

            var project = await _projectRepo.Get(id, cancellationToken);
            if (project is null)
            {
                return null;
            }

            project.Update(dto.Name, dto.Description);

            var updatedProject = await _projectRepo.Update(project, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return _mapper.Map<ProjectDTO>(updatedProject);
        }

        public async Task<bool> DeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var project = await _projectRepo.Get(id, cancellationToken);
            if (project is null)
            {
                return false;
            }

            await _projectRepo.Delete(id, cancellationToken);
            await _unitOfWork.SaveChanges(cancellationToken);

            return true;
        }
    }
}

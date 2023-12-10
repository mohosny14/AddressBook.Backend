using AdressBook.Domain.IRepositories;
using AdressBook.Domain.ViewModels;
using AdressBook.Services.IServices;
using AutoMapper;
using AdressBook.Domain.ViewModels.Common;
using AdressBook.Domain.Models;

namespace AdressBook.Services.Services
{
    public class JobService : IJobService
    {
        private readonly IJobsRepository _jobsRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public JobService(IJobsRepository jobsRepository,
                          IMapper mapper,
                          IUnitOfWork unitOfWork)
        {
            _jobsRepository = jobsRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> CreateAsync(CreateModelDto modelDto)
        {
            var job = _mapper.Map<Jobs>(modelDto);
            if(job != null)
            {
              await  _jobsRepository.AddAsync(job);
              await  _unitOfWork.CompleteAsync();

                return new ApiResponse<string>()
                {
                    Data = null,
                    Succeeded = true,
                    Message = "Added Successfully"
                };
            }
            return new ApiResponse<string>()
            {
                Message = "something wrong happened!",
                Succeeded = false
            };
        }

        public async Task<ApiResponse<IEnumerable<GetModelDto>>> GetAllAsync(string? searchtext)
        {
            var searchResults = await _jobsRepository.FindAllAsync(
                                                           e => ((string.IsNullOrEmpty(searchtext) ||
                                                           e.Name.Contains(searchtext) ||
                                                           e.Description.Contains(searchtext)) && !e.IsDeleted));

            if (searchResults != null)
            {
                var employees = _mapper.Map<List<GetModelDto>>(searchResults);
                return new ApiResponse<IEnumerable<GetModelDto>>()
                {
                    Succeeded = true,
                    Data = employees
                };
            }
            else
            {
                return new ApiResponse<IEnumerable<GetModelDto>>()
                {
                    Succeeded = false,
                    Data = null
                };
            }
        }

        public async Task<ApiResponse<GetModelDto>> GetById(int Id)
        {
            var result = await _jobsRepository.FindAsync(j => !j.IsDeleted && j.Id == Id);
            if (result != null)
            {
                var job = _mapper.Map<GetModelDto>(result);
                return new ApiResponse<GetModelDto>()
                {
                    Succeeded = true,
                    Data = job
                };
            }
            else
            {
                return new ApiResponse<GetModelDto>()
                {
                    Succeeded = false,
                    Data = null,
                    Message = "Not Found"
                };
            }
        }

        public async Task<ApiResponse<string>> SoftDelete(int Id)
        {
            var job = await _jobsRepository.FindAsync(e => e.Id == Id && !e.IsDeleted);
            if (job == null)
            {
                return new ApiResponse<string>()
                {
                    Data = null,
                    Message = "Invalid Employee",
                    Succeeded = false
                };
            }
            job.IsDeleted = true;
            _jobsRepository.Update(job);
            _unitOfWork.Complete();

            return new ApiResponse<string>()
            {
                Data = null,
                Message = "Deleted Successfully!",
                Succeeded = true
            };
        }

        public async Task<ApiResponse<string>> UpdateAsync(UpdateModelDto modelDto)
        {
            var job = await _jobsRepository.FindAsync(e => e.Id == modelDto.Id && !e.IsDeleted);
            if (job == null)
            {
                return new ApiResponse<string>()
                {
                    Data = null,
                    Message = "Invalid Employee",
                    Succeeded = false
                };
            }
            job = _mapper.Map<Jobs>(modelDto);

            _jobsRepository.Update(job);
            _unitOfWork.Complete();

            return new ApiResponse<string>()
            {
                Data = null,
                Succeeded = true,
                Message = "Updated Successfully"
            };
        }
    }
}
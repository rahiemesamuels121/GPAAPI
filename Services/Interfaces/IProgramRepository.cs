using GPACARICOMAPI.Models;

namespace GPACARICOMAPI.Services.Interfaces
{
    public interface IProgramRepository
    {
        public Task<List<ProgramModel>> GetProgramsAsync();
        public Task<ProgramModel> GetProgram(int id);
        public Task<bool> AddNewProgram(ProgramModel program);
        public Task<bool> UpdateProgram(ProgramModel article);
        public Task<bool> DeleteProgram(int id);
    }
}

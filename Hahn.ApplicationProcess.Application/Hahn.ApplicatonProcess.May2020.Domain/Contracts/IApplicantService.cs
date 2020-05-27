using Hahn.ApplicatonProcess.May2020.Domain.Dto;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Domain.Contracts
{
    public interface IApplicantService
    {
        Task<ApplicantDto> GetApplicant(int id);
        Task<int> CreateApplicant(ApplicantDto applicant);
        Task DeleteApplicant(int id);
        Task UpdateApplicant(ApplicantDto applicant);
    }
}

using Hahn.ApplicationProcess.May2020.Data;
using Hahn.ApplicationProcess.May2020.Data.Repository;
using Hahn.ApplicatonProcess.May2020.Domain.Contracts;
using Hahn.ApplicatonProcess.May2020.Domain.Dto;
using System;
using System.Threading.Tasks;

namespace Hahn.ApplicatonProcess.May2020.Domain
{
    public class ApplicantService : IApplicantService
    {
        private readonly ApplicantRepository _repository;
        public ApplicantService(ApplicantRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> CreateApplicant(ApplicantDto applicant)
        {
            var newApplicant = new Applicant()
            {
                Name = applicant.Name,
                FamilyName = applicant.FamilyName,
                Age = applicant.Age,
                EMailAdress = applicant.EMailAdress,
                CountryOfOrigin = applicant.CountryOfOrigin,
                Address = applicant.Address,
                Hired = applicant.Hired
            };

            return await _repository.CreateApplicant(newApplicant);
        }

        public async Task DeleteApplicant(int id)
        {
            await _repository.DeleteApplicant(id);
        }

        public async Task<ApplicantDto> GetApplicant(int id)
        {
            var applicant = await _repository.GetApplicant(id);
            return new ApplicantDto()
            {
                Name = applicant.Name,
                FamilyName = applicant.FamilyName,
                Age = applicant.Age,
                EMailAdress = applicant.EMailAdress,
                CountryOfOrigin = applicant.CountryOfOrigin,
                Address = applicant.Address,
                Hired = applicant.Hired
            };
        }

        public async Task UpdateApplicant(ApplicantDto applicantDto)
        {
            var applicantDomain = await _repository.GetApplicant(applicantDto.Id);
            applicantDomain.Name = applicantDto.Name;
            applicantDomain.FamilyName = applicantDto.Name;
            applicantDomain.Hired = applicantDto.Hired;
            applicantDomain.EMailAdress = applicantDto.EMailAdress;
            applicantDomain.Address = applicantDto.Address;
            applicantDomain.Age = applicantDto.Age;
            applicantDomain.CountryOfOrigin = applicantDto.CountryOfOrigin;

            await _repository.UpdateApplicant(applicantDomain);
        }
    }
}

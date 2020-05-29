using Hahn.ApplicationProcess.May2020.Data;
using Hahn.ApplicationProcess.May2020.Data.Repository;
using Hahn.ApplicatonProcess.May2020.Domain.Contracts;
using Hahn.ApplicatonProcess.May2020.Domain.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
                EMailAddress = applicant.EMailAddress,
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
            return applicant != null ? new ApplicantDto()
            {
                Id = applicant.Id,
                Name = applicant.Name,
                FamilyName = applicant.FamilyName,
                Age = applicant.Age,
                EMailAddress = applicant.EMailAddress,
                CountryOfOrigin = applicant.CountryOfOrigin,
                Address = applicant.Address,
                Hired = applicant.Hired
            } : null;
        }

        public async Task UpdateApplicant(ApplicantDto applicantDto)
        {
            var applicantDomain = await _repository.GetApplicant(applicantDto.Id);
            applicantDomain.Name = applicantDto.Name;
            applicantDomain.FamilyName = applicantDto.FamilyName;
            applicantDomain.Hired = applicantDto.Hired;
            applicantDomain.EMailAddress = applicantDto.EMailAddress;
            applicantDomain.Address = applicantDto.Address;
            applicantDomain.Age = applicantDto.Age;
            applicantDomain.CountryOfOrigin = applicantDto.CountryOfOrigin;

            await _repository.UpdateApplicant(applicantDomain);
        }

        public async Task<List<ApplicantDto>> ListApplicants()
        {
            return await _repository.ListApplicants().Select(applicant => new ApplicantDto()
            {
                Id = applicant.Id,
                Name = applicant.Name,
                FamilyName = applicant.FamilyName,
                Age = applicant.Age,
                EMailAddress = applicant.EMailAddress,
                CountryOfOrigin = applicant.CountryOfOrigin,
                Address = applicant.Address,
                Hired = applicant.Hired
            }).ToListAsync();
        }
    }
}

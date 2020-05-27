using FluentValidation;
using Hahn.ApplicationProcess.May2020.Data.Data;
using Hahn.ApplicationProcess.May2020.Data.Validators;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.May2020.Data.Repository
{
    public class ApplicantRepository
    {
        private readonly DataContext _context;
        private readonly ApplicantValidator _validator;

        public ApplicantRepository(DataContext context, ApplicantValidator applicantValidator)
        {
            _context = context;
            _validator = applicantValidator;
        }
        
        public async Task<int> CreateApplicant(Applicant applicant)
        {
            _validator.ValidateAndThrow(applicant);

            var newId = _context.Applicants.Any() ? _context.Applicants.Select(x => x.Id).Max() + 1
                                                  : 1;
            applicant.Id = newId;
            _context.Applicants.Add(applicant);
            await _context.SaveChangesAsync();

            return applicant.Id;
        }

        public async Task DeleteApplicant(int id)
        {
            var applicantToDelete = _context.Applicants.Find(id);
            if (applicantToDelete == null)
            {
                throw new InvalidOperationException("There is no applicant that matches the Id you informed. Please try again with another Id parameter.");
            }
           _context.Remove(applicantToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateApplicant(Applicant applicant)
        {
            _validator.ValidateAndThrow(applicant);

            _context.Set<Applicant>().Update(applicant);
            await _context.SaveChangesAsync();
        }

        public async Task<Applicant> GetApplicant(int id)
        {
            var applicant = await _context.Applicants.FindAsync(id);
            return applicant;
        }
    }
}

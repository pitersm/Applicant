using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Hahn.ApplicationProcess.May2020.Data;
using Hahn.ApplicatonProcess.May2020.Domain.Contracts;
using Hahn.ApplicatonProcess.May2020.Domain.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Hahn.ApplicationProcess.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApplicantController : ControllerBase
    {
        private readonly IApplicantService _applicantService;

        public ApplicantController(IApplicantService applicantService)
        {
            _applicantService = applicantService;
        }

        /// <summary>
        /// Gets a specific applicant by unique id
        /// </summary>
        /// <param name="id" example="1">The applicant id</param>
        [HttpGet("{id}")]
        public async Task<ActionResult<Applicant>> Get(int id)
        {
            var value = await _applicantService.GetApplicant(id);

            return Ok(value);
        }

        // POST api/<controller>
        /// <summary>
        /// Inserts a new applicant in the database
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post(ApplicantDto applicant)
        {
            try
            {
                var newCompanyId = await _applicantService.CreateApplicant(applicant);
                return new CreatedAtActionResult(nameof(Get), "Applicant", new { id = newCompanyId }, newCompanyId);
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            //catch
            //{
            //    return new HttpResponseException("There has been a problem with your request. Please try again.");
            //}
        }

        /// <summary>
        /// Updates an applicant record in the database
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Put(ApplicantDto applicant)
        {
            try
            {
                await _applicantService.UpdateApplicant(applicant);
                return Ok();
            }
            catch (FluentValidation.ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            //catch
            //{
            //    return new HttpResponseException("There has been a problem with your request. Please try again.");
            //}
        }

        /// <summary>
        /// Deletes an applicant record in the database
        /// </summary>
        /// <param name="id" example="1">The applicant id</param>
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
                await _applicantService.DeleteApplicant(id);
                return Ok();
            //catch 
            //{
            //    return new HttpResponseException("There has been a problem with your request. Please try again.");
            //}
        }
    }
}

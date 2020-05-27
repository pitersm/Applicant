using FluentValidation;
using FluentValidation.Validators;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.May2020.Data.Validators
{
    public class ApplicantValidator : AbstractValidator<Applicant>
    {
        private readonly HttpClient _httpClient;
        public ApplicantValidator()
        {
            _httpClient = new HttpClient();

            RuleFor(a => a.Name).MinimumLength(5).WithMessage("The name must contain more than 5 characters.");
            RuleFor(a => a.FamilyName).MinimumLength(5).WithMessage("The family name must contain more than 5 characters.");
            RuleFor(a => a.Address).MinimumLength(10).WithMessage("The address must contain more than 5 characters.");
            RuleFor(a => a.EMailAdress).EmailAddress().WithMessage("The e-mail address is invalid. Please inform a valid e-mail address");
            RuleFor(a => a.Age).GreaterThanOrEqualTo(20).LessThanOrEqualTo(60).WithMessage("The applicant's age must be between 20 and 60 years old.");
            RuleFor(a => a.CountryOfOrigin).MustAsync(async (country, cancellation) =>
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://restcountries.eu/rest/v2/name/{country}?fullText=true");
                try
                {
                    var response = await _httpClient.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    // log excp
                    throw ex;
                }
            }).WithMessage("The specified country does not exist");
        }
    }
}
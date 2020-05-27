using System.ComponentModel;

namespace Hahn.ApplicatonProcess.May2020.Domain.Dto
{
    public class ApplicantDto
    {
        public int Id { get; set; }
        /// <summary>
        /// The applicant's name
        /// </summary>
        /// <example>Piter</example>
        public string Name { get; set; }
        /// <summary>
        /// The applicant's family name
        /// </summary>
        /// <example>Machado</example>
        public string FamilyName { get; set; }
        /// <example>Mathilde Benner street</example>
        /// <summary>
        /// The applicant's family name
        /// </summary>
        public string Address { get; set; }
        /// <example>Brazil</example>
        public string CountryOfOrigin { get; set; }
        /// <example>pitersm@hotmail.com</example>
        public string EMailAddress { get;  set; }
        /// <example>26</example>
        public int Age { get; set; }
        /// <example>true</example>
        [DefaultValue(false)]
        public bool Hired { get; set; }
    }
}

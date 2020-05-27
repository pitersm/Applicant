using System.ComponentModel.DataAnnotations;

namespace Hahn.ApplicationProcess.May2020.Data
{
    public class Applicant
    {

        public Applicant()
        { }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public string Address { get; set; }
        public string CountryOfOrigin { get; set; }
        public string EMailAddress { get; set; }
        public int Age { get; set; }
        public bool Hired { get; set; }
    }
}

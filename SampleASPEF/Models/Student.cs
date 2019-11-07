using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace SampleASPEF.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        public DateTime EnrollmentDate { get; set; }

        [MaxLength(50)]
        [Required]
        public string Address { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assessments.Models
{
    public class Assess_Info
    {
        [Key]
        public int RecId { get; set; }
        [StringLength(10)]
        public string UIC { get; set; }
        [StringLength(10)]
        public string CTEUIC { get; set; }
        [StringLength(8)]
        public string CIPCode { get; set; }
        [StringLength(5)]
        public string OBNo { get; set; }
        [StringLength(75)]
        public string SchoolName { get; set; }
        [StringLength(5)]
        public string FANo { get; set; }
        [StringLength(10)]
        public string TestCode { get; set; }
        [StringLength(50)]
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        [StringLength(20)]
        public string Teacher { get; set; }
        [StringLength(10)]
        public string Cluster { get; set; }
        [StringLength(50)]
        public string LastName { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(5)]

        public decimal AssessScore { get; set; }
        [StringLength(8)]
        public string AssessYear { get; set; }
        [StringLength(4)]
        public string PassFail { get; set; }
        public DateTime UpdateDate { get; set; }
        [StringLength(10)]
        public string UpdateBy { get; set; }
        public string Comment { get; set; }
        [StringLength(100)]
        public byte encLastName { get; set; }
        [StringLength(100)]
        public byte encFirstName { get; set; }
        [StringLength(1)]
        public char Accepted { get; set; }
    }
}

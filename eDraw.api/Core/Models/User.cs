using System;
using System.ComponentModel.DataAnnotations;

namespace eDraw.api.Core.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string BusinessName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string TaxId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string StreetAddress { get; set; }
        public string AppOrSuite { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string DocumentAttached { get; set; }
        public bool IsActive { get; set; }

        #region Prerequired Fields
        public Guid CreatedBy { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
       
        #endregion










    }
}

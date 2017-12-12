namespace OfferMaker.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Account
    {
        public int Id { get; set; }

        [Required]
        [MinLength(DataConstants.EntityTitleMinLenght)]
        [MaxLength(DataConstants.EntityTitleMaxLenght)]
        public string Name { get; set; }

        public string Address { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        public IEnumerable<Opportunity> Opportunities { get; set; }

        public string ManagerId { get; set; }

        public User Manager { get; set; }
    }
}

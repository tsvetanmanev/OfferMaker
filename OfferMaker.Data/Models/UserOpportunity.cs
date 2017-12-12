namespace OfferMaker.Data.Models
{
    public class UserOpportunity
    {
        public string UserId { get; set; }

        public User User { get; set; }

        public int OpportunityId { get; set; }

        public Opportunity Opportunity { get; set; }
    }
}

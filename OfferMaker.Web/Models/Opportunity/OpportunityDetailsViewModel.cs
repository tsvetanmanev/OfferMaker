namespace OfferMaker.Web.Models.Opportunity
{
    using OfferMaker.Services.Models.Opportunity;

    public class OpportunityDetailsViewModel
    {
        public OpportunityDetailsServiceModel Opportunity { get; set; }

        public bool UserIsMemberOfOpportunity { get; set; }
    }
}

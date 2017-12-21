using OfferMaker.Common.Mapping;
using OfferMaker.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OfferMaker.Services.Models
{
    public class OpportunityListingServiceModel : IMapFrom<Opportunity>
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public DateTime CreationDate { get; set; }

    }
}

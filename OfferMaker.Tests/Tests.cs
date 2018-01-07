namespace OfferMaker.Tests
{
    using AutoMapper;
    using OfferMaker.Web.Infrastructure.Mapping;

    public class Tests
    {
        private static bool testsInitialized = false;

        public static void Initialize()
        {
            if (!testsInitialized)
            {
                Mapper.Initialize(config => config.AddProfile<AutoMapperProfile>());
                testsInitialized = true;
            }
        }
    }
}

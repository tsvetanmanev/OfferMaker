﻿namespace OfferMaker.Common.Mapping
{
    using AutoMapper;

    public interface IHaveCustomMapping
    {
        void ConfigureMapping(Profile mapper);
    }
}

﻿using System;
using System.Threading.Tasks;

namespace NSeed.Tests.Unit.Discovery.Seed
{
    internal class BaseTestSeed : ISeed
    {
        public Task<bool> OutputAlreadyExists()
        {
            throw new NotImplementedException();
        }

        public Task Seed()
        {
            throw new NotImplementedException();
        }

        public Task WeedOut()
        {
            throw new NotImplementedException();
        }
    }
}
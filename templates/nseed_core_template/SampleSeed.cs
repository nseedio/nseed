﻿using NSeed;
using System;
using System.Threading.Tasks;

namespace SeedsProject
{
    // TODO: To learn how to write seeds see TODO-URL.
    public sealed class SampleSeed : ISeed
    {
        public Task Seed()
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasAlreadyYielded()
        {
            throw new NotImplementedException();
        }
    }

    public class Yield : YieldOf<SampleSeed>
    {
        // TODO: Use the Yield class to provide the yield of this seed to other seeds.
        //       To learn how to use the Yield class see TODO-URL.
    }
}
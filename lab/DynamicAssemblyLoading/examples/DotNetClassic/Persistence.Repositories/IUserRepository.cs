﻿using SmokeTests.BusinessModel.Entities;

#if DotNetCore
using Microsoft.EntityFrameworkCore;
#endif

#if DotNetClassic
using System.Data.Entity;
#endif

namespace SmokeTests.Persistence.Repositories
{
    public interface IUserRepository
    {
        DbSet<User> Users { get; }
    }
}

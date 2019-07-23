﻿using Audit.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Infrastructure.Repositories
{
    public interface IRunningTotalRepository
    {
        RunningTotal CreateRunningTotal(RunningTotal runningTotal);

        Task<bool> SaveChangesAsync();

        Task<double> GetPrevioudTotal();
    }
}

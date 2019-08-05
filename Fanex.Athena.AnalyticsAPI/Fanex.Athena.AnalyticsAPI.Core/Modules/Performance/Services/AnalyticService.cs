using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Core.Modules.Performance.Services
{
    public class AnalyticService : IAnalyticService
    {
        public virtual Task<DistributionAnalyticResult> GetDistribution()
        {
            var fake = new DistributionAnalyticResult()
            {
                Items = new List<AnalyticResultItem<PieceResult>>()
                {
                    new AnalyticResultItem<PieceResult>()
                    {
                        Type = 1,
                        Items = new List<PieceResult>()
                        {
                            new PieceResult() { Name = "ABC", Value = 540, Order = 1 },
                            new PieceResult() { Name = "ABC", Value = 540, Order = 1 }
                        }
                    },
                    new AnalyticResultItem<PieceResult>()
                    {
                        Type = 2,
                        Items = new List<PieceResult>()
                        {
                            new PieceResult() { Name = "ABC", Value = 540, Order = 1 },
                            new PieceResult() { Name = "ABC", Value = 540, Order = 1 }
                        }
                    },
                    new AnalyticResultItem<PieceResult>()
                    {
                         Type = 3,
                        Items = new List<PieceResult>()
                        {
                            new PieceResult() { Name = "ABC", Value = 540, Order = 1 },
                            new PieceResult() { Name = "ABC", Value = 540, Order = 1 }
                        }
                    }
                }
            };

            return Task.FromResult(fake);
        }

        public virtual Task<GrowthAnalyticResult> GetGrowth()
        {
            throw new NotImplementedException();
        }

        public virtual Task<TotalAnalyticResult> GetTotal()
        {
            throw new NotImplementedException();
        }
    }
}
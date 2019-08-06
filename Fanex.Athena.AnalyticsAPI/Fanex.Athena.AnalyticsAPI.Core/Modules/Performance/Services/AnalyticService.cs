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
            var result = FakeData();
            return Task.FromResult(result);
        }

        public virtual Task<GrowthAnalyticResult> GetGrowth()
        {
            throw new NotImplementedException();
        }

        public virtual Task<TotalAnalyticResult> GetTotal()
        {
            throw new NotImplementedException();
        }

        private static DistributionAnalyticResult FakeData()
        {
            var fake = new DistributionAnalyticResult()
            {
                Charts = new List<ChartItemResult<ChartColumnResult<PartChartColumnResult>>>()
                {
                    new ChartItemResult<ChartColumnResult<PartChartColumnResult>>()
                    {
                        Type = 1,
                        ChartColumns = new List<ChartColumnResult<PartChartColumnResult>>()
                        {
                            new ChartColumnResult<PartChartColumnResult>()
                            {
                                PartChartColumns = new List<PartChartColumnResult>()
                                {
                                    new PartChartColumnResult()
                                    {
                                        Id = 1,
                                        Name = "Part chart column 1",
                                        Value = 1
                                    },
                                    new PartChartColumnResult()
                                    {
                                        Id = 1,
                                        Name = "Part chart column 1",
                                        Value = 1
                                    },
                                    new PartChartColumnResult()
                                    {
                                        Id = 1,
                                        Name = "Part chart column 1",
                                        Value = 1
                                    }
                                }
                            }
                        }
                    },
                    new ChartItemResult<ChartColumnResult<PartChartColumnResult>>()
                    {
                        Type = 2,
                        ChartColumns = new List<ChartColumnResult<PartChartColumnResult>>()
                        {
                            new ChartColumnResult<PartChartColumnResult>()
                            {
                                PartChartColumns = new List<PartChartColumnResult>()
                                {
                                    new PartChartColumnResult()
                                    {
                                        Id = 1,
                                        Name = "Part chart column 1",
                                        Value = 1
                                    },
                                    new PartChartColumnResult()
                                    {
                                        Id = 1,
                                        Name = "Part chart column 1",
                                        Value = 1
                                    },
                                    new PartChartColumnResult()
                                    {
                                        Id = 1,
                                        Name = "Part chart column 1",
                                        Value = 1
                                    }
                                }
                            }
                        }
                    }
                }
            };

            return fake;
        }

    }
}
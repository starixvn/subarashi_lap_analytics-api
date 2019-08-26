using Fanex.Athena.AnalyticsAPI.Core.Models.ClientParams;
using Fanex.Athena.AnalyticsAPI.Core.Models.DTO;
using Fanex.Athena.AnalyticsAPI.Core.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Models.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.AnalyticsAPI.Performance.Services
{
    public class PerformanceService : AnalyticService<PerformanceTotalResult, PerformanceGrowthResult, PerformanceDistributionResult>, IPerformanceService
    {
        public PerformanceService(IContextService context)
            : base(context)
        {
        }

        public override Task<PerformanceDistributionResult> GetDistribution(GeneralFilterParam filterParam)
        {
            return Task.FromResult(FakeDistributionResultData());
        }

        public override Task<PerformanceGrowthResult> GetGrowth(GeneralFilterParam filterParam)
        {
            return Task.FromResult(FakeGrowthResultData());
        }

        public override Task<PerformanceTotalResult> GetTotal(GeneralFilterParam filterParam)
        {
            return Task.FromResult(FakeTotalResultData());
        }

        private static PerformanceDistributionResult FakeDistributionResultData()
        {
            var fake = new PerformanceDistributionResult()
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

        private static PerformanceGrowthResult FakeGrowthResultData()
        {
            var fake = new PerformanceGrowthResult()
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

        private static PerformanceTotalResult FakeTotalResultData()
        {
            var fake = new PerformanceTotalResult()
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
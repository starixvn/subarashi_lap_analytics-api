using Fanex.Athena.AnalyticsAPI.Common.Mappers;
using Fanex.Athena.AnalyticsAPI.Common.Resources.Lang;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Sportsbook.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.AnalyticsAPI.Sportsbook.Mappers
{
    public class SportsbookMapper : ViewModelMapper, ISportsbookMapper
    {
        private const int NUMBER_OF_TOP = 5;

        public SportsbookMapper(ICalculationService calculationService)
            : base(calculationService)
        {
        }

        public IEnumerable<DimDistributionResponse> ToTurnoverResponse(IEnumerable<SportsbookDTO> fullItems, bool includeOther,
            IReadOnlyDictionary<int, IEnumerable<bool>> supportedData = null, bool includeSabaName = false)
        {
            var topItems = supportedData == null || !supportedData.Any() ? fullItems.OrderByDescending(_ => _.Turnover).Take(NUMBER_OF_TOP)
                : fullItems.Where(x => supportedData.ContainsKey(x.Id) && supportedData[x.Id].Any(y => y == x.IsSaba));

            var sumTurnoverAll = fullItems.Sum(_ => _.Turnover);

            var result = topItems
                .Select(_ => ToDimDistributionResponse(_, sumTurnoverAll, __ => __.Turnover, includeSabaName))
                .ToList();

            if (includeOther && ToDimDistributionOtherResponse(fullItems, topItems, sumTurnoverAll, _ => _.Turnover) is DimDistributionResponse otherItem)
            {
                result.Add(otherItem);
            }

            return result.OrderByDescending(_ => _.Distribution);
        }

        public IEnumerable<DimDistributionResponse> ToWinlossResponse(IEnumerable<SportsbookDTO> fullItems, bool includeOther,
            IReadOnlyDictionary<int, IEnumerable<bool>> supportedData = null, bool includeSabaName = false)
        {
            var topItems = supportedData == null || !supportedData.Any() ? fullItems.OrderByDescending(_ => _.Winloss).Take(NUMBER_OF_TOP)
                          : fullItems.Where(x => supportedData.ContainsKey(x.Id) && supportedData[x.Id].Any(y => y == x.IsSaba));

            var sumWinlossAll = fullItems.Sum(_ => _.Winloss);

            var result = topItems
                .Select(_ => ToDimDistributionResponse(_, sumWinlossAll, __ => __.Winloss, includeSabaName))
                .ToList();

            if (includeOther && ToDimDistributionOtherResponse(fullItems, topItems, sumWinlossAll, _ => _.Winloss) is DimDistributionResponse otherItem)
            {
                result.Add(otherItem);
            }

            return result.OrderByDescending(_ => _.Distribution);
        }

        private DimDistributionResponse ToDimDistributionResponse(SportsbookDTO rawData, double sumAll, Func<SportsbookDTO, double> valueFunc, bool includeSabaName = false)
        {
            var dimValue = valueFunc.Invoke(rawData);
            return new DimDistributionResponse
            {
                Id = rawData.Id,
                Name = includeSabaName && rawData.IsSaba ? $"{ViewResource.Saba} {rawData.Name}" : rawData.Name,
                Value = dimValue,
                Distribution = _calculationService.CalculateDistribution(dimValue, sumAll),
                IsSaba = rawData.IsSaba,
                Margin = _calculationService.ToPercentUnit(rawData.Winloss / rawData.TurnoverNoDraw)
            };
        }

        private DimDistributionResponse ToDimDistributionOtherResponse(IEnumerable<SportsbookDTO> fullItems, IEnumerable<SportsbookDTO> topItems, double sumDim, Func<SportsbookDTO, double> valueFunc)
        {
            var otherItems = fullItems.Except(topItems);
            if (!otherItems.Any())
            {
                return null;
            }

            var (sumTurnoverOther, sumWinlossOther, sumTurnoverNoDrawOther) = SumRawData(otherItems);
            var otherDto = new SportsbookDTO
            {
                Id = 0,
                Name = "Others",
                Turnover = sumTurnoverOther,
                Winloss = sumWinlossOther,
                TurnoverNoDraw = sumTurnoverNoDrawOther
            };

            return ToDimDistributionResponse(otherDto, sumDim, valueFunc);
        }

        private static (double turnover, double winloss, double tunoverNoDraw) SumRawData(IEnumerable<SportsbookDTO> rawData)
        {
            var turnover = 0.00;
            var winloss = 0.00;
            var tunoverNoDraw = 0.00;

            foreach (var item in rawData)
            {
                turnover += item.Turnover;
                winloss += item.Winloss;
                tunoverNoDraw += item.TurnoverNoDraw;
            }

            return (turnover, winloss, tunoverNoDraw);
        }
    }
}
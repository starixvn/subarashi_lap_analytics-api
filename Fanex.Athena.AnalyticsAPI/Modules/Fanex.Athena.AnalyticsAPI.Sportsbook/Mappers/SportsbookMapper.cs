using Fanex.Athena.AnalyticsAPI.Common.Mappers;
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
            IReadOnlyDictionary<int, IEnumerable<bool>> onlySupportedData = null, IEnumerable<int> excludedFromTopData = null)
            => ToResponse(fullItems, _ => _.Turnover, includeOther, onlySupportedData, excludedFromTopData ?? Enumerable.Empty<int>());

        public IEnumerable<DimDistributionResponse> ToWinlossResponse(IEnumerable<SportsbookDTO> fullItems, bool includeOther,
            IReadOnlyDictionary<int, IEnumerable<bool>> onlySupportedData = null, IEnumerable<int> excludedFromTopData = null)
            => ToResponse(fullItems, _ => _.Winloss, includeOther, onlySupportedData, excludedFromTopData ?? Enumerable.Empty<int>());

        private IEnumerable<DimDistributionResponse> ToResponse(IEnumerable<SportsbookDTO> fullItems, Func<SportsbookDTO, double> dimensionValueFunc, bool includeOther,
           IReadOnlyDictionary<int, IEnumerable<bool>> onlySupportedData, IEnumerable<int> excludedFromTopData)
        {
            var topItems = onlySupportedData == null || !onlySupportedData.Any() ?
                fullItems.Where(_ => !excludedFromTopData.Contains(_.Id)).OrderByDescending(dimensionValueFunc).Take(NUMBER_OF_TOP) 
                : fullItems.Where(x => onlySupportedData.ContainsKey(x.Id) && onlySupportedData[x.Id].Any(y => y == x.IsSaba));

            if (includeOther && ToOtherDTO(fullItems, topItems) is SportsbookDTO otherDto)
            {
                topItems = topItems.Append(otherDto);
            }

            var sumDimensionAll = topItems.Sum(_ => Math.Abs(dimensionValueFunc(_)));

            var result = topItems
                .Select(_ => ToDimDistributionResponse(_, sumDimensionAll, dimensionValueFunc))
                .ToList();

            return result.OrderByDescending(_ => _.Distribution);
        }


        private DimDistributionResponse ToDimDistributionResponse(SportsbookDTO rawData, double sumAll, Func<SportsbookDTO, double> valueFunc)
        {
            var dimValue = valueFunc.Invoke(rawData);
            return new DimDistributionResponse
            {
                Id = rawData.Id,
                Name = rawData.Name,
                Value = dimValue,
                Distribution = _calculationService.CalculateDistribution(dimValue, sumAll),
                IsSaba = rawData.IsSaba,
                Margin = _calculationService.CalculateMargin(rawData.Winloss, rawData.TurnoverNoDraw)
            };
        }

        private SportsbookDTO ToOtherDTO(IEnumerable<SportsbookDTO> fullItems,  IEnumerable<SportsbookDTO> topItems)
        {
            var otherItems = fullItems.Except(topItems);
            if(!otherItems.Any())
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

            return otherDto;
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
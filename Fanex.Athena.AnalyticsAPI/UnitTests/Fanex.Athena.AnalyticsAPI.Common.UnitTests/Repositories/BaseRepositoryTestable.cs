using Fanex.Athena.AnalyticsAPI.Common.Repositories;
using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Data.Repository;

namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Repositories
{
    internal class BaseRepositoryTestable : BaseRepository
    {
        public BaseRepositoryTestable(ISqlService sqlService) : base(sqlService)
        {
        }

        protected override string ModuleId => string.Empty;
    }

    public class CriteriaTest : ICriteria
    {
        public string PropTest { get; set; }

        public string GetSettingKey()
        {
            return "CriteriaTest";
        }

        public bool IsValid()
        {
            return true;
        }
    }
}
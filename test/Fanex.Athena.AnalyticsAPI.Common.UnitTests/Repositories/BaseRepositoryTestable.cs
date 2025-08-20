namespace Fanex.Athena.AnalyticsAPI.Common.UnitTests.Repositories
{
    using Common.Repositories;
    using Common.Services;
    using Data.Repository;

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
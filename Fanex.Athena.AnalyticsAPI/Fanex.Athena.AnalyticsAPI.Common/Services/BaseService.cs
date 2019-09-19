namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public class BaseService : IBaseService
    {
        private readonly IContextService _context;

        protected BaseService(IContextService context)
        {
            _context = context;
        }
    }
}
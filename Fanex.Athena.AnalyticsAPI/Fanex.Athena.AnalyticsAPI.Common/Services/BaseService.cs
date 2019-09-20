using Fanex.Athena.AnalyticsAPI.Common.Repositories;

namespace Fanex.Athena.AnalyticsAPI.Common.Services
{
    public abstract class BaseService
    {
        protected readonly IContextService _context;

        protected readonly ISsasService _ssasService;

        protected readonly IDAXQueryRepository _daxQueryRepo;

        protected BaseService(
            IContextService context,
            ISsasService ssasService,
            IDAXQueryRepository daxQueryRepo)
        {
            _context = context;
            _ssasService = ssasService;
            _daxQueryRepo = daxQueryRepo;
        }

        protected abstract string ConnectionKey { get; }
    }
}
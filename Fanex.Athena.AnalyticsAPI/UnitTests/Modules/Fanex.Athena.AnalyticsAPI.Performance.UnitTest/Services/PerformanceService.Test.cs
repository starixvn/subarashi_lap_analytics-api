using Fanex.Athena.AnalyticsAPI.Common.Services;
using Fanex.Athena.AnalyticsAPI.Performance.Repositories;
using Fanex.Athena.AnalyticsAPI.Performance.Services;
using Moq;
using System;
using System.Reflection;
using Xunit;

namespace Fanex.Athena.AnalyticsAPI.Performance.UnitTest.Services
{
    

    public class PerformanceService_Test
    {
        private readonly PerformanceService _targetService;
        private readonly Mock<IContextService> _mockContext;
        private readonly Mock<IPerformanceRepository> _repoMock;


        public PerformanceService_Test()
        {
            _mockContext = new Mock<IContextService>();
            _repoMock = new Mock<IPerformanceRepository>();
            _targetService = new PerformanceService(_mockContext.Object, _repoMock.Object);
        }

        [Fact]
        public void GetFieldNameOfMonthGrowthObject_WhenCall_ReturnCorrectResult()
        {
            //RunInstanceMethod(_targetService);
        }

        public static object RunInstanceMethod(System.Type type, string strMethod, object objInstance, object[] aobjParams)
        {
            BindingFlags eFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
            return RunMethod(type, strMethod, objInstance, aobjParams, eFlags);
        }

        private static object RunMethod(System.Type type, string strMethod, object objInstance, object[] aobjParams, BindingFlags eFlags)
        {
            MethodInfo m;
            try
            {
                m = type.GetMethod(strMethod, eFlags);
                if (m == null)
                {
                    throw new ArgumentException("There is no method '" +
                     strMethod + "' for type '" + type.ToString() + "'.");
                }

                object objRet = m.Invoke(objInstance, aobjParams);
                return objRet;
            }
            catch
            {
                throw;
            }
        }
    }
}
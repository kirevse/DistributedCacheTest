using FluentAssertions;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DistributedCacheTest
{
    public class UnitTest1
    {
        protected IServiceProvider ServiceProvider { get; }

        protected IDistributedCache DistributedCache => ServiceProvider.GetService<IDistributedCache>();

        public UnitTest1() =>
            ServiceProvider = new ServiceCollection()
                .AddStackExchangeRedisCache(rco => rco.Configuration = "127.0.0.1:6379")
                .BuildServiceProvider();

        [Fact]
        public async Task Test1()
        {
            await DistributedCache.SetStringAsync("Test", "Value");
            (await DistributedCache.GetStringAsync("Test"))
                .Should()
                .Be("Value");
            await DistributedCache.RemoveAsync("Test");
            (await DistributedCache.GetStringAsync("Test"))
                .Should()
                .BeNull();
        }
    }
}

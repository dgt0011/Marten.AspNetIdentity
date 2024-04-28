using System.Threading.Tasks;
using Testcontainers.PostgreSql;
using Xunit;

namespace Marten.AspNetIdentity.Tests
{
    public class PostgresSqlFixture : IAsyncLifetime
    {
        private readonly PostgreSqlContainer _testContainer;
        
        public PostgresSqlFixture()
        {
            var testContainerBuilder = new PostgreSqlBuilder()
                .WithCleanUp(true)
                .WithPortBinding(5432, true)
                .WithDatabase("aspnetidentity")
                .WithPassword("aspnetidentity")
                .WithUsername("aspnetidentity")
                .WithExposedPort(5432)
                .WithImage("clkao/postgres-plv8");

            _testContainer = testContainerBuilder.Build();
        }

        public string ConnectionString => _testContainer.GetConnectionString();
        
        public async Task InitializeAsync()
        {
            await _testContainer.StartAsync();

            var result = await _testContainer.ExecAsync(new[]
            {
                "/bin/sh", "-c",
                "psql -U aspnetidentity -c \"CREATE EXTENSION plv8; SELECT extversion FROM pg_extensions WHERE extname = 'plv8';\""
            });
            var thing = string.Empty;
        }

        public async Task DisposeAsync()
        {
            await _testContainer.StopAsync();
        }
    }
}
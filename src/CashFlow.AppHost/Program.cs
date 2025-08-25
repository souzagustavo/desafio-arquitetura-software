var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", value: "admin", secret: true);
var password = builder.AddParameter("password", value: "123456", secret: true);

var cashFlowDb = builder.AddPostgres("CashFlowDb", userName: username, password: password, port: 5432)
    .AddDatabase("CashFlowDb", databaseName: "cash-flow");

var identityServerDb = builder.AddPostgres("IdentityServerDb", userName: username, password: password, port: 5432)
    .AddDatabase("IdentityServerDb", databaseName: "identity-server");

var redis = builder.AddRedis("Redis", port: 6379, password: password);

var identityserver = builder.AddProject<Projects.CashFlow_IdentifyServer_Api>("cash-flow-identifyserver-api")
    .WithReference(identityServerDb)
    .WaitFor(identityServerDb);

builder.AddProject<Projects.CashFlow_Api>("cash-flow-api")
    .WithReference(cashFlowDb)
    .WaitFor(cashFlowDb)
    .WithReference(redis)
    .WaitFor(redis);

builder.AddProject<Projects.CashFlow_Consumer_Worker>("cash-flow-consumer-worker")
    .WithReference(cashFlowDb)
    .WaitFor(cashFlowDb)
    .WithReference(redis)
    .WaitFor(redis);

builder.AddContainer("pgadmin", "dpage/pgadmin4:latest")
    .WithVolume("pgadmin_data", "/var/lib/pgadmin")
    .WithEnvironment(context =>
    {
        context.EnvironmentVariables["PGADMIN_DEFAULT_EMAIL"] = "admin@admin.com";
        context.EnvironmentVariables["PGADMIN_DEFAULT_PASSWORD"] = "123456";
    })
    .WithEndpoint(name: "pgadmin", scheme: "http", port: 8080, targetPort: 80)
    .WithReference(cashFlowDb)
    .WaitFor(cashFlowDb)
    .WithReference(identityServerDb)
    .WaitFor(identityServerDb)
    .WithHttpHealthCheck(endpointName: "pgadmin", path: "/misc/ping");

var app = builder.Build();

await app.RunAsync();
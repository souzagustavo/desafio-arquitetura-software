var builder = DistributedApplication.CreateBuilder(args);

var username = builder.AddParameter("username", value: "admin", secret: true);
var password = builder.AddParameter("password", value: "123456", secret: true);

var meuBolsoDb = builder.AddPostgres("MeuBolsoDb", userName: username, password: password, port: 5432)
    .AddDatabase("MeuBolsoDb", databaseName: "meu-bolso");

var identityServerDb = builder.AddPostgres("IdentityServerDb", userName: username, password: password, port: 5432)
    .AddDatabase("IdentityServerDb", databaseName: "identity-server");

var redis = builder.AddRedis("Redis", port: 6379, password: password);

var identityserver = builder.AddProject<Projects.MeuBolso_IdentifyServer_Api>("meu-bolso-identifyserver-api")
    .WithReference(identityServerDb)
    .WaitFor(identityServerDb);

builder.AddProject<Projects.MeuBolso_Account_Api>("meu-bolso-api")
    .WithReference(meuBolsoDb)
    .WaitFor(meuBolsoDb)
    .WithReference(redis)
    .WaitFor(redis);

builder.AddProject<Projects.MeuBolso_Account_Worker>("meu-bolso-worker")
    .WithReference(meuBolsoDb)
    .WaitFor(meuBolsoDb)
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
    .WithReference(meuBolsoDb)
    .WaitFor(meuBolsoDb)
    .WithReference(identityServerDb)
    .WaitFor(identityServerDb)
    .WithHttpHealthCheck(endpointName: "pgadmin", path: "/misc/ping");

var app = builder.Build();

await app.RunAsync();
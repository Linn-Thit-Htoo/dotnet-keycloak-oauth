namespace dotnet_keycloak_oauth.Auth.Dependencies;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddDependencies(
        this IServiceCollection services,
        WebApplicationBuilder builder
    )
    {
        builder
            .Configuration.SetBasePath(builder.Environment.ContentRootPath)
            .AddJsonFile(
                $"appsettings.{builder.Environment.EnvironmentName}.json",
                optional: false,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();

        builder
            .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false;
                opt.Audience = builder.Configuration["Authentication:Audience"];
                opt.MetadataAddress = builder.Configuration["Authentication:MetadataAddress"]!;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["Authentication:ValidIssuer"]
                };
            });
        builder.Services.AddAuthorization();

        services.AddSwaggerGen(o =>
        {
            o.CustomSchemaIds(id => id.FullName!.Replace('+', '-'));

            o.AddSecurityDefinition(
                "Keycloak",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(
                                builder.Configuration["Keycloak:AuthorizationUrl"]!
                            ),
                            TokenUrl = new Uri(
                                "http://localhost:18080/realms/dotnet-keycloak-oauth/protocol/openid-connect/token"
                            ),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "Access your openid information" },
                                { "email", "Access your email" }
                            }
                        }
                        //Implicit = new OpenApiOAuthFlow
                        //{
                        //    AuthorizationUrl = new Uri(
                        //        builder.Configuration["Keycloak:AuthorizationUrl"]!
                        //    ),
                        //    Scopes = new Dictionary<string, string>
                        //    {
                        //        { "openid", "openid" },
                        //        { "profile", "profile" }
                        //    }
                        //}
                    }
                }
            );

            var securityRequirement = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Keycloak",
                            Type = ReferenceType.SecurityScheme
                        },
                        In = ParameterLocation.Header,
                        Name = "Bearer",
                        Scheme = "Bearer"
                    },
                    []
                }
            };

            o.AddSecurityRequirement(securityRequirement);
        });

        return services;
    }
}

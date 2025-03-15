namespace dotnet_keycloak_oauth.Auth.Configurations;

public class AppSetting
{
    public Logging Logging { get; set; }
    public Keycloak Keycloak { get; set; }
    public Authentication Authentication { get; set; }
}

public class Logging
{
    public Loglevel LogLevel { get; set; }
}

public class Loglevel
{
    public string Default { get; set; }
    public string MicrosoftAspNetCore { get; set; }
}

public class Keycloak
{
    public string AuthorizationUrl { get; set; }
}

public class Authentication
{
    public string MetadataAddress { get; set; }
    public string ValidIssuer { get; set; }
    public string Audience { get; set; }
}

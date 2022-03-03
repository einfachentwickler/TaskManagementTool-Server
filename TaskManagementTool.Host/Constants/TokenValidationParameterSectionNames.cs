namespace TaskManagementTool.Host.Constants
{
    public static class TokenValidationParameterSectionNames
    {
        public const string SHOULD_VALIDATE_ISSUER = "TokenValidationParameters:ValidateIssuer";

        public const string SHOULD_VALIDATE_AUDIENCE = "TokenValidationParameters:ValidateAudience";

        public const string SHOULD_REQUIRE_EXPIRATION_TIME = "TokenValidationParameters:RequireExpirationTime";

        public const string SHOULD_VALIDATE_ISSUER_SIGNIN_KEY = "TokenValidationParameters:ValidateIssuerSigningKey";
    }
}

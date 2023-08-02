using Mapster;
using Microsoft.Data.SqlClient;
using Pokemon.Application.Authentication.v1.Commands.Register;
using Pokemon.Application.Authentication.v1.Commands.VerifyEmail;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Authentication.v1.Queries.Me;
using Pokemon.Contracts.v1.Authentication;

namespace Pokemon.Api.Common.Mappings;

public class AccountsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<(string Email, string GivenName, string UserId), MeQuery>()
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.GivenName, src => src.GivenName)
            .Map(dest => dest.UserId, src => src.UserId);

        config.NewConfig<VerifyEmailRequest, VerifyEmailCommand>();

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest, src => src.User);
    }
}
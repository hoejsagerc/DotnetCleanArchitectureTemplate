using Mapster;
using Microsoft.Data.SqlClient;
using Pokemon.Application.Authentication.v1.Commands.Logout;
using Pokemon.Application.Authentication.v1.Commands.Refresh;
using Pokemon.Application.Authentication.v1.Commands.Register;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Authentication.v1.Queries.Login;
using Pokemon.Application.Authentication.v1.Queries.Me;
using Pokemon.Contracts.v1.Authentication;

namespace Pokemon.Api.Common.Mappings;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<RegisterRequest, RegisterCommand>();

        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<string, LogoutCommand>()
            .MapWith(src => new LogoutCommand(src));

        config.NewConfig<(string UserEmail, string GivenName, string UserId), MeQuery>()
            .Map(dest => dest.Email, src => src.UserEmail)
            .Map(dest => dest.GivenName, src => src.GivenName)
            .Map(dest => dest.Id, src => src.UserId);

        config.NewConfig<(RefreshTokenRequest Request, string UserId), RefreshCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.RefreshToken, src => src.Request.RefreshToken);

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest.Id, src => src.User.Id.Value.ToString())
            .Map(dest => dest, src => src.User);
    }
}
using Mapster;
using Pokemon.Application.Authentication.v1.Commands.Logout;
using Pokemon.Application.Authentication.v1.Common;
using Pokemon.Application.Authentication.v1.Queries.Login;
using Pokemon.Contracts.v1.Authentication;

namespace Pokemon.Api.Common.Mappings;

public class LoginMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<LoginRequest, LoginQuery>();

        config.NewConfig<string, LogoutCommand>()
            .MapWith(src => new LogoutCommand(src));

        config.NewConfig<AuthenticationResult, AuthenticationResponse>()
            .Map(dest => dest, src => src.User);
    }
}
using Microsoft.AspNetCore.Components.Web;

namespace Pokemon.Client.Errors;

public class CustomErrorBoundary : ErrorBoundary
{
    protected override Task OnErrorAsync(Exception exception)
    {
        return base.OnErrorAsync(exception);
    }
}
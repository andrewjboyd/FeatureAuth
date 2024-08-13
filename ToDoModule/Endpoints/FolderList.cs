using FastEndpoints;

namespace ToDoModule.Endpoints;

internal class FolderList : EndpointWithoutRequest<FolderListResponse>
{
    public override void Configure()
    {
        Get("/Folders");
        Policy(b => b.RequireAssertion(ctx =>
        {

            var authClaim = ctx.User.FindFirst(c => c.Type.Equals("folderList", StringComparison.OrdinalIgnoreCase));
            if (authClaim is null)
            {
                return false;
            }

            if (!int.TryParse(authClaim.Value, out int endPointPermissions))
            {
                return false;
            }

            var endPointIdentifier = (int)FolderListAuth.Read;

            return (endPointPermissions & endPointIdentifier) != 0;
        }));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync(new FolderListResponse
        {
            Id = 1,
            Name = "Testing",
        }, cancellation: ct);
    }
}

namespace FeatureAuth;

internal class FeatureAuthDetails<T>
    : IFeatureAuthDetails where T : struct, Enum
{
    public FeatureAuthDetails()
    {
        ClaimType = FeatureAuthHelpers.GetClaimType<T>();
        var enumNames = Enum.GetNames<T>();
        var enumValues = Enum.GetValues<T>();

        var details = new List<EndpointDetail>();
        for (var idx = 0; idx < enumNames.Length; idx++)
        {
            details.Add(new((int)(object)enumValues[idx], enumNames[idx]));
        }
        EndpointDetails = [.. details];
    }

    public string ClaimType { get; }

    public EndpointDetail[] EndpointDetails { get; }
}

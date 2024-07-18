namespace FeatureAuth;

internal interface IFeatureAuthDetails
{
    string ClaimType { get; }
    EndpointDetail[] EndpointDetails { get; }
}

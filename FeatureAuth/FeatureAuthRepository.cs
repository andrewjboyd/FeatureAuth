namespace FeatureAuth;

internal class FeatureAuthRepository(IEnumerable<IFeatureAuthDetails> endpointDetails) : IFeatureAuthRepository
{
    private readonly IEnumerable<IFeatureAuthDetails> _endpointDetails = endpointDetails;

    public Dictionary<string, EndpointDetail[]> GetDetails()
    {
        return _endpointDetails
            .ToDictionary(
                detail => detail.ClaimType,
                detail => detail.EndpointDetails
            );
    }
}

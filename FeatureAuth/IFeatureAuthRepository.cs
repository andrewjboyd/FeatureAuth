namespace FeatureAuth;

public interface IFeatureAuthRepository
{
    Dictionary<string, EndpointDetail[]> GetDetails();
}

namespace FeatureAuth;
public interface IFeatureAuthUser<TUserId>
    where TUserId : struct, IEquatable<TUserId>
{
    TUserId UserId { get; }
}

namespace BuroFactorWS.src.security
{
    public interface IValidateCredentials
    {
        bool validateCredentials(string userName, string password);
    }
}
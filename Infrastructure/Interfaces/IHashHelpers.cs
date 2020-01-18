namespace Infrastructure.Interfaces
{
    public interface IHashHelpers
    {
        byte[] GetNewHash(out byte[] salt, string stringToBeHashed);
        bool CompareHash(string stringToBeHashed, byte[] salt, byte[] hash);
    }
}

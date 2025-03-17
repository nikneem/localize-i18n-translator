namespace Localizr.Core;

public static class Randomizer
{
    private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

    public static string GetRandomInvitationCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, 8)
            .Select(_ => chars[Random.Next(chars.Length)])
            .ToArray())
            .Insert(4, "-");
    }
}

namespace Application.Services.Common;

public struct UserIdentity
{
    public Guid Id { get; }
    public string Nik { get; }
    public IList<string> Roles { get; }

    public UserIdentity(Guid id, string nik, IList<string> roles)
    {
        Id = id;
        Nik = nik;
        Roles = roles;
    }
}
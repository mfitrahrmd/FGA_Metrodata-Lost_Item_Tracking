namespace Application.Services.Common;

public struct UserIdentity
{
    public Guid Id { get; set; }
    public string Nik { get; set; }
    public IList<string> Roles { get; set; }

    public UserIdentity(Guid id, string nik, IList<string> roles)
    {
        Id = id;
        Nik = nik;
        Roles = roles;
    }
}
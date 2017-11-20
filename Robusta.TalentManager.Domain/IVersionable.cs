namespace Robusta.TalentManager.Domain
{
    public interface IVersionable
    {
        byte[] RowVersion { get; set; }
    }
}

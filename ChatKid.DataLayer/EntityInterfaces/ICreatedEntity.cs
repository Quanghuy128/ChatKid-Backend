namespace ChatKid.DataLayer.EntityInterfaces
{
    public interface ICreatedEntity
    {
        DateTime? CreatedAt { get; set; }
        string CreatedBy { get; set; }
    }
}

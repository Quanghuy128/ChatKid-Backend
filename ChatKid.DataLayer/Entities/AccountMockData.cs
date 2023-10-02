using ChatKid.DataLayer.EntityInterfaces;

namespace ChatKid.DataLayer.Entities
{
    public class AccountMockData : IBaseEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}

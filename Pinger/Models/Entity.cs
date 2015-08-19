namespace Pinger.Models
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = -1;
        }

        public int Id { get; set; }

        public bool IsNew()
        {
            return Id == -1;
        }
    }
}

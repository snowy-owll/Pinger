namespace Pinger.Models
{
    class Setting : Entity
    {
        public Setting() { }

        public Setting(string name, string value)
        {
            Name = name; Value = value;
        }

        public string Name { get; set; }
        public string Value { get; set; }
    }
}

namespace WangerWings.Data.Model
{
    public class Destinations
    {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Budget { get; set; }
            public bool IsQuiet { get; set; }
            public bool HasBeaches { get; set; }

    }
}

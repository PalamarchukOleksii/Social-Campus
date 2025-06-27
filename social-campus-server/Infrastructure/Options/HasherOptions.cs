namespace Infrastructure.Options;

public class HasherOptions
{
    public int SaltSize { get; set; }
    public int HashSize { get; set; }
    public int Iterations { get; set; }
}
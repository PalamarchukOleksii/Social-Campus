namespace Application.Dtos
{
    public class UserOnLoginDto
    {
        public TokensDto Tokens { get; set; } = new TokensDto();
        public ShortUserDto ShortUser { get; set; } = new ShortUserDto();
    }
}

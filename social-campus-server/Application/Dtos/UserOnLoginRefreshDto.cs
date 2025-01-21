namespace Application.Dtos
{
    public class UserOnLoginRefreshDto
    {
        public TokensDto Tokens { get; set; } = new TokensDto();
        public ShortUserDto ShortUser { get; set; } = new ShortUserDto();
    }
}

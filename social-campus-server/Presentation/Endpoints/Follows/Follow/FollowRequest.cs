namespace Presentation.Endpoints.Follows.Follow
{
    public class FollowRequest
    {
        public string UserLogin { get; set; } = string.Empty;
        public string FollowUserLogin { get; set; } = string.Empty;
    }
}

namespace Snap.APIs.DTOs
{
    public class UserWithPreferencesDto
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public PreferencesDto Preferences { get; set; }
    }
}

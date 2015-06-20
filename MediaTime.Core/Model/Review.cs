namespace MediaTime.Core.Model
{
    /// <summary>
    /// User feedback
    /// </summary>
    public class Review
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Avatar { get; set; }
        public string Login { get; set; }
        public string Date { get; set; }
        public bool IsBest { get; set; }
        public string UserPage { get; set; }

        public Review() { }
        public Review(string id, string text, string avatar, string login, string date, bool isBest, string userPage)
        {
            Id = id;
            Text = text;
            Avatar = avatar;
            Login = login;
            Date = date;
            IsBest = isBest;
            UserPage = userPage;
        }
    }
}
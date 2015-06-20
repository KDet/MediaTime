using System;
using System.Collections.Generic;
using System.Text;

namespace MediaTime.Core.Model
{
    /// <summary>
    /// The base class for all media elements. 
    /// Stores information about the media in preview TOP-mode
    /// </summary>
    public class Media
    {
        //link to detail page
        public string Url { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        //image url
        public string Image { get; set; }
        
        public Media() { }
        public Media(string url, string title, string subTitle, string image)
        {
            Url = url;
            Title = title;
            SubTitle = subTitle;
            Image = image;
        }

        public static bool Equals(Media first, Media second)
        {
            if(ReferenceEquals(first, second))
                return true;
            if(ReferenceEquals(first, null) || ReferenceEquals(second, null))
                return false;
            return string.Equals(first.Title, second.Title) && string.Equals(first.SubTitle, second.SubTitle) &&
                   string.Equals(first.Image, second.Image) && string.Equals(first.Url, second.Url);
        }
    }

    /// <summary>
    ///Information about new files
    /// </summary>
    public class UpdatedMedia : Media
    {
        public string Date { get; set; }
        public string Time { get; set; }

        public UpdatedMedia() { }
        public UpdatedMedia(string url, string title, string subTitle, string image, string date, string time) : base(url, title, subTitle, image)
        {
            Date = date;
            Time = time;
        }
    }

    public abstract class UserLikedMedia : Media
    {
        public int Likes { get; set; }
        public int Dislikes { get; set; }

        protected UserLikedMedia() { }
        protected UserLikedMedia(string url, string title, string subTitle, string image, int likes, int dislikes)
            : base(url, title, subTitle, image)
        {
            Likes = likes;
            Dislikes = dislikes;
        }
    }

    public abstract class MediaMarked : UserLikedMedia
    {
        private IList<string> _qualities = new List<string>();
        private IList<string> _infoField = new List<string>(); 
        public IList<string> Qualities
        {
            get { return _qualities; }
            set { _qualities = value; }
        }
        public IList<string> InfoFields
        {
            get { return _infoField; }
            set { _infoField = value; }
        } 
        public string QualitiesCombination
        {
            get
            {
                if (Qualities.Count > 0)
                {
                    var markersBuilder = new StringBuilder();
                    for (int i = 0; i < Qualities.Count - 1; i++)
                        markersBuilder.Append(Qualities[i] + " ");
                    markersBuilder.Append(Qualities[Qualities.Count - 1]);
                    return markersBuilder.ToString();
                }
                return string.Empty;
            }
        }
        // public string Date { get; set; }

        protected MediaMarked() { }
        protected MediaMarked(string url, string title, string subTitle, string image, int likes, int dislikes)
            : base(url, title, subTitle, image, likes, dislikes)
        {
        }
    }

    /// <summary>
    /// Detailed information about media
    /// </summary>
    public class MediaDetailed : MediaMarked
    {
        public string Description { get; set; }

        public MediaDetailed() { }
        public MediaDetailed(string url, string title, string subTitle, string image, int likes, int dislikes, string description) : base(url, title, subTitle, image, likes, dislikes)
        {
            Description = description;
        }
    }

    /// <summary>
    /// Media information in the list-mode (default)
    /// </summary>
    public class MediaListed : MediaMarked
    {
        public string Status { get; set; }

        public MediaListed() { }
        public MediaListed(string url, string title, string subTitle, string image, int likes, int dislikes, string status) : base(url, title, subTitle, image, likes, dislikes)
        {
            Status = status;
        }
    }

    /// <summary>
    /// Media display information after searching
    /// </summary>
    public sealed class SearchMedia : MediaDetailed
    {
        public string Section { get; set; }
        public string Genre { get; set; }
        //number of likes
        public int Rates { get; set; }
        //link to reviews (name - number of responses, url - links to commentary)
        public int Comments { get; set; }

        public SearchMedia() { }
        public SearchMedia(string url, string title, string subTitle, string image, int likes, int dislikes, string description, string genre, string section, int rates, int comments)
            : base(url, title, subTitle, image, likes, dislikes, description)
        {
            Genre = genre;
            Section = section;
            Rates = rates;
            Comments = comments;
        }
    }

    /// <summary>
    /// Media information in expanded form
    /// </summary>
    public sealed class RetrievedMedia : Media
    {
        public Dictionary<string, string> InfoTable { get; set; } 
        public string Description { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public string[] Screenshots { get; set; }
        public Storage[] FileList { get; set; }
        public Review[] Reviews { get; set; }
        public Media[] Similar { get; set; }

        public RetrievedMedia() { }
        public RetrievedMedia(string url, string title, string subTitle, string image, Dictionary<string, string> infoTable, string description, int likes, int dislikes, string[] screenshots, Storage[] fileList, Review[] reviews, Media[] similar): base(url, title, subTitle, image)
        {
            InfoTable = infoTable;
            Description = description;
            Likes = likes;
            Dislikes = dislikes;
            Screenshots = screenshots;
            FileList = fileList;
            Reviews = reviews;
            Similar = similar;
        }
    }

    /// <summary>
    /// Structure to display text information from reference
    /// </summary>
    public struct Hyperlink
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public Hyperlink(string name, string url)
            : this()
        {
            Name = name;
            Url = url;
        }
        public bool Equals(Hyperlink other)
        {
            return string.Equals(Url, other.Url) && string.Equals(Name, other.Name);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Hyperlink && Equals((Hyperlink)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Url != null ? Url.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
        public override string ToString()
        {
            return string.Format("Name: {0}, Url: {1}", Name, Url);
        }
    }
}
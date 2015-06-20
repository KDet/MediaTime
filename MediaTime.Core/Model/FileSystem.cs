namespace MediaTime.Core.Model
{
    /// <summary>
    /// An abstract class for all classes related to file system
    /// </summary>
    public abstract class Storage
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Reference { get; set; }
        public string Size { get; set; }
    }

    /// <summary>
    /// The class represents a folder of brb.to service
    /// </summary>
    public class Folder : Storage
    {
        //link to a text document with information about all the content packs (links to the files in it)
        public string FileListRef { get; set; }
        //video quality (high, medium)
        public string Quality { get; set; }
        public MarkerSet Marker { get; set; }
        //number of files in a folder 
        public string FileCountInfo { get; set; }
        public string PublicationDate { get; set; }
        public Storage[] Content { get; set; }
    }

    /// <summary>
    /// The class represents a file of brb.to service
    /// </summary>
    public class File : Storage
    {
        //Video and music has two links (download and play).
        //this is playing
        public string PlayReference { get; set; }
    }

    /// <summary>
    ///The structure of the set of markers directory <see cref="Storage"/>
    /// </summary>
    public struct MarkerSet
    {
        //folder Marker - quality
        public string Folder { get; set; }
        //title marker - flag
        public string Title { get; set; }
        public MarkerSet(string folder, string title)
            : this()
        {
            Folder = folder;
            Title = title;
        }
        public bool Equals(MarkerSet other)
        {
            return string.Equals(Title, other.Title) && string.Equals(Folder, other.Folder);
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is MarkerSet && Equals((MarkerSet)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                return ((Title != null ? Title.GetHashCode() : 0) * 397) ^ (Folder != null ? Folder.GetHashCode() : 0);
            }
        }
        public override string ToString()
        {
            return string.Format("Title: {0}, Folder: {1}", Title, Folder);
        }
    }
}
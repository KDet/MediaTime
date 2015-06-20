using Cirrious.MvvmCross.Community.Plugins.Sqlite;
using MediaTime.Core.Model;

namespace MediaTime.Core.Repositories
{
    public class FavoriteMedia : Media
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public override string ToString()
        {
            return Title;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Community.Plugins.Sqlite;

namespace MediaTime.Core.Model
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

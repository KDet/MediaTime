using System.Linq;
using MediaTime.Core.Model;

namespace MediaTime.Core.Services
{
    public class Categories
    {
        internal static System.Collections.Generic.Dictionary<string, CategoryList> CategoryStorage = new System.Collections.Generic.Dictionary<string, CategoryList>();

        public static bool LoadDictionaryResource(System.Collections.Generic.IDictionary<object, object> dictionaryResource)
        {
            return dictionaryResource != null &&
                   dictionaryResource.All(
                       dictionary =>
                           LoadDictionary(dictionary.Key.ToString(), dictionary.Value as CategoryList));
        }

        public static bool LoadDictionary(string typeKey, CategoryList categoryList)
        {
            if (CategoryStorage.ContainsKey(typeKey)) return false;
            CategoryStorage[typeKey] = categoryList;
            return true;
        }

        public static Category FindByName(string name)
        {
            return
                CategoryStorage.SelectMany(pair => pair.Value.Where(category => category.Name == name)).FirstOrDefault();
        }
    }
}

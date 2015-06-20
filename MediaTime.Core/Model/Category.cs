using System.Collections.Generic;
using MediaTime.Core.Services;

namespace MediaTime.Core.Model
{
    public class CategoryList : List<Category> { } 

    public class Category
    {
        public string Title { get { return Translator.Current.TranslateQuick(Name); } }
        public string ImageSource { get; set; }
        public string Color { get; set; }
        public bool HasSubCategories { get; set; }
        public string Name { get; set; }

        public Category() { }
        public Category(string imageSource, string color, bool hasSubCategories, string name)
        {
            ImageSource = imageSource;
            Color = color;
            HasSubCategories = hasSubCategories;
            Name = name;
        }
    }
}
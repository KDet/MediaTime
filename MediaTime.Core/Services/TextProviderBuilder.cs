using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.Plugins.JsonLocalisation;

namespace MediaTime.Core.Services
{
    public class TextProviderBuilder
        : MvxTextProviderBuilder
    {
        public TextProviderBuilder()
            : base(Constants.GeneralNamespace, Constants.RootFolderForResources)
        {
        }

        protected override IDictionary<string, string> ResourceFiles
        {
            get
            {
                var dictionary = GetType().GetTypeInfo().Assembly
                    .CreatableTypes()
                    .Where(t => t.Name.EndsWith("ViewModel"))
                    .ToDictionary(t => t.Name, t => t.Name);
                //var dictionary = new Dictionary<string, string>()
                //{
                //    {"HomeViewModel", "HomeViewModel"}
                //};

                return dictionary;
            }
        }
        
    }
}
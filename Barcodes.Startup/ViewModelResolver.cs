using Barcodes.Core;
using Barcodes.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barcodes.Startup
{
    public class ViewModelResolver
    {
        public Type Resolve(Type viewType)
        {
            var viewModelsAssembly = typeof(CoreModule).Assembly;
            var baseName = viewType.Name.Replace("View", string.Empty)
                .Replace("Window", string.Empty);
            var viewModelName = baseName + "ViewModel";

            foreach (var type in viewModelsAssembly.GetTypes())
            {
                if (type.Name == viewModelName)
                {
                    return viewModelsAssembly.GetType(type.FullName, true);
                }
            }

            throw new Exception($"Can not find {viewModelName} in {viewModelsAssembly.GetName().FullName}");
        }
    }
}

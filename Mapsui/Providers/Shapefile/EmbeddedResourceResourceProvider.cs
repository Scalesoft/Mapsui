using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Mapsui.Utilities;

namespace Mapsui.Providers.Shapefile
{
    public class EmbeddedResourceResourceProvider : IResourceProvider
    {
        private readonly string _shapeFileResourceName;
        private readonly string[] _resourceNames;
        private readonly Assembly _assembly;

        private readonly Dictionary<DataType, string> _resourceNameDataTypeDict = new Dictionary<DataType, string>();

        public EmbeddedResourceResourceProvider(string shapeFileResourceName)
        {
            _shapeFileResourceName = shapeFileResourceName;
            _resourceNameDataTypeDict.Add(DataType.Shape, shapeFileResourceName);
            _resourceNameDataTypeDict.Add(DataType.Index, ReplaceLast(shapeFileResourceName, $".{FileExtensions.Shape}",$".{FileExtensions.Index}"));
            _resourceNameDataTypeDict.Add(DataType.DB, ReplaceLast(shapeFileResourceName, $".{FileExtensions.Shape}",$".{FileExtensions.DB}"));
            _resourceNameDataTypeDict.Add(DataType.SpatialIndex, ReplaceLast(shapeFileResourceName, $".{FileExtensions.Shape}",$".{FileExtensions.SpatialIndex}"));
            _resourceNameDataTypeDict.Add(DataType.Projection, ReplaceLast(shapeFileResourceName, $".{FileExtensions.Shape}",$".{FileExtensions.Projection}"));

            _assembly = Assembly.GetExecutingAssembly();
            _resourceNames = _assembly.GetManifestResourceNames();
        }

        public Stream OpenStream(DataType dataType)
        {
            return GetEmbeddedResourceStream(dataType);
        }

        public bool ResourceExists(DataType dataType)
        {
            return _resourceNames.Contains(_resourceNameDataTypeDict[dataType]);
        }

        public string GetMainResourceName()
        {
            return _shapeFileResourceName;
        }

        private Stream GetEmbeddedResourceStream(DataType dataType)
        {
            var resourceName = _resourceNameDataTypeDict[dataType];

            return _resourceNames.Contains(resourceName)
                ? _assembly.GetManifestResourceStream(resourceName)
                : null;
        }

        private static string ReplaceLast(string source, string find, string replace)
        {
            var index = source.LastIndexOf(find, StringComparison.Ordinal);

            return index == -1 ? source : source.Remove(index, find.Length).Insert(index, replace);
        }
    }
}

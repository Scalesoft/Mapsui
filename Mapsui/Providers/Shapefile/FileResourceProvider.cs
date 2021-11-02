using System.Collections.Generic;
using System.IO;

namespace Mapsui.Providers.Shapefile
{
    public class FileResourceProvider : IResourceProvider
    {
        private readonly string _shapeFilePath;
        private readonly Dictionary<DataType, string> _fileNameDataTypeDict = new Dictionary<DataType, string>();

        public FileResourceProvider(string shapeFilePath)
        {
            _shapeFilePath = shapeFilePath;
            _fileNameDataTypeDict.Add(DataType.Shape, shapeFilePath);
            _fileNameDataTypeDict.Add(DataType.Index, Path.ChangeExtension(shapeFilePath, FileExtensions.Index));
            _fileNameDataTypeDict.Add(DataType.DB, Path.ChangeExtension(shapeFilePath, FileExtensions.DB));
            _fileNameDataTypeDict.Add(DataType.SpatialIndex, Path.ChangeExtension(shapeFilePath, FileExtensions.SpatialIndex));
            _fileNameDataTypeDict.Add(DataType.Projection, Path.ChangeExtension(shapeFilePath, FileExtensions.Projection));
        }

        public Stream OpenStream(DataType dataType)
        {
            return new FileStream(_fileNameDataTypeDict[dataType], FileMode.Open, FileAccess.Read);
        }

        public bool ResourceExists(DataType dataType)
        {
            return File.Exists(_fileNameDataTypeDict[dataType]);
        }

        public string GetMainResourceName()
        {
            return _shapeFilePath;
        }
    }
}

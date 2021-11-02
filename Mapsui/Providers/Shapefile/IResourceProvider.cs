using System;
using System.IO;

namespace Mapsui.Providers.Shapefile
{
    public interface IResourceProvider
    {
        Stream OpenStream(DataType dataType);

        bool ResourceExists(DataType dataType);

        string GetMainResourceName();
    }

    public enum DataType
    {
        Shape,
        Index,
        DB,
        SpatialIndex,
        Projection,
    }
}

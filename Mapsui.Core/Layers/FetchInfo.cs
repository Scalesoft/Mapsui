﻿namespace Mapsui.Layers
{
    public class FetchInfo
    {
        public FetchInfo() { }

        public FetchInfo(FetchInfo fetchInfo)
        {
            Extent = fetchInfo.Extent != null ? new MRect(fetchInfo.Extent) : null;
            Resolution = fetchInfo.Resolution;
            CRS = fetchInfo.CRS;
            ChangeType = fetchInfo.ChangeType;
        }

        public MRect? Extent { get; set; }
        public double Resolution { get; set; }
        public string? CRS { get; set; }
        public ChangeType ChangeType { get; set; }
    }
}

﻿using System.Linq;
using System.Net.Http;
using BruTile.Wmts;
using Mapsui.Layers;
using Mapsui.UI;

namespace Mapsui.Samples.Common.Maps
{
    public class WmtsMichelinSample : ISample
    {
        public string Name => "5 WMTS Michelin";
        public string Category => "Data";

        public void Setup(IMapControl mapControl)
        {
            mapControl.Map = CreateMap();
        }

        public static Map CreateMap()
        {
            var map = new Map();
            map.Layers.Add(CreateLayer());
            return map;
        }

        public static ILayer CreateLayer()
        {
            using var httpClient = new HttpClient();
            // When testing today (20-10-2021) tile 0,0,0 returned a 500. Perhaps this should be fixed in the xml.
            using var response = httpClient.GetStreamAsync("https://bertt.github.io/wmts/capabilities/michelin.xml").Result;
            var tileSource = WmtsParser.Parse(response).First();
            return new TileLayer(tileSource) { Name = tileSource.Name };
        }
    }
}
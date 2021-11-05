﻿using System.IO;
using BruTile.Cache;
using BruTile.Predefined;
using Mapsui.Extensions;
using Mapsui.Layers;
using Mapsui.Rendering;
using NUnit.Framework;

namespace Mapsui.Tests.Rendering
{
    [TestFixture]
    public class RenderFetchStrategyTests
    {
        [Test]
        public void GetFeaturesWithPartOfOptimalResolutionTilesMissing()
        {
            // arrange
            var schema = new GlobalSphericalMercator();
            var box = schema.Extent.ToBoundingBox();
            const int level = 3;
            var resolution = schema.Resolutions[level];
            var memoryCache = PopulateMemoryCache(schema, new MemoryCache<RasterFeature>(), level);
            var renderFetchStrategy = new RenderFetchStrategy();

            // act
            var tiles = renderFetchStrategy.Get(box, resolution.UnitsPerPixel, schema, memoryCache);

            // assert
            Assert.True(tiles.Count == 43);
        }

        private static ITileCache<RasterFeature> PopulateMemoryCache(GlobalSphericalMercator schema, MemoryCache<RasterFeature> cache, int levelId)
        {
            for (var i = levelId; i >= 0; i--)
            {
                var tiles = schema.GetTileInfos(schema.Extent, i);
                foreach (var tile in tiles)
                {
                    if ((tile.Index.Col + tile.Index.Row) % 2 == 0) // Add only 50% of the tiles with the arbitrary rule.
                    {
                        cache.Add(tile.Index, new RasterFeature { Raster = new MRaster(new MemoryStream(), new MRect(0, 0, 1, 1)) });
                    }
                }
            }
            return cache;
        }
    }
}

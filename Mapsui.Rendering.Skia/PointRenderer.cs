﻿using System;
using Mapsui.Extensions;
using Mapsui.Geometries;
using Mapsui.Providers;
using Mapsui.Styles;
using SkiaSharp;

namespace Mapsui.Rendering.Skia
{
    internal static class PointRenderer
    {
        public static void Draw(SKCanvas canvas, IReadOnlyViewport viewport, IStyle style, IGeometryFeature feature,
            IGeometry geometry, SymbolCache symbolCache, float opacity)
        {
            var point = (Point)geometry;
            var destination = viewport.WorldToScreen(point.X, point.Y).ToPoint();

            if (style is CalloutStyle calloutStyle)
            {
                CalloutStyleRenderer.Draw(canvas, viewport, opacity, destination, calloutStyle);
            }
            else if (style is LabelStyle labelStyle)
            {
                LabelRenderer.Draw(canvas, labelStyle, feature, destination, opacity);
            }
            else if (style is SymbolStyle symbolStyle)
            {
                if (symbolStyle.BitmapId >= 0)
                {
                    // todo: Remove this call. ImageStyle should be used instead of SymbolStyle with BitmapId
                    ImageStyleRenderer.Draw(canvas, symbolStyle, destination, symbolCache, opacity, viewport.Rotation);
                }
                else
                {
                    SymbolStyleRenderer.Draw(canvas, symbolStyle, destination, opacity, symbolStyle.SymbolType, viewport.Rotation);
                }
            }
            else if (style is ImageStyle imageStyle)
            {
                ImageStyleRenderer.Draw(canvas, imageStyle, destination, symbolCache, opacity, viewport.Rotation);
            }
            else if (style is VectorStyle vectorStyle)
            {
                // Use the SymbolStyleRenderer and specify Ellipse
                SymbolStyleRenderer.Draw(canvas, vectorStyle, destination, opacity, SymbolType.Ellipse);
            }
            else
            {
                throw new Exception($"Style of type '{style.GetType()}' is not supported for points");
            }
        }
    }
}
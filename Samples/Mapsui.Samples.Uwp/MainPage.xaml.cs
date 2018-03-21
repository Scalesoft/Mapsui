﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Mapsui.Samples.Common.Maps;
using Mapsui.UI;
using Mapsui.Utilities;

namespace Mapsui.Samples.Uwp
{
    // ReSharper disable once RedundantExtendsListEntry
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            DeployMbTilesFile();
            MbTilesSample.MbTilesLocation = MbTilesLocationOnUwp;

            MapControl.Map.Layers.Add(OpenStreetMap.CreateTileLayer());
            MapControl.RotationLock = true;
            MapControl.UnSnapRotationDegrees = 30;
            MapControl.ReSnapRotationDegrees = 5;

            FillComboBoxWithDemoSamples();

            SampleSet.SelectionChanged += SampleSet_SelectionChanged;
        }

        private void MapOnInfo(object sender, MapInfoEventArgs args)
        {
            if (args.MapInfo.Feature != null)
                FeatureInfo.Text = $"Click Info:{Environment.NewLine}{args.MapInfo.Feature.ToDisplayText()}";
        }

        private void FillComboBoxWithDemoSamples()
        {
            SampleList.Children.Clear();
            foreach (var sample in DemoSamples().ToList())
                SampleList.Children.Add(CreateRadioButton(sample));
        }

        private void FillComboBoxWithTestSamples()
        {
            SampleList.Children.Clear();
            foreach (var sample in TestSamples().ToList())
                SampleList.Children.Add(CreateRadioButton(sample));
        }

        private Dictionary<string, Func<Map>> TestSamples()
        {
            var result = new Dictionary<string, Func<Map>>();
            var i = 0;
            foreach (var sample in Tests.Common.AllSamples.CreateList())
            {
                result[i.ToString()] = sample;
                i++;
            }
            return result;
        }

        private static Dictionary<string, Func<Map>> DemoSamples()
        {
            return Common.AllSamples.CreateList();
        }

        private void SampleSet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBoxItem = (ComboBoxItem)((ComboBox)sender).SelectedItem;
            if (comboBoxItem?.Content != null)
            {
                var selectedValue = comboBoxItem.Content.ToString();

                if (selectedValue == "Demo samples")
                {
                    FillComboBoxWithDemoSamples();
                }
                else if (selectedValue == "Test samples")
                {
                    FillComboBoxWithTestSamples();
                }
                else
                {
                    throw new Exception("Unknown ComboBox item");
                }
            }
        }

        private UIElement CreateRadioButton(KeyValuePair<string, Func<Map>> sample)
        {
            var radioButton = new RadioButton
            {
                FontSize = 16,
                Content = sample.Key,
                Margin = new Thickness(4)
            };

            radioButton.Click += (s, a) =>
            {
                MapControl.Map.Layers.Clear();
                MapControl.Map = sample.Value();
                MapControl.Map.Info += MapOnInfo;
                MapControl.Refresh();
            };

            return radioButton;
        }

        private void DeployMbTilesFile()
        {
            var path = "Mapsui.Samples.Common.EmbeddedResources.world.mbtiles";
            var assembly = typeof(PointsSample).GetTypeInfo().Assembly;
            using (var image = assembly.GetManifestResourceStream(path))
            {
                if (image == null) throw new ArgumentException("EmbeddedResource not found");
                using (var dest = File.Create(MbTilesLocationOnUwp))
                {
                    image.CopyTo(dest);
                }
            }
        }

        private static string MbTilesLocationOnUwp
        {
            get
            {
                var folder = ApplicationData.Current.LocalFolder.Path;
                var path = Path.Combine(folder, "world.mbtiles");
                return path;
            }
        }

    }
}

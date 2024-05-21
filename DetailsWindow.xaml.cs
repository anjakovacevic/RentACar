﻿using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    public partial class DetailsWindow : Window
    {
        public DetailsWindow(string details)
        {
            InitializeComponent();
            PopulateDetails(details);
        }

        private void PopulateDetails(string details)
        {
            var detailLines = details.Split(new[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in detailLines)
            {
                var parts = line.Split(new[] { ':' }, 2);
                if (parts.Length == 2)
                {
                    var row = new Grid();
                    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) });
                    row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                    var label = new TextBlock
                    {
                        Text = parts[0] + ":",
                        Style = (Style)Resources["DetailLabelStyle"]
                    };
                    Grid.SetColumn(label, 0);

                    var value = new TextBlock
                    {
                        Text = parts[1].Trim(),
                        Style = (Style)Resources["DetailValueStyle"]
                    };
                    Grid.SetColumn(value, 1);

                    row.Children.Add(label);
                    row.Children.Add(value);
                    DetailsStackPanel.Children.Add(row);
                }
            }
        }
    }
}

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Projekat
{
    public partial class DetailsWindow : Window
    {
        public DetailsWindow(string details)
        {
            InitializeComponent();
            try
            {
                PopulateDetails(details);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while populating details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PopulateDetails(string details)
        {
            var detailLines = details.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
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

                    if (parts[0].Trim() == "Google Maps URL")
                    {
                        var link = new Hyperlink
                        {
                            NavigateUri = new Uri(parts[1].Trim()),
                            Inlines = { parts[1].Trim() }
                        };
                        link.RequestNavigate += (s, e) =>
                        {
                            Process.Start(new ProcessStartInfo
                            {
                                FileName = e.Uri.AbsoluteUri,
                                UseShellExecute = true
                            });
                        };

                        var value = new TextBlock
                        {
                            Style = (Style)Resources["DetailValueStyle"]
                        };
                        value.Inlines.Add(link);
                        Grid.SetColumn(value, 1);
                        row.Children.Add(label);
                        row.Children.Add(value);
                    }
                    else
                    {
                        var value = new TextBlock
                        {
                            Text = parts[1].Trim(),
                            Style = (Style)Resources["DetailValueStyle"]
                        };
                        Grid.SetColumn(value, 1);
                        row.Children.Add(label);
                        row.Children.Add(value);
                    }

                    DetailsStackPanel.Children.Add(row);
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

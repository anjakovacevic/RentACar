using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Cars selectedCar;
        private Locations selectedLocation;
        private int selectedTab;

        public Cars SelectedCar
        {
            get => selectedCar;
            set
            {
                selectedCar = value;
            }
        }

        public Locations SelectedLocation
        {
            get => selectedLocation;
            set
            {
                selectedLocation = value;
            }
        }

        public int SelectedTab
        {
            get => selectedTab;
            set
            {
                selectedTab = value;
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Load data for Cars
            RentACarContext.Instance.Cars.Load();
            dataGridCars.ItemsSource = RentACarContext.Instance.Cars.Local;

            // Load data for Locations
            RentACarContext.Instance.Locations.Load();
            dataGridLocations.ItemsSource = RentACarContext.Instance.Locations.Local;

            this.DataContext = this;
        }
        /*
        private void DetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            string details = SelectedTab == 0 ? SelectedCar?.ToString() ?? "No car selected." : SelectedLocation?.ToString() ?? "No location selected.";
            DetailsWindow detailsWindow = new DetailsWindow(details);
           // detailsWindow.ShowDialog();
        }
        */
        private void DetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTab == 0)
            {
                string details = SelectedCar?.ToString() ?? "No car selected.";
                DetailsWindow detailsWindow = new DetailsWindow(details);
                detailsWindow.ShowDialog();
            }
            else if (SelectedTab == 1)
            {
                string details = SelectedLocation?.ToString() ?? "No location selected.";
                DetailsWindow detailsWindow = new DetailsWindow(details);
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("No valid tab selected.");
            }
        }



        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult value = MessageBox.Show("Are you sure?", "Deleting Entry", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (value == MessageBoxResult.Yes)
            {
                if (SelectedTab == 0 && SelectedCar != null) // Cars tab
                {
                    RentACarContext.Instance.Cars.Remove(SelectedCar);
                }
                else if (SelectedTab == 1 && SelectedLocation != null) // Locations tab
                {
                    RentACarContext.Instance.Locations.Remove(SelectedLocation);
                }

                try
                {
                    RentACarContext.Instance.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    
                }
            }
        }

        private void AddCarBtn_Click(object sender, RoutedEventArgs e)
        {
            AddWindow addWindow = new AddWindow(null);
            addWindow.ShowDialog();
            dataGridCars.ItemsSource = RentACarContext.Instance.Cars.Local;

            try
            {
                RentACarContext.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the new car: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddLocationBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLocationWindow addLocationWindow = new AddLocationWindow(null);
            addLocationWindow.ShowDialog();
            dataGridLocations.ItemsSource = RentACarContext.Instance.Locations.Local;

            try
            {
                RentACarContext.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the new location: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedTab == 0 && SelectedCar != null) // Cars tab
            {
                AddWindow updateWindow = new AddWindow(SelectedCar);
                updateWindow.ShowDialog();
                dataGridCars.ItemsSource = RentACarContext.Instance.Cars.Local;
            }
            else if (SelectedTab == 1 && SelectedLocation != null) // Locations tab
            {
                AddLocationWindow updateWindow = new AddLocationWindow(SelectedLocation);
                updateWindow.ShowDialog();
                dataGridLocations.ItemsSource = RentACarContext.Instance.Locations.Local;
            }

            try
            {
                RentACarContext.Instance.SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenMapButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var url = button.Tag as string;
                if (!string.IsNullOrEmpty(url))
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    });
                }
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Selected tab: {SelectedTab}");
        }
    }
}

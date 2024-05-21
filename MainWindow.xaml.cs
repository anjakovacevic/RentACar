using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                // Notify property changed if implementing INotifyPropertyChanged (not shown here)
            }
        }

        public Locations SelectedLocation
        {
            get => selectedLocation;
            set
            {
                selectedLocation = value;
                // Notify property changed if implementing INotifyPropertyChanged (not shown here)
            }
        }

        public int SelectedTab
        {
            get => selectedTab;
            set
            {
                selectedTab = value;
                // Notify property changed if implementing INotifyPropertyChanged (not shown here)
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Load data for Cars
            RentACarContext.Instance.Cars.Load();
            dataGridCars.ItemsSource = RentACarContext.Instance.Cars.Local;
            //this.DataContext = RentACarContext.Instance;

            // Load data for Locations
            RentACarContext.Instance.Locations.Load();
            dataGridLocations.ItemsSource = RentACarContext.Instance.Locations.Local;

            //this.DataContext = RentACarContext.Instance;
            this.DataContext = this;
        }

        //private void DetailsBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (SelectedTab == 0) // Cars tab
        //        MessageBox.Show(SelectedCar?.ToString() ?? "No car selected.");
        //    else if (SelectedTab == 1) // Locations tab
        //        MessageBox.Show(SelectedLocation?.ToString() ?? "No location selected.");
        //}
        private void DetailsBtn_Click(object sender, RoutedEventArgs e)
        {
            string details = SelectedTab == 0 ? SelectedCar?.ToString() ?? "No car selected." : SelectedLocation?.ToString() ?? "No location selected.";
            DetailsWindow detailsWindow = new DetailsWindow(details);
            detailsWindow.ShowDialog();
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
                    MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Optionally log the error or perform additional actions
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
                //MessageBox.Show("Car added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the new car: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Optionally log the error or perform additional actions
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
                // MessageBox.Show("Location added successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the new location: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Optionally log the error or perform additional actions
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
                MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Optionally log the error or perform additional actions
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Selected tab: {SelectedTab}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Validation;
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
using System.Windows.Shapes;

namespace Projekat
{
    public partial class AddWindow : Window
    {
        private Cars NewCar = new Cars();

        public AddWindow(Cars carInfo)
        {
            InitializeComponent();
            this.DataContext = NewCar;

            if (carInfo != null)
            {
                NewCar.VehicleID = carInfo.VehicleID;
                NewCar.LocationID = carInfo.LocationID;
                NewCar.LicensePlate = carInfo.LicensePlate;
                NewCar.Model = carInfo.Model;
                NewCar.Year = carInfo.Year;
                NewCar.RentalState = carInfo.RentalState;
                NewCar.RentalStatus = carInfo.RentalStatus;
                rentalStateCmb.SelectedItem = rentalStateCmb.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == NewCar.RentalState.ToString());
                rentalStatusCmb.SelectedItem = rentalStatusCmb.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == NewCar.RentalStatus.ToString());
                NewCar.Mileage = carInfo.Mileage;
                NewCar.LastServiceDate = carInfo.LastServiceDate;

                this.Title = "Update Car";
                vehicleIDTxt.IsReadOnly = true;
            }
            else
            {
                this.Title = "Add Car";
                vehicleIDTxt.IsReadOnly = false;
            }

            LoadLocations();
        }

        private void LoadLocations()
        {
            locationIDCmb.ItemsSource = RentACarContext.Instance.Locations.ToList();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput(out string message))
            {
                try
                {
                    if (vehicleIDTxt.IsReadOnly)
                    {
                        Cars updateCar = (from car in RentACarContext.Instance.Cars
                                          where car.VehicleID == NewCar.VehicleID
                                          select car).FirstOrDefault();
                        if (updateCar != null)
                        {
                            updateCar.LocationID = locationIDCmb.SelectedValue.ToString();
                            updateCar.LicensePlate = NewCar.LicensePlate;
                            updateCar.Model = NewCar.Model;
                            updateCar.Year = NewCar.Year;
                            updateCar.RentalState = NewCar.RentalState;
                            updateCar.RentalStatus = NewCar.RentalStatus;
                            updateCar.Mileage = NewCar.Mileage;
                            updateCar.LastServiceDate = NewCar.LastServiceDate;
                        }
                    }
                    else
                    {
                        if (RentACarContext.Instance.Cars.Any(c => c.VehicleID == NewCar.VehicleID))
                        {
                            MessageBox.Show("A car with this VehicleID already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        NewCar.LocationID = locationIDCmb.SelectedValue.ToString();
                        RentACarContext.Instance.Cars.Add(NewCar);
                    }

                    RentACarContext.Instance.SaveChanges();
                    this.Close();
                }
                catch (DbEntityValidationException dbEx)
                {
                    var errorMessages = dbEx.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(dbEx.Message, " The validation errors are: ", fullErrorMessage);

                    MessageBox.Show(exceptionMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    var innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        ex = innerException;
                        innerException = ex.InnerException;
                    }

                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult value = MessageBox.Show("Are you sure?", "Cancel Operation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (value == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private bool ValidateInput(out string errorMessage)
        {
            bool retVal = true;
            var errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(vehicleIDTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter Vehicle ID!");
                vehicleIDTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                vehicleIDTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(licensePlateTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter License Plate!");
                licensePlateTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                licensePlateTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(modelTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter Model!");
                modelTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                modelTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(yearTxt.Text) || !int.TryParse(yearTxt.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
            {
                retVal = false;
                errors.AppendLine("Enter a valid Year (between 1900 and the current year)!");
                yearTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                yearTxt.ClearValue(BorderBrushProperty);
            }

            if (rentalStateCmb.SelectedValue == null)
            {
                retVal = false;
                errors.AppendLine("Select Rental State!");
                rentalStateCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                rentalStateCmb.ClearValue(BorderBrushProperty);
            }

            if (rentalStatusCmb.SelectedValue == null)
            {
                retVal = false;
                errors.AppendLine("Select Rental Status!");
                rentalStatusCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                rentalStatusCmb.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(mileageTxt.Text) || !int.TryParse(mileageTxt.Text, out int mileage) || mileage < 0)
            {
                retVal = false;
                errors.AppendLine("Enter a valid Mileage (non-negative number)!");
                mileageTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                mileageTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(lastServiceDateTxt.Text) || !DateTime.TryParse(lastServiceDateTxt.Text, out DateTime serviceDate))
            {
                retVal = false;
                errors.AppendLine("Enter a valid Last Service Date (format: MM/DD/YYYY)!");
                lastServiceDateTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                lastServiceDateTxt.ClearValue(BorderBrushProperty);
            }

            if (locationIDCmb.SelectedItem == null || locationIDCmb.SelectedValue.ToString().Length > 50)
            {
                retVal = false;
                errors.AppendLine("Select a valid Location ID! Ensure it does not exceed the maximum length.");
                locationIDCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                locationIDCmb.ClearValue(BorderBrushProperty);
            }

            errorMessage = errors.ToString();
            return retVal;
        }
    }
}

/*
namespace Projekat
{
    public partial class AddWindow : Window
    {
        private Cars NewCar = new Cars();

        public AddWindow(Cars carInfo)
        {
            InitializeComponent();
            this.DataContext = NewCar;

            //if (carInfo != null)
            //{
            //    NewCar.VehicleID = carInfo.VehicleID;
            //    NewCar.LocationID = carInfo.LocationID;
            //    NewCar.LicensePlate = carInfo.LicensePlate;
            //    NewCar.Model = carInfo.Model;
            //    NewCar.Year = carInfo.Year;
            //    NewCar.RentalState = carInfo.RentalState;
            //    NewCar.RentalStatus = carInfo.RentalStatus;
            //    rentalStateCmb.SelectedItem = rentalStateCmb.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == NewCar.RentalState.ToString());
            //    rentalStatusCmb.SelectedItem = rentalStatusCmb.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == NewCar.RentalStatus.ToString());

            //    NewCar.Mileage = carInfo.Mileage;
            //    NewCar.LastServiceDate = carInfo.LastServiceDate;

            //    this.Title = "Update Car";
            //    vehicleIDTxt.IsReadOnly = true;
            //}
            //else
            //{
            //    this.Title = "Add Car";
            //    vehicleIDTxt.IsReadOnly = false;
            //}
            if (carInfo != null)
            {
                NewCar.VehicleID = carInfo.VehicleID;
                NewCar.LocationID = carInfo.LocationID;
                NewCar.LicensePlate = carInfo.LicensePlate;
                NewCar.Model = carInfo.Model;
                NewCar.Year = carInfo.Year;
                NewCar.RentalState = carInfo.RentalState;
                NewCar.RentalStatus = carInfo.RentalStatus;
                rentalStateCmb.SelectedItem = rentalStateCmb.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == NewCar.RentalState.ToString());
                rentalStatusCmb.SelectedItem = rentalStatusCmb.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == NewCar.RentalStatus.ToString());
                NewCar.Mileage = carInfo.Mileage;
                NewCar.LastServiceDate = carInfo.LastServiceDate;

                this.Title = "Update Car";
                vehicleIDTxt.IsReadOnly = true;
            }
            else
            {
                this.Title = "Add Car";
                vehicleIDTxt.IsReadOnly = false;
            }

            LoadLocations();
        }

        private void LoadLocations()
        {
            locationIDCmb.ItemsSource = RentACarContext.Instance.Locations.ToList();
        }
        /*
        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput(out string message))
            {
                if (vehicleIDTxt.IsReadOnly)
                {
                    Cars updateCar = (from car in RentACarContext.Instance.Cars
                                      where car.VehicleID == NewCar.VehicleID
                                      select car).FirstOrDefault();
                    if (updateCar != null)
                    {
                        updateCar.LocationID = NewCar.LocationID;
                        updateCar.LicensePlate = NewCar.LicensePlate;
                        updateCar.Model = NewCar.Model;
                        updateCar.Year = NewCar.Year;
                        updateCar.RentalState = NewCar.RentalState;
                        updateCar.RentalStatus = NewCar.RentalStatus;
                        updateCar.Mileage = NewCar.Mileage;
                        updateCar.LastServiceDate = NewCar.LastServiceDate;
                    }
                }
                else
                {
                    if (RentACarContext.Instance.Cars.Any(c => c.VehicleID == NewCar.VehicleID))
                    {
                        MessageBox.Show("A car with this VehicleID already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    RentACarContext.Instance.Cars.Add(NewCar);
                }

                try
                {
                    RentACarContext.Instance.SaveChanges();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput(out string message))
            {
                try
                {
                    if (vehicleIDTxt.IsReadOnly)
                    {
                        Cars updateCar = (from car in RentACarContext.Instance.Cars
                                          where car.VehicleID == NewCar.VehicleID
                                          select car).FirstOrDefault();
                        if (updateCar != null)
                        {
                            updateCar.LocationID = NewCar.LocationID;
                            updateCar.LicensePlate = NewCar.LicensePlate;
                            updateCar.Model = NewCar.Model;
                            updateCar.Year = NewCar.Year;
                            updateCar.RentalState = NewCar.RentalState;
                            updateCar.RentalStatus = NewCar.RentalStatus;
                            updateCar.Mileage = NewCar.Mileage;
                            updateCar.LastServiceDate = NewCar.LastServiceDate;
                        }
                    }
                    else
                    {
                        // Check for primary key duplicates
                        if (RentACarContext.Instance.Cars.Any(c => c.VehicleID == NewCar.VehicleID))
                        {
                            MessageBox.Show("A car with this VehicleID already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        RentACarContext.Instance.Cars.Add(NewCar);
                    }

                    RentACarContext.Instance.SaveChanges();
                    this.Close();
                }
                catch (DbEntityValidationException dbEx)
                {
                    var errorMessages = dbEx.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(dbEx.Message, " The validation errors are: ", fullErrorMessage);

                    MessageBox.Show(exceptionMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    // Get the full stack trace of the inner exception
                    var innerException = ex.InnerException;
                    while (innerException != null)
                    {
                        ex = innerException;
                        innerException = ex.InnerException;
                    }

                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}\n\nStack Trace:\n{ex.StackTrace}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show(message, "Invalid input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult value = MessageBox.Show("Are you sure?", "Cancel Operation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (value == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
        private bool ValidateInput(out string errorMessage)
        {
            bool retVal = true;
            var errors = new StringBuilder();

            // Validate Vehicle ID
            if (String.IsNullOrWhiteSpace(vehicleIDTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter Vehicle ID!");
                vehicleIDTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                vehicleIDTxt.ClearValue(BorderBrushProperty);
            }

            // Validate License Plate
            if (String.IsNullOrWhiteSpace(licensePlateTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter License Plate!");
                licensePlateTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                licensePlateTxt.ClearValue(BorderBrushProperty);
            }

            // Validate Model
            if (String.IsNullOrWhiteSpace(modelTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter Model!");
                modelTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                modelTxt.ClearValue(BorderBrushProperty);
            }

            // Validate Year
            if (String.IsNullOrWhiteSpace(yearTxt.Text) || !int.TryParse(yearTxt.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
            {
                retVal = false;
                errors.AppendLine("Enter a valid Year (between 1900 and the current year)!");
                yearTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                yearTxt.ClearValue(BorderBrushProperty);
            }

            // Validate Rental State
            if (rentalStateCmb.SelectedValue == null)
            {
                retVal = false;
                errors.AppendLine("Select Rental State!");
                rentalStateCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                rentalStateCmb.ClearValue(BorderBrushProperty);
            }

            // Validate Rental Status
            if (rentalStatusCmb.SelectedValue == null)
            {
                retVal = false;
                errors.AppendLine("Select Rental Status!");
                rentalStatusCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                rentalStatusCmb.ClearValue(BorderBrushProperty);
            }

            // Validate Mileage
            if (String.IsNullOrWhiteSpace(mileageTxt.Text) || !int.TryParse(mileageTxt.Text, out int mileage) || mileage < 0)
            {
                retVal = false;
                errors.AppendLine("Enter a valid Mileage (non-negative number)!");
                mileageTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                mileageTxt.ClearValue(BorderBrushProperty);
            }

            // Validate Last Service Date
            if (String.IsNullOrWhiteSpace(lastServiceDateTxt.Text) || !DateTime.TryParse(lastServiceDateTxt.Text, out DateTime serviceDate))
            {
                retVal = false;
                errors.AppendLine("Enter a valid Last Service Date (format: MM/DD/YYYY)!");
                lastServiceDateTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                lastServiceDateTxt.ClearValue(BorderBrushProperty);
            }

            // Validate Location ID
            if (locationIDCmb.SelectedItem == null || locationIDCmb.SelectedValue.ToString().Length > 50)
            {
                retVal = false;
                errors.AppendLine("Select a valid Location ID! Ensure it does not exceed the maximum length.");
                locationIDCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                locationIDCmb.ClearValue(BorderBrushProperty);
            }

            errorMessage = errors.ToString();
            return retVal;
        }


        /*
        private bool ValidateInput(out string errorMessage)
        {
            bool retVal = true;
            var errors = new StringBuilder();

            if (String.IsNullOrWhiteSpace(vehicleIDTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter Vehicle ID!");
                vehicleIDTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                vehicleIDTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(licensePlateTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter License Plate!");
                licensePlateTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                licensePlateTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(modelTxt.Text))
            {
                retVal = false;
                errors.AppendLine("Enter Model!");
                modelTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                modelTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(yearTxt.Text) || !int.TryParse(yearTxt.Text, out int year) || year < 1900 || year > DateTime.Now.Year)
            {
                retVal = false;
                errors.AppendLine("Enter a valid Year (between 1900 and the current year)!");
                yearTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                yearTxt.ClearValue(BorderBrushProperty);
            }

            if (rentalStateCmb.SelectedItem == null)
            {
                retVal = false;
                errors.AppendLine("Select Rental State!");
                rentalStateCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                try
                {
                    NewCar.RentalState = (Cars.RentalStateEnum)Enum.Parse(typeof(Cars.RentalStateEnum), ((ComboBoxItem)rentalStateCmb.SelectedItem).Content.ToString());
                    rentalStateCmb.ClearValue(BorderBrushProperty);
                }
                catch (Exception ex)
                {
                    retVal = false;
                    errors.AppendLine($"Error parsing Rental State: {ex.Message}");
                    rentalStateCmb.BorderBrush = Brushes.Red;
                }
            }

            if (rentalStatusCmb.SelectedItem == null)
            {
                retVal = false;
                errors.AppendLine("Select Rental Status!");
                rentalStatusCmb.BorderBrush = Brushes.Red;
            }
            else
            {
                try
                {
                    NewCar.RentalStatus = (Cars.RentalStatusEnum)Enum.Parse(typeof(Cars.RentalStatusEnum), ((ComboBoxItem)rentalStatusCmb.SelectedItem).Content.ToString());
                    rentalStatusCmb.ClearValue(BorderBrushProperty);
                }
                catch (Exception ex)
                {
                    retVal = false;
                    errors.AppendLine($"Error parsing Rental Status: {ex.Message}");
                    rentalStatusCmb.BorderBrush = Brushes.Red;
                }
            }

            if (String.IsNullOrWhiteSpace(mileageTxt.Text) || !int.TryParse(mileageTxt.Text, out int mileage) || mileage < 0)
            {
                retVal = false;
                errors.AppendLine("Enter a valid Mileage (non-negative number)!");
                mileageTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                mileageTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(lastServiceDateTxt.Text) || !DateTime.TryParse(lastServiceDateTxt.Text, out DateTime serviceDate))
            {
                retVal = false;
                errors.AppendLine("Enter a valid Last Service Date (format: MM/DD/YYYY)!");
                lastServiceDateTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                lastServiceDateTxt.ClearValue(BorderBrushProperty);
            }

            errorMessage = errors.ToString();
            return retVal;
        }
        
    }
}
*/
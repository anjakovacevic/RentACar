using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Projekat
{
    public partial class AddLocationWindow : Window
    {
        private Locations NewLocation = new Locations();

        public AddLocationWindow(Locations locationInfo)
        {
            InitializeComponent();
            this.DataContext = NewLocation;

            if (locationInfo != null)
            {
                NewLocation.LocationID = locationInfo.LocationID;
                NewLocation.Address = locationInfo.Address;
                NewLocation.Manager = locationInfo.Manager;
                NewLocation.Capacity = locationInfo.Capacity;
                NewLocation.OpeningHours = locationInfo.OpeningHours;
                NewLocation.PhoneNumber = locationInfo.PhoneNumber;
                NewLocation.Email = locationInfo.Email;
                NewLocation.StaffCount = locationInfo.StaffCount;

                this.Title = "Update Location";
                locationIDTxt.IsReadOnly = true;
            }
            else
            {
                this.Title = "Add Location";
                locationIDTxt.IsReadOnly = false;
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput(out string message))
            {
                if (locationIDTxt.IsReadOnly)
                {
                    Locations updateLocation = (from location in RentACarContext.Instance.Locations
                                                where location.LocationID == NewLocation.LocationID
                                                select location).FirstOrDefault();
                    if (updateLocation != null)
                    {
                        updateLocation.Address = NewLocation.Address;
                        updateLocation.Manager = NewLocation.Manager;
                        updateLocation.Capacity = NewLocation.Capacity;
                        updateLocation.OpeningHours = NewLocation.OpeningHours;
                        updateLocation.PhoneNumber = NewLocation.PhoneNumber;
                        updateLocation.Email = NewLocation.Email;
                        updateLocation.StaffCount = NewLocation.StaffCount;
                    }
                }
                else
                {
                    // Check for primary key duplicates
                    if (RentACarContext.Instance.Locations.Any(l => l.LocationID == NewLocation.LocationID))
                    {
                        MessageBox.Show("A location with this LocationID already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    RentACarContext.Instance.Locations.Add(NewLocation);
                }

                try
                {
                    RentACarContext.Instance.SaveChanges();
                    MessageBox.Show("Changes saved successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    // Optionally log the error or perform additional actions
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
            errorMessage = "";

            if (String.IsNullOrWhiteSpace(locationIDTxt.Text))
            {
                retVal = false;
                errorMessage += "Enter Location ID!\n";
                locationIDTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                locationIDTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(addressTxt.Text))
            {
                retVal = false;
                errorMessage += "Enter Address!\n";
                addressTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                addressTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(managerTxt.Text))
            {
                retVal = false;
                errorMessage += "Enter Manager!\n";
                managerTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                managerTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(capacityTxt.Text) || !int.TryParse(capacityTxt.Text, out int capacity))
            {
                retVal = false;
                errorMessage += "Enter a valid Capacity!\n";
                capacityTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                capacityTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(openingHoursTxt.Text) || !Locations.IsValidOpeningHours(openingHoursTxt.Text))
            {
                retVal = false;
                errorMessage += "Enter valid Opening Hours in the format HH:mm - HH:mm!\n";
                openingHoursTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                openingHoursTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(phoneNumberTxt.Text))
            {
                retVal = false;
                errorMessage += "Enter Phone Number!\n";
                phoneNumberTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                phoneNumberTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(emailTxt.Text))
            {
                retVal = false;
                errorMessage += "Enter Email!\n";
                emailTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                emailTxt.ClearValue(BorderBrushProperty);
            }

            if (String.IsNullOrWhiteSpace(staffCountTxt.Text) || !int.TryParse(staffCountTxt.Text, out int staffCount))
            {
                retVal = false;
                errorMessage += "Enter a valid Staff Count!\n";
                staffCountTxt.BorderBrush = Brushes.Red;
            }
            else
            {
                staffCountTxt.ClearValue(BorderBrushProperty);
            }

            return retVal;
        }
    }
}

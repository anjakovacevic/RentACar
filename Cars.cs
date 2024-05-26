using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Projekat
{
    public class Cars : INotifyPropertyChanged
    {
        public enum RentalStateEnum
        {
            Available,
            Rented,
            Maintenance,
            Reserved
        }

        public enum RentalStatusEnum
        {
            Sedan,
            SUV,
            Hatchback,
            Luxury
        }

        #region Fields
        private string vehicleID;
        private string locationID;
        private string licensePlate;
        private string model;
        private int year;
        private RentalStateEnum rentalState;
        private RentalStatusEnum rentalStatus;
        private int mileage;
        private DateTime? lastServiceDate;
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        [Key]
        [Required]
        public string VehicleID
        {
            get { return vehicleID; }
            set
            {
                vehicleID = value;
                OnPropertyChanged(nameof(VehicleID));
            }
        }

        [Required]
        [ForeignKey("Location")]
        public string LocationID
        {
            get { return locationID; }
            set
            {
                locationID = value;
                OnPropertyChanged(nameof(LocationID));
            }
        }

        [Required]
        public string LicensePlate
        {
            get { return licensePlate; }
            set
            {
                licensePlate = value;
                OnPropertyChanged(nameof(LicensePlate));
            }
        }

        public string Model
        {
            get { return model; }
            set
            {
                model = value;
                OnPropertyChanged(nameof(Model));
            }
        }

        [Range(1900, 2100)]
        public int Year
        {
            get { return year; }
            set
            {
                year = value;
                OnPropertyChanged(nameof(Year));
            }
        }

        public RentalStateEnum RentalState
        {
            get { return rentalState; }
            set
            {
                rentalState = value;
                OnPropertyChanged("RentalState");
            }
        }

        public RentalStatusEnum RentalStatus
        {
            get { return rentalStatus; }
            set
            {
                rentalStatus = value;
                OnPropertyChanged("RentalStatus");
            }
        }

        [Range(0, int.MaxValue)]
        public int Mileage
        {
            get { return mileage; }
            set
            {
                mileage = value;
                OnPropertyChanged(nameof(Mileage));
            }
        }

        public DateTime? LastServiceDate
        {
            get { return lastServiceDate; }
            set
            {
                lastServiceDate = value;
                OnPropertyChanged(nameof(LastServiceDate));
            }
        }
        #endregion

        #region Methods
        public Cars() { }
        public virtual Locations Location { get; set; }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Vehicle ID: {VehicleID}");
            sb.AppendLine($"Location ID: {LocationID}");
            sb.AppendLine($"License Plate: {LicensePlate}");
            sb.AppendLine($"Model: {Model}");
            sb.AppendLine($"Year: {Year}");
            sb.AppendLine($"Rental State: {RentalState}");
            sb.AppendLine($"Rental Status: {RentalStatus}");
            sb.AppendLine($"Mileage: {Mileage}");
            sb.AppendLine($"Last Service Date: {LastServiceDate}");
            return sb.ToString();
        }

        public string GetModelAndLicensePlate()
        {
            var sb = new StringBuilder();
            sb.AppendLine($" - {Model}, License Plate: {LicensePlate}");
            // sb.AppendLine($"License Plate: {LicensePlate}");
            return sb.ToString();
        }
        #endregion
    }
}

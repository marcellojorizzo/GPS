// Filename: GPS.cs
// Author: Marcello Jorizzo
// Creation Date: 24.01.2023
// Last Modified: 09.06.2024
// Description: This program allows users to quickly calculate in between two GPS points in a given step range


//DataBindingHandler
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestGPS.MVVM.Model;
using TestGPS.MVVM.View;
using TestGPS.MVVM.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;

namespace TestGPS.MVVM.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {

        private double longitude;
        private double latitude;
        private double step;
        const double defaultValue = 0.000000;
        const double defaultStep = 0.7;
        private double _distance;

        private GPSList _gpsList;
        public GPSList GPSList
        {
            get 
            { 
                if (_gpsList == null)
                {
                    _gpsList = new GPSList();
                }
                return _gpsList; }
            set
            {
                _gpsList = value;
                OnPropertyChanged();
            }
        }

        private GPSList _GPSListComplete;

        public GPSList GPSListComplete
        {
            get 
            {
                if (_GPSListComplete == null)
                {
                    _GPSListComplete = new GPSList();
                }
                return _GPSListComplete; }
           
            set 
            { 
                _GPSListComplete = value;
                OnPropertyChanged("GPSListComplete");
            }
        }


        public double Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude != value)
                {
                    longitude = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Latitude
        {
            get { return latitude; }
            set
            {
                if (latitude != value)
                {
                    latitude = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Step
        {
            get { return step; }
            set
            {
                if (step != value)
                {
                    step = value;
                    OnPropertyChanged();
                }
            }
        }


        private string _gpsListAsString;
        public string GPSListAsString
        {
            get { return _gpsListAsString; }
            set
            {
                _gpsListAsString = value;
                OnPropertyChanged();
            }
        }



        private GPS _gps = new GPS();
        public GPS Gps
        {
            get { return _gps; }
            set { _gps = value; OnPropertyChanged(); }
        }

        public ICommand AddGPSCommand { get;  set; }
        public ICommand GetGPSCommand { get; set; }
        public ICommand WriteToFileCommand { get; }
        public ICommand ClearListCommand { get; set; }


        public MainWindowViewModel()
        {
            AddGPSCommand = new RelayCommand(AddGPS);
            GetGPSCommand = new RelayCommand(GetGPS);
            WriteToFileCommand = new RelayCommand(WriteToFile);
            ClearListCommand = new RelayCommand(ClearList);
           
            
            Longitude = defaultValue;
            Latitude = defaultValue;
            Step = defaultStep;
        }

        private void AddGPS()
        {
            double longitudeValue;
            double latitudeValue;
            double StepValue;

            if (double.TryParse(Longitude.ToString(), out longitudeValue) &&
                double.TryParse(Latitude.ToString(), out latitudeValue) &&
                double.TryParse(Step.ToString(), out StepValue))
            {
                
                // Übergabewerte / Parameterwerte überprüfen !!!! 
                GPS gps = new GPS(latitudeValue, longitudeValue, StepValue);

                GPSList.Add(gps);


                // Textfelder zurück auf default Values
                Longitude = defaultValue;
                Latitude = defaultValue;
                Step = StepValue;
            }
        }


        private void GetGPS()
        {
            
            this.GPSListComplete=GPSMovement.getNewPoints(GPSList);
        }


        public void WriteToFile() 
{
            GPSMovement.WriteGpxFile(this.GPSListComplete);
            GPSMovement.WriteListToFile(this.GPSListComplete);
        }

        private void ClearList() {
              
            this.GPSListComplete = null;
            this.GPSList = null;
        }

        //PropertyChanged kann von anderen Klassen abonniert werden, um auf Änderungen von Eigenschaften zu reagieren.
        public event PropertyChangedEventHandler PropertyChanged;

        // teilt anderen Klassen mit, dass eine Eigenschaft des ViewModels aktualisiert wurde und dass sie den neuen Wert abrufen können.
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

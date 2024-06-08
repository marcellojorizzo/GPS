// Filename: GPS.cs
// Author: Marcello Jorizzo
// Creation Date: 24.01.2023
// Last Modified: 09.06.2024
// Description: This program allows users to quickly calculate additional points in between two GPS points in a given step range; defaultStep is 0.7 meter

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestGPS.MVVM.Model;

namespace TestGPS.Data
{
    public class DBH : INotifyPropertyChanged

    {
        private ObservableCollection<GPS> gpsList;

        public DBH()
        {

            gpsList = new ObservableCollection<GPS>();

        }

        public ObservableCollection<GPS> GetGPSData()
        {
            return gpsList;
        }

        public void SaveGPSData(GPS gps)
        {
            //  GPS-Datensatz einfügen
            gpsList.Add(gps);
        }

        public void DeleteGPSData(GPS gps)
        {
            //  GPS-Datensatz entfernen
            gpsList.Remove(gps);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
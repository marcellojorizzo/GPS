// Filename: GPS.cs
// Author: Marcello Jorizzo
// Creation Date: 24.01.2023
// Last Modified: 09.06.2024
// Description: This program allows users to quickly calculate in between two GPS points in a given step range


using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using TestGPS;
using TestGPS.Data;
using TestGPS.MVVM;
using TestGPS.MVVM.Model;
using TestGPS.MVVM.ViewModel;
namespace TestGPS.MVVM.View
{

    public partial class MainWindow : Window
    {
        // Default Konstruktor

       public  MainWindow()
        {

            InitializeComponent();
        }



        private MainWindowViewModel _mainWindowViewModel;
        public MainWindowViewModel mainWindowViewModel {

            get 
            { 
                if (_mainWindowViewModel == null)

                { 
                    _mainWindowViewModel = new MainWindowViewModel(); 
                }

                return _mainWindowViewModel;
            }

        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}

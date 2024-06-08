// Filename: GPS.cs
// Author: Marcello Jorizzo
// Creation Date: 24.01.2023
// Last Modified: 09.06.2024
// Description: This program allows users to quickly calculate additional points in between two GPS points in a given step range; defaultStep is 0.7 meter

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestGPS.MVVM.Model;
using TestGPS.MVVM.ViewModel;
using TestGPS.MVVM.View; 

namespace TestGPS.MVVM.Model
{
    public class GPS
    {

        // Vaiablen von GPS
        private double latitude;
        private double longitude;

        // Konstanten von GPS
        const double e_radius = 6371000;
 

        // Properties von GPS
        public double Latitude
        {
            get
            {
                // retrun this.latitude;
                return Math.Round(this.latitude, 6);
            }

            set
            {
                this.latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                // return this.longitude;
                return Math.Round(this.longitude, 6);
            }

            set
            {
                this.longitude = value;
            }
        }
     
        public double Step { get; set; }


        // Kostruktoren
        public GPS() { }

        public GPS(double lat, double lon, double step)
        {
            // Dezimalstelle ist 'Punk' nicht 'Komma'!!!!
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Latitude = lat;
            Longitude = lon;
            Step = step;
        }


        // Funktion zur Berechnung der Entfernung zwischen zwei GPS-Koordinaten 
        // Übergabe Parameter Latitude und Longitude der GPS-Punkte 
        public static double HaversineDistance(double lat1, double lon1, double lat2, double lon2)
        {

            var lat1Rad = lat1 * Math.PI / 180;
            var lat2Rad = lat2 * Math.PI / 180;
            var deltaLat = (lat2 - lat1) * Math.PI / 180;
            var deltaLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return e_radius * c;
        }

        // Funktion zur Berechnung der Entfernung zwischen zwei GPS-Koordinaten
        public static double HaversineDistance(GPS point_1, GPS point_2)
        {

            double lat1 = point_1.Latitude;
            double lon1 = point_1.Longitude;
            double lat2 = point_2.Latitude;
            double lon2 = point_2.Longitude;

            var lat1Rad = lat1 * Math.PI / 180;
            var lat2Rad = lat2 * Math.PI / 180;
            var deltaLat = (lat2 - lat1) * Math.PI / 180;
            var deltaLon = (lon2 - lon1) * Math.PI / 180;

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return e_radius * c;
        }

        // Funktion zur Berechnung des Peilungswinkels von A nach B
        // Übergabe Parameter Latitude und Longitude der GPS-Punkte 
        public static double HaversineBearing(double lat1, double lon1, double lat2, double lon2)
        {
            var lat1Rad = lat1 * Math.PI / 180;
            var lat2Rad = lat2 * Math.PI / 180;
            var deltaLon = (lon2 - lon1) * Math.PI / 180;

            var y = Math.Sin(deltaLon) * Math.Cos(lat2Rad);
            var x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) -
                    Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLon);

            var bearingRad = Math.Atan2(y, x);
            var bearing = bearingRad * 180 / Math.PI;

            return bearing;
        }


        // Funktion zur Berechnung des Peilungswinkels von A nach B
        public static double HaversineBearing(GPS point_1, GPS point_2)
        {

            double lat1 = point_1.Latitude;
            double lon1 = point_1.Longitude;
            double lat2 = point_2.Latitude;
            double lon2 = point_2.Longitude;

            var lat1Rad = lat1 * Math.PI / 180;
            var lat2Rad = lat2 * Math.PI / 180;
            var deltaLon = (lon2 - lon1) * Math.PI / 180;

            var y = Math.Sin(deltaLon) * Math.Cos(lat2Rad);
            var x = Math.Cos(lat1Rad) * Math.Sin(lat2Rad) -
                    Math.Sin(lat1Rad) * Math.Cos(lat2Rad) * Math.Cos(deltaLon);

            var bearingRad = Math.Atan2(y, x);
            var bearing = bearingRad * 180 / Math.PI;

            return bearing;
        }

        // Funktion zur Berechnung der neuen GPS-Koordinaten ausgehend von einem Startpunkt und einer Distanz in einer bestimmten Richtung
        // Übergabe Parameter Latitude und Longitude von Punkt A
        public static (double, double) HaversineDestination(double lat1, double lon1, double bearing, double step)
        {

            var lat1Rad = lat1 * Math.PI / 180;
            var lon1Rad = lon1 * Math.PI / 180;
            var bearingRad = bearing * Math.PI / 180;

            var lat2Rad = Math.Asin(Math.Sin(lat1Rad) * Math.Cos(step / e_radius) +
                                    Math.Cos(lat1Rad) * Math.Sin(step / e_radius) * Math.Cos(bearingRad));
            var lon2Rad = lon1Rad + Math.Atan2(Math.Sin(bearingRad) * Math.Sin(step / e_radius) * Math.Cos(lat1Rad),
                                               Math.Cos(step / e_radius) - Math.Sin(lat1Rad) * Math.Sin(lat2Rad));

            var lat2 = lat2Rad * 180 / Math.PI;
            var lon2 = lon2Rad * 180 / Math.PI;

            double step2 = step;

            return (lat2, lon2);
        }

        // Funktion zur Berechnung der neuen GPS-Koordinaten ausgehend von einem Startpunkt und einer Distanz in einer bestimmten Richtung
        public static GPS HaversineDestination(GPS point_1, double bearing)
        {

            double lat1 = point_1.Latitude;
            double lon1 = point_1.Longitude;
            double step = point_1.Step;

            var lat1Rad = lat1 * Math.PI / 180;
            var lon1Rad = lon1 * Math.PI / 180;
            var bearingRad = bearing * Math.PI / 180;

            var lat2Rad = Math.Asin(Math.Sin(lat1Rad) * Math.Cos(step / e_radius) +
                                    Math.Cos(lat1Rad) * Math.Sin(step / e_radius) * Math.Cos(bearingRad));
            var lon2Rad = lon1Rad + Math.Atan2(Math.Sin(bearingRad) * Math.Sin(step / e_radius) * Math.Cos(lat1Rad),
                                               Math.Cos(step / e_radius) - Math.Sin(lat1Rad) * Math.Sin(lat2Rad));

            var lat2 = lat2Rad * 180 / Math.PI;
            var lon2 = lon2Rad * 180 / Math.PI;

            GPS point_a = new GPS();
            point_a.Latitude = lat2;
            point_a.Longitude = lon2;
            point_a.Step = step;

            return point_a;
        }


        // Umrechung der Koodinaten in Radiaten
        public static double deg2rad(double deg)
        {
            return deg * Math.PI / 180;
        }

        // Umrechung der Radianten in Koordinaten
        public static double rad2deg(double rad)
        {
            return rad * 180 / Math.PI;
        }

    }
    // ObservableCollection von GPS - Punkten
    public class GPSList : ObservableCollection<GPS>
    {
        public GPSList()
        {
        }
    }

    public  static class GPSMovement 
    {
    
        // Diese Methode berechnet neue GPS Koordinaten 
        public static GPSList getNewPoints(GPSList wayPoints)
        {
          

            // lege Liste mit fuer neue Waypoints an 
            GPSList n_wayPoints = new GPSList();

            for (int i = 1; i < wayPoints.Count; i++)
            {

                int j = i - 1;
                GPS point_A = wayPoints[j];
                GPS point_B = wayPoints[i];


                // Berechnen der Entfernung zwischen Punkt A und Punkt B
                // double distance = GPSMath.HaversineDistance(latA, lonA, latB, lonB);
                double distance = GPS.HaversineDistance(point_A, point_B);
                //#use for debugging!
                //Console.WriteLine($"Die Entfernung zwischen Punkt A und Punkt B beträgt {distance:F6} m");


                double step = point_A.Step;
                while (distance >= step)

                {
                    //Punkt A in ObservableCollection spreichern
                    if (n_wayPoints.Count == 0)
                        n_wayPoints.Add(point_A);

                    // Berechnen der Richtung von A nach B
                    //double bearing = GPSMath.HaversineBearing(latA, lonA, latB, lonB);
                    double bearing = GPS.HaversineBearing(point_A, point_B);

                    //Berchung des neuen point_A; Bewegung von Punkt A in Richtung von Punkt B um step
                    //point_A = GPSMath.HaversineDestination(point_A, bearing, step);
                    point_A = GPS.HaversineDestination(point_A, bearing);

                    // Ausgabe der neuen Koordinaten von Punkt A
                    //#use for debugging!
                    //Console.WriteLine($"Neue Koordinaten von Punkt A: lat = {point_A.Latitude:F6}, lon = {point_A.Longitude:F6} step ={point_A.Step}");

                    // neuen Punkt in GPS Liste einsetzten
                    GPS newPoint = new GPS(point_A.Latitude, point_A.Longitude, n_wayPoints[0].Step);
                    n_wayPoints.Add(newPoint);

                    // Neuberechnung der Entfernung zwischen Punkt A und Punkt B
                    //distance = GPSMath.HaversineDistance(latA, lonA, latB, lonB);
                    distance = GPS.HaversineDistance(point_A, point_B);

                    //#use for debugging!
                    //Console.WriteLine($"Die neue Entfernung zwischen Punkt A und Punkt B beträgt {distance:F6} m");
                }// end while Loop
            } //end for Loop [i]
            return n_wayPoints;
        }
      
        // Schreibt die GPS Koordinaten aus der ObservableList in eine .txt Datei im Format (Longitude), (Latitude) mit je sechs Dezimalstellen [0.000000]
        // In einem zweidimensionalen Koordinatensystem repräsentiert die x-Achse  die Longitude und die y-Achse die Latitude
        public static void WriteListToFile(GPSList n_wayPoints)
        {

            // Dateipfad  für  .txt TestFile output: 
            string testPutFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "_test.txt");

            // StreamWriter öffnen:
            StreamWriter k_writer = new StreamWriter(testPutFile);

            // Informationen für das txt File:
            string userName = Environment.UserName;
            k_writer.WriteLine($"creator: {userName}");
            

            foreach (GPS value in n_wayPoints)
            {
                double k_lon = value.Longitude;
                double k_lat = value.Latitude;
                double k_step = value.Step;
                k_writer.WriteLine($"{k_lat:F6}, {k_lon:F6}");

            }
            k_writer.Close();
        }

        // Schreibe die GPS Koorinaten der neuen Punkte in ein .gpx-File [ GPS Exchange Format ]
        public static void WriteGpxFile(GPSList n_wayPoints)
        {
            // Dateipfad  für .gpx TestFile output: 
            string gpxFileOut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "_test.gpx");

            // StreamWriter öffnen:
            StreamWriter pgxWriter = new StreamWriter(gpxFileOut);

            // Informationen für das gpx File:
           string userName = Environment.UserName;
            pgxWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"no\" ?>");
            pgxWriter.WriteLine ($"<gpx version=\"1.1\" creator=\"{userName}\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://www.topografix.com/GPX/1/1\" xsi:schemaLocation=\"http://www.topografix.com/GPX/1/1 http://www.topografix.com/GPX/1/1/gpx.xsd\">");
            

            // Zählvariable für die Kennzeichung der Koodrianatenpunkte definieren
            int count = 1;

            foreach (GPS point in n_wayPoints)
            {
                // lat und lon aus GPS Objekt auslesen
                double wp_lon = point.Longitude;
                double wp_lat = point.Latitude;

                pgxWriter.WriteLine($"<wpt lat=\"{wp_lat}\" lon =\"{wp_lon}\">");
                pgxWriter.WriteLine($"<name>WP{count}</name>");
                pgxWriter.WriteLine("</wpt>");
                count++;
            }
            pgxWriter.Close();
        }

    }
}





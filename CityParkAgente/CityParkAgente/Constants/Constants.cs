﻿using System.Collections.Generic;
using TK.CustomMap.Overlays;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace CityParkAgente.Constants
{
    public class Constants
    {
        public static List<Position> CuencaCollection = new List<Position>
        {

            new Position(-2.908594,-79.039565),
            new Position(-2.909783,-79.039554),
            new Position(-2.910083,-79.037666),
            new Position(-2.908497,-79.037194),
            new Position(-2.908122,-79.037977),
        };



        static TKPolygon poligono = new TKPolygon
        {
            Coordinates = CuencaCollection,
            Color = Color.Cyan,
            StrokeColor = Color.DarkBlue,
        };

        public static string WebServiceURL = "http://52.224.8.198:91";
        public static string CityparkWeb = "http://52.224.8.198:90";
        public static int TimeForSignalR = 30;
    }
}
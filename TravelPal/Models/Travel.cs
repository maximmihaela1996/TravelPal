﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using TravelPal.Enums;
using TravelPal.Interfaces;

namespace TravelPal.Models
{
    public class Travel
    {
        public string Destination { get; set; }
        public Countries Country { get; set; }
        public int Travellers { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TravelDays { get; set; }
        public User Owner { get; set; }
        public Travel(string destination, Countries country, int travellers, DateTime startDate, DateTime endDate, User owner)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            StartDate = startDate;
            EndDate = endDate;
            TravelDays = CalculateTravelDays();
            Owner = owner;
        }

       //Calculate difference between  two dateTime
        public virtual string GetInfo()
        {

            return $"{Country} | Travel Duration: {CalculateTravelDays()} days";
        }

        private int CalculateTravelDays()
        {
            return Convert.ToInt32((EndDate - StartDate).Days);
        }
    }
}

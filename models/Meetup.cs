
using System;
using System.Collections.Generic;

namespace LPI.Models
{
    public class Meetup
    {
        public string id { get; set; }
        public string label { get; set; }
        public DateTime date { get; set; }
        public string url { get; set; }
        public string location { get; set; }
        public string price { get; set; }
        public Event[] Events { get; set; }
    }
}
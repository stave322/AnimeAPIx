using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject_EventAttendance02
{
    public class Event
    {
        public static List<Event> EventList { get; set; } = new List<Event>();
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Semester { get; set; }
        public TimeSpan CheckInStart { get; set; }
        public TimeSpan CheckInEnd { get; set; }
        public TimeSpan? CheckOutStart { get; set; }
        public int TotalCheckIns { get; set; }
        public int TotalCheckOuts { get; set; }
        public string Status { get; set; }

        public TimeSpan? CheckOutEnd { get; set; } = null;



        public Event(string eventName, string location, DateTime date, string semester,
                     TimeSpan checkInStart, TimeSpan checkInEnd)
        {
            EventList.Add(this);
            EventID = EventList.Count; 
            EventName = eventName;
            Location = location;
            Date = date;
            Semester = semester;
            CheckInStart = checkInStart;
            CheckInEnd = checkInEnd;
            CheckOutStart = null;
            TotalCheckIns = 0;
            TotalCheckOuts = 0;
            Status = GetStatus();

           
        }

        public void StartCheckout()
        {
            CheckOutStart = DateTime.Now.TimeOfDay;
            Status = GetStatus();
        }

        public string GetStatus()
        {
            DateTime now = DateTime.Now;
            TimeSpan nowTs = now.TimeOfDay;

            if (Date.Date != DateTime.Today)
                return now < Date.Date + CheckInStart ? "Upcoming" : "Finished";

            if (CheckOutStart.HasValue)
            {
                DateTime coEnd = Date.Date + (CheckOutEnd ?? CheckOutStart.Value.Add(TimeSpan.FromHours(1)));
                return now <= coEnd ? "Ongoing" : "Finished";
            }

            DateTime start = Date.Date + CheckInStart;
            DateTime end = Date.Date + CheckInEnd;

            if (now < start) return "Upcoming";
            if (now <= end) return "Ongoing";
            return "Finished";
        }

        public static bool HasAnyOngoingEvent()
        {
            DateTime now = DateTime.Now;
            return EventList.Any(ev =>
            {
                DateTime start = ev.Date.Date + ev.CheckInStart;
                DateTime end = ev.Date.Date + ev.CheckInEnd;
                return now >= start && now <= end;
            });
        }

      
        public static void ReassignIDs()
        {
           
            var sortedEvents = EventList.OrderBy(e => e.EventID).ToList();

            
            int newId = 1;
            foreach (var ev in sortedEvents)
            {
                ev.EventID = newId++;
            }
        }


    }
}

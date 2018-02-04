using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Models
{
    public class WorkTrip
    {
        public Guid Id { get; set; }
        public string Distination { get; set; }
        public string PlannedBy { get; set; }
        public DateTime ScheduledFor { get; set; }
        public DateTime ReturnDate { get; set; }
        public WorkTripStatus Status { get; set; }
        public string Remarks { get; set; }
    }

    public enum WorkTripStatus
    {
        Planned, Completed, Cancelled
    }
}

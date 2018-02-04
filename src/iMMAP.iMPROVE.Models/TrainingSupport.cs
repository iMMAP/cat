using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Models
{
    public class TrainingSupport
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Conducted { get; set; }
        public string ConductedBy { get; set; }
        public TrainingOrSupport Type { get; set; }
        public string Description { get; set; }
        public string Clients { get; set; }
        public string Remarks { get; set; }
    }

    public enum TrainingOrSupport
    {
        Training, Support, Both
    }
}

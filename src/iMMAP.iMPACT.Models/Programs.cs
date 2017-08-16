using System;
using System.Collections.Generic;

namespace iMMAP.iMPACT.Models
{
    public class RegionalOfficeProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string OfficerName { get; set; }
        public string OfficerEmail { get; set; }
        public string OfficerPhone { get; set; }

        [Xod.ParentKey("ParentOfficeId")]
        public RegionalOffice ParentOffice { get; set; }
        public int ParentOfficeId { get; set; }
    }

    public class RegionalOffice
    {
        public int Id { get; set; }
        public Organization Organization { get; set; }
        public string Region { get; set; }

        public string Email { get; set; }
        public string Phone { get; set; }

        [Xod.Children]
        public List<RegionalOfficeProgram> Programs { get; set; }
    }

    public enum Organization
    {
        UNICEF, OCHA, UNHCR, WHO, FAO, WFP, USAID, CDC, IFRC, WRA, IOM, CARE, WorldBank, HandicapInternational, GICHD, ICARDA, KARTOZA
    }
}

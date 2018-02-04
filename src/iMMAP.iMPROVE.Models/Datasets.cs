using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iMMAP.iMPROVE.Models
{
    public class Dataset
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [Xod.Children]
        public List<DataColumn> Columns { get; set; }
    }

    public class DataColumn
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ColumnDataType Type { get; set; }

        [Xod.ParentKey("ParentDatasetId")]
        public Dataset ParentDataset { get; set; }
        public Guid ParentDatasetId { get; set; }
    }

    public enum ColumnDataType
    {
        String, Integer, Double, Boolean, DateTime
    }
}

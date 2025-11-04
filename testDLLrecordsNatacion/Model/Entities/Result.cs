using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class Result : DbEntity
    {
        public int Id;
        //public DateTime ResultDate { get; set; }
        public int SplitDistance { get; set; }
        //public string SwimStroke { get; set; }
        public string SwimTime { get; set; } //save as string but process as DateTime formatted
        public int Points { get; set; } //default -1 (if split)
        public int IsWaScoring { get; set; } = 1; //TODO: remove this attribute?
        public string EntryTime { get; set; }//splits dont have entry time //save as string but process as DateTime formatted
        public string Comment { get; set; }
        //public string AgeGroupName { get; set; } = null;
        public int AgeGroupMaxAge { get; set; } = -1;
        public int AgeGroupMinAge { get; set; } = -1;
        public int EventId { get; set; } = -1;
        public int AthleteId { get; set; } = -1;

        public Event Event; //{ get; set; }
        public Athlete Athlete; //{ get; set; }

    }
}

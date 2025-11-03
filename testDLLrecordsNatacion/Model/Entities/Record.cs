using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace testDLLrecordsNatacion.Model.Entities
{
    public class Record: DbEntity
    {
        public int Id;
        public DateTime RecordDate { get; set; }
        public int Position { get; set; }
        public string MeetStatus { get; set; }
        public string RecordType { get; set; }
        public string AgeCategory { get; set; }
        public string SwimTime { get; set; }

        public string SwimCourse { get; set; }
        public int SwimDistance { get; set; }
        public string SwimStroke { get; set; }
        public int Points { get; set; }

        public int AthleteId = -1;
        public int ResultId = -1;
        public Athlete Athlete = null;
        public Result Result = null;
        public Event Event = null;

        public Record GetRecord(int id) {
            //TODO
            return null;
        }

    }
}

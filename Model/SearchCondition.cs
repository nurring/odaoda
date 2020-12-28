using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class SearchCondition
    {
        private string compType;
        private string compCheck;
        private DateTime fromTime;
        private DateTime tillTime;

        public string CompType { get => compType; set => compType = value; }
        public string CompCheck { get => compCheck; set => compCheck = value; }
        public DateTime FromTime { get => fromTime; set => fromTime = value; }
        public DateTime TillTime { get => tillTime; set => tillTime = value; }
    }
}

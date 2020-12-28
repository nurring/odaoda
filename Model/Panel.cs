using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Panel
    {
        private string panelId;
        private string hdfsAddr;

        public string PanelId { get => panelId; set => panelId = value; }
        public string HdfsAddr { get => hdfsAddr; set => hdfsAddr = value; }
    }
}

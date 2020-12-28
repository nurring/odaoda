using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Model
{
    public class PanelCollection
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly Lazy<PanelCollection> instance =
            new Lazy<PanelCollection>(() => new PanelCollection());

        public static PanelCollection Instance { get { return instance.Value; } }

        private PanelCollection()
        {
            panelList = new List<Panel>();
        }
        private List<Panel> panelList;
        //public List<Panel> PanelList { get => panelList; set => panelList = value; }

        public void AddPanel(Panel panel)
        {
            panelList.Add(panel);
        }

        public void RemovePanel(Panel panel)
        {
            panelList.Remove(panel);
        }

        public void RemoveAllPanels()
        {
            panelList = null;
        }
    }
}

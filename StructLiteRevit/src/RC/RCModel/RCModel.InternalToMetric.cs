using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using StructLite.RevitAdapter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace StructLite.RC
{
    public partial class RCModel
    {
        /// <summary>
        /// Make the internal data of the finite element model to be in metric units/ N and mm
        /// </summary>
        public void InternalToMetric()
        {
            Units units = Adapter.doc.GetUnits();
            TaskDialog.Show("ss", units.ToString());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;

namespace Utility
{
    public static class WorksetUtil
    {
        private static RevitData revitData = RevitData.Instance;

        public static Autodesk.Revit.DB.Workset GetWorkset(string name )
        {
            var workset = revitData.UserWorksets.SingleOrDefault(x => x.Name == name);
            if(workset == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return workset;
        }
    }
}

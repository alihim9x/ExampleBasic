using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;

namespace Utility
{
    public static class LevelUtil
    {
        private static RevitData revitData = RevitData.Instance;
        public static Autodesk.Revit.DB.Level GetLevel(string name)
        {
            var level = revitData.Levels.SingleOrDefault(x => x.Name == name);
            if (level == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return level;
        }
    }
}

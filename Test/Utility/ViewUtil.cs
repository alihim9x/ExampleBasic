using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;
using Autodesk.Revit.DB;


namespace Utility
{
    public static class ViewUtil
    {
        private static RevitData revitData = RevitData.Instance;
        public static string GetIdentifyName (this View view)
        {
            
            var doc = revitData.Document;
            var viewFamilyType = view.GetTypeId().ToRevitElement() as ViewFamilyType;
            var viewFamily = viewFamilyType?.ViewFamily;
            return $"{viewFamily.ToString()}__{view.Name}";

               
        }
        public static View GetView (string name, ViewFamily viewFamily)
        {
            var instanceView = revitData.InstanceViews.SingleOrDefault(x => x.Name == name 
            && (x.GetTypeId().ToRevitElement() as ViewFamilyType).ViewFamily == viewFamily);
            if(instanceView == null)
            {
                throw new Model.Exception.ElementNotFoundException();
            }
            return instanceView;
        }
    }
}

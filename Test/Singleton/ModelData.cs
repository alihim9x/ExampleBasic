using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SingleData
{
    public class ModelData
    {
        public bool ToggleEvent { get; set; } = false;
        public List<BuiltInCategory> StructuralCategories { get; set; } = new List<BuiltInCategory> { BuiltInCategory.OST_Walls,
                                BuiltInCategory.OST_StructuralColumns,BuiltInCategory.OST_StructuralFraming,BuiltInCategory.OST_StructuralFoundation,
                                BuiltInCategory.OST_Floors};
    }
}

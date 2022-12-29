using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public class FuncSelectionFilter : ISelectionFilter
    {
        private Func<Element, bool> filterElement;
        private Func<Reference, bool> filterReference;

        public FuncSelectionFilter(Func<Element, bool> filterElement, Func<Reference, bool> filterReference = null) // filRef mặc định = null nên ko cần truyền vào nếu ko muốn
        {
            this.filterElement = filterElement; this.filterReference = filterReference;
        }
        public bool AllowElement(Element elem)
        {
            return filterElement(elem);
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return filterReference(reference);
        }
    }
}

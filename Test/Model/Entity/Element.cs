using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Model
{
    public class Element
    {
        public string Guid { get; set; }
        private Autodesk.Revit.DB.Element revitElement;
        public Autodesk.Revit.DB.Element RevitElement
        {
            get
            {
                if (revitElement == null)
                {
                    var doc = SingleData.Singleton.Instance.RevitData.Document;
                    revitElement = doc.GetElement(Guid);
                }

                return revitElement;
            }
        }
        private XYZ xyz;
        public XYZ XYZ
        {
            get
            {
                if (xyz == null) xyz = RevitElement.GetLocationPoint();
                return xyz;
            }
        }
        private Curve curve;
        public Curve Curve
        {
            get
            {
                if (curve == null) curve = RevitElement?.GetLocationCurve();
                return curve;
            }
        }

    }
}

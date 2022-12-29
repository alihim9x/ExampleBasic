using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Model.Entity
{
    public class Rebar
    {

        public Autodesk.Revit.DB.Structure.Rebar SingleRebar { get; set; }
        public Autodesk.Revit.DB.Structure.RebarInSystem  RebarInsystem { get; set; }
        public Rebar(Autodesk.Revit.DB.Structure.Rebar rebar)
        {
            SingleRebar = rebar;
        }
        public Rebar(Autodesk.Revit.DB.Structure.RebarInSystem rebarInsystem)
        {
            RebarInsystem = rebarInsystem;
        }
        private IEnumerable<Autodesk.Revit.DB.Curve> curves;
        public virtual IEnumerable<Autodesk.Revit.DB.Curve> Curves
        {
            get
            {
                if(curves == null)
                {
                    curves = this.GetCenterlineCurves();
                }
                return curves;
            }
        }
        private IEnumerable<Autodesk.Revit.DB.Curve> drivingCurves; 
        public virtual IEnumerable<Autodesk.Revit.DB.Curve> DrivingCurves
        {
            get
            {
                if(drivingCurves == null)
                {
                    drivingCurves = this.GetDrivingCurves();
                }
                return drivingCurves;
            }
        }
        private Autodesk.Revit.DB.XYZ maxOccurOriginZCenterLineCurve;
        public virtual Autodesk.Revit.DB.XYZ MaxOccurOriginZCenterLineCurve
        {
            get
            {
                if(maxOccurOriginZCenterLineCurve == null)
                {
                    maxOccurOriginZCenterLineCurve = this.MaxOccurOriginZCenterLineCurve();
                }
                return maxOccurOriginZCenterLineCurve;
            }
        }
        private Autodesk.Revit.DB.Line distributionPath;
        public Autodesk.Revit.DB.Line DistributionPath
        {
            get
            {
                if(distributionPath  == null)
                {
                    distributionPath = this.GetDistributionPath();
                }
                return distributionPath;
            }
        }
        private double distributionLength = -1;
        public double DistributionLength
        {
            get
            {
                if(distributionLength == -1)
                {
                    distributionLength = DistributionPath.Length.Feet2Milimeter(); ;
                }
                return distributionLength;
            }
        }
        private Autodesk.Revit.DB.XYZ distributionDirection;
        public virtual Autodesk.Revit.DB.XYZ DistributionDirection
        {
            get
            {
                if(distributionDirection == null)
                {
                    distributionDirection = DistributionPath.Direction;
                }
                return distributionDirection;
            }
        }
        
        
    }
}

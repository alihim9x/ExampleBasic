using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Model
{
    public class XYZ
    {
        public XYZ(double x,double y,double z)
        {
            X = x;
            Y = y;
            Z = z;

        }
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public static XYZ BasicX { get { return new XYZ(1, 0, 0); } }
        public static XYZ BasicY { get { return new XYZ(0, 1, 0); } }
        public static XYZ BasicZ { get { return new XYZ(0, 0, 1); } }
    }
}

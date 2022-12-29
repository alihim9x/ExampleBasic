using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using SingleData;
using Autodesk.Revit.UI.Selection;
using Utility;
using Autodesk.Revit.DB.Structure;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class TransformCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Initial
            var singleTon = Singleton.Instance = new Singleton();
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;
            var sel = revitData.Selection;
            var doc = revitData.Document;
            var activeView = revitData.ActiveView;
            var tx = revitData.Transaction;
            var uidoc = revitData.UIDocument;
            var app = revitData.Application;
            tx.Start();
            #endregion

            var pnt1 = sel.PickPoint();
            var pnt2 = sel.PickPoint();
            var pnt3 = sel.PickPoint();

            var tf = Transform.Identity;
            tf.Origin = pnt1;
            tf.BasisX = (pnt2 - pnt1).Normalize();
            tf.BasisY = (pnt3 - pnt1).Normalize();
            //Xác  định tọa độ gốc từ tọa độ địa phương của 1 điểm
            var pnt = tf.OfPoint(new XYZ(1000.0.Milimet2Feet(), 1000.0.Milimet2Feet(), 0));
            //doc.Create.NewDetailCurve(activeView, Line.CreateBound(pnt, pnt + (tf.BasisX + tf.BasisY) * 1000.0.Milimet2Feet()));
            //TaskDialog.Show("Revit", $"X:{pnt.X},Y:{pnt.Y}");


            //Xác định tọa độ địa phương từ tọa độ gốc của 1 điểm

            var pnt4 = sel.PickPoint();
            var localPnt = tf.Inverse.OfPoint(pnt4);
            TaskDialog.Show("Revit", $"{localPnt.X.Feet2Milimeter():0.00};{localPnt.Y.Feet2Milimeter():0.00}");

     
            //var pnt5 = sel.PickPoint();
            //var tf2 = Transform.Identity;
            //tf2.Origin = pnt1;
            //tf2.BasisX = (pnt4 - pnt1).Normalize();
            //tf2.BasisY = (pnt5 - pnt1).Normalize();

            //var testPnt = new XYZ(1, 1, 0);
            //var resultPnt = testPnt.GetPoint(tf, tf2);
            //TaskDialog.Show("Revit", $"{resultPnt.X.Feet2Milimeter():0.00},{resultPnt.Y.Feet2Milimeter():0.00}");


            tx.Commit();
            return Result.Succeeded;
        }
    }
}

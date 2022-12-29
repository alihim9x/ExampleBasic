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
using System.IO;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class NumberingElement : IExternalCommand
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
            var app = revitData.Application;
            #endregion
            tx.Start();
            List<Element> elems4 = new List<Element>();
            double a = 1;
            while (true) //Pick mãi

            {
                try
                {
                    
                    // add vào 1 mảng, quét được 1 mảng nên ko thể dùng .Add
                    var elem = sel.PickObject(ObjectType.Element).ToRevitElement();
                    elem.SetValue("Comments", a.ToString());
                    TaskDialog.Show("Revit", $"{elem.AsValue("Comments").ValueText}");
                    a += 1;
                }
                catch (Autodesk.Revit.Exceptions.OperationCanceledException) // Bấm escape để thoát lệnh
                {

                    break;
                }
            }
            // Hàm sel.SetElementIds trả về các đối tượng ra màn hình (highlight xanh)
            sel.SetElementIds(elems4.Select(x => x.Id).GroupBy(x => x.IntegerValue).Select(x => x.First()).ToList()); //GroupBy sẽ group các đối tượng giống nhau lại rồi chọn First

            tx.Commit();
            return Result.Succeeded;

            
        }
       
    }
}

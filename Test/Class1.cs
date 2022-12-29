using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.Attributes;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using System.Diagnostics;
using SingleData;
using System.IO;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class Class1 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //var person1 = new Person { Name = "KiênA" };
            //Debug.WriteLine(person1.Name);

            //var person2 = new Person { Name = "KiênB" };
            //Debug.WriteLine(person2.Name);

            //var person3 = new Person { Name = "KiênC" };
            //Debug.WriteLine(person3.Name);

            //TaskDialog.Show("Revit","Load successfully!");

            //  câu thần chú
            //var uiapp = commandData.Application;
            //var app = uiapp.Application;
            //var uidoc = uiapp.ActiveUIDocument;
            //var doc = uidoc.Document;

            var singleTon = Singleton.Instance;
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;

            var doc = revitData.Document;
            Transaction transaction = new Transaction(doc, "Add-in 1"); //Khi có thay đổi trong project thì phải có transaction. Ví dụ khi show dialog thì ko cần transaction
            transaction.Start();


            var app = revitData.Application;

            //var doc2 = app.NewProjectDocument(@"C:\ProgramData\Autodesk\RVT 2019\Templates\US Metric\DefaultMetric.rte");
            //doc2.SaveAs(@"D:\Study\RV Api\Test\1/Test1.rvt");

            var famDoc = app.NewFamilyDocument(@"C:\ProgramData\Autodesk\RVT 2019\Family Templates\English\Metric Generic Model.rft");
            Transaction famTx = new Transaction(famDoc, "Tạo hình khối");
            famTx.Start();
            Plane plane = Plane.CreateByOriginAndBasis(XYZ.Zero, XYZ.BasisX, XYZ.BasisY);
            SketchPlane sp = SketchPlane.Create(famDoc, plane);
            famDoc.FamilyCreate.NewModelCurve(Line.CreateBound(XYZ.Zero, XYZ.BasisX * 3), sp);
            famTx.Commit(); // Sau khi thực thi xong thì kết thúc

            var PathName = Path.Combine(Path.GetTempPath(), $"test{Guid.NewGuid()}.rfa");
            famDoc.SaveAs(PathName);

            Family fam = null;
            doc.LoadFamily(famDoc.PathName, out fam);
            famDoc.Close();
            transaction.Commit();


            if (File.Exists(PathName))
            {
                File.Delete(PathName);
            }


            return Result.Succeeded;
        }
    }
    
}

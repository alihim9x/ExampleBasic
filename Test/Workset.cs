using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using SingleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Utility;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class Workset : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            #region Initial
            var singleTon = Singleton.Instance = new Singleton();
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;
            var sel = revitData.Selection;
            var doc = revitData.Document;
            var uidoc = revitData.UIDocument;
            var activeView = revitData.ActiveView;
            var tx = revitData.Transaction;
            var app = revitData.Application;
            #endregion
            tx.Start();




            #region WOrkset

            //var worksets1 = new FilteredWorksetCollector(doc).Where(x => x.Kind == WorksetKind.UserWorkset);
            //var worksetsString = "";
            //worksets1.ToList().ForEach(x => worksetsString += x.Name + "-");
            //var worksets2 = revitData.UserWorksets;
            //worksets2.ToList().ForEach(x => worksetsString += x.Name + "-");
            //TaskDialog.Show("Revit", worksetsString);

            //Gắn workset cho rebar

            var worksetRebar = WorksetUtil.GetWorkset("REBAR");
            if (worksetRebar == null) throw new Model.Exception.ElementNotFoundException();
            var worksetRebarId = worksetRebar.Id.IntegerValue;
            var allRebars = revitData.Rebars;
            foreach (Autodesk.Revit.DB.Structure.Rebar rebar in allRebars)
            {
                rebar.SetValue("Workset", worksetRebarId);
            }




            //Gắn workset cho lanh tô
            //var worksetLintel = WorksetUtil.GetWorkset("B1030 (SE - LINTEL, SUBCOLUMN)");
            //if (worksetLintel == null) throw new Model.Exception.ElementNotFoundException();
            //var worksetLintelId = worksetLintel.Id.IntegerValue;
            //var allFraming = revitData.FramingInstances;
            //List<Autodesk.Revit.DB.FamilyInstance> allLintel = new List<FamilyInstance>();
            //foreach (var framing in allFraming.ToList())
            //{
            //    if(framing.AsValue("TEN CAU KIEN").ValueText == null || framing.AsValue("TEN CAU KIEN").ValueText.Contains("B"))
            //    {
            //        continue;
            //    }
            //    if (!framing.AsValue("TEN CAU KIEN").ValueText.Contains("LT")) ;
            //    {
            //        allLintel.Add(framing);
            //    }
            //}
            //allLintel.ForEach(x => x.SetValue("Workset", worksetLintelId));
            //

            //Gắn workset cho bổ trụ
            //var allColumns = revitData.ColumnInstances;
            //List<Autodesk.Revit.DB.FamilyInstance> allSubColumns = allColumns.ToList().Where(x => x.AsValue("TEN CAU KIEN").ValueText.Contains("BT")).ToList();
            //allSubColumns.ForEach(x => x.SetValue("Workset", worksetLintelId));
            ////

            //Gắn workset cho Reference Plan
            //var allRefPlanes = revitData.ReferencePlanes.ToList();
            //var allRefPlances1 = allRefPlanes.Where(x=>!x.LookupParameter("Workset").IsReadOnly).ToList();
            //var worksetRefPlane = WorksetUtil.GetWorkset("C1000 (SE - REFERENCE PLAN)");
            //var worksetRefPlaneId = worksetRefPlane.Id.IntegerValue;
            //allRefPlances1.ToList().ForEach(x => x.SetValue("Workset", worksetRefPlaneId));




            // Đổi các cột có Zone là Z1 sang Workset có tên là Z1
            //var columnCollector1 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x.Category?.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns);
            //var columnsZ1 = revitData.ColumnInstances.Where(x => x.AsValue("HB_Zone").Value == "Z1");
            //var worksets1 = WorksetUtil.GetWorkset("Z1");
            //if (worksets1 == null) throw new Model.Exception.ElementNotFoundException();
            //var worksetZ1Id = worksets1.Id.IntegerValue;
            ////foreach (var item in columnsZ1.ToList())
            ////{
            ////        item.LookupParameter("Workset").Set(worksetZ1Id);
            ////}
            //columnsZ1.ToList().ForEach(x => x.SetValue("Workset",worksetZ1Id));

            //var worksetZ11 = WorksetUtil.GetWorkset("Z1");
            //activeView.SetWorksetVisibility(worksetZ11.Id, WorksetVisibility.Hidden);


            #endregion







            tx.Commit();
            return Result.Succeeded;
        }

    }
    }

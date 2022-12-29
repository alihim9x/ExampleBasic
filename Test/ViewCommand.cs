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
    public class ViewCommand : IExternalCommand
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
            #region HIển thị
            //var view1 = revitData.ActiveView;
            //var origin1 = view1.Origin;
            //var basisX = view1.RightDirection;
            //var basisY = view1.UpDirection;
            //var basisZ = view1.ViewDirection;
            //TaskDialog.Show("Revit", $"Origin: {origin1}\n" + $"BasisX: {basisX}\n BasisY: {basisY}\n BasisZ: {basisZ}");


            //Lấy đối tượng đang hiển thị trên view
            //var elems = new FilteredElementCollector(doc, activeView.Id).WhereElementIsNotElementType(); 
            //TaskDialog.Show("Revit", $"{elems.Count()}");

            //var elems2 = new ElementId(123456).ToRevitElement();
            //uidoc.ShowElements(elems2);
            //sel.SetElementIds(new List<ElementId> { elems2.Id });

            //var elemsId3 = new List<int> { 123456, 567890 }.Select(x => new ElementId(x)).ToList();
            //uidoc.ShowElements(elemsId3.ToList());
            //sel.SetElementIds(elemsId3);



            //var instanceView3 = new FilteredElementCollector(doc).WhereElementIsElementType()
            //    .Where(x => x is View && !(x as View).IsTemplate);
            //var instanceViewName3 = instanceView3.Select(x => (x as View).GetIdentifyName()).ToList();

            //var templateView3 = new FilteredElementCollector(doc).WhereElementIsElementType()
            //    .Where(x => x is View && (x as View).IsTemplate);
            //var templateViewName3 = templateView3.Select(x => (x as View).GetIdentifyName()).ToList();

            //TaskDialog.Show("Revit",$"TemplateVIews:{templateView3.Count()}\n InstanceViews: {instanceView3.Count()}");

            #endregion

            #region View
            //Chuyển active view
            //tx.Commit();

            //uidoc.ActiveView = ViewUtil.GetView("TẦNG 1", ViewFamily.StructuralPlan); // Chuyển view phải nằm ngoài transaction

            //tx.Start();

            //Lấy các view đang hiện hành

            //var views5 = uidoc.GetOpenUIViews().Select(x => x.ViewId.ToRevitElement());
            //string sViews5 = "";
            //views5.ToList().ForEach(x => sViews5 += x.Name + " - ");
            //TaskDialog.Show("Revit", $"{sViews5}");


            //Ẩn và hiện các đối tượng trong view
            //activeView.AreModelCategoriesHidden = true;

            //activeView.SetCategoryHidden(new ElementId(BuiltInCategory.OST_StructuralColumns), true);
            //activeView.SetCategoryHidden(new ElementId(BuiltInCategory.OST_StructuralFraming), true);

            //activeView.HideCategoriesTemporary(new List<ElementId>
            //{ new ElementId(BuiltInCategory.OST_StructuralColumns), new ElementId(BuiltInCategory.OST_StructuralFraming) });
            //activeView.DisableTemporaryViewMode(TemporaryViewMode.TemporaryHideIsolate);

            //var elemes4 = sel.PickElementsByRectangle();
            //activeView.HideElements(elemes4.Select(x => x.Id).ToList());
            //TaskDialog.Show("Revit", "Đối tượng đã ẩn");
            //tx.Commit(); // nếu ko có commit ở đây thì sẽ thực hiện luôn 1 lúc 2 lệnh nên ko thể thấy được đối tượng ẩn

            //tx.Start();
            //activeView.UnhideElements(elemes4.Select(x => x.Id).ToList());
            //TaskDialog.Show("Revit", "Đối tượng đã hiện");
            #endregion

            #region Filter View
            // Tạo Filter trong VG
            //var column1 = revitData.InstanceElements.First(x => x.Category?.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns);
            //var paramcolum1 = column1.LookupParameter("Base Level");

            //var param3 = BuiltInCategory.OST_StructuralColumns.LookupParameter("Base Level");

            //FilterRule filterrule1 = ParameterFilterRuleFactory.CreateNotEqualsRule(param3.Id, LevelUtil.GetLevel("TẦNG 1").Id);
            //ElementParameterFilter elemParamFilter1 = new ElementParameterFilter(filterrule1, true);
            //FilterElement fe1 = ParameterFilterElement.Create(doc, "# Cột tầng 1"
            //    ,new List<ElementId> { new ElementId(BuiltInCategory.OST_StructuralColumns) }, elemParamFilter1);
            //activeView.AddFilter(fe1.Id);
            //activeView.SetFilterVisibility(fe1.Id, false);

            //var paramColumn1 = BuiltInCategory.OST_StructuralColumns.LookupParameter("HB_Zone");
            //var paramElementName1 = BuiltInCategory.OST_StructuralColumns.LookupParameter("HB_ElementName");
            //var bics1 = new List<BuiltInCategory> { BuiltInCategory.OST_StructuralColumns };
            //var cateIds = bics1.Select(x => new ElementId(x));

            //FilterRule colFilRule1 = ParameterFilterRuleFactory.CreateEqualsRule(paramColumn1.Id, "Z2", true); // sensitive là check viết hoa viết thường
            //FilterRule colFilRule2 = ParameterFilterRuleFactory.CreateEqualsRule(paramColumn1.Id, "Z1", true);
            //FilterRule colC1FilRule = ParameterFilterRuleFactory.CreateEqualsRule(paramElementName1.Id, "C1", true);
            //FilterRule framingHB1FilRule = ParameterFilterRuleFactory.CreateEqualsRule(paramElementName1.Id, "HB1", true);

            //var z1Fil = new ElementParameterFilter(colFilRule1);
            //var z2Fil = new ElementParameterFilter(colFilRule2);
            //var C1Fil = new ElementParameterFilter(colC1FilRule);
            //var HB1Fil = new ElementParameterFilter(framingHB1FilRule);
            //var colZ1orZ2 = new LogicalOrFilter(z1Fil, z2Fil);
            //var c1hb1 = new LogicalOrFilter(C1Fil, HB1Fil);
            //var c1hb1Z1orZ2 = new LogicalAndFilter(colZ1orZ2, c1hb1);

            //ElementParameterFilter colParamFil1 = new ElementParameterFilter(colFilRule1); // true true false true ra kết quả ngược lại???
            //FilterElement colFil1 = ParameterFilterElement.Create(doc, "Cột Tường Dầm # Zone 2",
            //    new List<ElementId> { new ElementId(BuiltInCategory.OST_StructuralColumns), new ElementId(BuiltInCategory.OST_StructuralFraming), new ElementId(BuiltInCategory.OST_Walls) }, colParamFil1);

            ////Cách khác
            //FilterElement c1hb1Z1orZ2Fil = ParameterFilterElement.Create(doc, "Cột Tường Dầm Zone 1 và Zone 2",
            //    new List<ElementId> { new ElementId(BuiltInCategory.OST_StructuralColumns), new ElementId(BuiltInCategory.OST_StructuralFraming), new ElementId(BuiltInCategory.OST_Walls) }, c1hb1Z1orZ2);
            //activeView.AddFilter(c1hb1Z1orZ2Fil.Id);
            //activeView.SetFilterVisibility(c1hb1Z1orZ2Fil.Id, false);
            #endregion

            #region WOrkset

            //var worksets1 = new FilteredWorksetCollector(doc).Where(x => x.Kind == WorksetKind.UserWorkset);
            //var worksetsString = "";
            //worksets1.ToList().ForEach(x => worksetsString += x.Name + "-");
            //TaskDialog.Show("Revit", worksetsString);

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

            #region Hiệu chỉnh đối tượng trong VG
            //var fillPatternElements = revitData.InstanceElements.Where(x => x is FillPatternElement);
            //var solidFillPatternElement = fillPatternElements.SingleOrDefault(x => x.Name == "Solid fill");
            //OverrideGraphicSettings ogs1 = new OverrideGraphicSettings();
            //ogs1.SetSurfaceBackgroundPatternColor(new Color(255, 0, 0));
            //ogs1.SetSurfaceBackgroundPatternId(solidFillPatternElement.Id);

            //activeView.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_StructuralColumns), ogs1);

            //Set category cột màu đỏ, vách màu vàng, dầm màu xanh lá

            //var solidFillPattern = FillPatternUtil.GetFillPatternElement("<Solid fill>");
            //OverrideGraphicSettings ogs2 = new OverrideGraphicSettings();
            //var redColor = new Color(255, 0, 0);
            //var yellowColor = new Color(255, 255, 0);
            //var greenColor = new Color(0, 255, 0);

            //OverrideGraphicSettings redfillOGS = ogs2.SetSurfaceBackgroundPatternColor(redColor);
            //redfillOGS = ogs2.SetSurfaceBackgroundPatternId(solidFillPattern.Id);
            //activeView.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_StructuralColumns), redfillOGS);

            //OverrideGraphicSettings yellowfillOGS = ogs2.SetSurfaceBackgroundPatternColor(yellowColor);
            //yellowfillOGS = ogs2.SetSurfaceBackgroundPatternId(solidFillPattern.Id);
            //activeView.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_StructuralFraming), yellowfillOGS);

            //OverrideGraphicSettings greenfillOGS = ogs2.SetSurfaceBackgroundPatternColor(greenColor);
            //greenfillOGS = ogs2.SetSurfaceBackgroundPatternId(solidFillPattern.Id);
            //activeView.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_Walls), greenfillOGS);



            #endregion





            tx.Commit();
            return Result.Succeeded;
        }

    }
    }

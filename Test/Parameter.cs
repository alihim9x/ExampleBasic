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
    public class ParameterRetrieve : IExternalCommand
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
            // Family fam = null;
            // var famSyms = fam.GetFamilySymbolIds().Select(x=>doc.GetElement(x) as FamilySymbol))
            #region Lấy giá trị parameter
            //var refPicked1 = sel.PickObject((ObjectType.Element),new FuncSelectionFilter(x=>x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns));
            //var elemPicked1 = doc.GetElement(refPicked1);
            //var elemType1 = (elemPicked1 as FamilyInstance).Symbol;
            //var paramElemType1 = elemType1.LookupParameter("b");
            //var paramElemIns1 = elemPicked1.LookupParameter("Volume");

            //var elem2 = doc.GetElement(refPicked1);
            // var elemType2 = doc.GetElement(elem2.GetTypeId()) as ElementType;

            //var elem3 = refPicked1.ToRevitElement();
            //var elemType3 = elem3.GetTypeId().ToRevitElement() as ElementType;
            //var volume3 = elem3.LookupParameter("Volume").AsValue();
            //var a = volume3.ValueNumber; // Dùng để tính toán nếu cần + các volume

            //var param = elem.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
            //var value = param.AsDouble();
            //TaskDialog.Show("Revit", $"{value.FeetCb2MeterCb():0.00}");


            //Parameter param2 = null;
            //foreach (Parameter item in elem.Parameters)
            //{
            //    if(item.Definition.Name == "Volume")
            //    {
            //        param2 = item; break;
            //    }
            //}
            //var value2 = param2.AsDouble();
            //TaskDialog.Show("Revit", $"{value2.FeetCb2MeterCb():0.00}");


            //TaskDialog.Show("Revit", volume3.Value);
            #endregion


            #region Definition of Parameter - Kiểm tra xem Parameter đã được thêm vào cho những Category nào
            //var elem4 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var pDef = elem4.LookupParameter("Volume").Definition;

            //var pBindings1 = doc.ParameterBindings;
            //var bindings1 = pBindings1.get_Item(pDef) as ElementBinding;
            //var categories1 = bindings1.Categories;
            //foreach (Category item in categories1)
            //{
            //    TaskDialog.Show("Revit", $"Category: {item.Name}");
            //}

            //var elem5 = sel.PickObject(ObjectType.Element).ToRevitElement();
            //var cates5 = elem5.LookupParameter("SoHieu").GetCategory().ConvertList();
            //TaskDialog.Show("Revit", cates5.Count.ToString());

            //TaskDialog.Show("Revit", $"Name: {pDef.Name} \n Unit: {pDef.UnitType} \n Group: {pDef.ParameterGroup} \n Type: {pDef.ParameterType}");



            #endregion

            #region Tạo parameter
            //tx.Start();
            //string name = "Text2";
            //ParameterType paramType = ParameterType.Text;

            //Definition def = null;
            //CategorySet cateSet5 = new CategorySet();
            //BuiltInParameterGroup paramGroup = BuiltInParameterGroup.PG_IDENTITY_DATA;
            //cateSet5.Insert(BuiltInCategory.OST_StructuralFraming.GetCategory());
            //cateSet5.Insert(BuiltInCategory.OST_StructuralColumns.GetCategory());
            //cateSet5.Insert(BuiltInCategory.OST_Floors.GetCategory());
            //cateSet5.Insert(BuiltInCategory.OST_Walls.GetCategory());
            //string sharedParameterTextName = Path.Combine(Path.GetTempPath(), $"add_parameter_{Guid.NewGuid()}.txt");
            //using (var fs = File.Create(sharedParameterTextName)) { } // Tạo ra file xong xóa bộ nhớ luôn
            //// nếu thay dòng trên = var fs=File.Create(sharedParameterTextName) thì ở dưới ko File.Delete được vì nó còn trong bộ nhớ
            //app.SharedParametersFilename = sharedParameterTextName;
            //    DefinitionFile defFile = app.OpenSharedParameterFile();
            //    DefinitionGroups defGroups = defFile.Groups;
            //    DefinitionGroup defGroup = defGroups.Create(name);
            //    def = defGroup.Definitions.Create
            //        (new ExternalDefinitionCreationOptions(name, paramType) { UserModifiable = true });


            //// Tạo project parameter từ shared parameter ở trên
            //var bindingMap5 = revitData.BindingMap;
            //bindingMap5.Insert(def, app.Create.NewInstanceBinding(cateSet5),paramGroup);

            //// Xóa file text shared parameter
            //if(File.Exists(sharedParameterTextName))
            //{
            //    File.Delete(sharedParameterTextName);
            //}
            //tx.Commit();

            //ParameterUtil.GetDefinition("Block", Model.DefinitionGroupType.General, ParameterType.Text);
            //ParameterUtil.GetDefinition("Zone", Model.DefinitionGroupType.General, ParameterType.Text);
            //ParameterUtil.GetDefinition("RoomName", Model.DefinitionGroupType.Architecture, ParameterType.Text);

            //tx.Start(); 
            //var structuralCate = new List<BuiltInCategory>
            //{
            //    BuiltInCategory.OST_StructuralColumns,BuiltInCategory.OST_StructuralFraming,BuiltInCategory.OST_Floors,
            //    BuiltInCategory.OST_Walls

            //};

            //ParameterUtil.AddParameter("Block", Model.AddParameterType.Instance, Model.DefinitionGroupType.General,
            //    ParameterType.Text, BuiltInParameterGroup.PG_IDENTITY_DATA, structuralCate);
            //ParameterUtil.AddParameter("Test", Model.AddParameterType.Type, Model.DefinitionGroupType.General,
            //    ParameterType.Text, BuiltInParameterGroup.PG_GEOMETRY, structuralCate);
            //tx.Commit();

            #endregion

            #region Set Parameter
            tx.Start();
            //var column6 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(x => x.Category.Id.IntegerValue
            //== (int)BuiltInCategory.OST_StructuralColumns)).ToRevitElement();
            //column6.LookupParameter("Block").Set("Podium");
            //column6.LookupParameter("Base Offset").Set(((double)-1000).Milimet2Feet());
            //column6.LookupParameter("Base Level").Set(new ElementID(123123));


            //var baseLevel = new FilteredElementCollector(doc).WhereElementIsNotElementType()
            //    .Where(x => x is Level).SingleOrDefault(x => x.Name == "Base");
            //column6.LookupParameter("Base Level").Set(baseLevel.Id);
            //column6.LookupParameter("Base Offset").Set(((double)-1000).Milimet2Feet());

            //var wall6 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(x => x is Wall)).ToRevitElement();
            //var level1 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x is Level).SingleOrDefault(x => x.Name == "TẦNG 1");
            //var level2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x is Level).SingleOrDefault(x => x.Name == "TẦNG 2");

            //wall6.LookupParameter("Base Constraint").Set(level1.Id);
            //wall6.LookupParameter("Top Constraint").Set(level2.Id);
            //wall6.LookupParameter("Base Offset").Set(((double)1000).Milimet2Feet());
            //wall6.LookupParameter("Top Offset").Set(((double)1000).Milimet2Feet());

            //wall6.LookupParameter("Base Constraint").SetValue(LevelUtil.GetLevel("TẦNG 1"));
            //wall6.LookupParameter("Base Offset").SetValue(GeomUtil.Milimet2Feet(2000));
            //wall6.LookupParameter("Top Constraint").SetValue(LevelUtil.GetLevel("TẦNG 2"));
            //wall6.LookupParameter("Top Offset").SetValue(GeomUtil.Milimet2Feet(2000));
            //var column7 = sel.PickObject(ObjectType.Element, new FuncSelectionFilter(x => x.Category.Id.IntegerValue
            //== (int)BuiltInCategory.OST_StructuralColumns)).ToRevitElement();
            //(column7 as FamilyInstance).Symbol.LookupParameter("b").SetValue(GeomUtil.Milimet2Feet(200));
            //(column7 as FamilyInstance).Symbol.LookupParameter("h").SetValue(GeomUtil.Milimet2Feet(300));


            StringBuilder sb = new StringBuilder();
            var cate = Category.GetCategory(revitData.Document, Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFoundation).Id;
            //var params1 = TableView.GetAvailableParameters(doc, cate).ToList();
            ////var a = params1.First().IntegerValue;
            ////TaskDialog.Show("Revit", $"{a}");
            //foreach (var item in params1)
            //{
            //    sb.Append(item.ToRevitElement()?.Name);
            //}

            //TaskDialog.Show("Revit", $"{params1.Count}");

            List<string> paramList = new List<string>();
            //var cate = Category.GetCategory(revitData.Document, Autodesk.Revit.DB.BuiltInCategory.OST_StructuralFoundation).Id;
            //var bingdingMap = doc.ParameterBindings;
            //DefinitionBindingMapIterator iter = bingdingMap.ForwardIterator();
            //iter.Reset();
            //while(iter.MoveNext())
            //{
            //    ElementBinding binding = iter.Current as ElementBinding;
            //    foreach (Category c in binding.Categories)
            //    {
            //        if(c.Id.IntegerValue == cate.IntegerValue)
            //        {
            //             paramList.Add(iter.Key.Name);
            //            break;
            //        }
            //    }
            //}
            //paramList.ForEach(x => sb.Append(x));
            //TaskDialog.Show("Revit", $"{sb}");

            //var a = doc.GetElement(new ElementId(1152385));
            //var b = a.Parameters;
            //TaskDialog.Show("Revit", a.Name);



            tx.Commit();
            List<ElementId> paramIds = new List<ElementId>();
         
            using (Transaction tr = new Transaction(doc, "make_schedule"))
            {
                tr.Start();
                ViewSchedule vs = ViewSchedule.CreateSchedule(doc, cate);
                doc.Regenerate();
                
                foreach (var sField in vs.Definition.GetSchedulableFields())
                {
                    if (sField.FieldType != ScheduleFieldType.Instance) continue;
                    paramIds.Add(sField.ParameterId);
                }
                

                tr.Commit();
            }
            //paramIds.ForEach(x => sb.Append(x.ToRevitElement().Name));
            TaskDialog.Show("Revit", $"{paramIds.Count}");
            //TaskDialog.Show("Revit", $"{sb}");






            //tx.Commit();




            #endregion


            return Result.Succeeded;

          
        }
       
    }
}

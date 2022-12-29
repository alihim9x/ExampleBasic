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
    public class SheetCommand : IExternalCommand
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

            var viewSheetCate = new List<BuiltInCategory> { BuiltInCategory.OST_Sheets };
            //ParameterUtil.AddParameter("STT", Model.AddParameterType.Instance, Model.DefinitionGroupType.Sheet,
            //                            ParameterType.Text, BuiltInParameterGroup.PG_IDENTITY_DATA, viewSheetCate);
            var allViewSheets = revitData.Sheets;
            var thanViewSheets = allViewSheets.Where(x => x.AsValue("THƯ MỤC CHÍNH").ValueText ==("CD") && x.AsValue("Appears In Sheet List").ValueNumber == 1);

            double i = 0;

            //var viewSheet = sel.PickObjects(ObjectType.Element).Cast<ViewSheet>();

            #region STT cho Sheet        
            foreach (var item in thanViewSheets.OrderBy(x => x.AsValue("Sheet Number").ValueText).ToList())
            {
                i++;
                item.SetValue("STT", i.ToString());
            }
            //foreach (var item in thanViewSheets.OrderBy(x => x.SheetNumber.ValueText).ToList())
            //{
            //    i++;
            //    item.SetValue("STT", i.ToString());
            //}
            #endregion


            #region Pseudo Sheet Number
            //string sheetNum = "a";
            //string sheetNumPseudo = "b";
            //foreach (var sheet in allViewSheets)
            //{
                
            //    sheetNum = sheet.AsValue("Sheet Number").ValueText;
            //    if(sheetNum.Length >3)
            //    {
            //        sheetNumPseudo = sheetNum.Remove(0, 3);
            //        sheet.SetValue("Sheet Number (Pseudo)", sheetNumPseudo);
            //    }
            //    else if(sheetNum.Length == 3)
            //    {
            //        sheet.SetValue("Sheet Number (Pseudo)", sheetNum);
            //    }
            //}
            #endregion

            tx.Commit();
            return Result.Succeeded;
        }
        }
    }

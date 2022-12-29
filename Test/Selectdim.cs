using Autodesk.Revit.Attributes;
using Autodesk.Revit.UI;
using SingleData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Utility;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    public class SelectDim : IExternalCommand
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
            var uiapp = commandData.Application;
            #endregion
            tx.Start();




            #region SelectDim
            //var dimFilter = new ElementClassFilter(typeof(Dimension));
            var elems2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x is Dimension && ((x as Dimension).View != null)
            && (((x as Dimension).View.GetTypeId().ToRevitElement() as ViewFamilyType).ViewFamily == ViewFamily.FloorPlan));
            //var elems2 = new FilteredElementCollector(doc).WhereElementIsNotElementType().Where(x => x is Dimension);
            sel.SetElementIds(elems2.Select(x => x.Id).ToList());

            //var sel1 = sel.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element).ToRevitElement() as Dimension;
            //ViewFamilyType test = sel1.View.GetTypeId().ToRevitElement() as ViewFamilyType;
            //TaskDialog.Show("Revit", $"{test.ViewFamily}");

            //RevitCommandId copyToClip = RevitCommandId.LookupPostableCommandId(PostableCommand.CopyToClipboard);
            //uiapp.PostCommand(copyToClip);
            //RevitCommandId pasteAligned = RevitCommandId.LookupPostableCommandId(PostableCommand.AlignedToCurrentView);
            //uiapp.PostCommand(pasteAligned);
            
            #endregion







            tx.Commit();
            return Result.Succeeded;
        }

    }
    }

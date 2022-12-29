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
    public class RebarCommand : IExternalCommand
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

            #region Phương Thép
            var framingInstance = revitData.FramingInstances;
            var framingRebars = revitData.FramingRebars;
            var floorRebars = revitData.FloorRebars;
            var floorInstance = revitData.FloorInstances;
            framingRebars.ToList().SetDirectionXYRebar();
            framingInstance.Cast<Element>().ToList().SetlayerTBRebar();

            //floorRebars.ToList().SetDirectionXYRebar();
            //floorInstance.Cast<Element>().ToList().SetlayerTBRebar();


            //foreach (var item in framingRebars)
            //{

            //    var hostIdElem = item?.GetHostId().ToRevitElement();
            //    var curveframing = (hostIdElem.Location as LocationCurve).Curve as Line;
            //    var directionframing = curveframing.Direction;
            //    if (directionframing.IsXOrY())

            //    {
            //        item.SetValue("Comments", "X");
            //    }
            //    else
            //    {
            //        item.SetValue("Comments", "Y");
            //    }
            //} 


            //foreach (var item in floorRebars.ToList())
            //{

            //    var shapedrivenDirRebar = item.DistributionDirection;

            //    if (shapedrivenDirRebar.IsXOrY())
            //    {
            //        item.SetValue("Comments", "Y");
            //    }
            //    else
            //    {
            //        item.SetValue("Comments", "X");
            //    }
            //}
            ////AreaReinforcement
            #endregion

            #region Đánh Partition Thép
            var foundationRebars = revitData.FoundationRebars;
            foreach (var foundationRebar in foundationRebars)
            {
                var hostIdElem = foundationRebar.GetHostId().ToRevitElement() as FamilyInstance;
                var tenCK = hostIdElem.AsValue("TEN CAU KIEN").ValueText;
                //try
                //{
                foundationRebar.SetValue("Partition", tenCK);

                //}
                //catch (Exception)
                //{

                //    TaskDialog.Show("Revit", $"{hostIdElem.Id}");
                //}
            }
            foreach (var framingRebar in framingRebars)
            {
                var hostIdElem = framingRebar.GetHostId().ToRevitElement() as FamilyInstance;
                var tenCK = hostIdElem.AsValue("TEN CAU KIEN").ValueText;
                try
                {
                    framingRebar.SetValue("Partition", tenCK);

                }
                catch (Exception)
                {

                    TaskDialog.Show("Revit", $"{hostIdElem.Id}");
                }
            }
            var columnRebars = revitData.ColumnRebars;
            //foreach (var columnRebar in columnRebars)
            //{
            //    var hostIdElem = columnRebar.GetHostId().ToRevitElement() as FamilyInstance;
            //    var tenCK = hostIdElem.AsValue("TEN CAU KIEN").ValueText;
            //    var baseLevelHostId = hostIdElem.AsValue("Base Level").ValueText;
            //    try
            //    {
            //        columnRebar.SetValue("Partition", tenCK);
            //        columnRebar.SetValue("Level", baseLevelHostId);

            //    }
            //    catch (Exception)
            //    {

            //        TaskDialog.Show("Revit", $"{hostIdElem.Id}");
            //    }
            //}

            #endregion

            tx.Commit();

            return Result.Succeeded;
        }
        }
    }

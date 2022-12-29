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
    public class DimensionCrossSection : IExternalCommand
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

            var framingInView = revitData.FramingInstancesInView.FirstOrDefault();
            var framingSolid = framingInView.GetSingleSolidValidRef();
            var framingInViewOrigin = framingInView.GetTotalTransform().Origin;

            var floorinView = revitData.FloorInstancesInview.FirstOrDefault();
            var floorSolid = floorinView?.GetSingleSolidValidRef();

            PlanarFace leftFaceFraming = null;
            PlanarFace rightFaceFraming = null;
            PlanarFace topFaceFraming = null;
            PlanarFace botFaceFraming = null;
            PlanarFace topFaceFloor = null;
            PlanarFace botFaceFloor = null;
            double originZFramingInview = framingInViewOrigin.Z;
            //double minOrizinZFaceFraming = 0;
            if(floorSolid != null)
            {
                for (int i = 0; i < framingSolid.Faces.Size; i++)

                {
                    //if (i == 0)
                    //{
                    //    maxOriginZFaceFraming = (framingSolid.Faces.get_Item(i) as PlanarFace).Origin.Z;
                    //    minOrizinZFaceFraming = (framingSolid.Faces.get_Item(i) as PlanarFace).Origin.Z;
                    //}
                    if (framingSolid.Faces.get_Item(i).ComputeNormal(UV.Zero).IsOppositeDirection(activeView.RightDirection))
                    {
                        leftFaceFraming = framingSolid.Faces.get_Item(i) as PlanarFace;

                    }
                    else if (framingSolid.Faces.get_Item(i).ComputeNormal(UV.Zero).IsSameDirection(activeView.RightDirection))
                    {
                        rightFaceFraming = framingSolid.Faces.get_Item(i) as PlanarFace;
                    }
                    else if (framingSolid.Faces.get_Item(i).ComputeNormal(UV.Zero).IsSameDirection(activeView.UpDirection)
                        && (framingSolid.Faces.get_Item(i) as PlanarFace).Origin.Z > originZFramingInview)
                    {
                        topFaceFraming = framingSolid.Faces.get_Item(i) as PlanarFace;
                        originZFramingInview = (framingSolid.Faces.get_Item(i) as PlanarFace).Origin.Z;
                    }

                    else if (framingSolid.Faces.get_Item(i).ComputeNormal(UV.Zero).IsOppositeDirection(activeView.UpDirection)
                        && (framingSolid.Faces.get_Item(i) as PlanarFace).Origin.Z < originZFramingInview)
                    {
                        botFaceFraming = framingSolid.Faces.get_Item(i) as PlanarFace;
                        //minOrizinZFaceFraming = (framingSolid.Faces.get_Item(i) as PlanarFace).Origin.Z;
                    }
                }
                foreach (Face item in floorSolid?.Faces)
                {
                    if (item.ComputeNormal(UV.Zero).IsSameDirection(activeView.UpDirection))
                    {
                        topFaceFloor = item as PlanarFace;
                    }
                    if (item.ComputeNormal(UV.Zero).IsOppositeDirection(activeView.UpDirection))
                    {
                        botFaceFloor = item as PlanarFace;
                    }
                }
            }
            else if(floorSolid == null)
            {
                foreach (Face item in framingSolid.Faces)
                {
                    if(item.ComputeNormal(UV.Zero).IsSameDirection(activeView.RightDirection))
                    {
                        rightFaceFraming = item as PlanarFace;
                    }
                    else if(item.ComputeNormal(UV.Zero).IsOppositeDirection(activeView.RightDirection))
                    {
                        leftFaceFraming = item as PlanarFace;
                    }
                    else if(item.ComputeNormal(UV.Zero).IsSameDirection(activeView.UpDirection))
                    {
                        topFaceFraming = item as PlanarFace;
                    }
                    else if(item.ComputeNormal(UV.Zero).IsOppositeDirection(activeView.UpDirection))
                    {
                        botFaceFraming = item as PlanarFace;
                    }
                }
            }
            ReferenceArray framingLRRefArray = new ReferenceArray();
            ReferenceArray framingTBRefArray1 = new ReferenceArray();
            ReferenceArray framingTBRefArray2 = new ReferenceArray();
            ReferenceArray framingTBRefArray3 = new ReferenceArray();

            Reference leftFaceFramingRef = leftFaceFraming.Reference;
            Reference rightFaceFramingRef = rightFaceFraming.Reference;
            Reference topFaceFramingRef = topFaceFraming.Reference;
            Reference botFaceFramingRef = botFaceFraming.Reference;
            Reference topFaceFloorRef = topFaceFloor?.Reference;
            Reference botFaceFloorRef = botFaceFloor?.Reference;

            framingLRRefArray.Append(leftFaceFramingRef);
            framingLRRefArray.Append(rightFaceFramingRef);
            if(framingInView.AsValue("z Offset Value").ValueNumber == floorinView?.AsValue("Height Offset From Level").ValueNumber)
            {
                framingTBRefArray1.Append(topFaceFloorRef);
                framingTBRefArray1.Append(botFaceFloorRef);
                framingTBRefArray1.Append(botFaceFramingRef);
                framingTBRefArray2.Append(topFaceFloorRef);
                framingTBRefArray2.Append(botFaceFramingRef);
            }
            else
            {
                framingTBRefArray1.Append(topFaceFloorRef);
                framingTBRefArray1.Append(botFaceFloorRef);
                framingTBRefArray1.Append(botFaceFramingRef);
                framingTBRefArray1.Append(topFaceFramingRef);
                framingTBRefArray2.Append(topFaceFramingRef);
                framingTBRefArray2.Append(botFaceFramingRef);
            }
            
           

            var dimensionOriginLR = framingInViewOrigin + 400.0.Milimet2Feet() * activeView.UpDirection;
            var dimensionOriginTB1 = framingInViewOrigin + 400.0.Milimet2Feet() * -activeView.RightDirection;
            var dimensionOriginTB2 = framingInViewOrigin + 600.0.Milimet2Feet() * -activeView.RightDirection;
            var dimensionLineLR = Line.CreateBound(dimensionOriginLR, dimensionOriginLR + activeView.RightDirection);
            var dimensionLineTB1 = Line.CreateBound(dimensionOriginTB1, dimensionOriginTB1 + activeView.UpDirection);
            var dimensionLineTB2 = Line.CreateBound(dimensionOriginTB2, dimensionOriginTB2 + activeView.UpDirection);
            

            //if(floorSolid != null)
            //{
            //    doc.Create.NewDimension(activeView, dimensionLineLR, framingLRRefArray);
            //    doc.Create.NewDimension(activeView, dimensionLineTB1, framingTBRefArray1);
            //    doc.Create.NewDimension(activeView, dimensionLineTB2, framingTBRefArray2);
            //}
            //else if(floorSolid == null)
            //{
            //    doc.Create.NewDimension(activeView, dimensionLineLR, framingLRRefArray);
            //    doc.Create.NewDimension(activeView, dimensionLineTB1, framingTBRefArray1);
            //}


            //var framingInView = revitData.FramingInstancesInView.FirstOrDefault();
            //var framingSolid = framingInView.GetSingleSolidValidRef();
            //var framingInViewOrigin = framingInView.GetTotalTransform().Origin;
            var rebarSetIdsInview = revitData.RebarsInView.OrderBy(x=>x.GetShapeDrivenAccessor().GetDistributionPath().Origin.Z).Where(x=>x.Quantity >1).Select(x => x.Id).ToList();
            var rebarSingleIdsInView = revitData.RebarsInView.Where(x => x.Quantity == 1).Select(x => x.Id).ToList();
            
            MultiReferenceAnnotationType multiRefAnnoType = revitData.MultiRefAnnoTypes.Where(x => x.Name == "Count_Diameter").SingleOrDefault();
            //RebarUtil.CreateMultiRebarTag(new List<ElementId>{ rebarSetIdsInview[0]}, multiRefAnnoType, 0.55);
            //RebarUtil.CreateMultiRebarTag(new List<ElementId> { rebarSetIdsInview[1] }, multiRefAnnoType, 0.15);           
            
            var groupByRebarNum = rebarSetIdsInview.GroupBy(x => x.ToRevitElement().AsValue("Rebar Number").ValueText).ToList();
            foreach (var item in groupByRebarNum)
            {

                var a = item.Select(x => x).ToList();
                    RebarUtil.CreateMultiRebarTag(item.Select(x=>x).ToList(), multiRefAnnoType, 0.55);
            }
            //MultiReferenceAnnotationOptions multiRefAnnoOtps = new MultiReferenceAnnotationOptions(multiRefAnnoType);
            //multiRefAnnoOtps.TagHeadPosition = new XYZ(0, 1, 0);
            //multiRefAnnoOtps.DimensionLineOrigin = framingInViewOrigin + 0.6 * activeView.UpDirection;
            //multiRefAnnoOtps.DimensionLineDirection = activeView.RightDirection;
            //multiRefAnnoOtps.DimensionPlaneNormal = activeView.ViewDirection;
            //multiRefAnnoOtps.SetElementsToDimension(rebarSetIdsInview);
            //var mra = MultiReferenceAnnotation.Create(doc, activeView.Id, multiRefAnnoOtps);
            //var rebarTag = doc.GetElement(mra.TagId) as IndependentTag;
            //rebarTag.TagHeadPosition = framingInViewOrigin + 0.5941 * (activeView.UpDirection) + 1 * (activeView.RightDirection);



            tx.Commit();

            return Result.Succeeded;
        }
        }
    }

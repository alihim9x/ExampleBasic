using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;


namespace Utility
{
    public static class RebarUtil
    {
        private static RevitData revitData = RevitData.Instance;

        public static IEnumerable<Autodesk.Revit.DB.Curve> GetCenterlineCurves(this Model.Entity.Rebar rebar)
        {
            if(rebar.SingleRebar != null)
            {
                return rebar.SingleRebar.GetCenterlineCurves(false,false,false,Autodesk.Revit.DB.Structure.MultiplanarOption.IncludeOnlyPlanarCurves,0);
            }
            return rebar.RebarInsystem.GetCenterlineCurves(false, false, false);
        }
        public static Autodesk.Revit.DB.Line GetDistributionPath(this Model.Entity.Rebar rebar)
        {
            if(rebar.SingleRebar != null)
            {
                var rsda = rebar.SingleRebar.GetShapeDrivenAccessor();
                return rsda.GetDistributionPath();
            }
            return rebar.RebarInsystem.GetDistributionPath();   
        }
        public static IEnumerable<Autodesk.Revit.DB.Curve> GetDrivingCurves (this Model.Entity.Rebar rebar)
        {
            if(rebar.SingleRebar != null)
            {
                var rsda = rebar.SingleRebar.GetShapeDrivenAccessor();
                return rsda.ComputeDrivingCurves();
            }
            throw new Model.Exception.RebarInSysHasNotDrivenCurves();
        }
        public static Autodesk.Revit.DB.XYZ MaxOccurOriginZCenterLineCurve (this Model.Entity.Rebar rebar)
        {
            
                var centerLineCurves = rebar.GetCenterlineCurves();
                var originZ = new List<Autodesk.Revit.DB.XYZ>();
                foreach (var item in centerLineCurves)
                {
                    originZ.Add(item.GetEndPoint(0));
                    originZ.Add(item.GetEndPoint(1));
                }
                return originZ.MaxRepeatedItem();
           
        }
        public static void SetValue(this Model.Entity.Rebar rebar, string name, object obj)
        {
            if(rebar.SingleRebar != null)
            {
                
                rebar.SingleRebar.SetValue(name,obj);
               
            }
            if (rebar.RebarInsystem != null)
            {
                rebar.RebarInsystem.SetValue(name,obj);
            }
        }
        public static Model.ParameterValue AsValue(this Model.Entity.Rebar rebar, string paramname)
        {
            if(rebar.SingleRebar != null)
            {
               return rebar.SingleRebar.AsValue(paramname);
            }
            return rebar.RebarInsystem.AsValue(paramname);
        }
        public static Autodesk.Revit.DB.ElementId GetHostId(this Model.Entity.Rebar rebar)
        {
            if(rebar.SingleRebar != null)
            {
                return rebar.SingleRebar.GetHostId();
            }
            return rebar.RebarInsystem.GetHostId();
        }
        public static void CreateMultiRebarTag (this List<Autodesk.Revit.DB.ElementId> rebarIds
            , Autodesk.Revit.DB.MultiReferenceAnnotationType multiRebarTagType,double y)
        {

            Autodesk.Revit.DB.MultiReferenceAnnotationOptions multiRebarTagOpts = new Autodesk.Revit.DB.MultiReferenceAnnotationOptions(multiRebarTagType);
            multiRebarTagOpts.TagHeadPosition = new Autodesk.Revit.DB.XYZ(0, 1, 0);
            multiRebarTagOpts.DimensionLineOrigin = revitData.FramingInstancesInView.FirstOrDefault().GetTotalTransform().Origin + y * revitData.ActiveView.UpDirection;
           

            multiRebarTagOpts.DimensionLineDirection = revitData.ActiveView.RightDirection;
            multiRebarTagOpts.DimensionPlaneNormal = revitData.ActiveView.ViewDirection;
            multiRebarTagOpts.SetElementsToDimension(rebarIds);
            var mra = Autodesk.Revit.DB.MultiReferenceAnnotation.Create(revitData.Document, revitData.ActiveView.Id, multiRebarTagOpts);
            var rebarTag = revitData.Document.GetElement(mra.TagId) as Autodesk.Revit.DB.IndependentTag;
            rebarTag.TagHeadPosition = revitData.FramingInstancesInView.FirstOrDefault().GetTotalTransform().Origin + (y-0.00559) * (revitData.ActiveView.UpDirection) + 1 * (revitData.ActiveView.RightDirection);
        }
        public static void SetDirectionXYRebar(this List<Model.Entity.Rebar> rebars )
        {
            var rebarCate = new List<Autodesk.Revit.DB.BuiltInCategory>
            {
                Autodesk.Revit.DB.BuiltInCategory.OST_Rebar
            };
            ParameterUtil.AddParameter("Rebar Direction (XY)", Model.AddParameterType.Instance, Model.DefinitionGroupType.KetCau
                , Autodesk.Revit.DB.ParameterType.Text, Autodesk.Revit.DB.BuiltInParameterGroup.PG_CONSTRUCTION, rebarCate);

            foreach (var item in rebars)
            {
                if(item.AsValue("Host Category").ValueNumber == (double)Autodesk.Revit.DB.Structure.RebarHostCategory.StructuralFraming)
                {
                    var hostIdElem = item?.GetHostId().ToRevitElement();
                    var curveframing = (hostIdElem.Location as Autodesk.Revit.DB.LocationCurve)?.Curve as Autodesk.Revit.DB.Line;
                    var directionframing = curveframing?.Direction;
                    if(curveframing != null && directionframing != null)
                    {
                        if (directionframing.IsXOrY())

                        {
                            item.SetValue("Rebar Direction (XY)", "X");
                        }
                        else
                        {
                            item.SetValue("Rebar Direction (XY)", "Y");
                        }
                    }
                   
                }
                else if(item.AsValue("Host Category").ValueNumber == (double)Autodesk.Revit.DB.Structure.RebarHostCategory.Floor)
                {
                    var shapedrivenDirRebar = item.DistributionDirection;

                    if (shapedrivenDirRebar.IsXOrY())
                    {
                        item.SetValue("Rebar Direction (XY)", "Y");
                    }
                    else
                    {
                        item.SetValue("Rebar Direction (XY)", "X");
                    }
                }
            }
        }
        public static void SetlayerTBRebar (this List<Autodesk.Revit.DB.Element> hostRebar)
        {
            var rebarCate = new List<Autodesk.Revit.DB.BuiltInCategory>
            {
                Autodesk.Revit.DB.BuiltInCategory.OST_Rebar
            };
            
            ParameterUtil.AddParameter("Rebar Layer", Model.AddParameterType.Instance, Model.DefinitionGroupType.KetCau
                , Autodesk.Revit.DB.ParameterType.Text, Autodesk.Revit.DB.BuiltInParameterGroup.PG_CONSTRUCTION, rebarCate);
            
            foreach (var item in hostRebar)
            {
                
                var topRebar = new List<Model.Entity.Rebar>();
                var botRebar = new List<Model.Entity.Rebar>();
                var hostLocationCurve = (item.Location as Autodesk.Revit.DB.LocationCurve)?.Curve as Autodesk.Revit.DB.Line;
                if(hostLocationCurve != null)
                {
                    var originHostRebar = item.GetOriginalSolid()?.ComputeCentroid();
                    if(originHostRebar != null)
                    {
                        var rebarInHost = ElementUtil.GetModelEntityRebarInHost(item);
                        foreach (var item1 in rebarInHost)
                        {
                            if (item1.AsValue("Host Category").ValueNumber == (double)Autodesk.Revit.DB.Structure.RebarHostCategory.StructuralFraming)
                            {

                                var originZRebar = item1.MaxOccurOriginZCenterLineCurve;
                                if (item1.DistributionDirection.IsPerpendicularDirection(((item1.GetHostId().ToRevitElement().Location
                                    as Autodesk.Revit.DB.LocationCurve).Curve as Autodesk.Revit.DB.Line).Direction))
                                {

                                    var hostRebar1 = item1?.GetHostId().ToRevitElement();
                                    if (originZRebar.Z > originHostRebar.Z + 0.0.Milimet2Feet())
                                    {
                                        topRebar.Add(item1);
                                    }
                                    if (originZRebar.Z < originHostRebar.Z - 0.0.Milimet2Feet())
                                    {
                                        botRebar.Add(item1);
                                    }
                                    List<Model.Entity.Rebar> top = topRebar.OrderByDescending(x => x.MaxOccurOriginZCenterLineCurve.Z).ToList();
                                    List<Model.Entity.Rebar> bot = botRebar.OrderBy(x => x.MaxOccurOriginZCenterLineCurve.Z).ToList();
                                    int index = 0;
                                    for (int i = 0; i < top.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            index = i + 1;
                                            top[i].SetValue("Rebar Layer", $"T{index}");
                                        }
                                        else if (i != 0 && Math.Abs(top[i].MaxOccurOriginZCenterLineCurve.Z - top[i - 1].MaxOccurOriginZCenterLineCurve.Z) < 10.0.Milimet2Feet())
                                        {

                                            top[i].SetValue("Rebar Layer", $"T{index}");
                                        }
                                        else if (i != 0 && Math.Abs(top[i].MaxOccurOriginZCenterLineCurve.Z - top[i - 1].MaxOccurOriginZCenterLineCurve.Z) > 10.0.Milimet2Feet())
                                        {
                                            index++;
                                            top[i].SetValue("Rebar Layer", $"T{index}");
                                        }

                                    }
                                    for (int i = 0; i < bot.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            index = i + 1;
                                            bot[i].SetValue("Rebar Layer", $"B{index}");
                                        }
                                        else if (i != 0 && Math.Abs(bot[i].MaxOccurOriginZCenterLineCurve.Z - bot[i - 1].MaxOccurOriginZCenterLineCurve.Z) < 10.0.Milimet2Feet())
                                        {

                                            bot[i].SetValue("Rebar Layer", $"B{index}");
                                        }
                                        else if (i != 0 && Math.Abs(bot[i].MaxOccurOriginZCenterLineCurve.Z - bot[i - 1].MaxOccurOriginZCenterLineCurve.Z) > 10.0.Milimet2Feet())
                                        {
                                            index++;
                                            bot[i].SetValue("Rebar Layer", $"B{index}");
                                        }
                                    }
                                }
                            }
                            else if (item1.AsValue("Host Category").ValueNumber == (double)Autodesk.Revit.DB.Structure.RebarHostCategory.Floor)
                            {

                                var originZRebar = item1.MaxOccurOriginZCenterLineCurve;
                                if (item1.AsValue("Bar Diameter").ValueNumber != 6)
                                {

                                    var hostRebar1 = item1?.GetHostId().ToRevitElement();
                                    if (originZRebar.Z > originHostRebar.Z + 0.0.Milimet2Feet())
                                    {
                                        topRebar.Add(item1);
                                    }
                                    if (originZRebar.Z < originHostRebar.Z - 0.0.Milimet2Feet())
                                    {
                                        botRebar.Add(item1);
                                    }
                                    List<Model.Entity.Rebar> top = topRebar.OrderByDescending(x => x.MaxOccurOriginZCenterLineCurve.Z).ToList();
                                    List<Model.Entity.Rebar> bot = botRebar.OrderBy(x => x.MaxOccurOriginZCenterLineCurve.Z).ToList();
                                    int index = 0;
                                    for (int i = 0; i < top.Count; i++)
                                    {

                                        if (i == 0)
                                        {
                                            index = i + 1;
                                            top[i].SetValue("Rebar Layer", $"T{index}");
                                        }
                                        else if (i != 0 && Math.Abs(top[i].MaxOccurOriginZCenterLineCurve.Z - top[i - 1].MaxOccurOriginZCenterLineCurve.Z) < 0.0.Milimet2Feet())
                                        {

                                            top[i].SetValue("Rebar Layer", $"T{index}");
                                        }
                                        else if (i != 0 && Math.Abs(top[i].MaxOccurOriginZCenterLineCurve.Z - top[i - 1].MaxOccurOriginZCenterLineCurve.Z) > 0.0.Milimet2Feet())
                                        {
                                            index++;
                                            top[i].SetValue("Rebar Layer", $"T{index}");
                                        }

                                    }
                                    for (int i = 0; i < bot.Count; i++)
                                    {
                                        if (i == 0)
                                        {
                                            index = i + 1;
                                            bot[i].SetValue("Rebar Layer", $"B{index}");
                                        }
                                        else if (i != 0 && Math.Abs(bot[i].MaxOccurOriginZCenterLineCurve.Z - bot[i - 1].MaxOccurOriginZCenterLineCurve.Z) < 0.0.Milimet2Feet())
                                        {

                                            bot[i].SetValue("Rebar Layer", $"B{index}");
                                        }
                                        else if (i != 0 && Math.Abs(bot[i].MaxOccurOriginZCenterLineCurve.Z - bot[i - 1].MaxOccurOriginZCenterLineCurve.Z) > 0.0.Milimet2Feet())
                                        {
                                            index++;
                                            bot[i].SetValue("Rebar Layer", $"B{index}");
                                        }
                                    }
                                }

                            }
                        }
                    }
                    
                }
                
                
            }           
        }       
    }
}

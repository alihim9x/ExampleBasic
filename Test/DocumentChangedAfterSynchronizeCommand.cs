using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using SingleData;
using Autodesk.Revit.Attributes;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    
    class DocumentChangedAfterSynchronizeCommand : IExternalCommand
        
    {
       
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (Singleton.Instance != null)
            {
                TaskDialog.Show("Revit", "Sự kiện DocumentChanged đã được tính hợp!");
                return Result.Succeeded;
            }
            var singleTon = Singleton.Instance = new Singleton();
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;
            var app = revitData.Application;
            //app.DocumentChanged += App_DocumentChanged;
            app.DocumentSynchronizedWithCentral += App_DocumentSynchronizedWithCentral1;

            return Result.Succeeded;
        }

        private void App_DocumentSynchronizedWithCentral1(object sender, Autodesk.Revit.DB.Events.DocumentSynchronizedWithCentralEventArgs e)
        {
            var singleTon = Singleton.Instance = new Singleton();
            var revitData = singleTon.RevitData;
           
            var app = revitData.Application;
            app.DocumentChanged += App_DocumentChanged;
        }

        

        private void App_DocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            var singleTon = Singleton.Instance;
            var revitData = singleTon.RevitData;
            var modelData = singleTon.ModelData;
            var doc = revitData.Document;
            var structuralCategories = modelData.StructuralCategories;
            if(modelData.ToggleEvent)
            {
                var elemIds = e.GetAddedElementIds();
                var elems = elemIds.Select(x => doc.GetElement(x))
                    .Where(x=>structuralCategories.Contains((BuiltInCategory)x.Category.Id.IntegerValue));
                foreach (var elem in elems)
                {
                    if (elem is Wall)
                    {
                        if (((Wall)elem).StructuralUsage == Autodesk.Revit.DB.Structure.StructuralWallUsage.NonBearing)
                        {
                            continue;
                        }
                    }
                    TaskDialog.Show("Revit", $"Đối tượng được thêm vào: Name: {elem.Name} - Category: {elem.Category.Name}");
                   
                }
            }
            
        }
        [Transaction(TransactionMode.Manual)]
        public class ToggleEvent : IExternalCommand
        {
            public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
            {
                var modelData = Singleton.Instance.ModelData;
                modelData.ToggleEvent = !modelData.ToggleEvent;
                if(modelData.ToggleEvent)
                {
                    TaskDialog.Show("Revit", "Kích hoạt sự kiện DocumentChanged.");
                }
                else
                {
                    TaskDialog.Show("Revit", "Hủy bỏ sự kiện DocumentChanged.");
                }
                return Result.Succeeded;
            }
        }
    }
   
}

using Autodesk.Revit.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SingleData;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;

namespace Test
{
    [Transaction(TransactionMode.Manual)]
    class Class2 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            if (Singleton.Instance != null) return Result.Succeeded;
            var singleTon = Singleton.Instance = new Singleton();
            var revitData = singleTon.RevitData;
            revitData.UIApplication = commandData.Application;


            var app = revitData.Application;


            app.DocumentChanged += App_DocumentChanged; // Sự kiện: kiểm tra xem status trong Project information nếu là allow thì cho phép thay đổi 

            return Result.Succeeded;
        }

        private void App_DocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            var singleTon = Singleton.Instance;
            var modelData = singleTon.ModelData;
            var doc = singleTon.RevitData.Document;
            if(modelData.ToggleEvent)
            {
                var elemIds = e.GetAddedElementIds(); // Những Id của đối tượng được thêm mới (load hoặc là các đối tượng được tạo ra trong project)
                var strucElems = elemIds.Select(x => doc.GetElement(x)).Where(x => modelData.StructuralCategories
                                                        .Contains((BuiltInCategory)x.Category.Id.IntegerValue));
                foreach (var item in strucElems)
                {
                    TaskDialog.Show("Revit", $"{item.Category.Name}: {item.Name}");
                }
            }
        }
    }
    [Transaction(TransactionMode.Manual)]
    class ToggleEvent : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Singleton.Instance.ModelData.ToggleEvent = !Singleton.Instance.ModelData.ToggleEvent;
            TaskDialog.Show("Revit", $"Event:{Singleton.Instance.ModelData.ToggleEvent}");
            return Result.Succeeded;
        }
    }

}

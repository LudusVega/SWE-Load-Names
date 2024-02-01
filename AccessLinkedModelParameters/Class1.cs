using Autodesk.Revit.DB;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;


public void AccessLinkedModelParameters(Document hostDoc)
{
    // Find all Revit link instances in the host model
    FilteredElementCollector collector = new FilteredElementCollector(hostDoc);
    var revitLinkInstances = collector.OfCategory(BuiltInCategory.OST_RvtLinks)
                                      .WhereElementIsNotElementType()
                                      .Cast<RevitLinkInstance>();

    foreach (var linkInstance in revitLinkInstances)
    {
        // Get the Document of the linked model
        Document linkedDoc = linkInstance.GetLinkDocument();

        // If the linked document is valid, access its elements
        if (linkedDoc != null)
        {
            FilteredElementCollector linkedCollector = new FilteredElementCollector(linkedDoc);
            var linkedElements = linkedCollector.OfClass(typeof(FamilyInstance)) // or any other class
                                                 .Where(e => e.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Rooms);

            foreach (var element in linkedElements)
            {
                Parameter roomNumberParam = element.get_Parameter(BuiltInParameter.ROOM_NUMBER);
                if (roomNumberParam != null)
                {
                    string roomNumber = roomNumberParam.AsString();
                    // Process room number as needed
                }
            }
        }
    }
}

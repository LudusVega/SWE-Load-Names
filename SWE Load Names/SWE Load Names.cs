using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;


namespace SWE_Load_Names
{
    

    [Transaction(TransactionMode.Manual)]
    [Regeneration(RegenerationOption.Manual)]

    public class ElectricalSystemInfoExtractor : IExternalCommand
    {
        private LocationPoint point;
        private GeometryElement spaceGeometry;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            var electricalSystems = collector.OfClass(typeof(ElectricalSystem));
            var circuitList = electricalSystems.Cast<ElectricalSystem>().ToList();
            HashSet<string> spaceNumbers = new HashSet<string>(100);
            HashSet<string> elementNames = new HashSet<string>(100);
            StringBuilder sb = new StringBuilder();

            foreach (var circuit in circuitList)
            {
                Parameter loadClassParam = circuit.get_Parameter(BuiltInParameter.CIRCUIT_LOAD_CLASSIFICATION_PARAM);
                
                var loadClass = loadClassParam.AsString();
                var elementSet = circuit.Elements;

                foreach(Element element in  elementSet)
                {
                    var elementName = element.Name;
                    elementNames.Add(elementName);
                    var spaceNumber = element.get_Parameter(BuiltInParameter.SPACE_ASSOC_ROOM_NAME);
                    spaceNumbers.Add(spaceNumber.AsString());
                }
                
                sb.AppendLine($"System ID: {circuit.CircuitNumber}, Load Class: {loadClass}, Element Description: {string.Join(", ", spaceNumbers)}");
            }
            TaskDialog.Show("Electrical System Info", sb.ToString());
            return Result.Succeeded;
        }
                
    }
}
    


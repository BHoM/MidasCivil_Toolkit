using System.IO;
using System.Collections.Generic;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            string path = CreateSectionFile("ELEMENT");
            List<string> midasElements = new List<string>();

            CreateGroups(bars);

            foreach (Bar bar in bars)
            {
                if (!(bar.Release == null)&&bar.FEAType==BarFEAType.TensionOnly)
                {
                    Engine.Reflection.Compute.RecordError("Tension only elements cannot support bar releases in Midas");
                }

                if (!(bar.Release == null) && bar.Release.Name!="FixFix")
                {
                        BarReleaseAssignment(bar.CustomData[AdapterId].ToString(), bar.Release.Name, "FRAME-RLS");
                }

                midasElements.Add(Engine.MidasCivil.Convert.ToMCElement(bar));
            }

            File.AppendAllLines(path, midasElements);

            return true;
        }

        private bool CreateCollection(IEnumerable<FEMesh> meshes)
        {
            string path = CreateSectionFile("ELEMENT");
            List<string> midasElements = new List<string>();

            CreateGroups(meshes);

            foreach (FEMesh mesh in meshes)
            {
                midasElements.Add(Engine.MidasCivil.Convert.ToMCElement(mesh));
            }

            File.AppendAllLines(path, midasElements);

            return true;
        }

    }
}
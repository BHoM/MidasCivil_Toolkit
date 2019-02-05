using System;
using System.Collections.Generic;
using BH.oM.Structure.Elements;

namespace BH.Adapter.MidasCivil
{
    public partial class MidasCivilAdapter
    {
        private bool CreateCollection(IEnumerable<Bar> bars)
        {
            string path = CreateSectionFile("ELEMENT");

            foreach (Bar bar in bars)
            {
                Engine.MidasCivil.Convert.ToMCElement(bar,path);
            }

            return true;
        }

        private bool CreateCollection(IEnumerable<FEMesh> meshes)
        {
            string path = CreateSectionFile("ELEMENT");

            foreach (FEMesh mesh in meshes)
            {
                Engine.MidasCivil.Convert.ToMCElement(mesh, path);
            }

            return true;
        }
    }
}
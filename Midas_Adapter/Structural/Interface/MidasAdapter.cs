using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BHOME = BHoM.Structural.Elements;
using BHoML = BHoM.Structural.Loads;
using BHoM.Structural.Interface;
using BHoM.Base;


namespace Midas_Adapter.Structural.Interface
{
    public partial class MidasAdapter : BHoM.Structural.Interface.IElementAdapter
    {

        //public string Path;
        public MidasAdapter(string fileName)
        {
            Filename = fileName;
        }

        public string Filename
        {
            get; set;
        }

        public ObjectSelection Selection
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public List<string> GetBars(out List<BHOME.Bar> bars, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFEMeshes(out List<BHOME.FEMesh> meshes, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetGrids(out List<BHOME.Grid> grids, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetGroups(out List<IGroup> groups, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetLevels(out List<BHOME.Storey> levels, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetLoadcases(out List<BHoML.ICase> cases)
        {
            throw new NotImplementedException();
        }

        public bool GetLoads(out List<BHoML.ILoad> loads, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetNodes(out List<BHOME.Node> nodes, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetOpenings(out List<BHOME.Opening> opening, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetPanels(out List<BHOME.Panel> panels, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public List<string> GetRigidLinks(out List<BHOME.RigidLink> links, List<string> ids = null)
        {
            throw new NotImplementedException();
        }

        public bool Run()
        {
            throw new NotImplementedException();
        }

        public bool SetBars(List<BHOME.Bar> bars, out List<string> ids)
        {
            throw new NotImplementedException();
        }

        public bool SetFEMeshes(List<BHOME.FEMesh> meshes, out List<string> ids)
        {
            throw new NotImplementedException();
        }

        public bool SetGrids(List<BHOME.Grid> grid, out List<string> ids)
        {
            throw new NotImplementedException();
        }

        public bool SetGroups(List<IGroup> groups, out List<string> ids)
        {
            throw new NotImplementedException();
        }

        public bool SetLevels(List<BHOME.Storey> stores, out List<string> ids)
        {
            throw new NotImplementedException();
        }

        public bool SetLoadcases(List<BHoML.ICase> cases)
        {
            throw new NotImplementedException();
        }

        public bool SetLoads(List<BHoML.ILoad> loads)
        {
            throw new NotImplementedException();
        }

        public bool SetNodes(List<BHOME.Node> nodes, out List<string> ids)
        {
            Structural.Elements.NodeIO.CreateNodes(Filename, nodes, out ids);
            return true;
        }

        public bool SetOpenings(List<BHOME.Opening> opening, out List<string> ids)
        {
            throw new NotImplementedException();
        }

        public bool SetPanels(List<BHOME.Panel> panels, out List<string> ids)
        {
            throw new NotImplementedException();
        }

        public bool SetRigidLinks(List<BHOME.RigidLink> rigidLinks, out List<string> ids)
        {
            throw new NotImplementedException();
        }
    }
}

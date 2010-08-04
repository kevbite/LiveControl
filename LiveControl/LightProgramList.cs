using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kevsoft.LiveControl.Interfaces;
using Kevsoft.LiveControl.LightPrograms;

namespace Kevsoft.LiveControl
{
    /// <summary>
    /// List for a Light Program
    /// </summary>
    [Serializable]
    public class LightProgramList : List<ILightProgram>
    {
        /// <summary>
        /// Adds a pallet to the current list
        /// </summary>
        /// <returns></returns>
        public Pallet AddPallet()
        {
            //create the pallet
            Pallet p = new Pallet(Show.Instance.FixPatch);
            //add to list
            Add(p);
            //return pallet
            return p;
        }
    }
}

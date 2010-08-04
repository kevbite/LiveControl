using System;
namespace Kevsoft.LiveControl.Interfaces
{
    /// <summary>
    /// Interface used for DmxObsevers
    /// </summary>
    public interface IDmxObserver
    {
        DmxOutput Output { get; set; }
        
        void UpdateValues();

        bool CloseConnection();
        
    }
}

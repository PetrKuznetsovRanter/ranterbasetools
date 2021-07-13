using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RanterTools.Base
{
    public interface ICommand
    {
        /// <summary>
        /// Execute command.
        /// </summary>
        void Execute();
        
        /// <summary>
        /// Undo command.
        /// </summary>
        void Undo();
    }

}

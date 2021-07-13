using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Base
{
    /// <summary>
    /// Interface for command history.
    /// </summary>
    public interface ICommandHistory
    {
        /// <summary>
        /// Add command in executing history.
        /// </summary>
        /// <param name="command">Command.</param>
        void AddCommand(ICommand command);
        
        /// <summary>
        /// Run next executing command.
        /// </summary>
        void RunNextCommand();
        
        /// <summary>
        /// Undo last executed command.
        /// </summary>
        void UndoLastCommant();
    }

}

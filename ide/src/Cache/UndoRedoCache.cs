using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOAI.Cache.UndoRedo;

namespace MOAI.Cache
{
    public class UndoRedoCache
    {
        private Stack<IUndoRedoState> m_Undo = new Stack<IUndoRedoState>();
        private Stack<IUndoRedoState> m_Redo = new Stack<IUndoRedoState>();

        /// <summary>
        /// Whether an undo operation can be performed.
        /// </summary>
        public bool CanUndo
        {
            get
            {
                return (this.m_Undo.Count > 0);
            }
        }

        /// <summary>
        /// Whether a redo operation can be performed.
        /// </summary>
        public bool CanRedo
        {
            get
            {
                return (this.m_Redo.Count > 0);
            }
        }

        /// <summary>
        /// Performs the next undo operation.
        /// </summary>
        public void Undo()
        {
            if (!this.CanUndo)
                return;

            IUndoRedoState state = this.m_Undo.Pop();
            state.Undo();
            this.m_Redo.Push(state);
        }

        /// <summary>
        /// Performs the next redo operation.
        /// </summary>
        public void Redo()
        {
            if (!this.CanRedo)
                return;

            IUndoRedoState state = this.m_Redo.Pop();
            state.Redo();
            this.m_Undo.Push(state);
        }

        /// <summary>
        /// Pushes a new IUndoRedoState into the cache.  Automatically
        /// clears the redo stack.
        /// </summary>
        /// <param name="state">The state to push into the cache.</param>
        public void Push(IUndoRedoState state)
        {
            this.m_Undo.Push(state);
            this.m_Redo.Clear();
        }
    }
}

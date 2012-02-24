using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Cache.UndoRedo
{
    public interface IUndoRedoState
    {
        void Undo();
        void Redo();
    }
}

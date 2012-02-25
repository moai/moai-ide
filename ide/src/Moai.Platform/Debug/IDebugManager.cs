using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Management;

namespace Moai.Platform.Debug
{
    public interface IDebugManager : IManager
    {
        event EventHandler DebugStart;
        event EventHandler DebugPause;
        event EventHandler DebugContinue;
        event EventHandler DebugStop;

        bool Start(Project project);
        bool StartWithout(Project project);
        void Pause();
        void Stop();
        void Evaluate(string expression, Action<object> callback);

        bool Running { get; }
        bool Paused { get; }
        List<IBreakpoint> Breakpoints { get; }
    }
}

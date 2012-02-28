using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Debug;
using Moai.Platform.Designers;
using Moai.Platform.Menus;
using Moai.Platform.Cache;
using Moai.Platform.Tools;
using Moai.Platform.Management;

namespace Moai.Platform
{
    public interface IRootManager : IManager
    {
        event EventHandler SolutionLoaded;
        event EventHandler SolutionUnloaded;
        event EventHandler IDEOpened;

        Solution ActiveSolution { get; }
        Project ActiveProject { get; }
        ICacheManager CacheManager { get; }
        IDebugManager DebugManager { get; }
        IDesignerManager DesignersManager { get; }
        MenusManager MenuManager { get; }
        IToolsManager ToolsManager { get; set; }
        IIDE IDE { get; }
        Dictionary<string, string> Settings { get; }

        void Initalize();
        void Start();
        void Stop();

        void LoadSolution(string file);
        void UnloadSolution();
    }
}

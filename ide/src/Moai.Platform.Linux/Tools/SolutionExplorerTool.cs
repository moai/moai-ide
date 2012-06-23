using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Moai.Platform.Management;
using Moai.Platform.Linux.UI;
using Qyoto;
using log4net;

namespace Moai.Platform.Linux.Tools
{
    public partial class SolutionExplorerTool : Tool
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(SolutionExplorerTool));

        private SolutionModel m_CurrentModel = null;
        private LinuxImageList m_LinuxImageList = null;

        public SolutionExplorerTool()
        {
            InitializeComponent();
            this.Title = "Solution Explorer";

            // Set the image list.
            this.m_LinuxImageList = Associations.ImageList as LinuxImageList;

            // Set the solution model.
            m_Log.Debug("Setting solution model to tool window.");
            this.c_TreeView.HeaderHidden = true;
            this.m_CurrentModel = new SolutionModel(this);
            LinuxNativePool.Instance.Retain(this.m_CurrentModel);
            this.c_TreeView.SetModel(this.m_CurrentModel);
            m_Log.Debug("Solution model has been initialized.");

        }

        ~SolutionExplorerTool()
        {
            //LinuxNativePool.Instance.Release();
        }

        public override void OnSolutionLoaded()
        {
            base.OnSolutionLoaded();

            this.m_CurrentModel.Refresh();
        }

        public override void OnSolutionUnloaded()
        {
            base.OnSolutionLoaded();

            this.m_CurrentModel.Refresh();
        }

        public override ToolPosition DefaultState
        {
            get
            {
                return ToolPosition.DockRight;
            }
        }

        private class SolutionModel : QAbstractItemModel
        {
            private static readonly ILog m_Log = LogManager.GetLogger(typeof(SolutionModel));

            private SolutionExplorerTool m_ExplorerTool = null;
            private SolutionItem m_RootItem = new SolutionItem();

            /// <summary>
            /// Creates a new solution model based on the current system.
            /// </summary>
            public SolutionModel(SolutionExplorerTool tool)
            {
                m_Log.Debug("Creating new solution model.");
                this.m_ExplorerTool = tool;
            }

            public void Refresh()
            {
                this.Reset();
            }

            public override QModelIndex Index(int row, int column, QModelIndex parent)
            {
                string start = "Index at " + row + ", " + column + " requested with parent " + this.SerializeModelIndex(parent) + "";

                if (!this.HasIndex(row, column, parent))
                {
                    m_Log.Debug(start + " (answer: empty).");
                    return new QModelIndex();
                }

                SolutionItem parentItem;
                if (!parent.IsValid())
                    parentItem = this.m_RootItem;
                else
                    parentItem = parent.InternalPointer() as SolutionItem;

                SolutionItem childItem = parentItem[row];
                if (childItem != null)
                {
                    QModelIndex model = this.CreateIndex(row, column, childItem);
                    m_Log.Debug(start + " (answer: " + this.SerializeModelIndex(model) + ").");
                    return model;
                }
                else
                {
                    m_Log.Debug(start + " (answer: empty).");
                    return new QModelIndex();
                }
            }

            public override QModelIndex Parent(QModelIndex child)
            {
                string start = "Parent of " + this.SerializeModelIndex(child) + " requested";

                if (!child.IsValid())
                {
                    m_Log.Debug(start + " (answer: empty).");
                    return new QModelIndex();
                }

                SolutionItem childItem = child.InternalPointer() as SolutionItem;
                SolutionItem parentItem = childItem.Parent;

                if (parentItem == this.m_RootItem || parentItem == null)
                {
                    // FIXME: This can cause weirdness with broken trees as the root node
                    // will be placed under folders if the structure is incorrect!
                    m_Log.Debug(start + " (answer: empty).");
                    return new QModelIndex();
                }

                QModelIndex result = this.CreateIndex(parentItem.Index, 0, parentItem);
                m_Log.Debug(start + " (answer: " + this.SerializeModelIndex(result) + ").");
                return result;
            }

            public override int RowCount(QModelIndex parent)
            {
                string start = "Row count of " + this.SerializeModelIndex(parent) + " requested";

                SolutionItem parentItem;
                if (parent.Column() > 0)
                {
                    m_Log.Debug(start + " (answer: 0).");
                    return 0;
                }

                if (!parent.IsValid())
                    parentItem = this.m_RootItem;
                else
                    parentItem = parent.InternalPointer() as SolutionItem;

                m_Log.Debug(start + " (answer: " + parentItem.Count + ").");
                return parentItem.Count;
            }

            public override int ColumnCount(QModelIndex parent)
            {
                m_Log.Debug("Column count of " + this.SerializeModelIndex(parent) + " requested (answer: 1).");

                return 1;
            }

            public override QVariant Data(QModelIndex index, int role)
            {
                string start = "Data of " + this.SerializeModelIndex(index) + " requested with role " + role;

                if (!index.IsValid())
                {
                    m_Log.Debug(start + " (answer: empty).");
                    return new QVariant();
                }

                if (role != (int)ItemDataRole.DisplayRole && role != (int)ItemDataRole.DecorationRole)
                {
                    m_Log.Debug(start + " (answer: empty).");
                    return new QVariant();
                }

                if (index.Column() == 0)
                {
                    if (role == (int)ItemDataRole.DisplayRole)
                    {
                        m_Log.Debug(start + " (answer: \"" + (index.InternalPointer() as SolutionItem).Text + "\").");
                        return (index.InternalPointer() as SolutionItem).Text;
                    }
                    else if (role ==(int)ItemDataRole.DecorationRole)
                    {
                        string key = (index.InternalPointer() as SolutionItem).ImageKey;
                        System.Drawing.Image image = this.m_ExplorerTool.m_LinuxImageList[key];
                        if (image == null)
                            m_Log.Error("No association icon found for image key '" + key + "'.");
                        else
                        {
                            m_Log.Debug(start + " (answer: <qicon>).");
                            return LinuxImageList.ConvertToQIcon(image);
                        }
                    }
                }

                m_Log.Debug(start + " (answer: empty).");
                return new QVariant();
            }

            public override uint Flags(QModelIndex index)
            {
                m_Log.Debug("Flags of " + this.SerializeModelIndex(index) + " requested.");

                if (!index.IsValid())
                    return 0;

                return (uint)(ItemFlag.ItemIsEnabled | ItemFlag.ItemIsSelectable);
            }

            private string SerializeModelIndex(QModelIndex index)
            {
                if (index == null)
                    return "<null>";
                else if (!index.IsValid())
                    return "<invalid>";

                return "<valid>";
                //return "(" + this.SerializeModelIndex(index.Parent()) + ", " + index.Row() + ", " + index.Column() + ")";
            }

            private class SolutionItem
            {
                private static readonly ILog m_Log = LogManager.GetLogger(typeof(SolutionModel));

                private bool m_IsRoot = false;
                private Solution m_Solution = null;
                private Project m_Project = null;
                private File m_File = null;

                /// <summary>
                /// Creates a new item object representing the root node.
                /// </summary>
                public SolutionItem()
                {
                    m_Log.Debug("Creating new solution item to represent root node.");
                    this.m_IsRoot = true;
                }

                /// <summary>
                /// Creates a new item object representing the solution information.
                /// </summary>
                /// <param name='solution'>The solution to represent.</param>
                public SolutionItem(Solution solution)
                {
                    if (solution == null)
                        throw new ArgumentNullException("solution");
                    try
                    {
                        m_Log.Debug("Creating new solution item to represent solution '" + solution.SolutionInfo.Name + "'.");
                    }
                    catch
                    {
                        m_Log.Debug("Creating new solution item to represent solution '<unknown>'.");
                    }
                    this.m_Solution = solution;
                }

                /// <summary>
                /// Creates a new item object representing the project information.
                /// </summary>
                /// <param name='project'>The project to represent.</param>
                public SolutionItem(Project project)
                {
                    if (project == null)
                        throw new ArgumentNullException("project");
                    try
                    {
                        m_Log.Debug("Creating new solution item to represent project '" + project.ProjectInfo.Name + "'.");
                    }
                    catch
                    {
                        m_Log.Debug("Creating new solution item to represent project '<unknown>'.");
                    }
                    this.m_Project = project;
                }
    
                /// <summary>
                /// Creates a new item object representing the file or folder information.
                /// </summary>
                /// <param name='project'>The file or folder to represent.</param>
                public SolutionItem(File file)
                {
                    if (file == null)
                        throw new ArgumentNullException("file");
                    try
                    {
                        m_Log.Debug("Creating new solution item to represent file '" + file.FileInfo.Name + "'.");
                    }
                    catch
                    {
                        m_Log.Debug("Creating new solution item to represent file '<unknown>'.");
                    }
                    this.m_File = file;
                }

                /// <summary>
                /// The image that represents this solution item.
                /// </summary>
                public string ImageKey
                {
                    get
                    {
                        m_Log.Debug("Retrieving image information for " + this.ToString() + " (answer: \"image\").");
                        if (this.m_IsRoot)
                            return "NotFound";
                        else if (this.m_Solution != null)
                            return (this.m_Solution.GetSyncData() as FileSyncData).ImageKey;
                        else if (this.m_Project != null)
                            return (this.m_Project.GetSyncData() as FileSyncData).ImageKey;
                        else if (this.m_File != null)
                            return (this.m_File.GetSyncData() as FileSyncData).ImageKey;
                        else
                            return "NotFound";
                    }
                }

                /// <summary>
                /// The text that represents this solution item.
                /// </summary>
                public string Text
                {
                    get
                    {
                        m_Log.Debug("Retrieving text information for " + this.ToString() + ".");
                        if (this.m_IsRoot)
                            return "<root>";
                        else if (this.m_Solution != null)
                            return (this.m_Solution.GetSyncData() as FileSyncData).Text;
                        else if (this.m_Project != null)
                            return (this.m_Project.GetSyncData() as FileSyncData).Text;
                        else if (this.m_File != null)
                            return (this.m_File.GetSyncData() as FileSyncData).Text;
                        else
                            return "<unknown>";
                    }
                }

                /// <summary>
                /// The parent solution item of this solution item.
                /// </summary>
                public SolutionItem Parent
                {
                    get
                    {
                        m_Log.Debug("Retrieving parent of " + this.ToString() + ".");
                        return this.FindImmediateParent();
                    }
                }

                /// <summary>
                /// The index of this solution item within it's parent.
                /// </summary>
                public int Index
                {
                    get
                    {
                        m_Log.Debug("Retrieving index of " + this.ToString() + ".");
                        SolutionItem parent = this.Parent;
                        if (parent.m_Solution != null)
                            return parent.m_Solution.Projects.IndexOf(this.m_Project);
                        else if (parent.m_Project != null)
                            return parent.m_Project.Files.IndexOf(this.m_File);
                        else if (parent.m_File != null && parent.m_File is Folder)
                            return (parent.m_File as Folder).Files.IndexOf(this.m_File);
                        else
                            return 0;
                    }
                }

                /// <summary>
                /// The total number of children this solution item has.
                /// </summary>
                public int Count
                {
                    get
                    {
                        m_Log.Debug("Retrieving children count of " + this.ToString() + ".");
                        if (this.m_IsRoot)
                            return (Central.Manager.ActiveSolution != null) ? 1 : 0;
                        else if (this.m_Solution != null)
                            return this.m_Solution.Projects.Count;
                        else if (this.m_Project != null)
                            return this.m_Project.Files.Count;
                        else if (this.m_File != null && this.m_File is Folder)
                            return (this.m_File as Folder).Files.Count;
                        else
                            return 0;
                    }
                }

                /// <summary>
                /// Gets the child item at the specified index.
                /// </summary>
                public SolutionItem this[int index]
                {
                    get
                    {
                        if (index < 0 || index >= this.Count)
                            throw new ArgumentOutOfRangeException("index");
                        m_Log.Debug("Retrieving child of " + this.ToString() + " at index " + index + ".");
                        if (this.m_IsRoot && Central.Manager.ActiveSolution != null)
                            return new SolutionItem(Central.Manager.ActiveSolution);
                        else if (this.m_Solution != null)
                            return new SolutionItem(this.m_Solution.Projects[index]);
                        else if (this.m_Project != null)
                            return new SolutionItem(this.m_Project.Files[index]);
                        else if (this.m_File != null && this.m_File is Folder)
                            return new SolutionItem((this.m_File as Folder).Files[index]);
                        else
                            throw new InvalidOperationException("There is not SolutionItem that currently represents the state.");
                    }
                }

                private SolutionItem FindImmediateParent()
                {
                    // Check to see if this is the root node.
                    if (this.m_IsRoot)
                        return null;

                    // Check solution to see if it's loaded.
                    if (Central.Manager.ActiveSolution == null)
                        return null;
                    if (this.m_Solution != null)
                        return new SolutionItem();
    
                    // Are we a project?
                    if (this.m_Project != null)
                        return new SolutionItem(Central.Manager.ActiveSolution);

                    // Check directly in projects.
                    foreach (Project p in Central.Manager.ActiveSolution.Projects)
                    {
                        if (p.Files.Contains(this.m_File))
                            return new SolutionItem(p);
                    }

                    // Recursively check files and folders.
                    foreach (Project p in Central.Manager.ActiveSolution.Projects)
                    {
                        foreach (File f in p.Files)
                        {
                            if (f is Folder)
                            {
                                Folder parent = this.FindImmediateParentFolder(f as Folder, this.m_File);
                                if (parent != null)
                                    return new SolutionItem(parent);
                            }
                        }
                    }

                    // Otherwise, we don't know what the parent is.
                    return null;
                }

                private Folder FindImmediateParentFolder(Folder root, File file)
                {
                    foreach (File f in root.Files)
                    {
                        if (f == file)
                            return root;
                        else if (f is Folder)
                        {
                            Folder parent = this.FindImmediateParentFolder(f as Folder, file);
                            if (parent != null)
                                return parent;
                        }
                    }

                    return null;
                }

                #region Operators

                public override bool Equals(object obj)
                {
                    if (!(obj is SolutionItem))
                        return false;
                    else if (obj == null)
                        return false;

                    return (this == obj as SolutionItem);
                }

                public static bool operator ==(SolutionItem a, SolutionItem b)
                {
                    if (object.ReferenceEquals(a, b))
                        return true;
                    else if ((object)a == null || (object)b == null)
                        return false;

                    return (a.m_IsRoot == b.m_IsRoot &&
                            a.m_File == b.m_File &&
                            a.m_Project == b.m_Project &&
                            a.m_Solution == b.m_Solution);
                }

                public static bool operator!=(SolutionItem a, SolutionItem b)
                {
                    return !(a == b);
                }

                public override string ToString()
                {
                    try
                    {
                        if (this.m_Solution != null)
                            return string.Format("[Solution: Name={0}]", this.m_Solution.SolutionInfo.Name);
                        else if (this.m_Project != null)
                            return string.Format("[Project: Name={0}]", this.m_Project.ProjectInfo.Name);
                        else if (this.m_File != null)
                            return string.Format("[File: Name={0}]", this.m_File.FileInfo.Name);
                        else
                            return "[Unknown]";
                    }
                    catch
                    {
                        return "[Unknown-Exception]";
                    }
                }

                #endregion
            }
        }
    }
}

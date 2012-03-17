using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Reflection;
using Moai.Platform.UI;

namespace Moai.Platform.Menus
{
    class MenuLoader
    {
        private XmlReader m_Reader;
        public DynamicGroupAction MainMenu = new DynamicGroupAction();
        public DynamicGroupAction ToolBar = new DynamicGroupAction();

        public DynamicGroupAction ActiveMenuStore;
        public DynamicGroupAction ActiveToolStore;
        public Stack<DynamicGroupAction> ParentMenuStores = new Stack<DynamicGroupAction>();
        public Stack<DynamicGroupAction> ParentToolStores = new Stack<DynamicGroupAction>();

        private Assembly m_CurrentAssembly = Assembly.GetExecutingAssembly();
        private Menus.MenusManager m_Manager;

        public MenuLoader(Menus.MenusManager manager)
        {
            this.m_Manager = manager;

            // Check to make sure our menu information file exists.
            if (!File.Exists(Central.Manager.Settings["RootPath"] + Path.DirectorySeparatorChar + "Menus.xml"))
                throw new Exception("Menu information file was not found.  Please make sure Menus.xml exists in the application directory.");
            
            // Set the default stores.
            this.ActiveMenuStore = this.MainMenu;
            this.ActiveToolStore = this.ToolBar;

            // Read our menu data.
            this.m_Reader = XmlReader.Create(new StreamReader(Central.Manager.Settings["RootPath"] + Path.DirectorySeparatorChar + "Menus.xml"));
            while (this.m_Reader.Read())
            {
                switch (this.m_Reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (this.m_Reader.Name)
                        {
                            case "menus":
                            case "menubar":
                            case "toolbar":
                                // Nothing to do here.
                                break;
                            case "menuitem":
                                {
                                    // Check to see if this is an empty element.
                                    if (this.m_Reader.IsEmptyElement)
                                    {
                                        // Use reflection to create an instance of the action.
                                        Action a = this.GetActionByName(this.m_Reader.GetAttribute("action"));

                                        // Add it to the list of main menu items.
                                        this.ActiveMenuStore.Add(a);
                                    }
                                    else
                                    {
                                        // This menu item contains other menu items, move the currently
                                        // active menu store into the parent list and create a new active
                                        // menu store.
                                        this.ParentMenuStores.Push(this.ActiveMenuStore);
                                        this.ActiveMenuStore = new DynamicGroupAction(this.m_Reader.GetAttribute("text"));
                                    }
                                }
                                break;
                            case "menuseperator":
                                {
                                    // Create a new SeperatorAction.
                                    Action a = new SeperatorAction();

                                    // Add it to the list of main menu items.
                                    this.ActiveMenuStore.Add(a);
                                }
                                break;
                            case "toolitem":
                                {
                                    // Check to see if this is an empty element.
                                    if (this.m_Reader.IsEmptyElement)
                                    {
                                        // Use reflection to create an instance of the action.
                                        Action a = this.GetActionByName(this.m_Reader.GetAttribute("action"));

                                        // Add it to the list of main menu items.
                                        this.ActiveToolStore.Add(a);
                                    }
                                    else if (this.m_Reader.GetAttribute("action") != null)
                                    {
                                        // Use reflection to create an instance of the action.
                                        Action a = this.GetActionByName(this.m_Reader.GetAttribute("action"));

                                        // This menu item contains other menu items, move the currently
                                        // active menu store into the parent list and create a new active
                                        // menu store.
                                        this.ParentToolStores.Push(this.ActiveToolStore);
                                        this.ActiveToolStore = new DynamicGroupAction(a);
                                    }
                                    else
                                    {
                                        // This menu item contains other menu items, move the currently
                                        // active menu store into the parent list and create a new active
                                        // menu store.
                                        this.ParentToolStores.Push(this.ActiveToolStore);
                                        this.ActiveToolStore = new DynamicGroupAction(this.m_Reader.GetAttribute("text"));
                                    }
                                }
                                break;
                            case "toolseperator":
                                {
                                    // Create a new SeperatorAction.
                                    Action a = new SeperatorAction();

                                    // Add it to the list of main menu items.
                                    this.ActiveToolStore.Add(a);
                                }
                                break;
                                /*
                            case "toolitem":
                                if (this.m_Reader.GetAttribute("type") == "dropdown")
                                    this.AddToolDropDown(this.m_Reader.GetAttribute("text"));
                                else
                                    this.AddToolItem(this.m_Reader.GetAttribute("text"));

                                this.AddReflectionHandler(this.m_Reader.GetAttribute("action"));

                                if (this.m_Reader.IsEmptyElement)
                                {
                                    // Automatically end element.
                                    c_ActiveItem = c_ActiveItem.OwnerItem;
                                    this.UpdateObjects();

                                    // The parent had some menu items, therefore we
                                    // enable it regardless of whether it has an action.
                                    if (c_ActiveItem != null)
                                        c_ActiveItem.Enabled = true;
                                }
                                break;
                            case "toolseperator":
                                this.AddToolItem("-");

                                if (this.m_Reader.IsEmptyElement)
                                {
                                    // Automatically end element.
                                    c_ActiveItem = c_ActiveItem.OwnerItem;
                                    this.UpdateObjects();

                                    // The parent had some menu items, therefore we
                                    // enable it regardless of whether it has an action.
                                    if (c_ActiveItem != null)
                                        c_ActiveItem.Enabled = true;
                                }
                                break;
                            case "toolcombo":
                                this.AddToolComboBox(this.m_Reader.GetAttribute("text"));

                                /* FIXME: Implement this.
                                if (this.m_Reader.GetAttribute("editable") == "false")
                                    c_ActiveComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                                 *

                                //this.AddReflectionHandler(this.Reader.GetAttribute("action"));

                                if (this.m_Reader.IsEmptyElement)
                                {
                                    // Automatically end element.
                                    c_ActiveItem = c_ActiveItem.OwnerItem;
                                    this.UpdateObjects();

                                    // The parent had some menu items, therefore we
                                    // enable it regardless of whether it has an action.
                                    if (c_ActiveItem != null)
                                        c_ActiveItem.Enabled = true;
                                }
                                break;
                            case "text":
                                this.UpdateObjects();

                                if (c_ActiveComboBox != null)
                                {
                                    Type enumType = m_CurrentAssembly.GetType(this.m_Reader.GetAttribute("type"));
                                    if (enumType != null)
                                    {
                                        Array enumValues = Enum.GetValues(enumType);
                                        foreach (object o in enumValues)
                                        {
                                            String name = Enum.GetName(enumType, o);
                                            if (this.m_Reader.GetAttribute("type") + "." + name == this.m_Reader.GetAttribute("value"))
                                            {
                                                // HACK: This could probably be organised better by
                                                //       using classes instead of enums, but oh well..
                                                switch (this.m_Reader.GetAttribute("type"))
                                                {
                                                    case "Moai.Compilation.BuildMode":
                                                        c_ActiveComboBox.Items.Add(
                                                            new EnumWrapper(
                                                                (Int32)o,
                                                                new List<String>() { "Debug", "Release" })
                                                            );
                                                        break;
                                                    default:
                                                        c_ActiveComboBox.Items.Add(o);
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                                break;
                                 */
                        }
                        break;
                    case XmlNodeType.EndElement:
                        switch (this.m_Reader.Name)
                        {
                            case "menus":
                            case "menubar":
                            case "toolbar":
                                // Nothing to do here.
                                break;
                            case "menuitem":
                                {
                                    // Check to see if this is not an empty element.
                                    if (!this.m_Reader.IsEmptyElement)
                                    {
                                        // Finish this menu section up.
                                        this.ParentMenuStores.Peek().Add(this.ActiveMenuStore);
                                        this.ActiveMenuStore = this.ParentMenuStores.Pop();
                                    }
                                }
                                break;
                            case "toolitem":
                                {
                                    // Check to see if this is not an empty element.
                                    if (!this.m_Reader.IsEmptyElement)
                                    {
                                        // Finish this menu section up.
                                        this.ParentToolStores.Peek().Add(this.ActiveToolStore);
                                        this.ActiveToolStore = this.ParentToolStores.Pop();
                                    }
                                }
                                break;
                                /*
                            case "toolitem":
                                c_ActiveItem = c_ActiveItem.OwnerItem;
                                this.UpdateObjects();

                                // The parent had some menu items, therefore we
                                // enable it regardless of whether it has an action.
                                if (c_ActiveItem != null)
                                    c_ActiveItem.Enabled = true;
                                break;
                            case "menuseperator":
                                c_ActiveItem = c_ActiveItem.OwnerItem;
                                this.UpdateObjects();

                                // The parent had some menu items, therefore we
                                // enable it regardless of whether it has an action.
                                if (c_ActiveItem != null && !(c_ActiveItem is IToolStripDropDownButton))
                                    c_ActiveItem.Enabled = true;
                                break;
                            case "toolseperator":
                                c_ActiveItem = c_ActiveItem.OwnerItem;
                                this.UpdateObjects();

                                // The parent had some menu items, therefore we
                                // enable it regardless of whether it has an action.
                                if (c_ActiveItem != null)
                                    c_ActiveItem.Enabled = true;
                                break;
                                 */
                        }
                        break;
                }
            }

            this.m_Reader.Close();
        }

        private Action GetActionByName(string name)
        {
            if (name != null)
            {
                // Use reflection to associate the menu item with a defined
                // action.
                Type type = m_CurrentAssembly.GetType("Moai.Platform.Menus.Definitions." + name);
                if (type != null)
                {
                    object obj = Activator.CreateInstance(type);
                    if (obj is Action)
                        return obj as Action;
                }
            }

            return null;
        }

        /*
        private void AddMenuItem(String text)
        {
            if (c_ActiveItem == null)
            {
                c_ActiveItem = this.MainMenu.AddByText(text);
                this.UpdateObjects();
            }
            else if (c_ActiveDropDown != null)
            {
                // Add to the drop down.
                c_ActiveItem = c_ActiveDropDown.AddByText(text);
                this.UpdateObjects();
            }
            else
            {
                c_ActiveItem = c_ActiveMenuItem.AddByText(text);
                this.UpdateObjects();
            }
        }

        private void AddToolItem(String text)
        {
            c_ActiveItem = this.ToolBar.AddByText(text);
            // FIXME: c_ActiveItem.TextImageRelation = TextImageRelation.ImageAboveText;
            c_ActiveItem.Text = "";
            // FIXME: c_ActiveItem.Image = IDE.Resources.Images.tools_unknown;
            this.UpdateObjects();
        }

        private void AddToolComboBox(String text)
        {
            // FIXME: c_ActiveComboBox = Central.Platform.UI.CreateToolStripComboBox(text);
            c_ActiveItem = c_ActiveComboBox;
            // FIXME: c_ActiveItem.TextImageRelation = TextImageRelation.ImageAboveText;
            c_ActiveItem.Text = "";
            // FIXME: c_ActiveItem.Image = null;
            // FIXME: this.ToolBar.Items.Add(c_ActiveComboBox);
            this.UpdateObjects();
        }

        private void AddToolDropDown(String text)
        {
            // FIXME: c_ActiveDropDown = new ToolStripDropDownButton(text);
            c_ActiveItem = c_ActiveDropDown;
            // FIXME: c_ActiveItem.TextImageRelation = TextImageRelation.ImageAboveText;
            c_ActiveItem.Text = "";
            // FIXME: c_ActiveItem.Image = IDE.Resources.Images.tools_unknown;
            // FIXME: this.ToolBar.Items.Add(c_ActiveDropDown);
            this.UpdateObjects();
        }

        private void AddReflectionHandler(String actionName)
        {
            if (actionName != null)
            {
                // Use reflection to associate the menu item with a defined
                // action.
                Type actionType = m_CurrentAssembly.GetType("Moai.Menus.Definitions." + actionName);
                if (actionType != null)
                {
                    object actionObj = Activator.CreateInstance(actionType);
                    if (actionObj is Action)
                    {
                        Action action = (Action)actionObj;
                        String resString = action.Text;
                        if (resString != null)
                        {
                            if (c_ActiveMenuItem != null)
                                c_ActiveItem.Text = resString;
                            // FIXME: else
                            // FIXME:     c_ActiveItem.ToolTipText = resString;
                        }
                        // FIXME: action.SetItem(this.c_ActiveMenuItem, this.c_ActiveItem);
                        if (this.c_ActiveMenuItem != null)
                        {
                            this.c_ActiveMenuItem.ShortcutKeys = action.Shortcut;
                            this.c_ActiveMenuItem.ShowShortcutKeys = true;
                        }
                        c_ActiveItem.Click += new EventHandler(delegate(object sender, EventArgs e)
                            {
                                action.OnActivate();
                            }
                        );
                        //this.m_Manager.Parent.IDEWindow.MenuActions.Add(action);

                        action.OnInitialize();
                        c_ActiveItem.Enabled = action.Enabled;
                        // FIXME: if (action.ItemIcon != null)
                        // FIXME:     c_ActiveItem.Image = action.ItemIcon;
                    }
                    else
                        c_ActiveItem.Enabled = false;
                }
                else
                    c_ActiveItem.Enabled = false;
            }
            else
                c_ActiveItem.Enabled = false;
        }

        private void UpdateObjects()
        {
            /* FIXME: if (c_ActiveItem is ToolStripMenuItem)
                c_ActiveMenuItem = (ToolStripMenuItem)c_ActiveItem;
            else
                c_ActiveMenuItem = null;
            
            if (c_ActiveItem is ToolStripDropDownButton)
                c_ActiveDropDown = (ToolStripDropDownButton)c_ActiveItem;
            else
                c_ActiveDropDown = null;

            if (c_ActiveItem is ToolStripComboBox)
                c_ActiveComboBox = (ToolStripComboBox)c_ActiveItem;
            else
                c_ActiveComboBox = null; *             
        }*/
    }
}

/*
 * Copyright (c) 2018 mebiusbox software. All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE AUTHOR AND CONTRIBUTORS ``AS IS'' AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED.  IN NO EVENT SHALL THE AUTHOR OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace libpixy.net.Shell
{
    public partial class ShellFolderTree : UserControl
    {
        #region Attribute

        private ShellItem m_currentItem = null;
        private ShellItem m_focusItem = null;
        private ShellBrowser ShellBrowser = new ShellBrowser();
        private TreeNode m_desktopNode = null;
        private TreeNode m_myCompNode = null;
        private string m_initialPath = null;

        #endregion Attribute

        #region Properties

        public ShellItem CurrentNodeItem
        {
            get { return m_currentItem; }
        }

        public ShellItem FocusNodeItem
        {
            get { return m_focusItem; }
        }

        public string InitialPath
        {
            get { return m_initialPath; }
            set { m_initialPath = value; }
        }

        #endregion Properties

        #region Constuctor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ShellFolderTree()
        {
            InitializeComponent();
            this.treeView.HandleCreated += new EventHandler(treeView_HandleCreated);
        }

        #endregion Constructor

        #region Public Evnets

        public event EventHandler NodeSelected;

        void RaiseNodeSelected()
        {
            if (this.NodeSelected != null)
            {
                this.NodeSelected(this, EventArgs.Empty);
            }
        }

        #endregion Public Events

        #region Method

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="parent"></param>
        private void ExpandTreeNode(TreeNode root, ShellItem parent)
        {
            root.Nodes.Clear();

            parent.ExpandFolders(IntPtr.Zero);

            List<TreeNode> nodes = new List<TreeNode>();
            foreach (ShellItem item in parent.SubFolders)
            {
                TreeNode newNode = new TreeNode(
                    item.Text,
                    item.ImageIndex,
                    item.SelectedImageIndex);
                newNode.Tag = item;
                newNode.Nodes.Add(new TreeNode(""));//Dummy
                nodes.Add(newNode);
            }

            root.Nodes.AddRange(nodes.ToArray());
        }

        #endregion Method

        #region SelectPath

        private string ConvertPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return path;

            string newPath = path.Trim();

            //TODO
#if false
            if (newPath.StartsWith(
                    string.Format(@"{0}\", ShellBrowser.MyComputerName),
                    false,
                    CultureInfo.InstalledUICulture) && newPath.Length > 12)
                newPath = newPath.Substring(path.IndexOf('\\') + 1);
#endif

            if (!newPath.EndsWith(@":\") && newPath.EndsWith(@"\"))
                newPath = newPath.Substring(0, newPath.Length - 1);

            if (newPath.EndsWith(@"\"))
                newPath = newPath.Substring(0, newPath.Length - 1);

            return newPath;
        }

        /// <summary>
        /// This method uses SHGetFileInfo to check whether a path to a directory exists.
        /// </summary>
        /// <param name="path">The path to check</param>
        /// <returns>true if it exists, false otherwise</returns>
        private bool PathExists(string path)
        {
            string realPath = ConvertPath(path);

            if (string.IsNullOrEmpty(realPath))
                return false;
            else if (string.Compare(path, "desktop", true) == 0)
                return true;

            string[] pathParts = realPath.Split('\\');

            for (int i = 0; i < pathParts.Length; i++)
            {
                bool found = false;
                if (ShellBrowser.DesktopItem.SubFolders.Contains(pathParts[i]))
                {
                    pathParts[i] = ShellBrowser.GetRealPath(
                        ShellBrowser.DesktopItem.SubFolders[pathParts[i]]);

                    found = true;
                }
                else
                {
                    ShellBrowser.DesktopItem.ExpandFolders(IntPtr.Zero);
                    ShellItem myComp =
                        ShellBrowser.DesktopItem.SubFolders[ShellBrowser.MyComputerName];

                    if (myComp.SubFolders.Contains(pathParts[i]))
                    {
                        pathParts[i] = ShellBrowser.GetRealPath(
                            myComp.SubFolders[pathParts[i]]);

                        found = true;
                    }
                }

                if (!found)
                    break;
            }

            realPath = string.Join("\\", pathParts);

            if (realPath.EndsWith(":"))
                realPath += "\\";

            API.SHFILEINFO info = new API.SHFILEINFO();
            IntPtr ptr = API.SHGetFileInfo(realPath, 0, ref info, API.cbFileInfo, API.SHGFI.DISPLAYNAME);
            bool exists = (ptr != IntPtr.Zero);

            Marshal.FreeCoTaskMem(ptr);
            return exists;
        }

        /// <summary>
        /// Selects a path from a string, this can be a direct path, or something like 
        /// "My Documents/My Music". It will set the directory as the current directory.
        /// </summary>
        /// <param name="path">The path to select</param>
        /// <returns>The TreeNode of the directory which was selected, this will be null if the directory
        /// doesn't exist</returns>
        public TreeNode SelectPath(string path, bool expandNode)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            if (PathExists(path))
            {
                string converted = ConvertPath(path);
                string[] pathParts = converted.Split('\\');

                TreeNode currentNode = null;

                #region Get Start Node
                // Change .Expand() to function which extends the node without expanding it

                if (string.Compare(pathParts[0], "desktop", true) == 0)
                {
                    currentNode = m_desktopNode;
                }
                else if (m_desktopNode.Nodes.ContainsKey(pathParts[0]))
                {
                    currentNode = m_desktopNode.Nodes[pathParts[0]];
                    ExpandTreeNode(currentNode, (ShellItem)currentNode.Tag);
                }
                else
                {
                    if (string.Compare(pathParts[0], m_myCompNode.Text, true) == 0)
                    {
                        currentNode = m_myCompNode;
                    }
                    else
                    {
                        if (pathParts[0][pathParts[0].Length - 1] == ':')
                            pathParts[0] += "\\";

                        foreach (TreeNode node in m_myCompNode.Nodes)
                        {
                            if (string.Compare(
                                    pathParts[0],
                                    ((ShellItem)node.Tag).Path, true) == 0)
                            {
                                currentNode = node;
                                ExpandTreeNode(currentNode, (ShellItem)node.Tag);
                                break;
                            }
                        }
                    }
                }

                #endregion

                if (currentNode == null)
                {
                    this.treeView.EndUpdate();
                    return null;
                }

                #region Iterate

                if (expandNode)
                {
                    currentNode.Expand();
                }

                string pname = pathParts[0];

                for (int i = 1; i < pathParts.Length; i++)
                {
                    if (pathParts[i][pathParts[i].Length - 1] == ':')
                        pathParts[i] += "\\";

                    pname = Path.Combine(pname, pathParts[i]);

                    bool found = false;
                    foreach (TreeNode child in currentNode.Nodes)
                    {
                        if (string.Compare(pathParts[i], child.Text, true) == 0)
                        {
                            currentNode = child;
                            found = true;
                            break;
                        }

                        if (string.Compare(pname, ((ShellItem)child.Tag).Path) == 0)
                        {
                            currentNode = child;
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        this.treeView.EndUpdate();
                        return null;
                    }

                    ExpandTreeNode(currentNode, (ShellItem)currentNode.Tag);
                    if (expandNode)
                    {
                        currentNode.Expand();
                    }
                }

                #endregion

                if (expandNode)
                {
                    currentNode.Expand();
                }

                this.treeView.SelectedNode = currentNode;

                return currentNode;
            }
            else
            {
                return null;
            }
        }

        #endregion SelectPath

        #region Event Handlers

        private void ShellFolderTree_Load(object sender, EventArgs e)
        {
            this.treeView.BeginUpdate();

            m_currentItem = libpixy.net.Shell.ShellItem.DesktopItem;
            m_desktopNode = new TreeNode(
                m_currentItem.Text,
                m_currentItem.ImageIndex,
                m_currentItem.SelectedImageIndex);
            this.treeView.Nodes.Add(m_desktopNode);
            m_desktopNode.Tag = m_currentItem;

            ExpandTreeNode(m_desktopNode, m_currentItem);
            m_desktopNode.Expand();

            foreach (ShellItem desktopChild in m_currentItem.SubFolders)
            {
                TreeNode desktopChildNode = new TreeNode(
                    desktopChild.Text,
                    desktopChild.ImageIndex,
                    desktopChild.SelectedImageIndex);
                desktopChildNode.Tag = desktopChild;
                desktopChildNode.Name = desktopChildNode.Text;

                if (desktopChildNode.Text == ShellBrowser.MyComputerName)
                {
                    m_myCompNode = desktopChildNode;

                    ExpandTreeNode(desktopChildNode, desktopChild);
                    desktopChildNode.Expand();

                    foreach (ShellItem myCompChild in desktopChild.SubFolders)
                    {
                        TreeNode myCompChildNode = new TreeNode(
                            myCompChild.Text,
                            myCompChild.ImageIndex,
                            myCompChild.SelectedImageIndex);
                        myCompChildNode.Tag = myCompChild;
                        myCompChildNode.Name = myCompChildNode.Text;

                        if (myCompChild.Flags.hasSubFolders)
                            myCompChildNode.Nodes.Add(string.Empty);

                        //navAddressBox.Items.Add(new BrowserComboItem(myCompChild, 2));
                        desktopChildNode.Nodes.Add(myCompChildNode);
                    }
                }
                else if (desktopChild.Flags.hasSubFolders)
                {
                    desktopChildNode.Nodes.Add(string.Empty);
                }

                m_desktopNode.Nodes.Add(desktopChildNode);
            }

            if (string.IsNullOrEmpty(m_initialPath) == false)
            {
                SelectPath(m_initialPath, true);
            }

            this.treeView.EndUpdate();
        }

        void treeView_HandleCreated(object sender, EventArgs e)
        {
            libpixy.net.Shell.ShellImageList.SetSmallImageList(this.treeView);
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag != null)
            {
                this.treeView.Cursor = Cursors.WaitCursor;
                ShellItem item = (ShellItem)e.Node.Tag;
                ExpandTreeNode(e.Node, item);
                this.treeView.Cursor = Cursors.Default;
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_focusItem = (ShellItem)e.Node.Tag;
            RaiseNodeSelected();
        }

        #endregion Event Handlers
    }
}

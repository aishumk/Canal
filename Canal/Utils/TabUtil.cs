﻿using Canal.Events;
using Canal.Properties;
using Canal.UserControls;
using Logging;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Canal.Utils
{
    using System.Drawing;
    using System.Windows.Forms;

    public class TabUtil
    {
        private readonly TabControl _tabControl;

        private readonly MainWindow _parent;

        public TabUtil(TabControl tabControl, MainWindow parent)
        {
            _parent = parent;
            _tabControl = tabControl;
            _tabControl.DrawMode = TabDrawMode.Normal;
            _tabControl.MouseDown += TabControlOnMouseDown;
        }

        private IEnumerable<FileControl> GetFileControls()
        {
            return (from TabPage tabPage in _tabControl.TabPages
                    select tabPage.Controls.Find("FileControl", false)
                    .FirstOrDefault(foundTab => foundTab is FileControl))
                    .OfType<FileControl>().ToList();
        }

        public void SetTabName(string text)
        {
            _tabControl.SelectedTab.Text = text;
        }

        public bool TryShowTab(string filepath)
        {
            try
            {
                foreach (var tab in from TabPage tab in _tabControl.TabPages
                                    let fileControl = tab.Controls.Find("FileControl", false)
                                    .FirstOrDefault(foundTab => foundTab is FileControl) as FileControl
                                    where fileControl != null && fileControl.CobolFile.FileReference.FilePath == filepath
                                    select tab)
                {
                    _tabControl.SelectedTab = tab;
                    Logger.Info("Switching to tab {0}: {1}", tab.TabIndex, tab.Text);
                    return true;
                }
                return false;
            }
            catch (Exception exception)
            {
                ErrorHandling.Exception(exception);
                return false;
            }
        }

        private void TabControlOnMouseDown(object sender, MouseEventArgs mouseEventArgs)
        {
            //Looping through the controls.
            for (int i = 0; i < _tabControl.TabPages.Count; i++)
            {
                Rectangle r = _tabControl.GetTabRect(i);
                //Getting the position of the "x" mark.
                Rectangle closeButton = new Rectangle(r.Right - 15, r.Top + 4, 9, 7);
                if (closeButton.Contains(mouseEventArgs.Location))
                {
                    CloseTab(i);
                }
            }
        }

        public void AddTab(CobolFile file)
        {
            Logger.Info("Adding tab for file {0}", file.Name);

            var newTab = new TabPage(file.Name + "     X");

            var fileControl = new FileControl(file, _parent);

            fileControl.UsedFileTypesChanged += UsedFileTypesChanged;
            FileUtil.Instance.FileCacheChanged += RefreshFileView;

            newTab.Controls.Add(fileControl);
            _tabControl.Controls.Add(newTab);
            _tabControl.SelectTab(newTab);
        }

        private void RefreshFileView(object sender, FileCacheChangedEventArgs fileCacheChangedEventArgs)
        {
            Logger.Info("Refreshing files view.");

            foreach (var fileControl in GetFileControls())
            {
                fileControl.RefreshFileView();
            }
        }

        private void UsedFileTypesChanged(object sender, UsedFileTypesChangedEventArgs usedFileTypesChangedEventArgs)
        {
            Logger.Info("Used file types changed.");

            foreach (var fileControl in GetFileControls())
            {
                fileControl.RefreshUsedFileTypes(sender, usedFileTypesChangedEventArgs);
            }
        }

        public bool CloseTab(int index = -1)
        {
            Logger.Info("Closing tab {0}.", index);

            var tabIndex = index < 0 ? _tabControl.SelectedIndex : index;

            if (MessageBox.Show(Resources.ReallyCloseThisTab, Resources.CloseTab, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                var fileControl = (FileControl)_tabControl.SelectedTab.Controls.Find("FileControl", false).FirstOrDefault(tab => tab is FileControl);
                // TODO dispose everything
                if (fileControl != null)
                    fileControl.Dispose();
                _tabControl.TabPages.RemoveAt(tabIndex);
                return true;
            }
            return false;
        }

        public bool CloseAllTabs()
        {
            Logger.Info("Closing all tabs.");

            for (int tabIndex = 0; tabIndex < _tabControl.TabCount; tabIndex++)
            {
                if (!CloseTab(tabIndex))
                    return false;
            }

            return true;
        }

        public FileControl CurrentFileControl
        {
            get { return (_tabControl.SelectedTab == null) ? null : (FileControl)_tabControl.SelectedTab.Controls.Find("FileControl", false).FirstOrDefault(); }
        }

        public void ShowStartTab()
        {
            Logger.Info("Showing start tab");

            var newTab = new TabPage("Start" + "     X");
            var firstTabPage = new FirstTabPage(_parent) { Dock = DockStyle.Fill };
            newTab.Controls.Add(firstTabPage);
            _tabControl.Controls.Add(newTab);
            _tabControl.SelectTab(newTab);
        }
    }
}

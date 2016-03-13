﻿using System;
using System.Windows.Forms;

namespace Canal
{
    using Level88ToEnum;
    using Utils;

    public partial class MainWindow : Form
    {
        private TabUtil tabUtil;

        public CodeBox CurrentCodeBox
        {
            get
            {
                if (tabUtil.CurrentFileControl != null && tabUtil.CurrentFileControl.CodeBox != null)
                    return tabUtil.CurrentFileControl.CodeBox;
                return null;
            }
        }

        public MainWindow(string[] files = null)
        {
            InitializeComponent();

            tabUtil = new TabUtil(FileTabs, this);

            if (files != null)
            {
                foreach (var filename in files)
                {
                    OpenFile(filename);
                }
            }
        }

        public void OpenFile(string filename)
        {
            Cursor = Cursors.WaitCursor;
            var file = FileUtil.Get(filename);
            tabUtil.AddTab(file);
            Cursor = Cursors.Default;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog.Filter = @"COBOL Files|*.cob;*.cbl;*.txt";
            openFileDialog.FileName = "";

            var dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                OpenFile(openFileDialog.FileName);
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabUtil.CloseTab();
        }

        private void level88ToEnumConverterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var converterWindow = new Level88ToEnum();
            converterWindow.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabUtil.CloseAllTabs())
                Close();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tabUtil.AddTab(new CobolFile("", "New File"));
        }
    }
}

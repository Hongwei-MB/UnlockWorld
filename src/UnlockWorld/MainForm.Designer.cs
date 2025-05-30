using System;
using System.Drawing;
using System.Windows.Forms;

namespace UnlockWorld
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelTop = new Panel();
            chkToggleOnly = new CheckBox();
            lblStatus = new Label();
            btnClearLogs = new Button();
            btnFinderTool = new Button();
            splitContainer = new SplitContainer();
            txtElementDetails = new TextBox();
            lblElementDetails = new Label();
            logTextBox = new RichTextBox();
            lblLogs = new Label();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolTip = new ToolTip(components);
            button1 = new Button();
            panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // panelTop
            // 
            panelTop.Controls.Add(button1);
            panelTop.Controls.Add(chkToggleOnly);
            panelTop.Controls.Add(lblStatus);
            panelTop.Controls.Add(btnClearLogs);
            panelTop.Controls.Add(btnFinderTool);
            panelTop.Dock = DockStyle.Top;
            panelTop.Location = new Point(0, 0);
            panelTop.Name = "panelTop";
            panelTop.Size = new Size(800, 60);
            panelTop.TabIndex = 0;
            // 
            // chkToggleOnly
            // 
            chkToggleOnly.AutoSize = true;
            chkToggleOnly.Location = new Point(12, 19);
            chkToggleOnly.Name = "chkToggleOnly";
            chkToggleOnly.Size = new Size(145, 19);
            chkToggleOnly.TabIndex = 3;
            chkToggleOnly.Text = "Toggle Checkbox Only";
            chkToggleOnly.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(189, 23);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(39, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "Ready";
            // 
            // btnClearLogs
            // 
            btnClearLogs.Location = new Point(663, 15);
            btnClearLogs.Name = "btnClearLogs";
            btnClearLogs.Size = new Size(125, 30);
            btnClearLogs.TabIndex = 1;
            btnClearLogs.Text = "Clear Logs";
            btnClearLogs.UseVisualStyleBackColor = true;
            btnClearLogs.Click += btnClearLogs_Click;
            // 
            // btnFinderTool
            // 
            btnFinderTool.Location = new Point(504, 15);
            btnFinderTool.Name = "btnFinderTool";
            btnFinderTool.Size = new Size(125, 30);
            btnFinderTool.TabIndex = 0;
            btnFinderTool.Text = "Element Finder";
            btnFinderTool.UseVisualStyleBackColor = true;
            btnFinderTool.MouseDown += btnFinderTool_MouseDown;
            btnFinderTool.MouseUp += btnFinderTool_MouseUp;
            // 
            // splitContainer
            // 
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(0, 60);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(txtElementDetails);
            splitContainer.Panel1.Controls.Add(lblElementDetails);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(logTextBox);
            splitContainer.Panel2.Controls.Add(lblLogs);
            splitContainer.Size = new Size(800, 368);
            splitContainer.SplitterDistance = 184;
            splitContainer.TabIndex = 1;
            // 
            // txtElementDetails
            // 
            txtElementDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtElementDetails.Font = new Font("Consolas", 9.75F);
            txtElementDetails.Location = new Point(12, 23);
            txtElementDetails.Multiline = true;
            txtElementDetails.Name = "txtElementDetails";
            txtElementDetails.ReadOnly = true;
            txtElementDetails.ScrollBars = ScrollBars.Vertical;
            txtElementDetails.Size = new Size(776, 149);
            txtElementDetails.TabIndex = 1;
            // 
            // lblElementDetails
            // 
            lblElementDetails.AutoSize = true;
            lblElementDetails.Location = new Point(12, 5);
            lblElementDetails.Name = "lblElementDetails";
            lblElementDetails.Size = new Size(91, 15);
            lblElementDetails.TabIndex = 0;
            lblElementDetails.Text = "Element Details:";
            // 
            // logTextBox
            // 
            logTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            logTextBox.Font = new Font("Consolas", 9F);
            logTextBox.Location = new Point(12, 23);
            logTextBox.Name = "logTextBox";
            logTextBox.ReadOnly = true;
            logTextBox.Size = new Size(776, 137);
            logTextBox.TabIndex = 1;
            logTextBox.Text = "";
            // 
            // lblLogs
            // 
            lblLogs.AutoSize = true;
            lblLogs.Location = new Point(12, 5);
            lblLogs.Name = "lblLogs";
            lblLogs.Size = new Size(78, 15);
            lblLogs.TabIndex = 0;
            lblLogs.Text = "Activity Logs:";
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel });
            statusStrip.Location = new Point(0, 428);
            statusStrip.Name = "statusStrip";
            statusStrip.Size = new Size(800, 22);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(39, 17);
            toolStripStatusLabel.Text = "Ready";
            // 
            // button1
            // 
            button1.Enabled = false;
            button1.Location = new Point(307, 23);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 4;
            button1.Text = "Test";
            button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer);
            Controls.Add(panelTop);
            Controls.Add(statusStrip);
            MinimumSize = new Size(500, 400);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UnlockWorld - UI Element Unlocker";
            panelTop.ResumeLayout(false);
            panelTop.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel1.PerformLayout();
            splitContainer.Panel2.ResumeLayout(false);
            splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private Panel panelTop;
        private Button btnClearLogs;
        private Label lblStatus;
        private CheckBox chkToggleOnly;
        private SplitContainer splitContainer;
        private TextBox txtElementDetails;
        private Label lblElementDetails;
        private RichTextBox logTextBox;
        private Label lblLogs;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel toolStripStatusLabel;
        private ToolTip toolTip;
        private Button btnFinderTool;
        private Button button1;
    }
}

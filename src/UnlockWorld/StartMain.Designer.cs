using System;
using System.Drawing;
using System.Windows.Forms;

namespace UnlockWorld
{
    partial class Form1
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
            splitContainer = new SplitContainer();
            txtElementDetails = new TextBox();
            lblElementDetails = new Label();
            logTextBox = new RichTextBox();
            lblLogs = new Label();
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolTip = new ToolTip(components);
            btnFinderTool = new Button();
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
            chkToggleOnly.Location = new Point(450, 22);
            chkToggleOnly.Name = "chkToggleOnly";
            chkToggleOnly.Size = new Size(126, 19);
            chkToggleOnly.TabIndex = 3;
            chkToggleOnly.Text = "仅切换复选框状态";
            chkToggleOnly.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(274, 22);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(57, 15);
            lblStatus.TabIndex = 2;
            lblStatus.Text = "准备就绪";
            // 
            // btnClearLogs
            // 
            btnClearLogs.Location = new Point(143, 15);
            btnClearLogs.Name = "btnClearLogs";
            btnClearLogs.Size = new Size(125, 30);
            btnClearLogs.TabIndex = 1;
            btnClearLogs.Text = "清除日志";
            btnClearLogs.UseVisualStyleBackColor = true;
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
            lblElementDetails.Size = new Size(84, 15);
            lblElementDetails.TabIndex = 0;
            lblElementDetails.Text = "元素详细信息:";
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
            lblLogs.Size = new Size(58, 15);
            lblLogs.TabIndex = 0;
            lblLogs.Text = "操作日志:";
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
            toolStripStatusLabel.Size = new Size(57, 17);
            toolStripStatusLabel.Text = "准备就绪";
            // 
            // btnFinderTool
            // 
            btnFinderTool.Location = new Point(12, 15);
            btnFinderTool.Name = "btnFinderTool";
            btnFinderTool.Size = new Size(125, 30);
            btnFinderTool.TabIndex = 0;
            btnFinderTool.Text = "查找控件 (拖动)";
            btnFinderTool.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(splitContainer);
            Controls.Add(panelTop);
            Controls.Add(statusStrip);
            MinimumSize = new Size(500, 400);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "UnlockWorld - 控件解锁工具";
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
    }
}

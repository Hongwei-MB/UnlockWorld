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
            this.components = new System.ComponentModel.Container();
            this.panelTop = new System.Windows.Forms.Panel();
            this.chkToggleOnly = new System.Windows.Forms.CheckBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnClearLogs = new System.Windows.Forms.Button();
            this.btnFinderTool = new System.Windows.Forms.Button();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.txtElementDetails = new System.Windows.Forms.TextBox();
            this.lblElementDetails = new System.Windows.Forms.Label();
            this.logTextBox = new System.Windows.Forms.RichTextBox();
            this.lblLogs = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.chkToggleOnly);
            this.panelTop.Controls.Add(this.lblStatus);
            this.panelTop.Controls.Add(this.btnClearLogs);
            this.panelTop.Controls.Add(this.btnFinderTool);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(800, 60);
            this.panelTop.TabIndex = 0;
            // 
            // chkToggleOnly
            // 
            this.chkToggleOnly.AutoSize = true;
            this.chkToggleOnly.Location = new System.Drawing.Point(450, 22);
            this.chkToggleOnly.Name = "chkToggleOnly";
            this.chkToggleOnly.Size = new System.Drawing.Size(174, 19);
            this.chkToggleOnly.TabIndex = 3;
            this.chkToggleOnly.Text = "仅切换复选框状态";
            this.chkToggleOnly.UseVisualStyleBackColor = true;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(274, 22);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(98, 15);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "准备就绪";
            // 
            // btnClearLogs
            // 
            this.btnClearLogs.Location = new System.Drawing.Point(143, 15);
            this.btnClearLogs.Name = "btnClearLogs";
            this.btnClearLogs.Size = new System.Drawing.Size(125, 30);
            this.btnClearLogs.TabIndex = 1;
            this.btnClearLogs.Text = "清除日志";
            this.btnClearLogs.UseVisualStyleBackColor = true;
            // 
            // btnFinderTool
            // 
            this.btnFinderTool.Location = new System.Drawing.Point(12, 15);
            this.btnFinderTool.Name = "btnFinderTool";
            this.btnFinderTool.Size = new System.Drawing.Size(125, 30);
            this.btnFinderTool.TabIndex = 0;
            this.btnFinderTool.Text = "查找控件 (拖动)";
            this.btnFinderTool.UseVisualStyleBackColor = true;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 60);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.txtElementDetails);
            this.splitContainer.Panel1.Controls.Add(this.lblElementDetails);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.logTextBox);
            this.splitContainer.Panel2.Controls.Add(this.lblLogs);
            this.splitContainer.Size = new System.Drawing.Size(800, 368);
            this.splitContainer.SplitterDistance = 184;
            this.splitContainer.TabIndex = 1;
            // 
            // txtElementDetails
            // 
            this.txtElementDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtElementDetails.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtElementDetails.Location = new System.Drawing.Point(12, 23);
            this.txtElementDetails.Multiline = true;
            this.txtElementDetails.Name = "txtElementDetails";
            this.txtElementDetails.ReadOnly = true;
            this.txtElementDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtElementDetails.Size = new System.Drawing.Size(776, 149);
            this.txtElementDetails.TabIndex = 1;
            // 
            // lblElementDetails
            // 
            this.lblElementDetails.AutoSize = true;
            this.lblElementDetails.Location = new System.Drawing.Point(12, 5);
            this.lblElementDetails.Name = "lblElementDetails";
            this.lblElementDetails.Size = new System.Drawing.Size(85, 15);
            this.lblElementDetails.TabIndex = 0;
            this.lblElementDetails.Text = "元素详细信息:";
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.logTextBox.Location = new System.Drawing.Point(12, 23);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.Size = new System.Drawing.Size(776, 137);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            // 
            // lblLogs
            // 
            this.lblLogs.AutoSize = true;
            this.lblLogs.Location = new System.Drawing.Point(12, 5);
            this.lblLogs.Name = "lblLogs";
            this.lblLogs.Size = new System.Drawing.Size(60, 15);
            this.lblLogs.TabIndex = 0;
            this.lblLogs.Text = "操作日志:";
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 428);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(800, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(112, 17);
            this.toolStripStatusLabel.Text = "准备就绪";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip);
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "UnlockWorld - 控件解锁工具";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Panel panelTop;
        private Button btnFinderTool;
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
    }
}

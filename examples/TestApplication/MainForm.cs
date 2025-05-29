using System;
using System.Drawing;
using System.Windows.Forms;

namespace TestApplication
{
    public partial class MainForm : Form
    {
        // Track our checkboxes for testing purposes
        private CheckBox? _disabledCheckbox;
        private CheckBox? _disabledCheckedCheckbox;
        private CheckBox? _disabledReadOnlyCheckbox;
        
        public MainForm()
        {
            InitializeComponent();
            SetupUI();
        }

        private void SetupUI()
        {
            // Setup form properties
            Text = "Test Application - Disabled UI Elements";
            Size = new Size(600, 500);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;

            // Create controls
            CreateButtons();
            CreateTextControls();
            CreateMenus();
            CreateCheckboxes();
            CreateDropdowns();
            
            // Add a status label at the bottom
            var statusLabel = new Label
            {
                Text = "Use UnlockWorld to enable these disabled controls",
                Location = new Point(30, 420),
                AutoSize = true,
                Font = new Font(Font.FontFamily, 10, FontStyle.Bold)
            };
            Controls.Add(statusLabel);
            
            // Add checkbox state test button
            var btnTestCheckbox = new Button
            {
                Text = "Test Checkbox State",
                Location = new Point(30, 380),
                Size = new Size(150, 30),
                Enabled = true
            };
            btnTestCheckbox.Click += TestCheckboxState_Click;
            Controls.Add(btnTestCheckbox);
        }

        private void TestCheckboxState_Click(object? sender, EventArgs e)
        {
            string status = "Checkbox Status:\n\n";
            
            if (_disabledCheckbox != null)
                status += $"Regular Checkbox - Enabled: {_disabledCheckbox.Enabled}, Checked: {_disabledCheckbox.Checked}\n";
                
            if (_disabledCheckedCheckbox != null)
                status += $"Pre-checked Checkbox - Enabled: {_disabledCheckedCheckbox.Enabled}, Checked: {_disabledCheckedCheckbox.Checked}\n";
                
            if (_disabledReadOnlyCheckbox != null)
                status += $"'Read-only' Checkbox - Enabled: {_disabledReadOnlyCheckbox.Enabled}, Checked: {_disabledReadOnlyCheckbox.Checked}\n";
                
            MessageBox.Show(status, "Checkbox State Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void CreateButtons()
        {
            // Normal enabled button
            var btnEnabled = new Button
            {
                Text = "Enabled Button",
                Location = new Point(30, 30),
                Size = new Size(120, 30),
                Enabled = true
            };
            btnEnabled.Click += (s, e) => MessageBox.Show("You clicked the enabled button!");
            Controls.Add(btnEnabled);

            // Disabled button
            var btnDisabled = new Button
            {
                Text = "Disabled Button",
                Location = new Point(30, 70),
                Size = new Size(120, 30),
                Enabled = false
            };
            btnDisabled.Click += (s, e) => MessageBox.Show("This should not be visible!");
            Controls.Add(btnDisabled);
        }

        private void CreateTextControls()
        {
            // Enabled text box
            var txtEnabled = new TextBox
            {
                Text = "Enabled TextBox",
                Location = new Point(200, 30),
                Size = new Size(150, 20),
                Enabled = true
            };
            Controls.Add(txtEnabled);

            // Disabled text box
            var txtDisabled = new TextBox
            {
                Text = "Disabled TextBox",
                Location = new Point(200, 70),
                Size = new Size(150, 20),
                Enabled = false
            };
            Controls.Add(txtDisabled);

            // Labels for text boxes
            Controls.Add(new Label
            {
                Text = "Enabled:",
                Location = new Point(170, 33),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Disabled:",
                Location = new Point(170, 73),
                AutoSize = true
            });
        }

        private void CreateMenus()
        {
            var menuStrip = new MenuStrip();
            var fileMenu = new ToolStripMenuItem("File");
            var editMenu = new ToolStripMenuItem("Edit");
            var viewMenu = new ToolStripMenuItem("View");

            // Add items to File menu
            fileMenu.DropDownItems.Add("New", null, (s, e) => MessageBox.Show("New file"));
            fileMenu.DropDownItems.Add("Open", null, (s, e) => MessageBox.Show("Open file"));
            fileMenu.DropDownItems.Add(new ToolStripSeparator());
            var saveItem = new ToolStripMenuItem("Save");
            saveItem.Enabled = false; // Disabled menu item
            fileMenu.DropDownItems.Add(saveItem);
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => Close());

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(editMenu);
            menuStrip.Items.Add(viewMenu);
            MainMenuStrip = menuStrip;
            Controls.Add(menuStrip);
        }

        private void CreateCheckboxes()
        {
            // Enabled checkbox
            var chkEnabled = new CheckBox
            {
                Text = "Enabled Checkbox",
                Location = new Point(30, 120),
                AutoSize = true,
                Enabled = true
            };
            Controls.Add(chkEnabled);

            // Disabled checkbox (unchecked)
            _disabledCheckbox = new CheckBox
            {
                Text = "Disabled Checkbox",
                Location = new Point(30, 150),
                AutoSize = true,
                Enabled = false,
                Checked = false
            };
            Controls.Add(_disabledCheckbox);
            
            // Disabled checkbox (pre-checked)
            _disabledCheckedCheckbox = new CheckBox
            {
                Text = "Disabled Checkbox (Pre-checked)",
                Location = new Point(30, 180),
                AutoSize = true,
                Enabled = false,
                Checked = true
            };
            Controls.Add(_disabledCheckedCheckbox);
            
            // Simulating a read-only checkbox using disabled state
            _disabledReadOnlyCheckbox = new CheckBox
            {
                Text = "Disabled 'Read-Only' Checkbox",
                Location = new Point(30, 210),
                AutoSize = true,
                Enabled = false,
                Checked = true
            };
            Controls.Add(_disabledReadOnlyCheckbox);
            
            // Add helper label
            var helperLabel = new Label
            {
                Text = "Try enabling these checkboxes with UnlockWorld",
                Location = new Point(30, 240),
                AutoSize = true,
                ForeColor = Color.Blue
            };
            Controls.Add(helperLabel);
        }

        private void CreateDropdowns()
        {
            // Enabled combo box
            var cboEnabled = new ComboBox
            {
                Location = new Point(200, 120),
                Size = new Size(150, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = true
            };
            cboEnabled.Items.AddRange(new string[] { "Option 1", "Option 2", "Option 3" });
            cboEnabled.SelectedIndex = 0;
            Controls.Add(cboEnabled);

            // Disabled combo box
            var cboDisabled = new ComboBox
            {
                Location = new Point(200, 150),
                Size = new Size(150, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false
            };
            cboDisabled.Items.AddRange(new string[] { "Option 1", "Option 2", "Option 3" });
            cboDisabled.SelectedIndex = 0;
            Controls.Add(cboDisabled);

            // Labels for combo boxes
            Controls.Add(new Label
            {
                Text = "Enabled:",
                Location = new Point(170, 123),
                AutoSize = true
            });

            Controls.Add(new Label
            {
                Text = "Disabled:",
                Location = new Point(170, 153),
                AutoSize = true
            });
        }
    }
}

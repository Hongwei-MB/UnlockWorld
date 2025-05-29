using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace UnlockWorld
{
    public partial class Form1 : Form
    {
        private readonly ILogger<Form1> _logger;

        public Form1(ILogger<Form1> logger)
        {
            InitializeComponent();
            _logger = logger;
            _logger.LogInformation("Form1 启动");
        }
    }
}

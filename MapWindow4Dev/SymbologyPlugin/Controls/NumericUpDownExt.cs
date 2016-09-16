﻿
namespace mwSymbology.Controls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    
    public partial class NumericUpDownExt : System.Windows.Forms.NumericUpDown 
    {
        /// <summary>
        /// Creates new instance of the NumericUpDownExt class
        /// </summary>
        public NumericUpDownExt()
        {
            InitializeComponent();
        }

        public NumericUpDownExt(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Setting the value of NumericUpDown control. The range of input value is checked
        /// </summary>
        public void SetValue(double value)
        {
            if (value <= (double)this.Minimum) this.Value = this.Minimum;
            else if (value >= (double)this.Maximum) this.Value = this.Maximum;
            else this.Value = (decimal)value;
        }
    }
}

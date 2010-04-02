﻿/*
Loconet toolbox
Copyright (C) 2010 Modelspoorgroep Venlo, Ewout Prangsma

This program is free software; you can redistribute it and/or
modify it under the terms of the GNU General Public License
as published by the Free Software Foundation; either version 2
of the License, or (at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LocoNetToolBox.Protocol;

namespace LocoNetToolBox.WinApp.Controls
{
    public partial class LocoIOConfigurationForm : Form
    {
        private LocoBuffer lb;
        private LocoNetAddress address;

        /// <summary>
        /// Default ctor
        /// </summary>
        public LocoIOConfigurationForm()
        {
            InitializeComponent();
            cmdWriteAll.Enabled = false;
            cmdWriteChanges.Enabled = false;
        }

        /// <summary>
        /// Initialize for a specific module
        /// </summary>
        internal void Initialize(LocoBuffer lb, LocoNetAddress address)
        {
            this.lb = lb;
            this.address = address;
            if (lb != null)
            {
                readWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Read all settings.
        /// </summary>
        private void readWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            configurationControl.ReadAll(lb, address);
        }

        /// <summary>
        /// Enable now.
        /// </summary>
        private void readWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmdWriteAll.Enabled = true;
            cmdWriteChanges.Enabled = true;
        }
    }
}
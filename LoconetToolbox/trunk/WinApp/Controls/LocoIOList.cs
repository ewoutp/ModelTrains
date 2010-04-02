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
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using LocoNetToolBox.Protocol;

namespace LocoNetToolBox.WinApp.Controls
{
    public partial class LocoIOList : UserControl
    {
        private LocoBuffer lb;

        /// <summary>
        /// Default ctor
        /// </summary>
        public LocoIOList()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Attach to the given locobuffer.
        /// </summary>
        internal LocoBuffer LocoBuffer
        {
            set
            {
                if (lb != null)
                {
                    lb.PreviewMessage -= new MessageHandler(lb_PreviewMessage);
                }
                this.lb = value;
                if (lb != null)
                {
                    lb.PreviewMessage += new MessageHandler(lb_PreviewMessage);
                }
            }
        }

        /// <summary>
        /// Listen to loconet message.
        /// Use results of Query requests to generate a list of locoio modules.
        /// </summary>
        private bool lb_PreviewMessage(byte[] message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MessageHandler(lb_PreviewMessage), message);
            }
            else
            {
                var response = Response.Decode(message) as PeerXferResponse;
                if (response != null)
                {
                    if (response.SvAddress == 0)
                    {
                        if (response.IsSourcePC)
                        {
                            // Query request
                            lbModules.Items.Clear();
                        }
                        else
                        {
                            var item = new ListViewItem();
                            item.Text = response.Source.ToString();
                            item.Tag = response.Source;
                            item.SubItems.Add(string.Format("{0}.{1}", response.LocoIOVersion / 100, response.LocoIOVersion % 100));
                            lbModules.Items.Add(item);
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Edit on activate
        /// </summary>
        private void ctxProgram_Click(object sender, EventArgs e)
        {
            using (var dialog = new LocoIOConfigurationForm())
            {
                if (lbModules.SelectedItems.Count > 0)
                {
                    var item = lbModules.SelectedItems[0];
                    dialog.Initialize(lb, (LocoNetAddress)item.Tag);
                }
                else
                {
                    dialog.Initialize(null, new LocoNetAddress(81, 1));
                }
                dialog.ShowDialog(this);
            }
        }
    }
}
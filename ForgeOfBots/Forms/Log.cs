using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Forms
{
   public partial class Log : Form
   {
      public bool AttachState = true;
      public Form mainForm = null;
      private Point lastLocation;
      public Log(Point startLocation)
      {
         InitializeComponent();
         Location = startLocation;
         lastLocation = Location;
         Text = $"Log ({strings.Attach})";
      }
      public void FillLog(List<string> msgHistory)
      {
         lbOutput.Items.Clear();
         lbOutput.Items.AddRange(msgHistory.ToArray());
         lbOutput.TopIndex = lbOutput.Items.Count - 1;
      }
      public void UpdateLocation(Point newLocation)
      {
         if (AttachState)
            Location = newLocation;
      }
      public void SwitchAttachState(bool newState)
      {
         Text = $"Log ({btnDeAttach.Text})";
         AttachState = newState;
         btnDeAttach.Text = AttachState ? strings.Detach : strings.Attach;
         if(AttachState)
            UpdateLocation(new Point(mainForm.Location.X + mainForm.Size.Width, mainForm.Location.Y));
      }
      private void btnDeAttach_Click(object sender, EventArgs e)
      {
         SwitchAttachState(!AttachState);
      }
      private void Log_LocationChanged(object sender, EventArgs e)
      {
         if (AttachState)
            Location = lastLocation;
         lastLocation = Location;
      }
   }
}

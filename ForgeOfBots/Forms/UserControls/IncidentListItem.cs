using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Forms.UserControls
{
   public partial class IncidentListItem : UserControl
   {
      public string IRarity
      {
         get
         {
            return lblRarity.Text;
         }
         set
         {
            lblRarity.Text = value;
         }
      }
      public string ILocation
      {
         get
         {
            return lblLocation.Text;
         }
         set
         {
            lblLocation.Text = value;
         }
      }
      public IncidentListItem()
      {
         InitializeComponent();
      }
   }
}

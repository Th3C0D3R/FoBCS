using ForgeOfBots.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ForgeOfBots.Forms.UserControls
{
   public partial class WorkerItem : UserControl
   {
      public string Title
      {
         get
         {
            return lblTitle.Text;
         }
         set
         {
            if (InvokeRequired)
               Invoker.SetProperty(lblTitle, () => lblTitle.Text, value);
            else
               lblTitle.Text = value;
         }
      }
      public string CountText
      {
         get
         {
            return lblTextCounter.Text;
         }
         set
         {
            if (InvokeRequired)
               Invoker.SetProperty(lblTextCounter, () => lblTextCounter.Text, value);
            else
               lblTextCounter.Text = value;
         }
      }
      public string BeforeCountText
      {
         get
         {
            return lblTextBeforeCounter.Text;
         }
         set
         {
            if (InvokeRequired)
               Invoker.SetProperty(lblTextBeforeCounter, () => lblTextBeforeCounter.Text, value);
            else
               lblTextBeforeCounter.Text = value;
         }
      }
      public int ProgressValue
      {
         get
         {
            return mpbProgress.Value;
         }
         set
         {
            if (InvokeRequired)
               Invoker.SetProperty(mpbProgress, () => mpbProgress.Value, value);
            else
               mpbProgress.Value = value;
         }
      }
      public MetroFramework.Controls.MetroProgressBar ProgressBar
      {
         get
         {
            return mpbProgress;
         }
         private set { }
      }
      public int ID { get; set; }
      public WorkerItem()
      {
         InitializeComponent();
      }
   }
}

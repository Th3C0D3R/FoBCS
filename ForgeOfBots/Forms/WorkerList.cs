using ForgeOfBots.Forms.UserControls;
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
   public partial class WorkerList : Form
   {
      private List<WorkerItem> WorkerItems = new List<WorkerItem>();
      public WorkerList()
      {
         InitializeComponent();
      }
      public void AddWorkerItem(WorkerItem wi)
      {
         WorkerItems.Add(wi);
         flpItems.Controls.Add(wi);
         if (WorkerItems.Count > 0)
         {
            Width = WorkerItems.OrderBy(o => o.Width).ToList().First().Width;
            Width += (Width / 16);
         }
      }
      public (bool, bool) RemoveWorkerByID(int id)
      {
         bool success = WorkerItems.Remove(WorkerItems.Find(e => e.ID == id));
         if (InvokeRequired)
            flpItems.Invoke((MethodInvoker)delegate
            {
               flpItems.Controls.RemoveByKey(id.ToString());
               flpItems.Invalidate();
               flpItems.Update();
            });
         else
         {
            flpItems.Controls.RemoveByKey(id.ToString());
            flpItems.Invalidate();
            flpItems.Update();
         }
         bool close = false;
         if (WorkerItems.Count == 0)
            close = true;
         return (success, close);
      }
      public void UpdateWorkerLabel(int id, string val)
      {
         WorkerItem item = WorkerItems.Find(e => e.ID == id);
         if (item.ID > -1)
         {
            if (InvokeRequired)
               item.Invoke((MethodInvoker)delegate
               {
                  item.CountText = val;
               });
            else
               item.CountText = val;
         }
      }
      public void UpdateWorkerProgressBar(int id, int value, int max = 100)
      {
         WorkerItem item = WorkerItems.Find(e => e.ID == id);
         if (item.ID > -1)
         {
            if (InvokeRequired)
            {
               item.Invoke((MethodInvoker)delegate
               {
                  item.ProgressValue = value;
                  item.ProgressBar.Maximum = max;
               });
            }
            else
            {
               item.ProgressValue = value;
               item.ProgressBar.Maximum = max;
            }
            if (value == max)
            {
               if (flpItems.Controls.Contains(item))
               {
                  flpItems.Invoke((MethodInvoker)delegate
                  {
                     flpItems.Controls.Remove(item);
                  });
               }
                  
            }
         }
      }
   }
}

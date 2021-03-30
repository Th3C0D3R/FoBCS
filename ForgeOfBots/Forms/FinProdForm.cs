using ForgeOfBots.GameClasses.ResponseClasses;
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

namespace ForgeOfBots.Forms
{
   public partial class FinProdForm : Form
   {
      List<EntityProd> Items = new List<EntityProd>();
      private CustomEvent _CollectAll;
      public event CustomEvent CollectAll
      {
         add
         {
            if (_CollectAll == null || !_CollectAll.GetInvocationList().ToList().Contains(value))
               _CollectAll += value;
         }
         remove
         {
            _CollectAll -= value;
         }
      }
      private CustomEvent _CollectSelected;
      public event CustomEvent CollectSelected
      {
         add
         {
            if (_CollectSelected == null || !_CollectSelected.GetInvocationList().ToList().Contains(value))
               _CollectSelected += value;
         }
         remove
         {
            _CollectSelected -= value;
         }
      }
      public FinProdForm()
      {
         InitializeComponent();
         btnCollectAll.Text = i18n.getString("GUI.FinProd.CollectAll");
         btnCollectSelected.Text = i18n.getString("GUI.FinProd.CollectSelected");
         Text = i18n.getString("GUI.Production.ListFinProd");
      }
      public void ClearList()
      {
         lvFinProds.Items.Clear();
      }
      public void AddItem(EntityProd item)
      {
         Items.Add(item);
      }
      public void AddItem(EntityProd[] items)
      {
         Items.AddRange(items);
      }

      private void FinProdForm_Shown(object sender, EventArgs e)
      {
         lvFinProds.Items.AddRange(Items.ToArray());
         lvFinProds.Update();
      }

      private void BtnCollectAll_Click(object sender, EventArgs e)
      {
         _CollectAll?.Invoke(null, lvFinProds.Items);
      }
      private void BtnCollectSelected_Click(object sender, EventArgs e)
      {
         _CollectSelected?.Invoke(null, lvFinProds.Items);
      }
   }
}

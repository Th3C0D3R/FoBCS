using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace UCPremiumLibrary
{
   public partial class UCPremium : UserControl
   {
      [Category("Appearance")]
      [Description("Set or get the Labletext")]
      [Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
      public string LabelName
      {
         get
         {
            return lblName.Text;
         }
         set
         {
            lblName.Text = value;
         }
      }
      public bool Checked
      {
         get
         {
            return mtToggle.Checked;
         }
         set
         {
            data = 1;
            mtToggle.Checked = value;
         }
      }
      public BotType Type{ get; set; }
      private int data = -1;
      private CustomEvent _ToggleFlipped;
      public event CustomEvent ToggleFlipped
      {
         add
         {
            if (_ToggleFlipped == null || !_ToggleFlipped.GetInvocationList().Contains(value))
               _ToggleFlipped += value;
         }
         remove
         {
            _ToggleFlipped -= value;
         }
      }
      public delegate void CustomEvent(object sender, dynamic data = null);
      public UCPremium()
      {
         InitializeComponent();
      }

      private void mtToggle_CheckedChanged(object sender, EventArgs e)
      {
         _ToggleFlipped?.Invoke(this, data);
      }
   }
}

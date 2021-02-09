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

namespace ForgeOfBots.Forms.UserControls
{
   public partial class ucIgnorePlayer : UserControl
   {
      private CustomEvent _Remove;
      public event CustomEvent Remove
      {
         add
         {
            if (_Remove == null || !_Remove.GetInvocationList().ToList().Contains(value))
               _Remove += value;
         }
         remove
         {
            _Remove -= value;
         }
      }
      public Player Player
      {
         get
         {
            return _player;
         }
         set
         {
            _player = value;
            label1.Text = _player.name + $"({i18n.getString($"GUI.Sniper.PlayerIndentifier.{(_player.is_friend ? "Friend" : _player.is_guild_member ? "Member" : "Neighbor")}")})";
         }
      }
      private Player _player;
      public ucIgnorePlayer()
      {
         InitializeComponent();
      }
      private void PbRemove_Click(object sender, EventArgs e)
      {
         _Remove?.Invoke(this, _player);
      }
   }
}

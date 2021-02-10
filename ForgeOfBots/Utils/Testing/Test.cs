using ForgeOfBots.Forms;
using ForgeOfBots.Forms.UserControls;
using System.Drawing;
using System.Timers;
using static ForgeOfBots.Utils.Helper;

namespace ForgeOfBots.Utils.Testing
{


   public class Rootobject
   {
      public Class1[] Property1 { get; set; }
   }

   public class Class1
   {
      public Responsedata responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class Responsedata
   {
      public int id { get; set; }
      public int player_id { get; set; }
      public string cityentity_id { get; set; }
      public string type { get; set; }
      public int x { get; set; }
      public int y { get; set; }
      public State state { get; set; }
      public int level { get; set; }
      public int max_level { get; set; }
      public dynamic bonus { get; set; }
      public string __class__ { get; set; }
   }

   public class State
   {
      public Paused_State paused_state { get; set; }
      public int next_state_transition_in { get; set; }
      public int next_state_transition_at { get; set; }
      public int invested_forge_points { get; set; }
      public int forge_points_for_level_up { get; set; }
      public string __class__ { get; set; }
   }

   public class Paused_State
   {
      public dynamic current_product { get; set; }
      public bool boosted { get; set; }
      public bool is_motivated { get; set; }
      public string __class__ { get; set; }
   }

}

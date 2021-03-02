using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{

   public class MessageRoot
   {
      public MessageData responseData { get; set; }
      public string requestClass { get; set; }
      public string requestMethod { get; set; }
      public int requestId { get; set; }
      public string __class__ { get; set; }
   }

   public class MessageData
   {
      public object[] unreadMessagesCategories { get; set; }
      public Category category { get; set; }
      public string __class__ { get; set; }
   }

   public class Category
   {
      public string type { get; set; }
      public int totalTeasers { get; set; }
      public Teaser[] teasers { get; set; }
      public string __class__ { get; set; }
   }

   public class Teaser
   {
      public int id { get; set; }
      public int type { get; set; }
      public string title { get; set; }
      public string teaserImage { get; set; }
      public Lastmessage lastMessage { get; set; }
      public bool isFavorite { get; set; }
      public bool isImportant { get; set; }
      public bool isHidden { get; set; }
      public bool areAllMessagesIgnored { get; set; }
      public string __class__ { get; set; }
   }

   public class Lastmessage
   {
      public Sender sender { get; set; }
      public int id { get; set; }
      public int conversationId { get; set; }
      public string text { get; set; }
      public string date { get; set; }
      public bool deleted { get; set; }
      public string __class__ { get; set; }
   }

   public class Sender
   {
      public int player_id { get; set; }
      public string name { get; set; }
      public string avatar { get; set; }
      public string __class__ { get; set; }
   }

}

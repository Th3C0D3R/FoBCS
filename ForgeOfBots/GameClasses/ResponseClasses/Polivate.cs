using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgeOfBots.GameClasses.ResponseClasses
{
    public class MapEntity
    {
        public int id { get; set; }
        public int player_id { get; set; }
        public string cityentity_id { get; set; }
        public string type { get; set; }
        public int connected { get; set; }
        public int level { get; set; }
    }

    public class ResponseData
    {
        public string action { get; set; }
        public MapEntity mapEntity { get; set; }
        public string __class__ { get; set; }
    }

    public class Polivate
    {
        public ResponseData responseData { get; set; }
        public string requestClass { get; set; }
        public string requestMethod { get; set; }
        public int requestId { get; set; }
        public string __class__ { get; set; }
    }

    public class PolivateResultResources
    {
        public int money { get; set; }
    }

    public class PolivateResultResponseData
    {
        public PolivateResultResources resources { get; set; }
        public string __class__ { get; set; }
    }

    public class PolivateResult
    {
        public PolivateResultResponseData responseData { get; set; }
        public string requestClass { get; set; }
        public string requestMethod { get; set; }
        public int requestId { get; set; }
        public string __class__ { get; set; }
    }
}

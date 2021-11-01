using System;

namespace CustomerServer.Controllers
{
    [Serializable]
    public class TransferObj {
        public string Action { get; set; }
        public string Arg { get; set; }
    }
}
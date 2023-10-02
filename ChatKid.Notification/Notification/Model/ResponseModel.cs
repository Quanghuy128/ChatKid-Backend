using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatKid.Notification.Notification.Model
{
    public class ResponseModel
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess {  get; set; }
        [JsonProperty("message")]
        public string? Message { get; set; }
    }
}

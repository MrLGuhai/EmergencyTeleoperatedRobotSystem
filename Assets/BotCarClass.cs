using ProtoBuf;
using System.Collections.Generic;
using System.IO;

namespace Cas.ProtobufNet
{
    [ProtoContract]
    public class BotCar
    {
        [ProtoMember(1)]
        public List<int> moveSequence { get; set; }
    }
}


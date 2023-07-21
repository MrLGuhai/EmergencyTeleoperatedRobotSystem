using ProtoBuf;
using System.IO;

namespace Cas.ProtobufNet
{
    [ProtoContract]
    public class SoundSource
    {
        [ProtoMember(1)]
        public float x { get; set; }
        [ProtoMember(2)]
        public float y { get; set; }
        [ProtoMember(3)]
        public float z { get; set; }
    }
}


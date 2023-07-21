using ProtoBuf;
using System.Collections.Generic;

namespace Cas.ProtobufNet
{
    [ProtoContract]
    public class Mesh
    {
        [ProtoMember(1)]
        public List<V1> v1 { get; set; }

        [ProtoMember(2)]
        public List<V2> v2 { get; set; }

        [ProtoMember(3)]
        public List<V3> v3 { get; set; }

        [ProtoMember(4)]
        public List<float> r { get; set; }

        [ProtoMember(5)]
        public List<float> g { get; set; }

        [ProtoMember(6)]
        public List<float> b { get; set; }
    }

    [ProtoContract]
    public class V1
    {
        [ProtoMember(1)]
        public float x { get; set; }

        [ProtoMember(2)]
        public float y { get; set; }

        [ProtoMember(3)]
        public float z { get; set; }
    }

    [ProtoContract]
    public class V2
    {
        [ProtoMember(1)]
        public float x { get; set; }

        [ProtoMember(2)]
        public float y { get; set; }

        [ProtoMember(3)]
        public float z { get; set; }
    }

    [ProtoContract]
    public class V3
    {
        [ProtoMember(1)]
        public float x { get; set; }

        [ProtoMember(2)]
        public float y { get; set; }

        [ProtoMember(3)]
        public float z { get; set; }
    }
}
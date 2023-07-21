using ProtoBuf;
using System.Collections.Generic;

[ProtoContract]
public class Polygon
{
    [ProtoMember(1)]
    public List<V1> v1 = new List<V1>();

    [ProtoMember(2)]
    public List<V2> v2 = new List<V2>();

    [ProtoMember(3)]
    public List<V3> v3 = new List<V3>();

    [ProtoMember(4)]
    public List<float> r = new List<float>();

    [ProtoMember(5)]
    public List<float> g = new List<float>();

    [ProtoMember(6)]
    public List<float> b = new List<float>();

    [ProtoMember(7)]
    public int type;
}

[ProtoContract]
public class V1
{
    [ProtoMember(1)]
    public float x;

    [ProtoMember(2)]
    public float y;

    [ProtoMember(3)]
    public float z;
}

[ProtoContract]
public class V2
{
    [ProtoMember(1)]
    public float x;

    [ProtoMember(2)]
    public float y;

    [ProtoMember(3)]
    public float z;
}

[ProtoContract]
public class V3
{
    [ProtoMember(1)]
    public float x;

    [ProtoMember(2)]
    public float y;

    [ProtoMember(3)]
    public float z;
}

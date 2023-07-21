using ProtoBuf;
using Cas.ProtobufNet;

namespace Cas.ProtobufNet
{
    [ProtoContract]
    public class DataMessage
    {
        public enum Type
        {
            MESH = 0,
            SOUND_SOURCE = 1,
            TEMPERATURE = 2,
            BOT_CAR = 3,
            BOT_GRIPPER = 4,
            BOT_MOTOR = 5
        }

        [ProtoMember(1)]
        public Type type { get; set; }

        [ProtoMember(2)]
        public Mesh mesh { get; set; }

        // [ProtoMember(3)]
        // public SoundSource soundSource { get; set; }

        // // [ProtoMember(4)]
        // // public Temperature temperature { get; set; }

        // [ProtoMember(4)]
        // public BotCar botCar { get; set; }

        // [ProtoMember(6)]
        // public BotGripper botGripper { get; set; }

        // [ProtoMember(7)]
        // public BotMotor botMotor { get; set; }
    }
}


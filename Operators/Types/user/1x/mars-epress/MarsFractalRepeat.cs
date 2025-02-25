using T3.Core.DataTypes;
using T3.Core.Operator;
using T3.Core.Operator.Attributes;
using T3.Core.Operator.Slots;

namespace T3.Operators.Types.Id_92cdd8e1_5f31_4636_9ed3_f59a1e018586
{
    public class MarsFractalRepeat : Instance<MarsFractalRepeat>
    {
        [Output(Guid = "786c3367-e7df-4811-a735-946e3b3d9ff3")]
        public readonly Slot<Command> Output = new Slot<Command>();

        [Output(Guid = "a35335dd-b653-430b-9fa5-68ee48aef6c6")]
        public readonly Slot<T3.Core.DataTypes.BufferWithViews> TargetPoints = new Slot<T3.Core.DataTypes.BufferWithViews>();

        [Input(Guid = "2d8ea2ac-b7ce-4296-a606-6e6d74830763")]
        public readonly InputSlot<T3.Core.DataTypes.MeshBuffers> Mesh = new InputSlot<T3.Core.DataTypes.MeshBuffers>();

        [Input(Guid = "499393f9-a751-4c72-b103-7fa849f9bbca")]
        public readonly InputSlot<float> Scaling = new InputSlot<float>();

        [Input(Guid = "59eb39d8-9708-4529-9dc6-32b7c43785a9")]
        public readonly InputSlot<System.Numerics.Vector3> Offset = new InputSlot<System.Numerics.Vector3>();

        [Input(Guid = "6c3c4830-cc8c-4a66-aa01-281fc0dafa16")]
        public readonly InputSlot<System.Numerics.Vector3> Rotation = new InputSlot<System.Numerics.Vector3>();


    }
}


using Raylib_CsLo;
using System;
using LogicText.Linking;
namespace LogicText
{
    [Serializable]
    public class BitGate : Node
    {


        public BitGate()
        {
                GateName = "#124# BitGate";
        }

        public BitGate(int InputCount = 2, int OutputCount = 1) : this()
        {
            for (int i = 0; i < InputCount; i++)
                Inputs.Add(new Input(i));
            for (int i = 0; i < OutputCount; i++)
                Outputs.Add(new Output(i));
        }
        public override void Draw()
        {
            base.Draw();

            Raylib.DrawText($"{OutPutValue}", Position.X + (Size.X / 2), Position.Y + 30, 8, Raylib.RED);
        }
        public override void StepUpdate()
        {
            if (Inputs[0].Value == true)
                OutPutValue = Convert.ToByte(Inputs[1].Value);
        }
    }
}
using Raylib_CsLo;
using System;
using System.Numerics;

namespace LogicText
{
    [Serializable]
    public class NotGate : Node
    {


        public NotGate()
        {
            Size = new Vector2(120, 145);
            GateName = "#139# Not Gate";
        }

        public NotGate(int InputCount = 1, int OutputCount = 1) : this()
        {
            for (int i = 0; i < InputCount; i++)
                Inputs.Add(new Input(i));
            for (int i = 0; i < OutputCount; i++)
                Outputs.Add(new Output(i));
        }

        public override void Draw()
        {
            base.Draw();
        }

        public override void StepUpdate()
        {
            base.StepUpdate();

            OutPutValue = Convert.ToByte(!Inputs[0].Value);
        }
    }
}
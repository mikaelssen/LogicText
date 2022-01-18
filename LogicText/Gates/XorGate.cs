using Raylib_CsLo;
using System;
using LogicText.Linking;

namespace LogicText
{
    /// <summary>
    /// Only Ture if one input is True.
    /// </summary>
    [Serializable]
    public class XorGate : Node
    {
        public XorGate()
        {
                GateName = "#39# Xor Gate";
        }

        public XorGate(int InputCount = 2, int OutputCount = 1) : this()
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
            if (Inputs[0].Value ^ Inputs[1].Value)
                OutPutValue = 1;
            else
                OutPutValue = 0;
        }
    }
}
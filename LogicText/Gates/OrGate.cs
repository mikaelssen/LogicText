using Raylib_CsLo;
using System;
using System.Numerics;
using LogicText.Linking;

namespace LogicText
{
    /// <summary>
    /// True if any input is True
    /// </summary>
    [Serializable]
    public class OrGate : Node
    {

        public OrGate()
        {
            GateName = "#119# Or Gate";
        }

        public OrGate(int InputCount = 2, int OutputCount = 1) : this()
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


            bool v = false;

            foreach (var i in Inputs) //it's irrelevant how big the gate is :)
            {
                if (i.Value)
                {
                    v = true;
                    break;
                }
                else
                    v = false;
            }

            OutPutValue = Convert.ToByte(v);
        }
    }
}
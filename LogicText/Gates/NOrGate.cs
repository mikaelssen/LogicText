using Raylib_CsLo;
using System;
using System.Numerics;

namespace LogicText
{
    [Serializable]
    public class NOrGate : Node
    {


        public NOrGate()
        {
            GateName = "#19# NOr Gate";
        }

        public NOrGate(int InputCount = 4, int OutputCount = 1) : this()
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


            bool v = true;

            foreach (var i in Inputs) //it's irrelevant how big the gate is :)
            {
                if (i.Value)
                {
                    v = false;
                    break;
                }
                else
                    v = true;
            }

            OutPutValue = Convert.ToByte(v);
        }
    }
}
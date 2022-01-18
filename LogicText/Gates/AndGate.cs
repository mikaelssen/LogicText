using Raylib_CsLo;
using System;

namespace LogicText
{

    [Serializable]
    public class AndGate : Node
    {
        public AndGate()
        {
            GateName = "#198# AND GATE";
        }

        public AndGate(int InputCount = 2, int OutputCount = 1) : this()
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

            foreach (var i in Inputs) //it's irrelevant how big the and gate is :)
            {
                if (i.Value)
                    v = true;
                else
                {
                    v = false;
                    break;
                }
            }

            foreach (var i in Outputs)
            {
                i.Value = v;
            }

            OutPutValue = Convert.ToByte(v);


        }
    }
}
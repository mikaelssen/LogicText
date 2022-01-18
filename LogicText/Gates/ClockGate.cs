using Raylib_CsLo;
using System;

namespace LogicText
{
    [Serializable]
    public class ClockGate : Node
    {
        DateTime lastTime;
        public ClockGate()
        {
                GateName = "#39# TickTocker";
        }

        public ClockGate(int InputCount = 0, int OutputCount = 1) : this()
        {
            for (int i = 0; i < InputCount; i++)
                Inputs.Add(new Input(i));
            for (int i = 0; i < OutputCount; i++)
                Outputs.Add(new Output(i));
            lastTime = DateTime.UtcNow;
        }
        public override void Draw()
        {
            base.Draw();

        }
        public override void StepUpdate()
        {
            if ((DateTime.UtcNow - lastTime ).TotalSeconds >= 1)
            {
                if (OutPutValue > 0)
                    OutPutValue--;
                else
                    OutPutValue++;
                lastTime = DateTime.UtcNow;
            }
            base.StepUpdate();
        }

    }
}
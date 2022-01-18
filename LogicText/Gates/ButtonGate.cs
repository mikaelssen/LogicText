using LogicText.Linking;
using Raylib_CsLo;
using System;

namespace LogicText
{

    [Serializable]
    public class ButtonGate : Node
    {
        public ButtonGate()
        {
                GateName = "#129# Button Gate";
        }

        public ButtonGate(int InputCount = 0, int OutputCount = 1) : this()
        {
            for (int i = 0; i < InputCount; i++)
                Inputs.Add(new Input(i));
            for (int i = 0; i < OutputCount; i++)
                Outputs.Add(new Output(i));
        }
        public override void Draw()
        {
            base.Draw();

            Rectangle r = new Rectangle(Position.X, Position.Y + 40 + (0 * 25), 40, 25);
            var Clicked = RayGui.GuiButton(r, $"{OutPutValue}");

            if (Clicked)
                if (OutPutValue > 0)
                    OutPutValue--;
                else
                    OutPutValue++;
        }
        public override void StepUpdate()
        {
            base.StepUpdate();
        }
    }
}
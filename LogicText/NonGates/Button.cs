using LogicText.Linking;
using Raylib_CsLo;
using System;

namespace LogicText
{

    /// <summary>
    /// Only True if clicked to be True
    /// </summary>
    [Serializable]
    public class Button : Node
    {


        public Button()
        {
                GateName = "#129# Button Gate";
        }

        public Button(int InputCount = 0, int OutputCount = 1) : this()
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
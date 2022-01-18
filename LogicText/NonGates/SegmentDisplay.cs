using LogicText.Linking;
using Raylib_CsLo;
using System;

namespace LogicText
{

    /// <summary>
    /// Displays segments
    /// </summary>
    [Serializable]
    public class SegmentDisplay : Node
    {
        public SegmentDisplay()
        {
                GateName = "#132# Indicator Gate";
        }

        public SegmentDisplay(int InputCount = 7, int OutputCount = 0) : this()
        {
            for (int i = 0; i < InputCount; i++)
                Inputs.Add(new Input(i));
            for (int i = 0; i < OutputCount; i++)
                Outputs.Add(new Output(i));
        }
        public override void Draw()
        {
            base.Draw();

            Rectangle a = new Rectangle(Position.X + 55 + 25, Position.Y + 35, 25 * 2, 40);
            Rectangle b = new Rectangle(Position.X + 55 + 25 * 3, Position.Y + 40 + 35, 25, 40 * 2);
            Rectangle c = new Rectangle(Position.X + 55 + 25 * 3, Position.Y + 40 + 35 * 4, 25, 40 * 2);
            Rectangle d = new Rectangle(Position.X + 55 + 25, Position.Y + 40 + 35 * 6, 25 * 2, 40);
            Rectangle e = new Rectangle(Position.X + 55, Position.Y + 40 + 35 * 4, 25, 40 * 2);
            Rectangle f = new Rectangle(Position.X + 55, Position.Y + 40 + 35, 25, 40 * 2);
            Rectangle g = new Rectangle(Position.X + 55 + 25, Position.Y + 35 + 40 * 3, 25 * 2, 40);

            Raylib.DrawRectangleRec(a, Inputs[0].Value ? Raylib.GREEN : Raylib.BLACK);
            Raylib.DrawRectangleRec(b, Inputs[1].Value ? Raylib.GREEN : Raylib.BLACK);
            Raylib.DrawRectangleRec(c, Inputs[2].Value ? Raylib.GREEN : Raylib.BLACK);
            Raylib.DrawRectangleRec(d, Inputs[3].Value ? Raylib.GREEN : Raylib.BLACK);
            Raylib.DrawRectangleRec(e, Inputs[4].Value ? Raylib.GREEN : Raylib.BLACK);
            Raylib.DrawRectangleRec(f, Inputs[5].Value ? Raylib.GREEN : Raylib.BLACK);
            Raylib.DrawRectangleRec(g, Inputs[6].Value ? Raylib.GREEN : Raylib.BLACK);


        }
        public override void StepUpdate()
        {
            base.StepUpdate();
        }
    }
}
namespace LogicText
{

    using Raylib_CsLo;
    using System;
    using System.Collections.Generic;
    using System.Numerics;
    using System.Runtime.CompilerServices;
    using System.Linq;
    using System.Text.Json.Serialization;
    using System.ComponentModel;
    using LogicText.Linking;

    public class StandinVector2
    {
        public float X { get; set; }
        public float Y { get; set; }

        public StandinVector2() { }
        public StandinVector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public bool CanConvert(Type typeToConvert)
        {
            if (typeof(Vector2) == typeToConvert)
                return true;

            return false;
        }
    }


    [Serializable]
    public class Node
    {
        public int NodeID { get;}

        public Vector2 Position { get; set; }

        public Vector2 Size { get; set; } = new Vector2(130,200);

        [JsonIgnore]
        public bool ExitWindow { get; set; }

        public byte OutPutValue { get; set; }

        public string GateName { get; set; } = "Undefined Gate";

        public List<Input> Inputs { get; set; }
        public List<LinkedLink> InputLinks { get; set; } = new List<LinkedLink> { };
        public List<Output> Outputs { get; set; }
        public List<LinkedLink> OutputLinks { get; set; } = new List<LinkedLink> { };

        private Vector2 MousePosition;
        private Vector2 StartPosition;
        private bool dragWindow = false;


        protected Node()
        {
            NodeID = RuntimeHelpers.GetHashCode(this);
            Inputs = new List<Input>();
            Outputs = new List<Output>();
        }

        /// <summary>
        /// Used to updated the nodes logic, only fierd from other nodes. 'simulates signal delay' sort of
        /// </summary>
        public virtual void StepUpdate()
        {
        }

        public void DeleteNode()
        {
            Linker.DeleteNodeLinks(this);
        }

        public virtual void Update(Camera2D camera)
        {
            MousePosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);

            if (Raylib.IsMouseButtonPressed(Raylib.MOUSE_LEFT_BUTTON))
            {
                if (Raylib.CheckCollisionPointRec(MousePosition, new Rectangle(Position.X, Position.Y, Size.X - 25, 23))) 
                {
                    dragWindow = true;
                    StartPosition = MousePosition;
                }
            }

            if (dragWindow)
            {
                Vector2 temp = Position - StartPosition;
                StartPosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
                Position = StartPosition + temp;

                if (Raylib.IsMouseButtonReleased(Raylib.MOUSE_LEFT_BUTTON)) 
                    dragWindow = false;
            }
        }

        public virtual void Draw()
        {
            //Whenever or not the exit button per node is clicked.
            ExitWindow = RayGui.GuiWindowBox(new Rectangle(Position.X, Position.Y, Size.X, Size.Y), GateName);


            //Handle GUI Inputs clicking, yes it's the draw method, but this is where we do be doing it
            for (int i = 0; i < Inputs.Count; i++)
            {
                Rectangle r = new Rectangle(Position.X, Position.Y + 40 + (i * 25), 40, 25);
                Inputs[i].Clicked = RayGui.GuiButton(r, "Input" + i);
                Inputs[i].Rec = r;
                Inputs[i].RefNode = this;

                if (Inputs[i].Clicked)
                {
                    Linker.Input = Inputs[i];
                    Linker.InputNode = this;
                }
            }

            //Handle GUI Outputs clicking, yes it's the draw method, but this is where we do be doing it
            for (int i = 0; i < Outputs.Count; i++)
            {
                Rectangle r = new Rectangle(Position.X + Size.X - 40, Position.Y + 40 + (i * 25), 40, 25);
                Outputs[i].Clicked = RayGui.GuiButton(r, "Output" + i);
                Outputs[i].Rec = r;
                Outputs[i].RefNode = this;
                if (Outputs[i].Clicked)
                {
                    Linker.Output = Outputs[i];
                    Linker.OutputNode = this;
                }
            }

            //Gotta draw that wavy line for every output. no need to draw inputs, as they all correlate.
            foreach (var l in OutputLinks)
            {
                if (l.LinkOutput != null && l.LinkInput != null)
                    Raylib.DrawLineBezier(new Vector2((int)(l.LinkOutput.Rec.X + l.LinkOutput.Rec.width), (int)(l.LinkOutput.Rec.Y + (l.LinkOutput.Rec.height / 2))), new Vector2((int)(l.LinkInput.Rec.X + (l.LinkInput.Rec.width / 2)), (int)l.LinkInput.Rec.Y + (int)(l.LinkInput.Rec.height / 2)), 2, l.LinkInput.Value ? Raylib.RED : Raylib.BLACK);
            }

            //Draw the gate outputvalue to the center of the gate. Usefull for debugging, all gates do this by default.
            Raylib.DrawText($"{OutPutValue}", Position.X + (Size.X / 2), Position.Y + 30, 8, OutPutValue == 1 ? Raylib.GREEN : Raylib.RED);

            //Raylib.DrawRectangleLines((int)Position.X, (int)Position.Y, (int)Size.X - 25, 23, Raylib.RED);

        }

    }
}
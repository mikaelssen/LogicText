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

    [Serializable]
    public static class Linker
    {

        public static Input? Input = null;
        public static Node? InputNode = null;

        public static Output? Output = null;
        public static Node? OutputNode = null;


        public static void Deselect()
        {
            Input = null; 
            Output = null;
        }

        public static void Connect()
        {
            if (Input != null && Output != null && InputNode != null && OutputNode != null)
            {
                if (Input.RefNode != Output.RefNode)
                {
                    InputNode.InputLinks.Add(new LinkedLink() { LinkInput = Input, LinkOutput = Output });
                    OutputNode.OutputLinks.Add(new LinkedLink() { LinkInput = Input, LinkOutput = Output });
                    Console.WriteLine($"Linked {Input} and {Output}");
                }
                else
                    Console.WriteLine($"Possible self linking discovered, stopping.");
                
                Input = null;
                Output = null;
                InputNode = null;
                OutputNode = null;

            }
        }

        public static void DeleteNodeLinks(Node target)
        {
            foreach (var item in target.InputLinks)
            {
                item.LinkOutput.RefNode.OutputLinks.RemoveAll(l => l.LinkOutput.ID == item.LinkOutput.ID);
                item.LinkOutput.RefNode.InputLinks.RemoveAll(l => l.LinkOutput.ID == item.LinkOutput.ID);
            }
            foreach (var item in target.OutputLinks)
            {
                item.LinkInput.RefNode.OutputLinks.RemoveAll(l => l.LinkInput.ID == item.LinkInput.ID);
                item.LinkInput.RefNode.InputLinks.RemoveAll(l => l.LinkInput.ID == item.LinkInput.ID);
            }

            target.InputLinks.Clear();
            target.OutputLinks.Clear();
        }
    }

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
            ExitWindow = RayGui.GuiWindowBox(new Rectangle(Position.X, Position.Y, Size.X, Size.Y), GateName);
       

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

            foreach (var l in OutputLinks)
            {
                if (l.LinkOutput != null && l.LinkInput != null)
                    Raylib.DrawLineBezier(new Vector2((int)(l.LinkOutput.Rec.X + l.LinkOutput.Rec.width), (int)(l.LinkOutput.Rec.Y + (l.LinkOutput.Rec.height / 2))), new Vector2((int)(l.LinkInput.Rec.X + (l.LinkInput.Rec.width / 2)), (int)l.LinkInput.Rec.Y + (int)(l.LinkInput.Rec.height / 2)), 2, l.LinkInput.Value ? Raylib.RED : Raylib.BLACK);
            }

            Raylib.DrawText($"{OutPutValue}", Position.X + (Size.X / 2), Position.Y + 30, 8, OutPutValue == 1 ? Raylib.GREEN : Raylib.RED);

            //Raylib.DrawRectangleLines((int)Position.X, (int)Position.Y, (int)Size.X - 25, 23, Raylib.RED);

        }

    }

    [Serializable]
    [TypeConverter(typeof(Link))]
    public class Input : Link
    {
        public Input() { }
        public Input(int id)
        {
            ID = id;
        }
    }

    [Serializable]
    [TypeConverter(typeof(Link))]
    public class Output : Link
    {
        public Output() { }
        public Output(int id)
        {
            ID = id;
        }
    }

    [Serializable]
    public class LinkedLink
    {
        public Link? LinkInput { get; set; }
        public Link? LinkOutput { get; set; }
    }

    [Serializable]
    public class Link
    {
        public Link(){}
        public Link(int id)
        {
            ID = id;
        }

        public bool Value { get; set; }
        public bool Clicked { get; set; }
        public Rectangle Rec { get; set; }
        public Node? RefNode { get; set; }
        public int ID { get; set; }
    }
}
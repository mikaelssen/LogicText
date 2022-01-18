namespace LogicText.Linking
{

    using Raylib_CsLo;
    using System;
    using System.ComponentModel;

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
namespace LogicText.Linking
{
    using System;
    using LogicText.Linking;

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
            if (Input != null && Output != null && InputNode != null && OutputNode != null && Input.RefNode != null && Output.RefNode != null)
            {
                LinkedLink l = new LinkedLink() { LinkInput = Input, LinkOutput = Output };

                //Note this has a bug in it. if you want to remove the last link... it might remove the first link, cause this is a search. 
                if (Input.RefNode.InputLinks.Find(m => m.LinkInput == l.LinkInput) != null && Output.RefNode.OutputLinks.Find(m => m.LinkOutput == l.LinkOutput) != null)
                {
                    InputNode.InputLinks.Remove(Input.RefNode.InputLinks.Find(m => m.LinkInput.ID == l.LinkInput.ID));
                    OutputNode.OutputLinks.Remove(Output.RefNode.OutputLinks.Find(m => m.LinkOutput.ID == l.LinkOutput.ID));
                } else if (Input.RefNode != Output.RefNode)
                {
                    InputNode.InputLinks.Add(l);
                    OutputNode.OutputLinks.Add(l);
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
}
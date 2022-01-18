using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Raylib_CsLo;
using System.Numerics;
using Newtonsoft.Json;

namespace LogicText
{
    public class Sim
    {

        public List<Node> nodes = new List<Node>();

        public void Simulate()
        {
            Raylib.InitWindow(1280, 720, "Graps Are Logical Creatures");
            RayGui.GuiLoadStyleDefault();

            
            Raylib.SetTargetFPS(120);
            Console.Clear();

            nodes = new List<Node>();

            Vector2 cameravector = new Vector2();

            Camera2D camera = new Camera2D() { target = cameravector, zoom = 1, offset = new Vector2(0, 0), rotation = 0 };
            

            // Main game loop
            while (!Raylib.WindowShouldClose()) // Detect window close button or ESC key
            {


                if (Raylib.IsKeyDown(KeyboardKey.KEY_D))
                    cameravector.X += 2;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_A))
                    cameravector.X -= 2;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_W))
                    cameravector.Y -= 2;
                if (Raylib.IsKeyDown(KeyboardKey.KEY_S))
                    cameravector.Y += 2;


                camera.target = cameravector;


                var item = nodes.FirstOrDefault(x => x.ExitWindow == true);
                if (item != null)
                {
                    item.DeleteNode();
                    nodes.Remove(item);
                }

                foreach (var n in nodes)
                {
                    n.Update(camera);

                    // Input is where we want to feed data into. it feels backwards

                    foreach (var link in n.OutputLinks)
                    {

                        if (link.LinkOutput != null && link.LinkOutput.RefNode != null && link.LinkInput != null && link.LinkInput.RefNode != null)
                        {
                            link.LinkOutput.RefNode.StepUpdate();

                            link.LinkInput.RefNode.StepUpdate();
                            link.LinkOutput.Value = link.LinkOutput.RefNode.OutPutValue > 0;
                            link.LinkInput.Value = link.LinkOutput.Value;
                            if (link.LinkInput.RefNode.Inputs.Count > 0)
                                link.LinkInput.RefNode.Inputs[link.LinkInput.ID].Value = link.LinkInput.Value;
                        }
                    }

                }

                Linker.Connect(); //Check if connections are ready. :)



                if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_BUTTON_RIGHT))
                    Linker.Deselect();

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ONE))
                {
                    nodes.Add(new AndGate(2,1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_TWO))
                {
                    nodes.Add(new ButtonGate(0,1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_THREE))
                {
                    nodes.Add(new OrGate(2,1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_FOUR))
                {
                    nodes.Add(new XorGate(2,1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_FIVE))
                {
                    nodes.Add(new NotGate(1,1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SIX))
                {
                    nodes.Add(new ClockGate(0,1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_SEVEN))
                {
                    nodes.Add(new BitGate(2,1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_EIGHT))
                {
                    nodes.Add(new NAndGate(2, 1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }


                if (Raylib.IsKeyPressed(KeyboardKey.KEY_NINE))
                {
                    nodes.Add(new NOrGate(2, 1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_ZERO))
                {
                    nodes.Add(new XNorGate(2, 1)
                    {
                        Position = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera),
                        Size = new Vector2(120, 100)
                    });
                }


                if (Raylib.IsKeyPressed(KeyboardKey.KEY_F1))
                    Save();
                if (Raylib.IsKeyPressed(KeyboardKey.KEY_F2))
                    Load();

                if (Raylib.IsKeyPressed(KeyboardKey.KEY_E))
                { 

                    var Mpos = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
                    foreach (var n in nodes)
                    {
                        if (Raylib.CheckCollisionPointRec(Mpos, new Rectangle(n.Position.X, n.Position.Y, n.Size.X, n.Size.Y)))
                        {

                            Console.WriteLine(Environment.NewLine);
                            Console.WriteLine($"mya clicked {n}");
                            Console.WriteLine($"OutPutValue {n.OutPutValue}");
                            Console.WriteLine($"Position {n.Position}");

                            Console.WriteLine($"Outputs: {n.Outputs.Count}");
                            foreach (var outp in n.Outputs)
                                Console.WriteLine($"{outp.ID}-{outp.Value}");

                            Console.WriteLine($"Inputs: {n.Inputs.Count}");
                            foreach (var inp in n.Inputs)
                      
                            Console.WriteLine(Environment.NewLine);
                        }
                    }
                }


                Raylib.BeginDrawing();
                {
                    
                    Raylib.ClearBackground(Raylib.SKYBLUE);

                    RayGui.GuiGrid(new Rectangle( 0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()),50.0f, 2); // Draw a fancy grid

                    Raylib.DrawFPS(10, 10);
                    Raylib.DrawText(Raylib.TextFormat("Mouse Position: [ %.0f, %.0f ]", Raylib.GetMouseX(), Raylib.GetMouseY()), 10, 40, 10, Raylib.DARKGRAY);
                    Raylib.DrawText($"GateCount:{nodes.Count}", 10, 48, 10, Raylib.DARKGRAY);
                    Raylib.DrawText($"CameraPos:{cameravector}", 10, 54, 10, Raylib.DARKGRAY);


                    Raylib.DrawText(
                        $"Controls:" + Environment.NewLine +
                        $"F1 - Save" + Environment.NewLine +
                        $"F2 - Load" + Environment.NewLine +
                        $"1 - And Gate" + Environment.NewLine +
                        $"2 - Button" + Environment.NewLine +
                        $"3 - Or Gate" + Environment.NewLine +
                        $"4 - Xor Gate" + Environment.NewLine +
                        $"5 - Not Gate" + Environment.NewLine +
                        $"6 - Clock" + Environment.NewLine +
                        $"7 - 1 Bit Memory, Use top to toggle setting, bottom to set value" + Environment.NewLine +
                        $"8 - NAnd Gate" + Environment.NewLine +
                        $"9 - NOr Gate" + Environment.NewLine +
                        $"0 - XNOr Gate" + Environment.NewLine +
                        $"Right Click - Deselect selection" + Environment.NewLine +
                        $"W A S D - Camera movement" + Environment.NewLine,
                        10, 64, 10, Raylib.DARKGRAY);


                    Raylib.BeginMode2D(camera);
                    {
                        Raylib.SetMouseOffset((int)camera.target.X, (int)camera.target.Y);
                        Raylib.SetMouseScale(camera.zoom,camera.zoom);

                        foreach (var n in nodes)
                            n.Draw();

                        if (Linker.Input != null && Linker.Input.RefNode != null)
                        {
                            Raylib.DrawRectangleLinesEx(Linker.Input.Rec,1, Raylib.GREEN);
                            Raylib.DrawRectangleLinesEx(new Rectangle(Linker.Input.RefNode.Position.X, Linker.Input.RefNode.Position.Y, Linker.Input.RefNode.Size.X, Linker.Input.RefNode.Size.Y), 1, Raylib.GREEN);

                        }
                        if (Linker.Output != null && Linker.Output.RefNode != null)
                        {
                            Raylib.DrawRectangleLinesEx(Linker.Output.Rec, 1, Raylib.DARKGREEN);
                            Raylib.DrawRectangleLinesEx(new Rectangle(Linker.Output.RefNode.Position.X,Linker.Output.RefNode.Position.Y,Linker.Output.RefNode.Size.X,Linker.Output.RefNode.Size.Y), 1, Raylib.DARKGREEN);
                        }
                    }
                    Raylib.EndMode2D();
                    Raylib.SetMouseOffset(0,0);
                }
                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
        }

        public void Save()
        {  
            try
            {
                File.WriteAllText("NodeData.json", JsonConvert.SerializeObject(nodes, Formatting.Indented, new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All,
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    TypeNameHandling = TypeNameHandling.Auto
                }));

                Console.WriteLine($"Saved");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Nico broke saving, here's why {e.Message} stack: {e.StackTrace}");
            } // end try-catch

        }

        public void Load()
        {
            // Check if we had previously Save information of our friends
            // previously
            if (File.Exists("NodeData.json"))
            {
                try
                {
                    nodes = JsonConvert.DeserializeObject<List<Node>>(File.ReadAllText("NodeData.json"),new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        TypeNameHandling = TypeNameHandling.All
                    });

                    Console.WriteLine($"Loaded");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"File potentially corrupted by Nico. So we ruined a pizza. also: {e.Message} stack: {e.StackTrace}");
                }
            }
        }
    }
}

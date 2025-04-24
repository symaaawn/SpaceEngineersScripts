using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Policy;
using System.Text;
using VRage;
using VRage.Collections;
using VRage.Game;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ObjectBuilders.Definitions;
using VRageMath;
using VRageRender;

namespace IngameScript
{
    /**
     * <summary>
     * Text Panel Display
     * </summary>
     * <remarks>
     * This script is responsible for displaying information on a text panel screen.
     * The first line will always be 'Symeon Mining Corporation', the fictional space mining company.
     * The second line is the name of the program.
     * The following lines are the information passed to the display via the <c>RenderDisplay</c> method.
     * </remarks>
     */
    partial class Program
    {
        public class TextPanelDisplayTwoColumns : TextPanelDisplay
        {
            #region private fields

            #endregion

            #region construction

            /**
             * <summary>
             * Initializes the display.
             * </summary>
             * <param name="drawingSurface">The text panel surface to draw on.</param>
             */
            public TextPanelDisplayTwoColumns(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
            }

            #endregion

            /**
             * <summary>
             * Renders the display new.
             * </summary>
             * <param name="infos">The information to display. Each Element will be rendered in a new line.</param>
             */
            internal void RenderDisplay(string[][] infos)
            {
                var frame = DrawingSurface.DrawFrame();

                // Symeon Mining Corporation
                var position = new Vector2(5, 20) + Viewport.Position;
                var sprite = new MySprite
                {
                    Type = SpriteType.TEXT,
                    Data = "Symeon Mining Corporation",
                    Position = position,
                    RotationOrScale = 0.8f,
                    Color = Color.Gold,
                    Alignment = TextAlignment.LEFT,
                    FontId = "Monospace"
                };
                frame.Add(sprite);

                // Program name
                position += new Vector2(0, 20);
                sprite = new MySprite
                {
                    Type = SpriteType.TEXT,
                    Data = ProgramName,
                    Position = position,
                    RotationOrScale = 0.8f,
                    Color = Color.Gold,
                    Alignment = TextAlignment.LEFT,
                    FontId = "Monospace"
                };
                frame.Add(sprite);

                position += new Vector2(0, 20);

                // Additional information
                //foreach (var info in infos)
                //{
                //    position += new Vector2(0, 20);
                //    sprite = new MySprite
                //    {
                //        Type = SpriteType.TEXT,
                //        Data = info,
                //        Position = position,
                //        RotationOrScale = 0.8f,
                //        Color = Color.Gold,
                //        Alignment = TextAlignment.LEFT,
                //        FontId = "White"
                //    };
                //    frame.Add(sprite);
                //}

                position += new Vector2(0, 20);
                sprite = new MySprite
                {
                    Type = SpriteType.TEXT,
                    Data = DrawingSurface.SurfaceSize.Y.ToString() + " - " + DrawingSurface.SurfaceSize.X.ToString(),
                    Position = position,
                    RotationOrScale = 0.8f,
                    Color = Color.Gold,
                    Alignment = TextAlignment.LEFT,
                    FontId = "Monospace"
                };
                frame.Add(sprite);

                position += new Vector2((DrawingSurface.SurfaceSize.X / 2) + 5, 20);
                var spriteSize = sprite.Size;
                sprite = new MySprite
                {
                    Type = SpriteType.TEXT,
                    Data = DrawingSurface.SurfaceSize.Y.ToString() + " - " + DrawingSurface.SurfaceSize.X.ToString(),
                    Position = position,
                    RotationOrScale = 0.8f,
                    Color = Color.Gold,
                    Alignment = TextAlignment.LEFT,
                    FontId = "Monospace"
                };
                frame.Add(sprite);


                var fonts = new List<string>();
                DrawingSurface.GetFonts(fonts);
                foreach(var font in fonts)
                {
                    position += new Vector2(0, 20);
                    sprite = new MySprite
                    {
                        Type = SpriteType.TEXT,
                        Data = font,
                        Position = position,
                        RotationOrScale = 0.8f,
                        Color = Color.Gold,
                        Alignment = TextAlignment.LEFT,
                        FontId = font
                    };
                    frame.Add(sprite);
                }

                frame.Dispose();
            }
        }
    }
}

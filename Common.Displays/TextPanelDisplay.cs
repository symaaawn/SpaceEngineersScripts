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
        public class TextPanelDisplay
        {
            #region private fields

            #endregion

            #region properties

            public IMyTextSurface DrawingSurface { get; private set; }
            public RectangleF Viewport { get; private set; }

            #endregion

            #region construction

            /**
             * <summary>
             * Initializes the display.
             * </summary>
             * <param name="drawingSurface">The text panel surface to draw on.</param>
             */
            public TextPanelDisplay(IMyTextSurface drawingSurface)
            {
                DrawingSurface = drawingSurface;

                Viewport = new RectangleF(
                    (DrawingSurface.TextureSize - DrawingSurface.SurfaceSize) / 2f,
                    DrawingSurface.SurfaceSize
                );

                DrawingSurface.ContentType = ContentType.SCRIPT;
                DrawingSurface.Script = "";
                DrawingSurface.ScriptBackgroundColor = Color.Black;

                RenderDisplay(new string[0]);
            }

            #endregion

            /**
             * <summary>
             * Renders the display new.
             * </summary>
             * <param name="infos">The information to display. Each Element will be rendered in a new line.</param>
             */
            internal virtual void RenderDisplay(string[] infos)
            {
                var frameAndPosition = DrawTitle();
                var frame = frameAndPosition.Item1;
                var position = frameAndPosition.Item2;

                position += new Vector2(0, 20);

                // Additional information
                foreach (var info in infos)
                {
                    position += new Vector2(0, 20);
                    var sprite = new MySprite
                    {
                        Type = SpriteType.TEXT,
                        Data = info,
                        Position = position,
                        RotationOrScale = 0.8f,
                        Color = Color.Gold,
                        Alignment = TextAlignment.LEFT,
                        FontId = "Monospace"
                    };
                    frame.Add(sprite);
                }

                frame.Dispose();
            }


            /**
             * <summary>
             * Draws the program title rows on the display.
             * </summary>
             * <returns>The sprite frame and last drawn position.</returns>
             */
            protected MyTuple<MySpriteDrawFrame, Vector2> DrawTitle()
            {
                var frame = DrawingSurface.DrawFrame();

                // Symeon Mining Corporation
                var position = new Vector2(5, 20) + Viewport.Position;
                var sprite = new MySprite
                {
                    Type = SpriteType.TEXT,
                    Data = CompanyName,
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

                return new MyTuple<MySpriteDrawFrame, Vector2>(frame, position);
            }

            protected MySprite DrawBar(Vector2 position, int value, int maxValue)
            {
                var barWidth = 502;
                var barHeight = 20;
                var barColor = Color.Green;
                var barValue = (float)value / (float)maxValue;
                return new MySprite
                {
                    Type = SpriteType.TEXTURE,
                    Data = "SquareSimple",
                    Position = position,
                    Size = new Vector2(barWidth * barValue, barHeight),
                    Color = barColor,
                    Alignment = TextAlignment.LEFT
                };
            }
        }
    }
}

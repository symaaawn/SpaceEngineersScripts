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

            private IMyTextSurface _drawingSurface;
            private RectangleF _viewport;
            private string _programName;

            #endregion

            #region construction

            /**
             * <summary>
             * Initializes the display.
             * </summary>
             * <param name="drawingSurface">The text panel surface to draw on.</param>
             * <param name="programName">The name of the program.</param>
             */
            public TextPanelDisplay(IMyTextSurface drawingSurface, string programName)
            {
                _drawingSurface = drawingSurface;

                _viewport = new RectangleF(
                    (_drawingSurface.TextureSize - _drawingSurface.SurfaceSize) / 2f,
                    _drawingSurface.SurfaceSize
                );

                _drawingSurface.ContentType = ContentType.SCRIPT;
                _drawingSurface.Script = "";
                _drawingSurface.ScriptBackgroundColor = Color.Black;

                _programName = programName;

                RenderDisplay(new string[0]);
            }

            #endregion

            /**
             * <summary>
             * Renders the display new.
             * </summary>
             * <param name="infos">The information to display. Each Element will be rendered in a new line.</param>
             */
            internal void RenderDisplay(string[] infos)
            {
                var frame = _drawingSurface.DrawFrame();

                // Symeon Mining Corporation
                var position = new Vector2(5, 20) + _viewport.Position;
                var sprite = new MySprite
                {
                    Type = SpriteType.TEXT,
                    Data = "Symeon Mining Corporation",
                    Position = position,
                    RotationOrScale = 0.8f,
                    Color = Color.Gold,
                    Alignment = TextAlignment.LEFT,
                    FontId = "White"
                };
                frame.Add(sprite);

                // Program name
                position += new Vector2(0, 20);
                sprite = new MySprite
                {
                    Type = SpriteType.TEXT,
                    Data = _programName,
                    Position = position,
                    RotationOrScale = 0.8f,
                    Color = Color.Gold,
                    Alignment = TextAlignment.LEFT,
                    FontId = "White"
                };
                frame.Add(sprite);

                position += new Vector2(0, 20);

                // Additional information
                foreach (var info in infos)
                {
                    position += new Vector2(0, 20);
                    sprite = new MySprite
                    {
                        Type = SpriteType.TEXT,
                        Data = info,
                        Position = position,
                        RotationOrScale = 0.8f,
                        Color = Color.Gold,
                        Alignment = TextAlignment.LEFT,
                        FontId = "White"
                    };
                    frame.Add(sprite);
                }

                frame.Dispose();
            }
        }
    }
}

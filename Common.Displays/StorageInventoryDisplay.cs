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
        public class StorageInventoryDisplay : TextPanelDisplay
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
            public StorageInventoryDisplay(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
            }

            #endregion

            /**
             * <summary>
             * Renders the display new.
             * </summary>
             * <param name="infos">The information to display. Each Element will be rendered in a new line.</param>
             */
            internal void RenderDisplay(int value, int maxvalue, string[] infos)
            {
                var frameAndPosition = DrawTitle();
                var frame = frameAndPosition.Item1;
                var position = frameAndPosition.Item2;

                position += new Vector2(0, 40);
                frame.Add(DrawBar(position, value, maxvalue));

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
        }
    }
}

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
     * This class is responsible for displaying information about the storage inventory on a text panel screen.
     * </remarks>
     */
    partial class Program
    {
        public class StorageInventoryDisplay : TextPanelDisplay
        {
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
             * <param name="value">The current value to be displayed</param>
             * <param name="maxvalue">The maximum value to be displayed</param>
             * <param name="infos">The information to display. Each Element will be rendered in a new line.</param>
             */
            internal void RenderDisplay(float value, float maxvalue, string[] infos)
            {
                RenderDisplay();

                Position += new Vector2(5, 0);
                Frame.Add(SpriteHelper.DrawBar(Position, Viewport.Width - 10, value, maxvalue));
                Position += new Vector2(-5, 0);

                // Additional information
                foreach (var info in infos)
                {
                    Position += new Vector2(0, 20);
                    var sprite = new MySprite
                    {
                        Type = SpriteType.TEXT,
                        Data = info,
                        Position = Position,
                        RotationOrScale = 0.8f,
                        Color = Color.Gold,
                        Alignment = TextAlignment.LEFT,
                        FontId = "Monospace"
                    };
                    Frame.Add(sprite);
                }

                Frame.Dispose();
            }
        }
    }
}

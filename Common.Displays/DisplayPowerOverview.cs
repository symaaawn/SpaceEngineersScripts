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
        public class DisplayPowerOverview : TextPanelDisplay
        {
            #region construction

            /**
             * <summary>
             * Initializes the display.
             * </summary>
             * <param name="drawingSurface">The text panel surface to draw on.</param>
             */
            public DisplayPowerOverview(IMyTextSurface drawingSurface) : base(drawingSurface)
            {
            }

            #endregion

            /**
             * <summary>
             * Renders the display new.
             * </summary>
             * <param name="infos">The information to display. Each Element will be rendered in a new line.</param>
             */
            internal void RenderDisplay(List<PowerOverviewDc> infos)
            {
                RenderDisplay();

                // Additional information
                foreach (var info in infos)
                {
                    Position += new Vector2(0, 20);
                    var infoString = $"{info.Name.Truncate(10):-10}: {info.CurrentPowerOutput:N3}/{info.MaxPowerOutput:N3}";
                    var sprite = new MySprite
                    {
                        Type = SpriteType.TEXT,
                        Data = infoString,
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

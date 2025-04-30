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
        public abstract class Display
        {
            #region properties

            protected IMyTextSurface DrawingSurface { get; private set; }
            public RectangleF Viewport { get; private set; }
            internal MySpriteDrawFrame Frame { get; private set; }
            protected Vector2 Position { get; set; }

            #endregion

            #region construction

            /**
             * <summary>
             * Initializes the display.
             * </summary>
             * <param name="drawingSurface">The text panel surface to draw on.</param>
             */
            protected Display(IMyTextSurface drawingSurface)
            {
                DrawingSurface = drawingSurface;

                Viewport = new RectangleF(
                    (DrawingSurface.TextureSize - DrawingSurface.SurfaceSize) / 2f,
                    DrawingSurface.SurfaceSize
                );

                DrawingSurface.ContentType = ContentType.SCRIPT;
                DrawingSurface.Script = "";
                DrawingSurface.ScriptBackgroundColor = Color.Black;

                var frame = DrawingSurface.DrawFrame();
                RenderDisplay();
            }

            #endregion

            /**
             * <summary>
             * Renders the display new and adds the title rows.
             * </summary>
             */
            internal void RenderDisplay()
            {
                Frame = DrawingSurface.DrawFrame();
                Position = new Vector2(5, 20) + Viewport.Position;
                var titleSprite = SpriteHelper.DrawTitle(Position);
                
                Frame.AddRange(titleSprite);

                Position += new Vector2(0, 30);
            }
        }
    }
}

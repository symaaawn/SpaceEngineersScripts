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
    partial class Program
    {
        #region private fields

        private IMyTextSurface _drawingSurface;
        private RectangleF _viewport;
        private string _programName;

        #endregion


        internal void InitializeDisplay(string programName)
        {
            _drawingSurface = Me.GetSurface(0);

            _viewport = new RectangleF(
                (_drawingSurface.TextureSize - _drawingSurface.SurfaceSize) / 2f,
                _drawingSurface.SurfaceSize
            );

            _drawingSurface.ContentType = ContentType.SCRIPT;
            _drawingSurface.Script = "";
            _drawingSurface.ScriptBackgroundColor = Color.Black;

            _programName = programName;

            RenderDisplay(new string [0]);
        }

        internal void RenderDisplay(string[] infos)
        {
            var frame = _drawingSurface.DrawFrame();

            // line 1
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

            // line 2
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

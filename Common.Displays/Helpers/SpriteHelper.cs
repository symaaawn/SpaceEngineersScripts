using System;
using System.Collections.Generic;
using VRage;
using VRage.Game.GUI.TextPanel;
using VRageMath;

namespace IngameScript
{
    partial class Program
    {
        public static class SpriteHelper
        {
            /**
             * <summary>
             * Draws the company name and the program title on the display.
             * </summary>
             * <param name="position">The position of the title</param>
             * <returns>The sprite containing the company and the program name</returns>
             */
            public static IEnumerable<MySprite> DrawTitle(Vector2 position)
            {
                var sprites = new List<MySprite>();
                // Symeon Mining Cooperation
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
                sprites.Add(sprite);

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
                sprites.Add(sprite);

                return sprites;
            }


            /**
             * <summary>
             * Draws a bar on the display.
             * </summary>
             * <param name="position">The position of the bar.</param>
             * <param name="barWidth">The width of the bar.</param>
             * <param name="value">The current value of the bar.</param>
             * <param name="maxValue">The maximum value of the bar.</param>
             * <returns>The sprite for the bar.</returns>
             */
            public static MySprite DrawBar(Vector2 position, float barWidth, float value, float maxValue)
            {
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
namespace IngameScript
{
    partial class Program
    {
        public static class ItemType
        {
            #region constants

            public const string Ore = "MyObjectBuilder_Ore";
            public const string Ingot = "MyObjectBuilder_Ingot";
            public const string Component = "MyObjectBuilder_Component";
            public const string Gas = "MyObjectBuilder_GasProperties";
            public const string Tool = "MyObjectBuilder_PhysicalGunObject";

            #endregion

            #region methods

            public static bool IsOre(string itemType)
            {
                return itemType.Contains(Ore);
            }

            public static bool IsIngot(string itemType)
            {
                return itemType.Contains(Ingot);
            }

            public static bool IsComponent(string itemType)
            {
                return itemType.Contains(Component);
            }

            public static bool IsGas(string itemType)
            {
                return itemType.Contains(Gas);
            }

            public static bool IsTool(string itemType)
            {
                return itemType.Contains(Tool);
            }

            #endregion
        }
    }
}
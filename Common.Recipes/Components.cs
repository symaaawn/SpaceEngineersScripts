using System.Collections.Generic;

namespace IngameScript
{
    partial class Program
    {
        public static IComponentRecipe GetRecipe(string componentName)
        {
            switch (componentName) { 
            case "BulletproofGlass":
                    return new BulletproofGlassRecipe();
                case "ComputerComponent":
                    return new ComputerRecipe();
                case "ConstructionComponent":
                    return new ConstructionComponentRecipe();
                case "DetectorComponent":
                    return new DetectorComponentRecipe();
                case "Display":
                    return new DisplayRecipe();
                case "ExplosivesComponent":
                    return new ExplosivesRecipe();
                case "GirderComponent":
                    return new GirderRecipe();
                case "GravityGeneratorComponent":
                    return new GravityComponentRecipe();
                case "InteriorPlate":
                    return new InteriorPlateRecipe();
                case "LargeTube":
                    return new LargeSteelTubeRecipe();
                case "MedicalComponent":
                    return new MedicalComponentRecipe();
                case "MetalGrid":
                    return new MetalGridRecipe();
                case "MotorComponent":
                    return new MotorRecipe();
                case "PowerCell":
                    return new PowerCellRecipe();
                case "RadioCommunicationComponent":
                    return new RadioCommunicationComponentRecipe();
                case "ReactorComponent":
                    return new ReactorComponentRecipe();
                case "SmallTube":
                    return new SmallSteelTubeRecipe();
                case "SolarCell":
                    return new SolarCellRecipe();
                case "SteelPlate":
                    return new SteelPlateRecipe();
                case "Superconductor":
                    return new SuperconductorRecipe();
                case "ThrustComponent":
                    return new ThrusterComponentRecipe();
                default:
                    return null;
            }
        }

        public interface IComponentRecipe
        {
            Dictionary<string, double> GetMaterials();
        }

        public class BulletproofGlassRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Silicon", 15.0 }
                };
            }
        }

        public class ComputerRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 0.5 },
                    { "Silicon", 0.2 }
                };
            }
        }

        public class ConstructionComponentRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 10 }
                };
            }
        }

        public class DetectorComponentRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 5.0 },
                    { "Nickel", 15.0 }
                };
            }
        }

        public class DisplayRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 1.0 },
                    { "Silicon", 5.0 }
                };
            }
        }

        public class ExplosivesRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Magnesium", 2.0 },
                    { "Silicon", 0.5 }
                };
            }
        }

        public class GirderRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 7.0 }
                };
            }
        }

        public class GravityComponentRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Cobalt", 220.0 },
                    { "Gold", 10.0 },
                    { "Iron", 600.0 },
                    { "Silver", 5.0 }
                };
            }
        }

        public class InteriorPlateRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 3.5 }
                };
            }
        }

        public class LargeSteelTubeRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 30.0 }
                };
            }
        }

        public class MedicalComponentRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 60.0 },
                    { "Nickel", 70.0 },
                    { "Silver", 20.0 }
                };
            }
        }

        public class MetalGridRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Cobalt", 3.0 },
                    { "Iron", 12.0 },
                    { "Nickel", 5.0 }
                };
            }
        }

        public class MotorRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 20.0 },
                    { "Nickel", 5.0 }
                };
            }
        }

        public class PowerCellRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 10.0 },
                    { "Nickel", 2.0 },
                    { "Silicon", 1.0 }
                };
            }
        }

        public class RadioCommunicationComponentRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 8.0 },
                    { "Silicon", 1.0 }
                };
            }
        }

        public class ReactorComponentRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 15.0 },
                    { "Silver", 5.0 },
                    { "Stone", 20.0 }
                };
            }
        }

        public class SmallSteelTubeRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 5.0 }
                };
            }
        }

        public class SolarCellRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Nickel", 10.0 },
                    { "Silicon", 8.0 }
                };
            }
        }

        public class SteelPlateRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Iron", 21.0 }
                };
            }
        }

        public class SuperconductorRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Gold", 2.0 },
                    { "Iron", 10.0 }
                };
            }
        }

        public class ThrusterComponentRecipe : IComponentRecipe
        {
            public Dictionary<string, double> GetMaterials()
            {
                return new Dictionary<string, double> {
                    { "Cobalt", 10.0 },
                    { "Gold", 1.0 },
                    { "Iron", 30.0 },
                    { "Platinum", 0.4 }
                };
            }
        }
    }
}

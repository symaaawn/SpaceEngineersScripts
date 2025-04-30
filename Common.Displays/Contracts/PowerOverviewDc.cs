namespace IngameScript
{
    partial class Program
    {
        public class PowerOverviewDc
        {
            public string Name { get; set; }
            public string Type { get; set; }
            public float MaxPowerOutput { get; set; }
            public float CurrentPowerOutput { get; set; }
            public float MaxCapacity { get; set; }
            public float StorageCapacity { get; set; }
            

            public PowerOverviewDc(string name, string type, float maxPowerOutput, float currentPowerOutput, float maxCapacity = 0, float storageCapacity = 0)
            {
                Name = name;
                Type = type;
                MaxPowerOutput = maxPowerOutput;
                CurrentPowerOutput = currentPowerOutput;
                MaxCapacity = maxCapacity;
                StorageCapacity = storageCapacity;

            }
        }
    }
}
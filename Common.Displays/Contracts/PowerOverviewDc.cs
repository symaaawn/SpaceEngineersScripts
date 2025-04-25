namespace IngameScript
{
    partial class Program
    {
        public class PowerOverviewDc
        {
            public string Name { get; set; }
            public float MaxPowerOutput { get; set; }
            public float CurrentPowerOutput { get; set; }
            public float MaxCapacity { get; set; }
            public float StorageCapacity { get; set; }
            public bool IsBattery => MaxCapacity > 0;

            public PowerOverviewDc(string name, float maxPowerOutput, float currentPowerOutput,
                float maxCapacity = 0, float storageCapacity = 0)
            {
                Name = name;
                MaxPowerOutput = maxPowerOutput;
                CurrentPowerOutput = currentPowerOutput;
                MaxCapacity = maxCapacity;
                StorageCapacity = storageCapacity;
            }
        }
    }
}
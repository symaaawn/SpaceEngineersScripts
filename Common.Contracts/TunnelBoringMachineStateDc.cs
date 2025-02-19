namespace IngameScript
{
    partial class Program
    {
        /**
         * <summary>
         * Enum representing the state of the tunnel boring machine.
         * </summary>
         * 
         * <remarks>
         * 
         * Forward:
         *  FrontExtending,
         *  FrontMerging,
         *  FrontRetracting,
         *  
         * Backward:
         * 
         * </remarks>
         */
        public enum TunnelBoringMachineStateDc
        {
            Idle,
            ForwardExtending,
            ForwardFrontMerging,
            ForwardFrontSeparating,
            ForwardBackMerging,
            ForwardBackSeparating,
            ForwardRetracting,
            BackwardExtending,
            BackwardFrontMerging,
            BackwardRetracting,
            BackwardBackMerging,
            MiningExtending,
            MiningRetracting,
            Error = -1
        }
    }
}
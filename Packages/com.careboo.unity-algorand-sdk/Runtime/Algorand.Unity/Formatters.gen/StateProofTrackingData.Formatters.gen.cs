//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using UnityCollections = Unity.Collections;


namespace Algorand.Unity
{
    
    
    public partial struct StateProofTrackingData
    {
        
        private static bool @__generated__IsValid = StateProofTrackingData.@__generated__InitializeAlgoApiFormatters();
        
        private static bool @__generated__InitializeAlgoApiFormatters()
        {
            Algorand.Unity.AlgoApiFormatterLookup.Add<Algorand.Unity.StateProofTrackingData>(new Algorand.Unity.AlgoApiObjectFormatter<Algorand.Unity.StateProofTrackingData>(false).Assign("v", (Algorand.Unity.StateProofTrackingData x) => x.StateProofVotersCommitment, (ref Algorand.Unity.StateProofTrackingData x, System.Byte[] value) => x.StateProofVotersCommitment = value, Algorand.Unity.ArrayComparer<System.Byte>.Instance).Assign("t", (Algorand.Unity.StateProofTrackingData x) => x.StateProofOnlineTotalWeight, (ref Algorand.Unity.StateProofTrackingData x, Algorand.Unity.MicroAlgos value) => x.StateProofOnlineTotalWeight = value).Assign("n", (Algorand.Unity.StateProofTrackingData x) => x.StateProofNextRound, (ref Algorand.Unity.StateProofTrackingData x, System.UInt64 value) => x.StateProofNextRound = value));
            return true;
        }
    }
}

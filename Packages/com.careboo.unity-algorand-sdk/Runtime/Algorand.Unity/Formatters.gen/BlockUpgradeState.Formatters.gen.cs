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
    
    
    public partial struct BlockUpgradeState
    {
        
        private static bool @__generated__IsValid = BlockUpgradeState.@__generated__InitializeAlgoApiFormatters();
        
        private static bool @__generated__InitializeAlgoApiFormatters()
        {
            Algorand.Unity.AlgoApiFormatterLookup.Add<Algorand.Unity.BlockUpgradeState>(new Algorand.Unity.AlgoApiObjectFormatter<Algorand.Unity.BlockUpgradeState>(false).Assign("current-protocol", (Algorand.Unity.BlockUpgradeState x) => x.CurrentProtocol, (ref Algorand.Unity.BlockUpgradeState x, UnityCollections.FixedString128Bytes value) => x.CurrentProtocol = value).Assign("next-protocol", (Algorand.Unity.BlockUpgradeState x) => x.NextProtocol, (ref Algorand.Unity.BlockUpgradeState x, UnityCollections.FixedString128Bytes value) => x.NextProtocol = value).Assign("next-protocol-approvals", (Algorand.Unity.BlockUpgradeState x) => x.NextProtocolApprovals, (ref Algorand.Unity.BlockUpgradeState x, System.UInt64 value) => x.NextProtocolApprovals = value).Assign("next-protocol-switch-on", (Algorand.Unity.BlockUpgradeState x) => x.NextProtocolSwitchOn, (ref Algorand.Unity.BlockUpgradeState x, System.UInt64 value) => x.NextProtocolSwitchOn = value).Assign("next-protocol-vote-before", (Algorand.Unity.BlockUpgradeState x) => x.NextProtocolVoteBefore, (ref Algorand.Unity.BlockUpgradeState x, System.UInt64 value) => x.NextProtocolVoteBefore = value));
            return true;
        }
    }
}
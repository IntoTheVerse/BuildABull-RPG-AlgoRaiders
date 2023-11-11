import FungibleToken from 0x9a0766d93b6608b7;
import DungeonToken from 0xbf86544799a07261;

transaction()
{
  prepare(accAddress: AuthAccount)
  {
    let vault <- DungeonToken.createEmptyVault();
    accAddress.save(<-vault, to: DungeonToken.VaultStoragePath)
    accAddress.link<&DungeonToken.Vault{FungibleToken.Receiver, FungibleToken.Balance}>(DungeonToken.ReceiverPublicPath, target: DungeonToken.VaultStoragePath)
    accAddress.link<&DungeonToken.Vault{FungibleToken.Provider}>(DungeonToken.VaultPublicPath, target: DungeonToken.VaultStoragePath)
  }
}
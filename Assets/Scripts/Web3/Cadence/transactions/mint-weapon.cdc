import WeaponsOfDungeon from 0x130c419318ed0231
import FungibleToken from 0x9a0766d93b6608b7;
import DungeonToken from 0xbf86544799a07261;
import NonFungibleToken from 0x631e88ae7f1d7c20

transaction(acc: Address, id: UInt64, amount: UFix64) {
  prepare(acct: AuthAccount) 
  {
    let accRef = getAccount(acc).getCapability<&DungeonToken.Vault{FungibleToken.Provider}>(DungeonToken.VaultPublicPath).borrow() ?? panic("Can't Borrow!")
    let vault <- accRef.withdraw(amount: amount)
    destroy(vault)
    let weaponMinter <- WeaponsOfDungeon.createMinter()
    let weaponPubCollection = acct.borrow<&WeaponsOfDungeon.Collection{NonFungibleToken.CollectionPublic}>(from: WeaponsOfDungeon.CollectionStoragePath) ?? panic("Cant find collection")
    weaponMinter.mintNFT(metadataID: id, recipient: weaponPubCollection, royalties: [])
    destroy weaponMinter
  }
}
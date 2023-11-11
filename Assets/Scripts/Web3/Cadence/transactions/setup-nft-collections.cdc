import PlayersOfDungeon from 0x130c419318ed0231
import WeaponsOfDungeon from 0x130c419318ed0231
import NonFungibleToken from 0x631e88ae7f1d7c20

transaction(id: UInt64) {
  prepare(acct: AuthAccount) 
  {
    acct.save<@NonFungibleToken.Collection>(<- PlayersOfDungeon.createEmptyCollection(), to: PlayersOfDungeon.CollectionStoragePath)
    acct.save<@NonFungibleToken.Collection>(<- WeaponsOfDungeon.createEmptyCollection(), to: WeaponsOfDungeon.CollectionStoragePath)
    acct.link<&PlayersOfDungeon.Collection>(PlayersOfDungeon.CollectionPublicPath, target: PlayersOfDungeon.CollectionStoragePath)
    acct.link<&WeaponsOfDungeon.Collection>(WeaponsOfDungeon.CollectionPublicPath, target: WeaponsOfDungeon.CollectionStoragePath)

    let playerMinter <- PlayersOfDungeon.createMinter()
    let playerPubCollection = acct.borrow<&PlayersOfDungeon.Collection{NonFungibleToken.CollectionPublic}>(from: PlayersOfDungeon.CollectionStoragePath) ?? panic("Cant find collection")
    playerMinter.mintNFT(metadataID: id, recipient: playerPubCollection, royalties: [])
    destroy playerMinter

    let weaponMinter <- WeaponsOfDungeon.createMinter()
    let weaponPubCollection = acct.borrow<&WeaponsOfDungeon.Collection{NonFungibleToken.CollectionPublic}>(from: WeaponsOfDungeon.CollectionStoragePath) ?? panic("Cant find collection")
    weaponMinter.mintNFT(metadataID: id, recipient: weaponPubCollection, royalties: [])
    destroy weaponMinter
  }
}
import PlayersOfDungeon from 0x130c419318ed0231
import WeaponsOfDungeon from 0x130c419318ed0231

pub fun main(address: Address): Result
{
  let characterCollection = getAccount(address).getCapability<&PlayersOfDungeon.Collection>(PlayersOfDungeon.CollectionPublicPath).borrow() ?? panic("Can't borrow NFT Collection!");
  let ownedCharacters = characterCollection.getIDs()
  let weaponCollection = getAccount(address).getCapability<&WeaponsOfDungeon.Collection>(WeaponsOfDungeon.CollectionPublicPath).borrow() ?? panic("Can't borrow NFT Collection!");
  let ownedWeapons = weaponCollection.getIDs()

  return Result(characterIDs: ownedCharacters, weaponsIDs: ownedWeapons)
}

pub struct Result
{
  pub let ownedCharactersId: [UInt64]
  pub let ownedWeaponsId: [UInt64]

  init(characterIDs: [UInt64], weaponsIDs: [UInt64])
  {
    self.ownedCharactersId = characterIDs;
    self.ownedWeaponsId = weaponsIDs;
  }
}
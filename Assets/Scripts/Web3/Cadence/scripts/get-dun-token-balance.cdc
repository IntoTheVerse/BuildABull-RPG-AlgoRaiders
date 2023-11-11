import FungibleToken from 0x9a0766d93b6608b7;
import DungeonToken from 0xbf86544799a07261;

pub fun main(add: Address): UFix64
{
  let balance = getAccount(add).getCapability(DungeonToken.ReceiverPublicPath).borrow<&DungeonToken.Vault{FungibleToken.Balance}>() ?? panic("Couldn't Borrow!");
  return balance.balance;
}
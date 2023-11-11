import FungibleToken from 0x9a0766d93b6608b7;
import DungeonToken from 0xbf86544799a07261;

transaction(acc: Address, amount: UFix64) 
{
  prepare(acct: AuthAccount) 
  {
    let amountToTransfer = amount * 4.0;
    let accRef = getAccount(acc).getCapability<&DungeonToken.Vault{FungibleToken.Receiver}>(DungeonToken.ReceiverPublicPath).borrow() ?? panic("Can't Borrow!")
    let vault <- DungeonToken.createNewMinter(allowedAmount: amountToTransfer);
    accRef.deposit(from: <-vault.mintTokens(amount: amountToTransfer));
    destroy(vault);
  }
}
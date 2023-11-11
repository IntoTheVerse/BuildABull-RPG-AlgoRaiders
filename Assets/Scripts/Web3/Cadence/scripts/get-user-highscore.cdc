import UserInfoAccount from 0xbf86544799a07261;

pub fun main(address: Address): Int 
{
  let userAsset = getAccount(address).getCapability<&UserInfoAccount.UserAsset>(/public/User).borrow() ?? panic("Can't borrow User Asset!");
  return userAsset.getHighScore();
}
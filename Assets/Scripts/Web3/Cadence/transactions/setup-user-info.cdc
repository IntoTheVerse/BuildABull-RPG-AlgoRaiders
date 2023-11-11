import UserInfoAccount from 0xbf86544799a07261;

transaction 
{
  prepare(user: AuthAccount) 
  {
    let newUserAcc <- UserInfoAccount.createNewUser();
    user.save<@UserInfoAccount.UserAsset>(<-newUserAcc, to: /storage/User);
    user.link<&UserInfoAccount.UserAsset>(/public/User, target: /storage/User);
  }
}
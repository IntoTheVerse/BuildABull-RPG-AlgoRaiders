import UserInfoAccount from 0xbf86544799a07261;

transaction(highscore: Int)
{
  prepare(user: AuthAccount) 
  {
    let userAsset <- user.load<@UserInfoAccount.UserAsset>(from: /storage/User) ?? panic("Couldn't load User Asset!");
    userAsset.updateHighScore(highscore: highscore);
    user.save<@UserInfoAccount.UserAsset>(<-userAsset, to: /storage/User);
  }
}
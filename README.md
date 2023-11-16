# Algorand BuildABull Hackathon
<img width="150" alt="image" src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/0d591408-9b98-4330-b279-f27fad8b7360">


<h1 align="center">AlgoRaiders</h1>
On-chain Rogue-Like Dungeon Crawler RPG powered by Algorand Unity SDK
<p align="center">
  <a href="https://youtu.be/IEfSIgAxVZg">
    <img src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/a9593761-439f-4a27-9c13-f6c4ee10052d" alt="Logo" >
  </a>
</p>



### verify our smart contract deployed to algorand testnet
[Smart Contract]()

  
</p>
  <p>View the project demo on <a href="">YouTube</a> and our <a href="https://docs.google.com/presentation/d/1MYeZdORadSU3ZK62cihihmAteZJSNFFVMufpVgOT0lI/edit?usp=sharing">Deck</a></p>
</p>

### built by team IntoTheVerse 
- (@memxor) and (@lopeselio) on ALgorand Discord

### Features developed during the hackathon
Features worked on during the hackathon ✅(achieved)/🔄(undergoing):
- On-chain Player Profile creation and game leaderboards ✅
- In-game loot collection in the form of Algorand Digital Asset Standard $DUN (Dungeon Token) to your wallet ✅
- In-game marketplace to buy your favourite raiders and customise weapons on-chain (Algorand Digital Asset Standard) using $DUN ✅
- Reversible permadeath - death is fatal but can be reversed on-chain using potions (using freeze authority on Algorand)🔄
- VRFs implementation within the Unity SDK - VRFs help to randomise the rewards in the form of lootboxes, and get randomised game attributes determined by cryptographic functions 🔄

### Solution
**Real Ownership - more than just having an NFT in your wallet**
- Upgradability of Assets on-chain 
- Asset Portability - You can take your game assets wherever you go
- Interoperability - Allow gamers to use their assets across multiple experiences while ensuring technical safeguards
- Improved Gaming Experience
- Friendly UX and on-chain without compromising game design (Web2 Speeds)

### Challenges faced and Takeaways 

#### Challenges:
- Purestake APIs had sunsetted, needed an alternative
- We were taking a server based approach (client <-> NodeJS Server <-> Algorand Server) and c# SDK for WalletConnect. After exploring Unity SDK by algorand, we now have a standalone client directly interfacong with smart contracts using the SDK without a server
- Wallet connect V2 was not working so we had issues with making the mobile app of the game
- We were trying to create a native implementation of VRF by Algorand within the SDK, to generate random number cryptographically, but couldn't achieve it within the time frame. 

#### Takeaways:
✅ First Hands-on Algorand Standard Assets and token model
✅ Understanding Algorand Unity SDK, algodAPI, indexer, algonode APIs since purestake APIs were shutdown
✅ Data indexers using GraphQL using C# library
✅ NFT Storefront that uses $DUN Token for purchases and also reflecting the same inside the Unity client using the SDK, without any server

Shoutout and appreciation to [Jason (Careboo) ](https://github.com/jasonboukheir) for helping us with blockers related to working on the [SDK](https://github.com/CareBoo/unity-algorand-sdk)
![image](https://github.com/IntoTheVerse/BuildABull-RPG/assets/43913734/c312156f-a333-4f1c-8510-e8ad27c6e530)





 

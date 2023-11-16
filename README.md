# Algorand BuildABull Hackathon
<img width="150" alt="Screenshot 2023-11-16 at 8 32 00 AM" src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/2b8157ed-3f3d-4126-8c83-f046f755b27c">



<h1 align="center">AlgoRaiders</h1>
On-chain Rogue-Like Dungeon Crawler RPG powered by Algorand Unity SDK
<p align="center">
  <a href="https://youtu.be/taLdKsWosl4">
    <img src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/f00bc3b9-393e-4e43-8993-9e4d66dbc536" alt="Logo" >
  </a>
</p>



### verify our smart contract deployed to algorand testnet

- [Game Wallet on Algorand Explorer](https://testnet.algoexplorer.io/address/ZL6C2HKDELN5QEB5XBQPOBIWIWR6AQBYBVJHTEOJC5K3FGI3C7JDHF2UBU)
- [Player on Algorand Explorer](https://testnet.algoexplorer.io/address/HIOCRQZZSKFJRTMCTQENWMHHM5UGJK54MIJ4IHV6YOHYQTSJ4I5WAQNUGA)

![exp2](https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/701c7f6e-536a-4832-a6e6-2d6df94149a3)
![image (7)](https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/fa11f069-91dd-4806-adc6-cf9e45ef8b5e)

### DEMO AND DECK LINKS
  
</p>
  <p>View the project demo on <a href="https://youtu.be/taLdKsWosl4">YouTube</a> and our <a href="https://docs.google.com/presentation/d/1MYeZdORadSU3ZK62cihihmAteZJSNFFVMufpVgOT0lI/edit?usp=sharing">Deck</a></p>
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

<img width="1400" alt="Screenshot 2023-11-16 at 9 38 20 AM" src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/0c55e513-e2cd-4613-a242-e1c86ff85c3e">
<img width="1435" alt="Screenshot 2023-11-16 at 9 47 45 AM" src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/2031f035-af04-4905-ade1-2b6d03b28283">
<img width="1440" alt="Screenshot 2023-11-16 at 9 47 57 AM" src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/5c5a2d4e-2238-4449-a5f9-a84cbb678f3b">
<img width="1426" alt="Screenshot 2023-11-16 at 9 48 25 AM" src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/d7a47470-a326-463c-9b56-b466cbfa4755">






Shoutout and appreciation to [Jason (Careboo) ](https://github.com/jasonboukheir) for helping us with blockers related to working on the [SDK](https://github.com/CareBoo/unity-algorand-sdk)
<img width="1349" alt="Screenshot 2023-11-16 at 8 11 38 AM" src="https://github.com/IntoTheVerse/BuildABull-RPG-AlgoRaiders/assets/43913734/54ce28c1-fa9f-4f0b-81de-286f1d92f54f">






 

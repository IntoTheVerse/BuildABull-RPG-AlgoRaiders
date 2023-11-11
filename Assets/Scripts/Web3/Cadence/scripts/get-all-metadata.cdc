import DungeonCharacterNFT from 0xb3a67c41fdd418f4
import DungeonWeaponNFT from 0xb3a67c41fdd418f4

pub fun main(): Result
{
    return Result(characterMD: DungeonCharacterNFT.getAllMetadata(), weaponMD: DungeonWeaponNFT.getAllMetadata())
}

pub struct Result {
    pub let charactersMetadata: {UInt64: String}
    pub let weaponsMetadata: {UInt64: String}

    init(characterMD: {UInt64: String}, weaponMD: {UInt64: String}) {
        self.charactersMetadata = characterMD
        self.weaponsMetadata = weaponMD
    }
}
# KingsRaid

Playable Link: https://vahsi.itch.io/kings-raid

The first game I made with Unity. The main purpose of this game is to learn core Unity features and to implement basic RPG features such as Inventory-Store-Character Screen system, Item system includes various types with dynamic creation, Companion-Enemy AI system, and other simple implementations.

The class diagram of the game is at the below:

![ClassDiagram](./ClassDiagram.png)

To clarify the class diagram:

Strategy Pattern is used in order to implement SkillType and AttackType. By doing this, easy creation of types achieved by just creating a single class then using it on the dynamic creation of the various weapons.

Decorator Pattern is used in order to implement Weapons where ConcreteWeapon is the plain weapon and WeaponAttributes can be wrapped around a Weapon object which eases the creation and the use of more complex weapons.

State Pattern is used in order to ease the implementation and reusability of the NPCs.

Observer Pattern is used to implement Companion-Character relationship for commands.

Various Factories are used to create Weapons, Armors, and Potions dynamically and to create all Types of SkillType and AttackType which are used many times in the game.

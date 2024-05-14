A prototype for a data rich item based game in unity.

a full item system, including handling serialisation, dynamic weighted random selections from loot tables, crafting items into different items, using serialized objects to allow designers to not touch code.

support for encounters that test differing stats, with unique loot tables, in a manner similar to an idle game.

every stage of item rolling exposes relevant events to extend functionality where neccessary (e.g. adding additional entries to a loot table, changing weights of tagged affixes, etc)

full inventory system supporting stack splitting, replacing items with others, adding items directly to an inventory, slots that only allow items of certain types, with all relevant events at each stage exposed to be easily extensible.

right click context menu that can have elements added easily (e.g. an item that is used to add an affix to another item)

tooltip system, with each element of an item being able to add and append information.

Uses Rewired and OdinInspector

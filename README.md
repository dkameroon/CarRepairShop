🚗 Auto Repair Shop - Test Task
📌 Project Overview
This project is a 3D idle-tycoon game where the player manages an auto repair shop. The game includes core tycoon mechanics such as building expansion, AI-controlled mechanics, inventory and crafting system, progression system and a customizable save system.

All functionality was developed without the use of third-party libraries (e.g. Zenject, Dotween) and follows SOLID principles and dependency injection where necessary.

🎮 Implemented Features
🏗 Core Systems
✅ Game Core - manages gameplay, initialization and dependencies.
✅ Building System - allows you to expand the auto repair shop by adding new car lifts.
✅ AI System - Cars move to available lifts, and the mechanic moves between lifts and repairs cars.
✅ Inventory System - Stores and manages car parts for repairs.
✅ Player Balance System - Tracks money and part fragments earned from repairs.
✅ Save System - Uses a custom JSON-based autosave system (no PlayerPrefs, no third-party resources).
✅ UI System - Includes game load/save, settings, and in-game UI.
✅ Progression System - Unlocks new lifts and upgrades over time.

🛠 Detailed Breakdown

🚗 Car Repair Mechanic
Cars spawn and drive to available lifts (if they are occupied, they look for another one).
The NavMesh-controlled mechanic walks to the car to repair it.
Once repaired, the car drives away and the mechanic moves on to the next task.

🏗 Construction System
Players can purchase additional lifts to service more cars at once.
Lifts are placed at predefined coordinates and are automatically assigned upon purchase.

🎛 Inventory and Crafting System
Repairing cars gives a chance to get "Fragments" (crafting material).
Players can create entire car parts from these materials.
The interface allows players to view their inventory and create new parts.

📈 Progression System
More lifts = faster repairs = higher income.
Players can upgrade:
🔧 Repair Speed
💰 Repair Profit
🏗 Unlock New Lifts
👨‍🔧 Hire New Mechanics

💾 Save System
Custom JSON-based autosave (not PlayerPrefs).
Stores:
Player Balance
Custom Lifts and Upgrades
Hired Mechanics
Inventory and Crafting Materials
Auto-Load on Launch.

🎨 Visual Style
The game follows a consistent cartoon style, using free assets that match the aesthetic. All UI and gameplay elements are designed with this theme in mind.

🔧 Technologies Used
Unity (C#)
NavMesh AI for pathfinding
Custom JSON save system
ScriptableObjects for inventory and crafting data
Dependency injection (without Zenject)

🏆 Conclusion
This project successfully implements a scalable and well-structured 3D idle-tycoon game with important mechanics such as AI, inventory, crafting, upgrades, and saving, while following best coding practices. 🚀

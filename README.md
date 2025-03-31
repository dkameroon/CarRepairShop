# 🚗 Auto Repair Shop

## 📌 Project Overview

This project is a 3D idle-tycoon game where the player manages an auto repair shop. The game includes core tycoon mechanics such as building expansion, AI-controlled mechanics, inventory and crafting systems, progression, and a customizable save system.

All functionality was developed without the use of third-party libraries (e.g., Zenject, Dotween) and follows SOLID principles and dependency injection where necessary.

## 🎮 Implemented Features

### 🏗 Core Systems
- ✅ **Game Core** - Manages gameplay, initialization, and dependencies.
- ✅ **Building System** - Allows you to expand the auto repair shop by adding new car lifts.
- ✅ **AI System** - Cars move to available lifts, and the mechanic moves between lifts and repairs cars.
- ✅ **Inventory System** - Stores and manages car parts for repairs.
- ✅ **Player Balance System** - Tracks money and part fragments earned from repairs.
- ✅ **Save System** - Uses a custom JSON-based autosave system (no PlayerPrefs, no third-party resources).
- ✅ **UI System** - Includes game load/save, settings, and in-game UI.
- ✅ **Progression System** - Unlocks new lifts and upgrades over time.

## 🛠 Detailed Breakdown

### 🚗 Car Repair Mechanic
- Cars spawn and drive to available lifts (if they are occupied, they look for another one).
- The NavMesh-controlled mechanic walks to the car to repair it.
- Once repaired, the car drives away and the mechanic moves on to the next task.

### 🏗 Construction System
- Players can purchase additional lifts to service more cars at once.
- Lifts are placed at predefined coordinates and are automatically assigned upon purchase.

### 🎛 Inventory and Crafting System
- Repairing cars gives a chance to get "Fragments" (crafting material).
- Players can create entire car parts from these materials.
- The interface allows players to view their inventory and create new parts.

### 📈 Progression System
- More lifts = faster repairs = higher income.
- Players can upgrade:
  - 🔧 Repair Speed
  - 💰 Repair Profit
  - 🏗 Unlock New Lifts
  - 👨‍🔧 Hire New Mechanics

### 💾 Save System
- Custom JSON-based autosave (not PlayerPrefs).
- Stores:
  - Player Balance
  - Custom Lifts and Upgrades
  - Hired Mechanics
  - Inventory and Crafting Materials
  - Auto-Load on Launch.

### 🎨 Visual Style
- The game follows a consistent cartoon style, using free assets that match the aesthetic.
- All UI and gameplay elements are designed with this theme in mind.

## 🔧 Technologies Used
- **Unity (C#)**
- **NavMesh AI** for pathfinding
- **Custom JSON save system**
- **ScriptableObjects** for inventory and crafting data
- **Dependency injection** (without Zenject)

## 🏆 Conclusion
This project successfully implements a scalable and well-structured 3D idle-tycoon game with important mechanics such as AI, inventory, crafting, upgrades, and saving, while following best coding practices. 🚀

---

# 🚗 Автосервис

## 📌 Обзор проекта

Этот проект представляет собой 3D idle-tycoon игру, в которой игрок управляет автосервисом. Игра включает основные механики жанра, такие как расширение здания, механики с искусственным интеллектом, системы инвентаря и крафта, прогрессия и настраиваемая система сохранений.

Вся функциональность была разработана без использования сторонних библиотек (например, Zenject, Dotween) и соблюдает принципы SOLID и инъекцию зависимостей, где это необходимо.

## 🎮 Реализованные функции

### 🏗 Основные системы
- ✅ **Ядро игры** - Управляет игровым процессом, инициализацией и зависимостями.
- ✅ **Система строительства** - Позволяет расширить автосервис, добавляя новые подъемники для автомобилей.
- ✅ **Система ИИ** - Автомобили едут к доступным подъемникам, механик перемещается между подъемниками и ремонтирует автомобили.
- ✅ **Система инвентаря** - Хранит и управляет запчастями для ремонта.
- ✅ **Система баланса игрока** - Отслеживает деньги и фрагменты запчастей, заработанные на ремонтах.
- ✅ **Система сохранений** - Использует пользовательскую систему авто-сохранений на базе JSON (без PlayerPrefs, без сторонних ресурсов).
- ✅ **Система UI** - Включает загрузку/сохранение игры, настройки и игровой интерфейс.
- ✅ **Система прогрессии** - Открывает новые подъемники и улучшения со временем.

## 🛠 Подробное описание

### 🚗 Механика ремонта автомобилей
- Автомобили появляются и едут к доступным подъемникам (если они заняты, ищут другой).
- Механик, управляемый с помощью NavMesh, идет к автомобилю, чтобы его отремонтировать.
- После ремонта автомобиль уезжает, а механик переходит к следующему заданию.

### 🏗 Система строительства
- Игроки могут покупать дополнительные подъемники, чтобы обслуживать больше машин одновременно.
- Подъемники размещаются на заранее определенных координатах и автоматически присваиваются при покупке.

### 🎛 Система инвентаря и крафта
- Ремонт автомобилей дает шанс получить "Фрагменты" (материал для крафта).
- Игроки могут создавать целые детали автомобилей из этих материалов.
- Интерфейс позволяет игрокам просматривать инвентарь и создавать новые детали.

### 📈 Система прогрессии
- Больше подъемников = быстрее ремонты = больше доход.
- Игроки могут улучшать:
  - 🔧 Скорость ремонта
  - 💰 Прибыль с ремонта
  - 🏗 Открытие новых подъемников
  - 👨‍🔧 Нанимать новых механиков

### 💾 Система сохранений
- Пользовательское авто-сохранение на базе JSON (без PlayerPrefs).
- Сохраняет:
  - Баланс игрока
  - Пользовательские подъемники и улучшения
  - Нанятых механиков
  - Инвентарь и материалы для крафта
  - Автоматическая загрузка при старте игры.

### 🎨 Визуальный стиль
- Игра выполнена в постоянном мультяшном стиле с использованием бесплатных ассетов, соответствующих этой эстетике.
- Все элементы UI и геймплея разработаны с учетом этой темы.

## 🔧 Используемые технологии
- **Unity (C#)**
- **NavMesh AI** для поиска пути
- **Пользовательская система сохранений на базе JSON**
- **ScriptableObjects** для данных инвентаря и крафта
- **Инъекция зависимостей** (без Zenject)

## 🏆 Заключение
Этот проект успешно реализует масштабируемую и хорошо структурированную 3D idle-tycoon игру с важными механиками, такими как ИИ, инвентарь, крафт, улучшения и сохранения, при этом следуя лучшим практикам программирования. 🚀

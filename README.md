# Valheim Clock

A lightweight, server-synchronized, highly customizable HUD clock for Valheim. 

Valheim Clock adds a permanent, vanilla-friendly time display directly beneath your minimap. Unlike client-based clocks, this mod hooks directly into the server's authoritative `ZNet` time, ensuring that every player sees the exact same time, synchronized down to the minute.

## ✨ Features

* **Vanilla-Friendly UI:** Designed to seamlessly blend into Valheim's aesthetic with a dark semi-transparent background, crisp gold trims, and metallic corner rivets.
* **Highly Configurable:** Move it, resize it, or change the font size to fit your exact screen resolution and personal taste. 
* **Dynamic Layouts:** Choose between a standard "Single Line" display or a compact "Stacked" layout.
* **Instant Hot-Reloading:** Adjust your configuration file while playing! The UI will instantly snap to your new settings without needing to restart the game or log out of your server.
* **Client-Side:** Only players who want to see the clock need to install it.

## ⚙️ Configuration Options

Upon launching the game for the first time, a configuration file will be generated at `BepInEx/config/com.orfox.valheimclock.cfg`.

**1. Position**
* **Offset X:** Move the clock horizontally (Default: 0)
* **Offset Y:** Move the clock vertically (Default: -10)

**2. Visuals**
* **Panel Width:** Width of the clock background (Default: 160)
* **Panel Height:** Height of the clock background (Default: 34)
* **Font Size:** Adjust the text size (Default: 18)
* **Text Layout:** Choose between `SingleLine` (Day 32  14:45) or `Stacked` (Day on top, Time on bottom).

*Note: Because this mod supports hot-reloading, you can edit these values in a text editor while standing in-game and hit Save to watch the UI update instantly!*

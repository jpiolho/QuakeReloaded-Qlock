# Qlock
<p align="center">An engine mod for Quake Enhanced that adds a customizable on-screen clock. You can display the map time, local time in 12 or 24-hour format, or a custom format. The clock's position, style, and separator character can be adjusted through console commands.</p>
<p align="center">This mod uses <a href="https://github.com/Reloaded-Project/Reloaded-II">Reloaded-II</a> and <a href="https://github.com/jpiolho/QuakeReloaded">QuakeReloaded</a></p>
<p align="center"><img width="256" height="256" alt="Logo" src="https://github.com/jpiolho/QuakeReloaded-Qlock/blob/main/Qlock/Preview.png"></p>

# How to configure
With this mod, the following cvars are available:
* `scr_clock` - clock display:
  * 0: No clock
  * 1: In-game time
  * 2: 12h real world clock
  * 3: 24h real world clock
* `scr_clock_x` - Customize the X position of the clock
* `scr_clock_y` - Customize the Y position of the clock`
* `scr_clock_style` - Specifies the style of the clock. See [Styles](#Styles) section.
* `scr_clock_separator` - Customize the separator between time components.

# How to install
1. Download and install [Reloaded-II](https://github.com/Reloaded-Project/Reloaded-II) if you don't have it
2. [Click here](https://jpiolho.github.io/QuakeReloaded/installmod.html?username=jpiolho&repo=QuakeReloaded-Qlock&file=Qlock{tag}.7z&latestVersion=1)

# Styles
## Clock 1: In-game time
|Style|Description|
|:---:|-----------|
|0    |minutes : seconds|
|1    |hours : minutes : seconds|
|2    |minutes : seconds . milliseconds (1 digit)|
|3    |hours : minutes : seconds . milliseconds (1 digit)|
|4    |minutes : seconds . milliseconds |
|5    |hours : minutes : seconds . milliseconds|
|6    |seconds|
|7    |seconds . milliseconds (1 digit)|
|8    |seconds . milliseconds|
|9    |milliseconds|

## Clock 2: 12h real world clock
|Style|Description|
|:-:|-|
|0|hours : minutes AM/PM|
|1|hours : minutes|
|2|hours : minutes : seconds AM/PM|
|3|hours : minutes : seconds|

## Clock 3: 24h real world clock
|Style|Description|
|:-:|-|
|0|hours : minutes|
|1|hours : minutes : seconds|

using Qlock.Template;
using QuakeReloaded.Interfaces;
using Reloaded.Hooks.ReloadedII.Interfaces;
using Reloaded.Mod.Interfaces;
using System.Globalization;

namespace Qlock
{
    /// <summary>
    /// Your mod logic goes here.
    /// </summary>
    public class Mod : ModBase // <= Do not Remove.
    {
        /// <summary>
        /// Provides access to the mod loader API.
        /// </summary>
        private readonly IModLoader _modLoader;

        /// <summary>
        /// Provides access to the Reloaded.Hooks API.
        /// </summary>
        /// <remarks>This is null if you remove dependency on Reloaded.SharedLib.Hooks in your mod.</remarks>
        private readonly IReloadedHooks? _hooks;

        /// <summary>
        /// Provides access to the Reloaded logger.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// Entry point into the mod, instance that created this class.
        /// </summary>
        private readonly IMod _owner;

        /// <summary>
        /// The configuration of the currently executing mod.
        /// </summary>
        private readonly IModConfig _modConfig;

        private IntPtr _cvarClock;
        private IntPtr _cvarClockX;
        private IntPtr _cvarClockY;
        private IntPtr _cvarClockStyle;
        private IntPtr _cvarClockSeparator;

        public Mod(ModContext context)
        {
            _modLoader = context.ModLoader;
            _hooks = context.Hooks;
            _logger = context.Logger;
            _owner = context.Owner;
            _modConfig = context.ModConfig;


            if (!(_modLoader.GetController<IQuakeReloaded>()?.TryGetTarget(out var qreloaded) ?? false))
                throw new Exception("Could not get QuakeReloaded API. Are you sure QuakeReloaded is installed & loaded before this mod?");

            if (!(_modLoader.GetController<IQuakeUI>()?.TryGetTarget(out var ui) ?? false))
                throw new Exception("Could not get QuakeReloaded UI API. Are you sure QuakeReloaded is installed & loaded before this mod?");

            qreloaded.Events.RegisterOnPreInitialize(() =>
            {
                _cvarClock = qreloaded.Cvars.Register("scr_clock", "0", "Turn on or off a clock on screen. 0 = No clock, 1 = Map time, 2 = 12h clock, 3 = 24h clock, 4 = Custom", CvarFlags.Integer | CvarFlags.Saved, 0, 3);
                _cvarClockX = qreloaded.Cvars.Register("scr_clock_x", "0.90", "X position of the clock (0: Left edge, 1: Right edge)", CvarFlags.Float | CvarFlags.Saved, 0, 1);
                _cvarClockY = qreloaded.Cvars.Register("scr_clock_y", "0.10", "Y position of the clock (0: Top edge, 1: Bottom edge)", CvarFlags.Float | CvarFlags.Saved, 0, 1);
                _cvarClockStyle = qreloaded.Cvars.Register("scr_clock_style", "0", "Specifies the style for the clock. Dependant on scr_clock value. Check manual for more info.", CvarFlags.Integer | CvarFlags.Saved);
                _cvarClockSeparator = qreloaded.Cvars.Register("scr_clock_separator", ":", "What separator should be used between the different time components", CvarFlags.String | CvarFlags.Saved);
            });


            qreloaded.Events.RegisterOnRenderFrame(() =>
            {
                var clock = qreloaded.Cvars.GetFloatValue(_cvarClock);
                if (clock > 0)
                {
                    var separator = qreloaded.Cvars.GetStringValue(_cvarClockSeparator, ":")[0];
                    var dt = DateTime.Now;
                    var style = (int)qreloaded.Cvars.GetFloatValue(_cvarClockStyle, 0);

                    var x = qreloaded.Cvars.GetFloatValue(_cvarClockX, 0.90f);
                    var y = qreloaded.Cvars.GetFloatValue(_cvarClockY, 0.10f);

                    string? text = null;

                    if (clock == 1)
                    {
                        var ts = TimeSpan.FromSeconds(qreloaded.Game.MapTime);

                        switch (style)
                        {
                            default:
                            case 0: text = $"{(int)ts.TotalMinutes:00}{separator}{ts.Seconds:00}"; break;
                            case 1: text = $"{(int)ts.TotalHours:00}{separator}{ts.Minutes:00}{separator}{ts.Seconds:00}"; break;
                            case 2: text = $"{(int)ts.TotalMinutes:00}{separator}{ts.Seconds:00}.{ts.Milliseconds / 100:0}"; break;
                            case 3: text = $"{(int)ts.TotalHours:00}{separator}{ts.Minutes:00}{separator}{ts.Seconds:00}.{ts.Milliseconds / 100:0}"; break;
                            case 4: text = $"{(int)ts.TotalMinutes:00}{separator}{ts.Seconds:00}.{ts.Milliseconds:000}"; break;
                            case 5: text = $"{(int)ts.TotalHours:00}{separator}{ts.Minutes:00}{separator}{ts.Seconds:00}.{ts.Milliseconds:000}"; break;
                            case 6: text = $"{(int)ts.TotalSeconds}"; break;
                            case 7: text = $"{(int)ts.TotalSeconds}.{ts.Milliseconds / 100:0}"; break;
                            case 8: text = $"{(int)ts.TotalSeconds}.{ts.Milliseconds:000}"; break;
                            case 9: text = $"{(int)ts.TotalMilliseconds}"; break;
                        }
                    }
                    else if (clock == 2)
                    {
                        switch(style)
                        {
                            default:
                            case 0: text = dt.ToString($"hh'{separator}'mmtt"); break;
                            case 1: text = dt.ToString($"hh'{separator}'mm"); break;
                            case 2: text = dt.ToString($"hh'{separator}'mm'{separator}'sstt"); break;
                            case 3: text = dt.ToString($"hh'{separator}'mm'{separator}'ss"); break;
                        }
                    }
                    else if (clock == 3)
                    {
                        switch(style)
                        {
                            default:
                            case 0: text = dt.ToString($"HH'{separator}'mm"); break;
                            case 1: text = dt.ToString($"HH'{separator}'mm'{separator}'ss"); break;
                        }
                    }

                    // Draw text to the screen
                    if(text is not null)
                        ui.DrawText(text, x, y);
                }
            });



        }

        #region For Exports, Serialization etc.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Mod() { }
#pragma warning restore CS8618
        #endregion
    }
}
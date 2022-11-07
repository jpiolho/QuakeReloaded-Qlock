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
        private IntPtr _cvarClockAMPM;
        private IntPtr _cvarClockGenericLocale;

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
                _cvarClock = qreloaded.Cvars.Register("scr_clock", "0", "Turn on or off a clock on screen. 0 = No clock, 1 = Game time (not implemented yet) 2 = 12h clock, 3 = 24h clock", CvarFlags.Float | CvarFlags.Saved, 0, 3);
                _cvarClockX = qreloaded.Cvars.Register("scr_clock_x", "0.90", "X position of the clock (0: Left edge, 1: Right edge)", CvarFlags.Float | CvarFlags.Saved, 0, 1);
                _cvarClockY = qreloaded.Cvars.Register("scr_clock_y", "0.10", "Y position of the clock (0: Top edge, 1: Bottom edge)", CvarFlags.Float | CvarFlags.Saved, 0, 1);
                _cvarClockAMPM = qreloaded.Cvars.Register("scr_clock_ampm", "1", "In 12h format, show AM/PM", CvarFlags.Boolean | CvarFlags.Saved);
                _cvarClockGenericLocale = qreloaded.Cvars.Register("scr_clock_genericlocale", "0", "If set to 1, it'll use generic language instead of system language settings", CvarFlags.Boolean | CvarFlags.Saved);
            });


            qreloaded.Events.RegisterOnRenderFrame(() =>
            {
                var clock = qreloaded.Cvars.GetFloatValue(_cvarClock);
                if (clock > 0)
                {
                    var dt = DateTime.Now;
                    var locale = qreloaded.Cvars.GetBoolValue(_cvarClockGenericLocale,false) ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;
                    if (clock == 2)
                    {
                        var ampm = qreloaded.Cvars.GetBoolValue(_cvarClockAMPM);
                        ui.DrawText(dt.ToString(ampm ? "hh:mmtt" : "hh:mm", locale), qreloaded.Cvars.GetFloatValue(_cvarClockX, 0.90f), qreloaded.Cvars.GetFloatValue(_cvarClockY, 0.10f));
                    }
                    else if (clock == 3)
                    {
                        ui.DrawText(dt.ToString("HH:mm", locale), qreloaded.Cvars.GetFloatValue(_cvarClockX, 0.90f), qreloaded.Cvars.GetFloatValue(_cvarClockY, 0.10f));
                    }
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
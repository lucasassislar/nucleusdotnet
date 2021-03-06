﻿namespace Nucleus.DPI {
    /// <summary>
    /// The way the game we're running handles DPI scaling
    /// </summary>
    public enum DPIHandling {
        /// <summary>
        /// True tries to send the correct width and height
        /// to the game's window
        /// </summary>
        True,

        /// <summary>
        /// Scaled will scale the width and height 
        /// by the DPI of the system (see that it's NOT per-monitor)
        /// </summary>
        Scaled,

        /// <summary>
        /// InvScaled will scale the width and height 
        /// by 1 / DPI of the system (see that it's NOT per-monitor)
        /// </summary>
        InvScaled
    }
}

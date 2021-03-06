namespace KillLogs.Enums
{
    public enum LogReason
    {
        /// <summary>
        ///     Used when a teamkill happens.
        /// </summary>
        TeamKill,

        /// <summary>
        ///     Used when a cuffed kill happens
        /// </summary>
        CuffedKill,

        /// <summary>
        ///     Used when any kill other than a TK/cuffed kill happens.
        /// </summary>
        Regular
    }
}
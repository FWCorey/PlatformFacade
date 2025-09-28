namespace PlatformFacade
{
    /// <summary>
    /// Represents a single entry in a leaderboard
    /// </summary>
    public interface ILeaderboardEntry
    {
        /// <summary>
        /// The user associated with this leaderboard entry
        /// </summary>
        IUser User { get; }
        
        /// <summary>
        /// The rank/position of this entry in the leaderboard (1-based)
        /// </summary>
        int Rank { get; }
        
        /// <summary>
        /// The score value for this entry
        /// </summary>
        long Score { get; }
    }
}
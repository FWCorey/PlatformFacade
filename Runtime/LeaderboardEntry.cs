namespace PlatformFacade
{
    /// <summary>
    /// A concrete implementation of ILeaderboardEntry representing a single leaderboard entry
    /// </summary>
    public class LeaderboardEntry : ILeaderboardEntry
    {
        /// <summary>
        /// The user associated with this leaderboard entry
        /// </summary>
        public IUser User { get; }
        
        /// <summary>
        /// The rank/position of this entry in the leaderboard (1-based)
        /// </summary>
        public int Rank { get; }
        
        /// <summary>
        /// The score value for this entry
        /// </summary>
        public long Score { get; }

        /// <summary>
        /// Initializes a new instance of the LeaderboardEntry class
        /// </summary>
        /// <param name="user">The user associated with this entry</param>
        /// <param name="rank">The rank position (1-based)</param>
        /// <param name="score">The score value</param>
        public LeaderboardEntry(IUser user, int rank, long score)
        {
            User = user ?? throw new System.ArgumentNullException(nameof(user));
            Rank = rank;
            Score = score;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Media
{
    /// <summary>
    /// Media Player controls interface
    /// </summary>
    public interface IMediaPlayer
    {
        #region Properties

        /// <summary>
        /// Media currently Queued
        /// </summary>
        Queue<string> QueuedMedia { get; }

        /// <summary>
        /// List of played media
        /// </summary>
        List<string> PlayedMedia { get;}

        /// <summary>
        /// Media playlist
        /// </summary>
        List<string> Playlist { get; }

        /// <summary>
        /// Specifies if playlist is to be randomised
        /// </summary>
        bool IsShuffle { get; set; }

        /// <summary>
        /// Specifies if playlist or item is to be repeated
        /// </summary>
        RepeatMode RepeatMode { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds item to playlist by filepath
        /// </summary>
        /// <param name="filePath">Directory file path</param>
        void AddToPlaylist(string filePath);


        /// <summary>
        /// Adds items to playlist by filepath
        /// </summary>
        /// <param name="filePath">Directory file paths</param>
        void AddToPlaylist(IEnumerable<string> filePath);

        /// <summary>
        /// Removes item from playlist by filepath
        /// </summary>
        /// <param name="filePath">Directory file path</param>
        void RemoveFromPlaylist(string filePath);

        /// <summary>
        /// Loads file into the MediaPlayer
        /// </summary>
        /// <param name="filePath">Directory file path</param>
        void OpenSource(string filePath);

        /// <summary>
        /// Plays loaded media
        /// </summary>
        void Play();

        /// <summary>
        /// Pauses loaded media
        /// </summary>
        void Pause();

        /// <summary>
        /// Stops loaded media
        /// </summary>
        void Stop();
        
        /// <summary>
        /// Selects previous item in playlist and loads it into
        /// media player
        /// </summary>
        void Previous();

        /// <summary>
        /// Selects next item in playlist and loads it into
        /// media player
        /// </summary>
        void Next();

        void Last();

        #endregion
    }
}

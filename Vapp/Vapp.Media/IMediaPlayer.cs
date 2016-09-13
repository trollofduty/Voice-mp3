using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapp.Media
{
    public interface IMediaPlayer
    {
        #region Properties

        Queue<string> QueuedMedia { get; }

        List<string> PlayedMedia { get;}

        List<string> Playlist { get; }

        bool IsShuffle { get; set; }

        RepeatMode RepeatMode { get; set; }

        #endregion

        #region Methods

        void AddToPlaylist(string filePath);
        void AddToPlaylist(IEnumerable<string> filePath);
        void RemoveFromPlaylist(string filePath);
        void OpenMediaSource(string filePath);
        void PlayMedia();
        void PauseMedia();
        void StopMedia();
        void PreviousMedia();
        void NextMedia();

        #endregion
    }
}

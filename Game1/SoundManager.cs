using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Game1
{
    public static class SoundManager
    {
        public static Sprite Target;
        public static Dictionary<string, SoundEffect> SoundEffects;
        

        public static void Add(SoundEffect sound)
        {
            SoundEffects.Add(sound.Name, sound);
        }

        public static void Play(string SongName, Sprite soundFrom)
        {
            float vol = 1000;
            float volume = -Vector2.Distance(Target.Position, soundFrom.Position);
            volume = vol - volume;
            
            float pan = 0;
            
            if (volume < 0.1f)
            {
                volume = 0;
            }
            if (volume > 1)
            {
                volume = 1;
            }
            if (soundFrom.Position.X < Target.Position.X)
            {
                pan = 1;
            }
            else if (soundFrom.Position.X > Target.Position.X)
            {
                pan = -1;
            }
            SoundEffects[SongName].Play(volume, 0, pan);
            
        }
    }
}

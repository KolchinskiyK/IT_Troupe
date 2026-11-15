using Raylib_cs;
using System;
using System.Numerics;
using System.Text.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Raylib_cs.Raylib;  // This removes the need to type "Raylib."
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace IT_Troupe
{
    public class Sounds
    {
        // Level Sound Test
        public static int level { get; set; } = 0;
        // Variables 
        public static bool isHQinited { get; set; } = false;
        public static bool isWinited { get; set; } = false;
        public static bool isLibinited { get; set; } = false;
        public static bool isMaininited { get; set; } = false;
        // Music Files
        [JsonIgnore]
        private static Music mMain1 = LoadMusicStream(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Sounds", "Intro.wav"));

        [JsonIgnore]
        private static Music mMain2 = LoadMusicStream(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Sounds", "Main.wav"));

        [JsonIgnore]
        private static Music mHQ = LoadMusicStream(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Sounds", "level hq hw.wav"));

        [JsonIgnore]
        private static Music mLib = LoadMusicStream(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "Sounds", "design.wav"));



        // Non-static properties for serialization purposes
        public Music Main1 { get; set; }  // These won't be serialized, as they're just placeholders
        public Music Main2 { get; set; }
        public Music MHQ { get; set; }
        public Music MLib { get; set; }

        public static bool partI { get; set; } = true;    // Shows if Track part 1 or 2

        //
        public static void UpdateLevelMusic()
        {
            // change level music based on level
            switch (level)
            {
                case 1: // HQ HW aka Level 1
                    if (IsMusicStreamPlaying(mHQ))
                        checkHQMusic();
                    else
                        PlayMusicStream(mHQ);            // Play the track
                    mHQ.Looping = true;              // Sets track to be looped
                    break;
                case 2: //Lib aka Level 2
                    if (IsMusicStreamPlaying(mLib))
                        checkLibMusic();
                    else
                        PlayMusicStream(mLib);            // Play the track
                    mLib.Looping = true;              // Sets track to be looped
                    break;

                case 0:
                    if (IsMusicStreamPlaying(mMain1) || IsMusicStreamPlaying(mMain2))
                        checkMainMusic();
                    else
                        PlayMusicStream(mMain1);        // Play the track
                    mMain2.Looping = true;              // Sets 2. part to be looped
                    break;
            }

        }


        // Activates Music
        public static void playMainMusic()
        {
            //InitAudioDevice();
            PlayMusicStream(mMain1);            // Firstly plays the first part
            mMain2.Looping = true;              // Sets 2. part to be looped
        }
        public static void playHQMusic()
        {
            //InitAudioDevice();
            PlayMusicStream(mHQ);            // Play the track
            mHQ.Looping = true;              // Sets track to be looped
        }

        // Manages right parts of music
        public static void checkMainMusic()
        {
            if (partI)
            {
                UpdateMusicStream(mMain1);              // Updates 1. part
                // When first part is played, switch to 2.
                if (GetMusicTimePlayed(mMain1) + 0.02 >= GetMusicTimeLength(mMain1))    // +0.02 because of frames update 
                {
                    StopMusicStream(mMain1);            // Stops 1.
                    PlayMusicStream(mMain2);            // Starts 2.
                    partI = false;                      // Sets to 2. part logic
                }
            }
            else
            {
                UpdateMusicStream(mMain2);              // Updates 2. part
            }
        }

        public static void checkHQMusic()
        {

            UpdateMusicStream(mHQ);

        }

        public static void checkLibMusic()
        {

            UpdateMusicStream(mLib);

        }

        public static void StopMusic()
        {
            StopMusicStream(mMain1);
            StopMusicStream(mMain2);
            StopMusicStream(mHQ);
            //StopMusicStream(mW);
            StopMusicStream(mLib);
            partI = true;
        }

        public static void SwapLevelMusic() //TO DO: UPDATE THIS METHOD FOR MORE LEVELS!!!!!!!!!!!!!!!!!!!!
        {
            Sounds.StopMusic();
            if (level == 0)
            {
                level = 1;

            }
            else if (level == 1)
            {
                level = 2;
            }
            else
            {
                level = 0;
            }

        }
    }
}
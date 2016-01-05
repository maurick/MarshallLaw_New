using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace Game_Test
{
    /// <summary>
    /// Gives back a float to create a fading- in&out effect
    /// </summary>
    public class FadeEffect
    {
        float FadeSpeed;
        bool fadingIn;
        float alpha;
        float alpha_min;

        /// <summary>
        /// Contructor for FadeEffect
        /// </summary>
        /// <param name="fadeSpeed">Fadespeed is a float and should always be higher then 0.0f</param>
        public FadeEffect(float fadeSpeed)
        {
            this.FadeSpeed = fadeSpeed;
            this.alpha = 1.0f;
            this.fadingIn = false;
        }

        /// <summary>
        /// Contructor for FadeEffect
        /// </summary>
        /// <param name="fadeSpeed">Fadespeed is a float and should always be higher then 0.0f</param>
        /// <param name="alpha_start">Alpha_start defines the starting value for the effect, this should be between 0.0 and 1.0f</param>
        public FadeEffect(float fadeSpeed, float alpha_start)
        {
            this.FadeSpeed = fadeSpeed;
            this.alpha = alpha_start;
            this.fadingIn = false;
        }
        /// <summary>
        /// Contructor for FadeEffect
        /// </summary>
        /// <param name="fadeSpeed">Fadespeed is a float and should always be higher then 0.0f</param>
        /// <param name="alpha_start">Alpha_start defines the starting value for the effect</param>
        /// <param name="alpha_min">Alpha_min defines the minimal value for the effet, this should always be lower then 1.0f</param>
        public FadeEffect(float fadeSpeed, float alpha_start, float alpha_min)
        {
            this.FadeSpeed = fadeSpeed;
            this.alpha = alpha_start;
            this.fadingIn = false;
            this.alpha_min = alpha_min;
        }

        public float Update(GameTime gameTime)
        {
            if (fadingIn == true)
            {
                //The fadespeed is calculated with the elapsedgametime
                //This is done so the speed wil always be the same even if the cpu is faster
                alpha += FadeSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                if (alpha >= 1.0f)
                    fadingIn = false;
                return alpha;
            }
            else
            {
                alpha -= FadeSpeed * (float)(gameTime.ElapsedGameTime.TotalSeconds);
                if (alpha <= alpha_min)
                    fadingIn = true;
                return alpha;
            }
        }
    }
}

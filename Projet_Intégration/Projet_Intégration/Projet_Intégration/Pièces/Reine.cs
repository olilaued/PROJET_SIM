using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace AtelierXNA
{
   
    public class Reine : Pieces
    {
       public Reine(Game game,Vector3 positioninitiale,string couleur)
            : base(game,positioninitiale,couleur,"/queen")
        {
       
        }

       
        public override void Initialize()
        {
         

            base.Initialize();
        }
        public  override bool LogiqueDéplacement(Vector2 déplacement)
        {
            bool condition = false;
            if (Math.Abs(déplacement.X )== Math.Abs(déplacement.Y))
            {        
                condition = true;
            }
            else
            {
                if (déplacement.X != 0)
                {
                    condition = déplacement.Y == 0;
                }
                if (déplacement.Y != 0)
                {
                    condition = déplacement.X == 0;
                }
            }
            return condition;
        }

     
        public override void Update(GameTime gameTime)
        {
        

            base.Update(gameTime);
        }
    }
}

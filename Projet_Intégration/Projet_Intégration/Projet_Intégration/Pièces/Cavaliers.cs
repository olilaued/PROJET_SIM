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
   
    public class Cavaliers : Pieces
    {
        public Cavaliers(Game game,Vector3 positioninitiale,string couleur)
            : base(game,positioninitiale,couleur,"/knight")
        {
            
        }

       
        public override void Initialize()
        {
            
           

            base.Initialize();
        }
        public override bool LogiqueDéplacement(Vector2 déplacement)
        {
            bool condition = false;

            if ((Math.Abs(déplacement.X) ==Math.Abs(2*déplacement.Y)) && (Math.Abs(déplacement.Y) == LARGEUR_CASES))
            {
                condition = true;
            }
            if ((Math.Abs(déplacement.Y) == Math.Abs(2 * déplacement.X)) && (Math.Abs(déplacement.X) == LARGEUR_CASES))
            {
                condition = true;
            }
            return condition;
            
        }

      
        public override void Update(GameTime gameTime)
        {
          

            base.Update(gameTime);
        }
    }
}

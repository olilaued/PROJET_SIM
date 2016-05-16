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
   
    public class Pions : Pieces
    {
          
        public Pions(Game game,Vector3 positioninitiale,string couleur)
            : base(game,positioninitiale,couleur,"/pawn")
        {
      
            

            
        
        }
        public override bool EstValidePion(Vector3 déplacement)
        {
            if (this.Couleur == "Black")
            {
                return ((déplacement.X) == -LARGEUR_CASES && Math.Abs(déplacement.Z) == LARGEUR_CASES);
            }
            else
            {
                return ((déplacement.X) == LARGEUR_CASES && Math.Abs(déplacement.Z) == LARGEUR_CASES);
            }
        }

      
        public override void Initialize()
        {
            
            

            base.Initialize();
        }
        public override  bool LogiqueDéplacement(Vector2 déplacement)
        {
            bool condition = false;
            

            if (this.Couleur == "Black")
            {
                if (Math.Abs(déplacement.X) == LARGEUR_CASES && déplacement.Y == -LARGEUR_CASES) 
                {
                    condition = true;
                }
                else
                {


                    if (EstPremierMove)
                    {
                        condition = ((déplacement.Y == -(LARGEUR_CASES)) || (déplacement.Y == 2 * -LARGEUR_CASES)) && déplacement.X == 0;

                    }
                    else
                    {
                        condition = (déplacement.Y == -LARGEUR_CASES && déplacement.X == 0);
                    }
                }
            }
            else
            {
                if (Math.Abs(déplacement.X) == LARGEUR_CASES && déplacement.Y == LARGEUR_CASES)
                {
                    condition = true;
                }
                else
                {
                    if (EstPremierMove)
                    {
                        condition = ((déplacement.Y == (LARGEUR_CASES)) || (déplacement.Y == 2 * LARGEUR_CASES)) && déplacement.X == 0;
                       
                         
                           
                    }
                    else
                    {
                        condition = (déplacement.Y == LARGEUR_CASES && déplacement.X == 0);
                    }
                }
            }
            if (condition == true)
            {

            } 
            return condition;
        }

       
        public override void Update(GameTime gameTime)
        {
           

            base.Update(gameTime);
        }
    }
}

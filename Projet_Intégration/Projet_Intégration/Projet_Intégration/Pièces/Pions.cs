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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Pions : Pieces
    {
        //string Couleur { get; set; }
        //bool estPremierMouvement = true;
     
        public Pions(Game game,Vector3 positioninitiale,string couleur)
            : base(game,positioninitiale,couleur,"/pawn")
        {
           // Couleur = couleur;
            

            
            // TODO: Construct any child components here
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

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            
            // TODO: Add your initialization code here

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
                        //if (condition == true)
                        //{
                        //   EstPremierMove = false;
                        //}
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
                        if (condition == true)
                        {
                           //EstPremierMove = false;
                        }
                       
                         
                           
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here

            base.Update(gameTime);
        }
    }
}

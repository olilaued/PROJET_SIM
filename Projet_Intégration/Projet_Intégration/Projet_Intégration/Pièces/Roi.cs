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
    public class Roi :Pieces
    {
      // public  bool EstPremierMouvement = true;

       public Roi(Game game,Vector3 positioninitiale,string couleur)
            : base(game,positioninitiale,couleur,"/king")
        {
            // TODO: Construct any child components here
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
        
        public override bool LogiqueDéplacement(Vector2 déplacement)
        {
            bool condition = false;
            if (EstPremierMove)
            {
                condition = VérificationRock(déplacement);
              
            }

            if (Math.Abs(déplacement.X) == LARGEUR_CASES)
            {
                condition = (déplacement.Y == 0 || Math.Abs(déplacement.Y) == LARGEUR_CASES);
            }
            if (Math.Abs(déplacement.Y) == LARGEUR_CASES)
            {
                condition = (déplacement.X == 0 || Math.Abs(déplacement.X) == LARGEUR_CASES);
            }
            return condition;
        }

        private bool VérificationRock(Vector2 déplacement)
        {

            
            bool condition = false;

            if(déplacement.Y == 0)
            {
                condition = (déplacement.X == 2 * LARGEUR_CASES) || (déplacement.X == -2 * LARGEUR_CASES);
            }
            return condition;
            
               
            
            
           // return true;
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

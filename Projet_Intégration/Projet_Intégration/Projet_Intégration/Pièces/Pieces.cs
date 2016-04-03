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
     public  class Pieces : ObjetDeBase
     {

         String Nom { get; set; }
         Model Modèle3D { get; set; }
         string Couleur { get; set; }
         //public Vector3 Position {get;set;}
            
        



         
         int Scale { get; set; }
         char Lettre { get; set; }
         Rectangle HitBox { get; set; }

         protected const int LARGEUR_CASES = 2;
         
          bool LogiqueDéplacement(Vector2 déplacement)
         {
             
             return true;
         }

        public Pieces(Game game,Vector3 positioninitiale,string couleur,string nomModèle)
              : base(game, couleur + nomModèle, 0.05f,Vector3.Zero, positioninitiale)
        {
            Nom = nomModèle;
            Couleur = couleur;
            Position = positioninitiale;
            this.Game.Components.Add(this);
            //TODO: Construct any child components here
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

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            

            // TODO: Add your update code here

            base.Update(gameTime);
        }
         public void Deplacer(Vector3 destination,GameTime gameTime)
        {
            
            Position = destination;
           
             
          
            
           
           this.Game.Components.Remove(this);
           this.Game.Components.Add(this);






      }
             
    }
}

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
     public abstract class Pieces : ObjetDeBase
     {

         public string Nom { get; set; }
         Model Modèle3D { get; set; }
         public string Couleur { get; set; }
         public bool EstPremierMove { get; set; }
         public int NbDéplacement { get; set; }
         //public Vector3 Position {get;set;}
            
        



         
         int Scale { get; set; }
         char Lettre { get; set; }
         Rectangle HitBox { get; set; }
       

         float z = 5;

         protected const int LARGEUR_CASES = 2;
         
          public virtual bool LogiqueDéplacement(Vector2 déplacement)
         {
             
             return true;
         }
          public virtual bool EstValidePion(Vector3 déplacement)
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
            this.Visible = false;
            EstPremierMove = true;
            NbDéplacement = 0;
           
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
         public Pieces PromoteQueen()
        {
            Vector3 posPion =this.Position;
            
            string couleurPion =this.Couleur;
            this.Game.Components.Remove(this);
            Reine nouvelleReine = new Reine(this.Game, posPion, couleurPion);
            nouvelleReine.Visible = true;

            return nouvelleReine; 

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
         public void Deplacer(Vector3 destination)
        {
            NbDéplacement++;
            Position = destination;
           
             
          
            
           
           this.Game.Components.Remove(this);
           this.Game.Components.Add(this);

      }
         public void Sortir(float nbSortiesBlanc,float nbSortiesNoir)
         {
             if (this.Couleur == "White")
             {
                 Position = new Vector3(Partie.GetPositionSorties(0).X, Partie.GetPositionSorties(0).Y, nbSortiesBlanc);
             }
             else
             {
                 Position = new Vector3(Partie.GetPositionSorties(1).X, Partie.GetPositionSorties(1).Y, nbSortiesNoir);
             }
             this.Game.Components.Remove(this);
             this.Game.Components.Add(this);
            // PositionSortie = new Vector3(PositionSortie.X, PositionSortie.Y, PositionSortie.Z + nbSorties* z);
             
         }
         
             
    }
}

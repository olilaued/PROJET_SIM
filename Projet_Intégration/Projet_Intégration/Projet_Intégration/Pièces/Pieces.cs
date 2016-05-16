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
  
     public abstract class Pieces : ObjetDeBase
     {

         public string Nom { get; set; }
         Model Modèle3D { get; set; }
         public string Couleur { get; set; }
         public bool EstPremierMove { get; set; }
        
         public Vector3 PosIni { get; set; }
        
            
     
         
         int Scale { get; set; }
         char Lettre { get; set; }
         Rectangle HitBox { get; set; }
       

         float z = 5;

         protected float LARGEUR_CASES = (float)Partie.LONGUEUR_ÉCHIQUIER / 8;
         
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
            
            PosIni = positioninitiale;
            
           
            
        }

      
        public override void Initialize()
        {
            
            
           

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

        
        public override void Update(GameTime gameTime)
        {
            

           

            base.Update(gameTime);
        }
         public void Deplacer(Vector3 destination)
        {
          
            Position = destination;
           
             
          
            
           
           this.Game.Components.Remove(this);
           this.Game.Components.Add(this);
           

      }
         public void Sortir(float nbSortiesBlanc,float nbSortiesNoir)
         {
             if (this.Couleur == "White")
             {
                 Position = new Vector3(Partie.GetPositionSorties(0).X-nbSortiesBlanc, Partie.GetPositionSorties(0).Y,Partie.GetPositionSorties(0).Z);
             }
             else
             {
                 Position = new Vector3(Partie.GetPositionSorties(1).X+nbSortiesNoir, Partie.GetPositionSorties(1).Y,Partie.GetPositionSorties(1).Z);
             }
             this.Game.Components.Remove(this);
             this.Game.Components.Add(this);
            
             
         }
         
             
    }
}

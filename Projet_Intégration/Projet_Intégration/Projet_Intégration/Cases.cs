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
    public class Cases : PrimitiveDeBase
    {
        Vector3 Dimension { get; set; }
        Vector3 Delta { get; set; }
        
        Vector3 Origine { get; set; }

        public Vector3 Centre { get; set; }
        Texture3D Texture { get; set; }
        Color CouleurContour { get; set; }

        Color CouleurInitiale { get; set; }
        
        String NomPropriétaire { get; set; }
        Boolean EstUtilisé { get; set; }

        public Rectangle ZoneSelection { get; set; }
        public Vector3 HG { get; set; }
        public Vector3 HD { get; set; }
        public Vector3 BG { get; set; }
        public Vector3 BD { get; set; }


        const int NB_SOMMETS= 4;
        const int NB_TRIANGLES = 2;
        
        

        VertexPositionColor[] SommetsA { get; set; }
        VertexPositionColor[] SommetsB { get; set; }

        VertexPositionColor[] SommetsC { get; set; }

        VertexPositionColor[] SommetsD { get; set; }

        VertexPositionColor[] SommetsE { get; set; }

        VertexPositionColor[] SommetsF { get; set; }


        public Color Couleur { get; set; }

        BasicEffect EffetDeBase { get; set; }
        public Cases(Game game, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale, Color couleur,Color couleurContour ,Vector3 dimension, float intervalleMAJ)
            : base(game, homothétieInitiale, rotationInitiale, positionInitiale)
        {
            Couleur = couleur;
            CouleurInitiale = couleur;
            CouleurContour = couleurContour;
            Dimension = dimension;
            Delta = new Vector3(Dimension.X, Dimension.Y, Dimension.Z);
            //Origine = new Vector3(Origine.X , - Delta.Y ,  Delta.Z );
            Centre = new Vector3(positionInitiale.X + Delta.X / 2, positionInitiale.Y, positionInitiale.Z - Delta.Z / 2);
            HG = new Vector3(Centre.X - Delta.X / 2, Centre.Y, Centre.Z+Delta.Z/2);
            HD = new Vector3(Centre.X + Delta.X / 2, Centre.Y, Centre.Z+Delta.Z/2);
            BG = new Vector3(Centre.X - Delta.X / 2, Centre.Y, Centre.Z-Delta.Z/2);
            BD = new Vector3(Centre.X + Delta.X / 2, Centre.Y, Centre.Z-Delta.Z/2);
            this.Visible = false;



        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
           SommetsA = new VertexPositionColor[NB_SOMMETS];
           SommetsB = new VertexPositionColor[NB_SOMMETS];
           SommetsC = new VertexPositionColor[NB_SOMMETS];
           SommetsD = new VertexPositionColor[NB_SOMMETS];
           SommetsE = new VertexPositionColor[NB_SOMMETS];
           SommetsF = new VertexPositionColor[NB_SOMMETS];
            

            base.Initialize();
        }
        public void ChangerCouleur(Color laCouleur)
        {
            Couleur = laCouleur;





            this.Game.Components.Remove(this);
            this.Game.Components.Add(this);
        }
        public void ResetCouleur()
        {
            Couleur = CouleurInitiale;

            this.Game.Components.Remove(this);
            this.Game.Components.Add(this);

        }
    
        protected override void LoadContent()
        {
            EffetDeBase = new BasicEffect(GraphicsDevice);
            EffetDeBase.VertexColorEnabled = true;
            base.LoadContent();
            
            

            // à compléter

        }
        protected override void InitialiserSommets()
        {
            Vector3 Pos0 = new Vector3(Origine.X + Delta.X, Origine.Y, Origine.Z);
            Vector3 Pos1 = new Vector3(Origine.X, Origine.Y - Delta.Y, Origine.Z);
            Vector3 Pos2 = new Vector3(Origine.X + Delta.X, Origine.Y - Delta.Y, Origine.Z);
            Vector3 Pos3 = new Vector3(Origine.X, Origine.Y - Delta.Y, Origine.Z - Delta.Z);
            Vector3 Pos4 = new Vector3(Origine.X + Delta.X, Origine.Y - Delta.Y, Origine.Z - Delta.Z);
            Vector3 Pos5 = new Vector3(Origine.X, Origine.Y, Origine.Z - Delta.Z);
            Vector3 Pos6 = new Vector3(Origine.X + Delta.X, Origine.Y, Origine.Z - Delta.Z);

            
           
            
            //SOMMETS AVANT
            SommetsA[0] = new VertexPositionColor(Origine, CouleurContour);
            SommetsA[1] = new VertexPositionColor(Pos0, CouleurContour);
            SommetsA[2] = new VertexPositionColor(Pos1, CouleurContour);
            SommetsA[3] = new VertexPositionColor(Pos2, CouleurContour);

            //SOMMETS GAUCHE
            SommetsB[0] = new VertexPositionColor(Origine, CouleurContour);
            SommetsB[1] = new VertexPositionColor(Pos1, CouleurContour);
            SommetsB[2] = new VertexPositionColor(Pos5, CouleurContour);
            SommetsB[3] = new VertexPositionColor(Pos3, CouleurContour);

            //SOMMETS DERRIERE

            SommetsC[0] = new VertexPositionColor(Pos6, CouleurContour);
            SommetsC[1] = new VertexPositionColor(Pos5, CouleurContour);
            SommetsC[2] = new VertexPositionColor(Pos4, CouleurContour);
            SommetsC[3] = new VertexPositionColor(Pos3, CouleurContour);

            //SOMMETS DROITE

            SommetsD[0] = new VertexPositionColor(Pos0, CouleurContour);
            SommetsD[1] = new VertexPositionColor(Pos6, CouleurContour);
            SommetsD[2] = new VertexPositionColor(Pos2, CouleurContour);
            SommetsD[3] = new VertexPositionColor(Pos4, CouleurContour);

            //SOMMETS DESSUS

            SommetsE[0] = new VertexPositionColor(Pos5, Couleur);
            SommetsE[1] = new VertexPositionColor(Pos6, Couleur);
            SommetsE[2] = new VertexPositionColor(Origine, Couleur);
            SommetsE[3] = new VertexPositionColor(Pos0, Couleur);


            

            //SOMMETS DESSOUS

            SommetsF[0] = new VertexPositionColor(Pos4, CouleurContour);
            SommetsF[1] = new VertexPositionColor(Pos3, CouleurContour);
            SommetsF[2] = new VertexPositionColor(Pos2, CouleurContour);
            SommetsF[3] = new VertexPositionColor(Pos1, CouleurContour);

            ZoneSelection = new Rectangle((int)Pos5.X, (int)Pos5.Z, (int)this.Dimension.X,(int)this.Dimension.X);
           
            


            
            

        }
        public override void Draw(GameTime gameTime)
        {

            EffetDeBase.World = GetMonde();
            EffetDeBase.View = CaméraJeu.Vue;
            EffetDeBase.Projection = CaméraJeu.Projection;
            foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
            {
                passeEffet.Apply();
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsA, 0, NB_TRIANGLES);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsB, 0, NB_TRIANGLES);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsC, 0, NB_TRIANGLES);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsD, 0, NB_TRIANGLES);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsE, 0, NB_TRIANGLES);
                GraphicsDevice.DrawUserPrimitives<VertexPositionColor>(PrimitiveType.TriangleStrip, SommetsF, 0, NB_TRIANGLES);
                
                
            }
            
            base.Draw(gameTime);
       
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
        public void ModifierPropriétaire(string nouvellePièce)
        {

        }

    }
}

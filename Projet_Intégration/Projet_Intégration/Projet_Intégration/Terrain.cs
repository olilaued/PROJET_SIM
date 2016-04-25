using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace AtelierXNA
{
   public class Terrain : PrimitiveDeBaseAnimée
   {
      const int NB_TRIANGLES_PAR_TUILE = 2;
      const int NB_SOMMETS_PAR_TRIANGLE = 3;
      const int NB_SOMMETS_PAR_TUILE = 4;
      const float MAX_COULEUR = 255f;

      Vector3 Étendue { get; set; }
      string NomCarteTerrain { get; set; }
      string NomTextureTerrain { get; set; }
      int NbNiveauTexture { get; set; }
      int NbColonnes { get; set; }
      int NbRangées { get; set; }
      Vector2 Delta { get; set; }
      Vector2 DeltaTexture { get; set; }

 
      BasicEffect EffetDeBase { get; set; }
      RessourcesManager<Texture2D> GestionnaireDeTextures { get; set; }
      Texture2D CarteTerrain { get; set; }
      Texture2D TextureTerrain { get; set; }
       
      Vector3 Origine { get; set; }
      
      Color[] DataTexture { get; set; }
      Vector3[,] PtsSommets { get; set; }
      Vector2[,] PtsTexture { get; set; }
      VertexPositionTexture[] Sommets { get; set; }
      // à compléter


      public Terrain(Game jeu, float homothétieInitiale, Vector3 rotationInitiale, Vector3 positionInitiale,
                     Vector3 étendue, string nomCarteTerrain, string nomTextureTerrain, int nbNiveauxTexture, float intervalleMAJ)
         : base(jeu, homothétieInitiale, rotationInitiale, positionInitiale, intervalleMAJ)
      {
         Étendue = étendue;
         NomCarteTerrain = nomCarteTerrain;
         NomTextureTerrain = nomTextureTerrain;
         NbNiveauTexture = nbNiveauxTexture;
      }

      public override void Initialize()
      {
         GestionnaireDeTextures = Game.Services.GetService(typeof(RessourcesManager<Texture2D>)) as RessourcesManager<Texture2D>;
         InitialiserDonnéesCarte();
         InitialiserDonnéesTexture();
         Origine = new Vector3(-Étendue.X / 2, 0, -Étendue.Z / 2); //pour centrer la primitive au point (0,0,0)
         AllouerTableaux();
         CréerTableauPoints();
         CréerTableauPointsTexture();
         base.Initialize();
      }

      //
      // à partir de la texture servant de carte de hauteur (HeightMap), on initialise les données
      // relatives à la structure de la carte
      //
      void InitialiserDonnéesCarte()
      {
         CarteTerrain = GestionnaireDeTextures.Find(NomCarteTerrain);
         NbColonnes = CarteTerrain.Width;
         NbRangées = CarteTerrain.Height;
         Delta = new Vector2(Étendue.X / NbColonnes, Étendue.Z / NbRangées);
        // Delta = new Vector2(NbColonnes / MAX_COULEUR, NbRangées / MAX_COULEUR);
         DataTexture = new Color[NbColonnes * NbRangées];
         CarteTerrain.GetData<Color>(DataTexture);
         NbTriangles =  2 * NbColonnes * NbRangées;
      }

      //
      // à partir de la texture contenant les textures carte de hauteur (HeightMap), on initialise les données
      // relatives à l'application des textures de la carte
      //
      void InitialiserDonnéesTexture()
      {
         TextureTerrain = GestionnaireDeTextures.Find(NomTextureTerrain);
         DeltaTexture = new Vector2(1 ,(1 / (float)NbNiveauTexture));
         PtsTexture = new Vector2[2, NbNiveauTexture +1];
         
      }

      //
      // Allocation des deux tableaux
      //    1) celui contenant les points de sommet (les points uniques), 
      //    2) celui contenant les sommets servant à dessiner les triangles
      void AllouerTableaux()
      {
         PtsSommets = new Vector3[NbColonnes,NbRangées];
         Sommets = new VertexPositionTexture[3*NbTriangles];
      }

      protected override void LoadContent()
      {
         base.LoadContent();
         EffetDeBase = new BasicEffect(GraphicsDevice);
         InitialiserParamètresEffetDeBase();
      }

      void InitialiserParamètresEffetDeBase()
      {
         EffetDeBase.TextureEnabled = true;
         EffetDeBase.Texture = TextureTerrain;
      }

      //
      // Création du tableau des points de sommets (on crée les points)
      // Ce processus implique la transformation des points 2D de la texture en coordonnées 3D du terrain
      //
      private void CréerTableauPoints()
      {
         float positionZ = Origine.Z;
         int a = 0;

         for(int i = 0; i < NbRangées;i++)
         {
            float positionX = Origine.X;
            for (int o = 0; o < NbColonnes;o++)
            {
                PtsSommets[o,i] = new Vector3(positionX ,Étendue.Y * (DataTexture[a].G/MAX_COULEUR), positionZ );
                positionX += Delta.X;
                a++;
            }
            positionZ += Delta.Y;
            
            
         }
         
      }
      protected void CréerTableauPointsTexture()
      {
         
         float positionY = 0;
          

         for (int i = 0; i < NbNiveauTexture + 1; i++)
         {
            float positionX = 0;
            for (int o = 0; o < 2 ;o++)
            {
               PtsTexture[o, i] = new Vector2(positionX, positionY);
               positionX += DeltaTexture.X;
            }
            positionY += DeltaTexture.Y;
         }
      }

      //
      // Création des sommets.
      // N'oubliez pas qu'il s'agit d'un TriangleList...
      //
      protected override void InitialiserSommets()
      {
         int NoSommet = -1;
         int a = 0;
         int b = 0;
         for (int j = 0; j < NbRangées -1; ++j)
         {
            for (int i = 0; i < NbColonnes-1; ++i)
            {
                
                
                float hauteurMOY = (PtsSommets[i, j].Y + PtsSommets[i, j + 1].Y + PtsSommets[i + 1, j].Y 
                   + PtsSommets[i + 1, j + 1].Y) / 4;
                float pourcent = (float)Math.Round(hauteurMOY/Étendue.Y,2);
                
                for (int o = 0; o <= NbNiveauTexture; o++ )
                {
                    if(pourcent >= PtsTexture[b, o].Y && pourcent <= PtsTexture[b, o + 1].Y)
                    {
                        a = o;
                        o = (PtsTexture.Length - 1);
                    }
                }

               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j], PtsTexture[b, a]);
               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[b + 1, a]);
               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[b, a + 1]);
               
               
               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i, j + 1], PtsTexture[b, a + 1]);
               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i + 1, j], PtsTexture[b + 1, a]);
               Sommets[++NoSommet] = new VertexPositionTexture(PtsSommets[i + 1, j + 1], PtsTexture[b + 1, a + 1]);
            }
         }
      }

      //
      // Deviner ce que fait cette méthode...
      //
      public override void Draw(GameTime gameTime)
      {
         EffetDeBase.World = GetMonde();
         EffetDeBase.View = CaméraJeu.Vue;
         EffetDeBase.Projection = CaméraJeu.Projection;
         foreach (EffectPass passeEffet in EffetDeBase.CurrentTechnique.Passes)
         {
            passeEffet.Apply();

            for (int i = 0; i < NbRangées; i++)
            {
              GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Sommets, (NB_SOMMETS_PAR_TRIANGLE*NbTriangles/NbRangées)*i, NbTriangles / NbRangées);
            }
             // GraphicsDevice.DrawUserPrimitives<VertexPositionTexture>(PrimitiveType.TriangleList, Sommets, 0, NbTriangles / NbRangées);

         }
         base.Draw(gameTime);
         // à compléter
      }
   }
}

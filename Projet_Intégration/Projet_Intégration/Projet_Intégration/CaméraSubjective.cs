using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace AtelierXNA
{
   public class CaméraSubjective : Caméra
   {
      const float INTERVALLE_MAJ_STANDARD = 1f / 60f;
      const float ACCÉLÉRATION = 0.001f;
      const float VITESSE_INITIALE_ROTATION = 5f;
      const float VITESSE_INITIALE_TRANSLATION = 0.5f;
      const float DELTA_LACET = MathHelper.Pi / 180; // 1 degré à la fois
      const float DELTA_TANGAGE = MathHelper.Pi / 180; // 1 degré à la fois
      const float DELTA_ROULIS = MathHelper.Pi / 180; // 1 degré à la fois
      const float RAYON_COLLISION = 1f;

      Vector3 Direction { get; set; }
      Vector3 Latéral { get; set; }
      float VitesseTranslation { get; set; }
      float VitesseRotation { get; set; }

      float IntervalleMAJ { get; set; }
      float TempsÉcouléDepuisMAJ { get; set; }
      InputManager GestionInput { get; set; }
      Matrix Rotation { get; set; }

      bool estEnZoom;
      bool EstEnZoom
      {
         get { return estEnZoom; }
         set
         {
            float ratioAffichage = Game.GraphicsDevice.Viewport.AspectRatio;
            estEnZoom = value;
            if (estEnZoom)
            {
               CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF / 2, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            }
            else
            {
               CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, ratioAffichage, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
            }
         }
      }

      public CaméraSubjective(Game jeu, Vector3 positionCaméra, Vector3 cible, Vector3 orientation, float intervalleMAJ)
         : base(jeu)
      {
         IntervalleMAJ = intervalleMAJ;
         CréerVolumeDeVisualisation(OUVERTURE_OBJECTIF, DISTANCE_PLAN_RAPPROCHÉ, DISTANCE_PLAN_ÉLOIGNÉ);
         CréerPointDeVue(positionCaméra, cible, orientation);
         EstEnZoom = false;
      }

      public override void Initialize()
      {
         VitesseRotation = VITESSE_INITIALE_ROTATION;
         VitesseTranslation = VITESSE_INITIALE_TRANSLATION;
         TempsÉcouléDepuisMAJ = 0;
         base.Initialize();
         GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
         
      }

      protected override void CréerPointDeVue()
      {
         // Méthode appelée s'il est nécessaire de recalculer la matrice de vue.
         // Calcul et normalisation de certains vecteurs
         // (à compléter)
         Direction = Vector3.Normalize(Direction);
         Latéral = Vector3.Cross(Direction, OrientationVerticale);
         Vue = Matrix.CreateLookAt(Position, Position + Direction, OrientationVerticale);
         GénérerFrustum();
      }

      protected override void CréerPointDeVue(Vector3 position, Vector3 cible, Vector3 orientation)
      {
         // À la construction, initialisation des propriétés Position, Cible et OrientationVerticale,
         // ainsi que le calcul des vecteur Direction, Latéral et le recalcul du vecteur OrientationVerticale
         // permettant de calculer la matrice de vue de la caméra subjective
         // (à compléter)
         Position = position;
         Cible = cible;
         OrientationVerticale = orientation;
         Direction = Cible - Position;
         Latéral = Vector3.Cross(Direction, OrientationVerticale);
         CréerPointDeVue();
      }

      public override void Update(GameTime gameTime)
      {
         float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
         TempsÉcouléDepuisMAJ += TempsÉcoulé;
         GestionClavier();
         if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
         {
            if (GestionInput.EstEnfoncée(Keys.LeftShift) || GestionInput.EstEnfoncée(Keys.RightShift))
            {
              GérerAccélération();
               GérerDéplacement();
               GérerRotation();
              CréerPointDeVue();
            }
            TempsÉcouléDepuisMAJ = 0;
         }
         base.Update(gameTime);
      }

      private int GérerTouche(Keys touche)
      {
         return GestionInput.EstEnfoncée(touche) ? 1 : 0;
      }

      private void GérerAccélération()
      {
         int valAccélération = (GérerTouche(Keys.Subtract) + GérerTouche(Keys.OemMinus)) - (GérerTouche(Keys.Add)+GérerTouche(Keys.OemPlus));
         if (valAccélération != 0)
         {
            IntervalleMAJ += ACCÉLÉRATION * valAccélération;
            IntervalleMAJ = MathHelper.Max(INTERVALLE_MAJ_STANDARD, IntervalleMAJ);
         }
      }

      private void GérerDéplacement()
      {
         Vector3 nouvellePosition = Position;
         float déplacementDirection = (GérerTouche(Keys.W) - GérerTouche(Keys.S)) * VitesseTranslation;
         float déplacementLatéral = (GérerTouche(Keys.D) - GérerTouche(Keys.A)) * VitesseTranslation;
        
         nouvellePosition = new Vector3(Position.X + (Direction.X * déplacementDirection), Position.Y + (Direction.Y * déplacementDirection),
             Position.Z + (Direction.Z * déplacementDirection));
         nouvellePosition = new Vector3(nouvellePosition.X + (Latéral.X * déplacementLatéral), nouvellePosition.Y + (Latéral.Y * déplacementLatéral),
             nouvellePosition.Z + (Latéral.Z * déplacementLatéral));

         Position = nouvellePosition;

         
      }

      private void GérerRotation()
      {
         GérerLacet();
         GérerTangage();
         GérerRoulis();
      }

      private void GérerLacet()
      {
         float variationLacet = ((GérerTouche(Keys.Left) - GérerTouche(Keys.Right)) * VitesseRotation);
         Rotation = Matrix.CreateFromAxisAngle(OrientationVerticale, variationLacet * DELTA_LACET);
         Vector3 nouvelleDirection = Vector3.Transform(Direction, Rotation);
         Direction = nouvelleDirection;
         OrientationVerticale = Vector3.Normalize(Vector3.Cross(Latéral,Direction));
      }

      private void GérerTangage()
      {
         float variationTangue = ((GérerTouche(Keys.Down) - GérerTouche(Keys.Up)) * VitesseRotation);
         Rotation = Matrix.CreateFromAxisAngle(Latéral, variationTangue * DELTA_TANGAGE);
         Vector3 nouvelleDirection = Vector3.Transform(Direction, Rotation);
         Direction = nouvelleDirection;

         Latéral = Vector3.Cross(Direction, OrientationVerticale);
         
         Vector3 nouvelleOV =  Vector3.Transform(OrientationVerticale, Rotation);
         OrientationVerticale = Vector3.Normalize(nouvelleOV);
      }

      private void GérerRoulis()
      {
        float variationRoulis = ((GérerTouche(Keys.PageUp) - GérerTouche(Keys.PageDown)) * VitesseRotation);
        Rotation = Matrix.CreateFromAxisAngle(Direction, variationRoulis * DELTA_ROULIS);
        Vector3 nouvelleOV = Vector3.Transform(OrientationVerticale, Rotation);
        OrientationVerticale = Vector3.Normalize(nouvelleOV);
          
      }
       
      private void GestionClavier()
      {
         //if (GestionInput.EstNouvelleTouche(Keys.Z))
         //{
         //   EstEnZoom = !EstEnZoom;
         //}
          if (GestionInput.EstNouvelleTouche(Keys.P))
          {
              StreamWriter sw = File.CreateText(@"C:\Users\Tristan\Documents\GitHub\PROJET_SIM\Projet_Intégration\Projet_Intégration\Projet_IntégrationContent");
              sw.WriteLine(this.Position.ToString(), this.Cible.ToString(), this.OrientationVerticale.ToString());
          }

      }
   }
}

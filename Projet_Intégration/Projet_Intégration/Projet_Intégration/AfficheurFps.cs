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
    public class AfficheurFps : Microsoft.Xna.Framework.DrawableGameComponent
    {
        const int MARGE_BAS = 10;
        const int MARGE_DROITE = 15;
        const float AUCUNE_ROTATION = 0f;
        const float AUCUNE_HOMOTHÉTIE = 1f;
        const float AVANT_PLAN = 0f;

        float IntervalleMAJ { get; set; }
        float TempsÉcouléDepuisMAJ { get; set; }
        int CptFrames { get; set; }
        float ValFPS { get; set; }

        string ChaîneFPS { get; set; }
        Vector2 PositionDroiteBas { get; set; }
        Vector2 PositionChaîne { get; set; }
        Vector2 Dimension { get; set; }

        SpriteBatch GestionSprites { get; set; }
        RessourcesManager<SpriteFont> GestionnaireDeFonts { get; set; }
        string NomFont { get; set; }
        SpriteFont policeDeCaractères { get; set; }
        Color CouleurFPS { get; set; }

        public AfficheurFps(Game game, string nomFont, Color couleurFPS, float intervalleMAJ)
           : base(game)
       {
           NomFont = nomFont;
           CouleurFPS = couleurFPS;
           IntervalleMAJ = intervalleMAJ;
       }

       public override void Initialize()
       {
           TempsÉcouléDepuisMAJ = 0;
           ValFPS = 0;
           CptFrames = 0;
           ChaîneFPS = "";
           PositionDroiteBas = new Vector2(Game.Window.ClientBounds.Width - MARGE_DROITE,
                                           Game.Window.ClientBounds.Height - MARGE_BAS);
           base.Initialize();
       }

       protected override void LoadContent()
       {
           GestionSprites = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;
           GestionnaireDeFonts = Game.Services.GetService(typeof(RessourcesManager<SpriteFont>)) as RessourcesManager<SpriteFont>;
           policeDeCaractères = GestionnaireDeFonts.Find(NomFont);
           
           base.LoadContent();
       }

       public override void Update(GameTime gameTime)
       {
           float tempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
           ++CptFrames;
           TempsÉcouléDepuisMAJ += tempsÉcoulé;
           if (TempsÉcouléDepuisMAJ >= IntervalleMAJ)
           {
               CalculerFPS();
               TempsÉcouléDepuisMAJ = 0;
           }

           base.Update(gameTime);
       }

       void CalculerFPS()
       {
           float ancienneValFPS = ValFPS;
           ValFPS = CptFrames / TempsÉcouléDepuisMAJ;
           if (ancienneValFPS != ValFPS)
           {
               ChaîneFPS = ValFPS.ToString("0");
               Dimension = policeDeCaractères.MeasureString(ChaîneFPS);
               PositionChaîne = PositionDroiteBas - Dimension;
           }
           CptFrames = 0;
       }

       public override void Draw(GameTime gameTime)
       {
           GestionSprites.Begin();
           GestionSprites.DrawString(policeDeCaractères, ChaîneFPS, PositionChaîne, CouleurFPS, AUCUNE_ROTATION,
                                     new Vector2(10, 10), AUCUNE_HOMOTHÉTIE, SpriteEffects.None, AVANT_PLAN);
           GestionSprites.End();

           base.Draw(gameTime);
           
       }
    }
}

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
    public class Partie : Microsoft.Xna.Framework.GameComponent
    {
        const float LARGEUR_ECHIQUIER = 0.3f;
        public static float LONGUEUR_ÉCHIQUIER = 16f;
        //const float PROFONDEUR_DEFAUT = 0.5f;
        const float SCALE_DEFAUT = 1.0f;

        //const string VAINQUEUR_B = "Les blancs l'emportent!";
        //const string VAINQUEUR_N = "Les noirs l'emportent!";

       public static float TempsLimite { get; set; }
        List<Pieces> ListeDesPièces { get; set; }
        protected List<string> ListeDesMoves { get; set; }
        protected string Map { get; set; }
        protected  Echiquier UnÉchiquier { get; set; }
        protected CaméraSubjective CaméraJeu { get; set; }
        protected float NbSortiesBlanc { get; set; }
        protected float NbSortiesNoir { get; set; }
        protected float TempsÉcouléDepuisMAJ { get; set; }
        public Tour TourActuel { get; set; }
        ObjetDeBase Environnement {get; set;}
        protected static Vector2[] PositionSorties { get; set; }
        protected TexteAffichable GagnantB { get; set; }
        protected TexteAffichable GagnantN { get; set; }
        public bool PartieTerminée {get; set;}
       

        InputManager GestionInput { get; set; }
        
        






        public Partie(Game game, float tempsLimite, string map, Color[] couleursÉchiquier, Vector3 origineÉchiquier)
            : base(game)
        {
           game.Components.Add(UnÉchiquier = new Echiquier(Game, origineÉchiquier, new Vector2(LONGUEUR_ÉCHIQUIER, LARGEUR_ECHIQUIER), couleursÉchiquier[0],
                couleursÉchiquier[1], couleursÉchiquier[2]));
            TempsLimite = tempsLimite;
            Map = map;
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            PositionSorties = new Vector2[2];
            ListeDesPièces = new List<Pieces>();
            TempsÉcouléDepuisMAJ = 0;
            NbSortiesBlanc = 0;
            NbSortiesNoir = 0;
            GestionInput = Game.Services.GetService(typeof(InputManager)) as InputManager;
            CaméraJeu = Game.Services.GetService(typeof(CaméraSubjective)) as CaméraSubjective;
            PositionSorties[0] = new Vector2(UnÉchiquier.Origine.X - 1, UnÉchiquier.Origine.Y);
            PositionSorties[1] = new Vector2(UnÉchiquier.Origine.X + 17, UnÉchiquier.Origine.Y);
            //GagnantN = new TexteAffichable(Game, "Arial", VAINQUEUR_N, Color.LightGreen, 0, 3.0f, PROFONDEUR_DEFAUT);
            //GagnantB = new TexteAffichable(Game, "Arial", VAINQUEUR_B, Color.LightGreen, 0, 3.0f, PROFONDEUR_DEFAUT);
            Game.Components.Add(Environnement = new ObjetDeBase(Game, Map, 0.01f, new Vector3(0, 0, 0), Vector3.Zero));
            Game.Components.Add(TourActuel = new Tour(Game,"White", UnÉchiquier.ListeCases, ListeDesPièces, NbSortiesBlanc, NbSortiesNoir));
            Environnement.Visible = false;
            TourActuel.Enabled = false;
            InitialiserPièces(UnÉchiquier);
            
            
           
            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            float TempsÉcoulé = (float)gameTime.ElapsedGameTime.TotalSeconds;
            TempsÉcouléDepuisMAJ += TempsÉcoulé;

            if (Jeu.CurrentGameState == Jeu.GameState.EnJeu)
            {
                if (Environnement.Visible == false)
                {
                    TourActuel.Enabled = true;
                    Environnement.Visible = true;
                    ModifierEstVisiblePièces();
                    UnÉchiquier.ModifierVisibilitéCases();
                }


                if (TempsÉcouléDepuisMAJ > TempsLimite || TourActuel.PartieTerminée || TourActuel.EstMat())
                {
                    PartieTerminée = true;
                }
            }
            if (Jeu.CurrentGameState == Jeu.GameState.EnPause && Environnement.Visible == true)
            {
                UnÉchiquier.ModifierVisibilitéCases();
                ModifierEstVisiblePièces();
                Environnement.Visible = false;
                Environnement.Enabled = false;
            }







            else
            {
                ///code pour mode bot
            }
            base.Update(gameTime);

        }
        void InitialiserPièces(Echiquier unEchiquier)
        {
            
            for (int i = 0; i < 8; i++)
            {
                Pions pionB = new Pions(Game, unEchiquier.ListeCases[1 + 8 * i].Centre, "Black");
                ListeDesPièces.Add(pionB);
                Pions pionW = new Pions(Game, unEchiquier.ListeCases[(1 + 8 * i) + 5].Centre, "White");
                ListeDesPièces.Add(pionW);


            }
            for (int i = 0; i < 2; i++)
            {
                //CRÉATION TOURS
                Tours tourB = new Tours(Game, unEchiquier.ListeCases[0 + 56 * i].Centre, "Black");
                ListeDesPièces.Add(tourB);
                Tours tourW = new Tours(Game, unEchiquier.ListeCases[(0 + 56 * i) + 7].Centre, "White");
                ListeDesPièces.Add(tourW);
                

                //CRÉATION CAVALIERS
                Cavaliers cavalierB = new Cavaliers(Game, unEchiquier.ListeCases[8 + 40 * i].Centre, "Black");
                ListeDesPièces.Add(cavalierB);
                Cavaliers cavalierW = new Cavaliers(Game, unEchiquier.ListeCases[(8 + 40 * i) + 7].Centre, "White");
                ListeDesPièces.Add(cavalierW);

                //CRÉATION FOUS
                Fous fouB = new Fous(Game, unEchiquier.ListeCases[16 + 24 * i].Centre, "Black");
                ListeDesPièces.Add(fouB);
                Fous fouW = new Fous(Game, unEchiquier.ListeCases[(16 + 24 * i) + 7].Centre, "White");
                ListeDesPièces.Add(fouW);
               

            }
            //CRÉATION REINES
            Reine reineB = new Reine(Game, unEchiquier.ListeCases[24].Centre, "Black");
            ListeDesPièces.Add(reineB);
            Reine reineW = new Reine(Game, unEchiquier.ListeCases[24 + 7].Centre, "White");
            ListeDesPièces.Add(reineW);

            //CRÉATION ROI
            Roi roiB = new Roi(Game, unEchiquier.ListeCases[32].Centre, "Black");
            ListeDesPièces.Add(roiB);
            Roi roiW = new Roi(Game, unEchiquier.ListeCases[32 + 7].Centre, "White");
            ListeDesPièces.Add(roiW);

        }
        
        public static Vector2 GetPositionSorties(int index)
        {
            
            if (index >= 0 && index < PositionSorties.Length)
            {
                return PositionSorties[index];
            }
            else
            {
                return Vector2.Zero;
            }
            
        }

        public void ModifierEstVisiblePièces()
        {
           foreach (Pieces p in ListeDesPièces)
            {
                p.Visible = !p.Visible;
               //p.Visible = true;
            }

        }
        public void Retirer()
        {
            Game.Components.Remove(Environnement);
            foreach (Pieces p in ListeDesPièces)
            {
                Game.Components.Remove(p);
            }
            UnÉchiquier.Retirer();
            Game.Components.Remove(TourActuel);
            Game.Components.Remove(this);
        }

        
        
        
    
    }
        
    
}
